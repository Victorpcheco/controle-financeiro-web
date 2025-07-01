// ===== VARIÁVEIS GLOBAIS =====
let apiAccountsData = null
let apiSaldoTotal = 0
let apiPendingIncomes = 0
let apiPendingExpenses = 0
let showValues = true
let showAccountDetails = false
let currentEditingTransaction = null

// ===== FUNÇÕES DE FORMATAÇÃO DE DATA =====
function formatDateToBR(dateString) {
  if (!dateString) return ""
  const date = new Date(dateString + "T00:00:00")
  return date.toLocaleDateString("pt-BR")
}

function formatDateToISO(brDateString) {
  if (!brDateString) return ""

  console.log("Convertendo data BR para ISO:", brDateString)

  // Remove caracteres não numéricos
  const cleanDate = brDateString.replace(/\D/g, "")
  if (cleanDate.length !== 8) {
    console.log("Data inválida - comprimento incorreto:", cleanDate.length)
    return ""
  }

  // Extrai dia, mês e ano
  const day = cleanDate.substring(0, 2)
  const month = cleanDate.substring(2, 4)
  const year = cleanDate.substring(4, 8)

  console.log(`Extraído: dia=${day}, mês=${month}, ano=${year}`)

  // Valida se é uma data válida
  const date = new Date(year, month - 1, day)
  if (date.getFullYear() != year || date.getMonth() != month - 1 || date.getDate() != day) {
    console.log("Data inválida após validação")
    return ""
  }

  const isoDate = `${year}-${month}-${day}`
  console.log("Data ISO resultante:", isoDate)
  return isoDate
}

function applyDateMask(value) {
  // Remove tudo que não é número
  value = value.replace(/\D/g, "")

  // Aplica a máscara DD/MM/AAAA
  if (value.length >= 2) {
    value = value.substring(0, 2) + "/" + value.substring(2)
  }
  if (value.length >= 5) {
    value = value.substring(0, 5) + "/" + value.substring(5, 9)
  }

  return value
}

// ===== CONFIGURAÇÃO DA API =====
const API_CONFIG = {
  baseUrl: "https://localhost:7101/api",
  endpoints: {
    saldoTotalRotaApi: "/dashboard/saldo-total",
    accountsBalance: "/dashboard/saldo-contas",
    ReceitasTotaisRotaApi: "/dashboard/valor-em-aberto-receitas",
    pendingExpenses: "/dashboard/valor-em-aberto-despesas",
    // ENDPOINT ORIGINAL PARA CARREGAR MOVIMENTAÇÕES INICIAIS
    transactions: "/dashboard/movimentacoes-em-aberto",
    // NOVO ENDPOINT PARA FILTROS
    transactionsFilter: "/movimentacoes/filtrar",
    editTransaction: "/movimentacoes/editar",
    categories: "/categoria",
    accounts: "/contabancaria",
    cards: "/cartao",
    months: "/MesReferencia",
  },
}

// ===== FUNÇÕES DE AUTENTICAÇÃO =====
function verificaAutenticacao() {
  const token = buscaTokenJwt()

  if (!token) {
    console.warn("Usuário não autenticado, redirecionando para login")
    window.location.replace("/login.html")
    return false
  }

  return true
}

function buscaTokenJwt() {
  const keys = ["authToken", "token", "jwt"]
  for (const key of keys) {
    const token = localStorage.getItem(key)
    if (token) return token
  }
  return null
}

// ===== FUNÇÕES DE API =====
async function buscaApiSaldoTotal() {
  try {
    const token = buscaTokenJwt()

    if (!token) {
      console.warn("Usuário não autenticado, token não encontrado")
      return null
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.saldoTotalRotaApi}`, {
      method: "GET",
      headers: headers,
    })

    if (!response.ok) {
      console.error(`Erro na API (saldo total): ${response.status} - ${response.statusText}`)
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}`)
    }

    const data = await response.json()
    apiSaldoTotal = typeof data === "number" ? data : data.saldo || data.balance || data.total || 0
    return apiSaldoTotal
  } catch (error) {
    console.error("Erro ao buscar saldo total da API:", error.message)
    return null
  }
}

async function buscaAPiReceitasPendentes() {
  try {
    const token = buscaTokenJwt()

    if (!token) {
      console.warn("Usuário não autenticado, token não encontrado")
      return null
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.ReceitasTotaisRotaApi}`, {
      method: "GET",
      headers: headers,
    })

    if (!response.ok) {
      const errorBody = await response.text()
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`)
    }

    const data = await response.json()
    apiPendingIncomes = typeof data === "number" ? data : data.receitas || data.pendingIncomes || data.total || 0
    return apiPendingIncomes
  } catch (error) {
    console.error("Erro ao buscar receitas pendentes da API:", error.message)
    return null
  }
}

async function fetchPendingExpenses() {
  try {
    const token = buscaTokenJwt()

    if (!token) {
      console.warn("Usuário não autenticado, token não encontrado")
      return null
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.pendingExpenses}`, {
      method: "GET",
      headers: headers,
    })

    if (!response.ok) {
      const errorBody = await response.text()
      throw new Error(`Erro na API (despesas): ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`)
    }

    const data = await response.json()
    apiPendingExpenses = typeof data === "number" ? data : data.despesas || data.pendingExpenses || data.total || 0
    return apiPendingExpenses
  } catch (error) {
    console.error("Erro ao buscar despesas pendentes da API:", error.message)
    return null
  }
}

async function fetchAccountsBalance() {
  try {
    const token = buscaTokenJwt()

    if (!token) {
      console.warn("Usuário não autenticado, token não encontrado")
      return null
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.accountsBalance}`, {
      method: "GET",
      headers: headers,
    })

    if (!response.ok) {
      const errorBody = await response.text()
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`)
    }

    const data = await response.json()
    apiAccountsData = data
    return data
  } catch (error) {
    console.error("Erro ao buscar saldos das contas da API:", error.message)
    return null
  }
}

// ===== FUNÇÃO ORIGINAL PARA BUSCAR MOVIMENTAÇÕES (SEM FILTROS) =====
async function fetchTransactionsFromApi(pagina = 1, quantidadePorPagina = 10) {
  try {
    const token = buscaTokenJwt()
    if (!token) {
      console.warn("Usuário não autenticado, token não encontrado")
      return { movimentacoes: [], total: 0 }
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    // Construir URL com parâmetros básicos de paginação
    const params = new URLSearchParams({
      pagina: pagina.toString(),
      quantidadePorPagina: quantidadePorPagina.toString(),
    })

    const url = `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.transactions}?${params.toString()}`
    console.log("Buscando movimentações da URL (sem filtros):", url)

    const response = await fetch(url, {
      method: "GET",
      headers: headers,
    })

    if (!response.ok) {
      const errorBody = await response.text()
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`)
    }

    const data = await response.json()
    console.log("Dados recebidos da API (sem filtros):", data)

    // Verificar se a resposta tem a estrutura esperada
    if (!data.movimentacoes || !Array.isArray(data.movimentacoes)) {
      console.warn("Estrutura de dados inesperada:", data)
      return { movimentacoes: [], total: 0 }
    }

    return {
      movimentacoes: data.movimentacoes,
      total: data.total || 0,
      pagina: data.pagina || 1,
      quantidade: data.quantidade || quantidadePorPagina,
    }
  } catch (error) {
    console.error("Erro ao buscar movimentações da API:", error.message)
    return { movimentacoes: [], total: 0 }
  }
}

// ===== NOVA FUNÇÃO PARA BUSCAR MOVIMENTAÇÕES COM FILTROS =====

// ===== FUNÇÃO PARA EDITAR MOVIMENTAÇÃO =====
async function updateTransaction(transactionData) {
  try {
    const token = buscaTokenJwt()
    if (!token) {
      throw new Error("Usuário não autenticado")
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    console.log("Enviando dados para API:", transactionData)

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.editTransaction}`, {
      method: "PUT",
      headers: headers,
      body: JSON.stringify(transactionData),
    })

    if (!response.ok) {
      const errorBody = await response.text()
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`)
    }

    const data = await response.json()
    console.log("Movimentação atualizada com sucesso:", data)
    return data
  } catch (error) {
    console.error("Erro ao atualizar movimentação:", error.message)
    throw error
  }
}

// ===== FUNÇÕES PARA BUSCAR DADOS DOS SELECTS DA API =====
async function fetchCategories() {
  try {
    const token = buscaTokenJwt()
    if (!token) {
      console.warn("Token não encontrado para buscar categorias")
      return []
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.categories}?pagina=1&quantidade=100`, {
      method: "GET",
      headers: headers,
    })

    if (!response.ok) {
      console.error(`Erro ao buscar categorias: ${response.status} - ${response.statusText}`)
      return []
    }

    const data = await response.json()
    console.log("Resposta completa das categorias:", data)

    const categorias = data.categorias || data.data || data || []
    console.log("Categorias extraídas:", categorias)
    return Array.isArray(categorias) ? categorias : []
  } catch (error) {
    console.error("Erro ao buscar categorias:", error)
    return []
  }
}

async function fetchAccounts() {
  try {
    const token = buscaTokenJwt()
    if (!token) {
      console.warn("Token não encontrado para buscar contas")
      return []
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.accounts}?pagina=1&quantidade=100`, {
      method: "GET",
      headers: headers,
    })

    if (!response.ok) {
      console.error(`Erro ao buscar contas: ${response.status} - ${response.statusText}`)
      return []
    }

    const data = await response.json()
    console.log("Resposta completa das contas:", data)

    const contas = data.contas || data.contasBancarias || data.data || data || []
    console.log("Contas extraídas:", contas)
    return Array.isArray(contas) ? contas : []
  } catch (error) {
    console.error("Erro ao buscar contas:", error)
    return []
  }
}

async function fetchCards() {
  try {
    const token = buscaTokenJwt()
    if (!token) {
      console.warn("Token não encontrado para buscar cartões")
      return []
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.cards}?pagina=1&quantidade=100`, {
      method: "GET",
      headers: headers,
    })

    if (!response.ok) {
      console.error(`Erro ao buscar cartões: ${response.status} - ${response.statusText}`)
      return []
    }

    const data = await response.json()
    console.log("Resposta completa dos cartões:", data)

    const cartoes = data.cartoes || data.data || data || []
    console.log("Cartões extraídos:", cartoes)
    return Array.isArray(cartoes) ? cartoes : []
  } catch (error) {
    console.error("Erro ao buscar cartões:", error)
    return []
  }
}

async function fetchMonths() {
  try {
    const token = buscaTokenJwt()
    if (!token) {
      console.warn("Token não encontrado para buscar meses")
      return []
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.months}?pagina=1&quantidade=100`, {
      method: "GET",
      headers: headers,
    })

    if (!response.ok) {
      console.error(`Erro ao buscar meses: ${response.status} - ${response.statusText}`)
      return []
    }

    const data = await response.json()
    console.log("Resposta completa dos meses:", data)

    const meses = data.meses || data.mesesReferencia || data.data || data || []
    console.log("Meses extraídos:", meses)
    return Array.isArray(meses) ? meses : []
  } catch (error) {
    console.error("Erro ao buscar meses:", error)
    return []
  }
}

// ===== FUNÇÕES AUXILIARES =====
function formatCurrency(value) {
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(value)
}

// ===== FUNÇÃO CORRIGIDA PARA FORMATAÇÃO DE DATA =====
function formatDate(dateString) {
  if (!dateString) return "Data inválida"

  // Extrair apenas a parte da data (YYYY-MM-DD) ignorando horário e timezone
  const dateOnly = dateString.split("T")[0]
  const [year, month, day] = dateOnly.split("-")

  // Criar data local sem problemas de timezone
  const date = new Date(Number.parseInt(year), Number.parseInt(month) - 1, Number.parseInt(day))

  return date.toLocaleDateString("pt-BR")
}

function formatDateForInput(dateString) {
  if (!dateString) return ""

  // Extrair apenas a parte da data (YYYY-MM-DD) ignorando horário
  const dateOnly = dateString.split("T")[0]
  return dateOnly
}

function updateValueDisplay(element, value) {
  if (element) {
    if (showValues) {
      element.textContent = formatCurrency(value)
    } else {
      element.textContent = "••••••"
    }
  }
}

// ===== FUNÇÃO PARA RENDERIZAR DETALHES DAS CONTAS =====
async function renderAccountDetails() {
  const accountDetailsEl = document.getElementById("accountDetails")

  if (!showAccountDetails || !accountDetailsEl) return

  console.log("Renderizando detalhes das contas...")

  const accountsData = await fetchAccountsBalance()
  let html = ""

  if (accountsData && Array.isArray(accountsData)) {
    console.log("Dados das contas:", accountsData)

    accountsData.forEach((account) => {
      html += `
        <div class="account-item">
          <div class="account-info">
            <span class="account-name">${account.nomeConta || account.name || "Conta sem nome"}</span>
            <span class="account-type">${account.tipoConta || account.type || ""}</span>
          </div>
          <div class="account-balance">
            <span class="balance-value ${(account.saldoAtual || account.balance || 0) >= 0 ? "positive" : "negative"}">
              ${showValues ? formatCurrency(account.saldoAtual || account.balance || 0) : "••••••"}
            </span>
          </div>
        </div>
      `
    })
  } else {
    console.error("Falha ao carregar dados das contas da API")
    html = `
      <div class="account-item error">
        <div class="account-info">
          <span class="account-name">Erro ao carregar contas</span>
          <span class="account-type">Verifique sua conexão</span>
        </div>
        <div class="account-balance">
          <span class="balance-value">-</span>
        </div>
      </div>
    `
  }

  const totalBalance = await buscaApiSaldoTotal()
  html += `
    <div class="account-total">
      <div class="account-info">
        <span class="account-name">Total Geral</span>
      </div>
      <div class="account-balance">
        <span class="balance-value total ${totalBalance >= 0 ? "positive" : "negative"}">
          ${showValues ? formatCurrency(totalBalance) : "••••••"}
        </span>
      </div>
    </div>
  `

  accountDetailsEl.innerHTML = html
}

// ===== FUNÇÃO PARA ATUALIZAR DASHBOARD =====
async function updateDashboard() {
  console.log("Atualizando dashboard...")

  const totalBalanceEl = document.getElementById("totalBalance")
  const pendingIncomesEl = document.getElementById("pendingIncomes")
  const pendingExpensesEl = document.getElementById("pendingExpenses")

  try {
    const totalBalance = await buscaApiSaldoTotal()
    if (totalBalance !== null) {
      updateValueDisplay(totalBalanceEl, totalBalance)
    }

    const pendingIncomes = await buscaAPiReceitasPendentes()
    if (pendingIncomes !== null) {
      updateValueDisplay(pendingIncomesEl, pendingIncomes)
    }

    const pendingExpenses = await fetchPendingExpenses()
    if (pendingExpenses !== null) {
      updateValueDisplay(pendingExpensesEl, pendingExpenses)
    }

    if (showAccountDetails) {
      await renderAccountDetails()
    }

    console.log("Dashboard atualizado com sucesso")
  } catch (error) {
    console.error("Erro ao atualizar dashboard:", error)
  }
}

// ===== CLASSE ATUALIZADA PARA GERENCIAR FILTROS E MOVIMENTAÇÕES =====
class MovementsManager {
  constructor() {
    this.filters = {
      search: "",
      specificDate: "",
      month: "",
      card: "",
      category: "",
      account: "",
      status: "",
    }

    this.isFiltersExpanded = false
    this.searchTimeout = null
    this.allTransactions = []
    this.filteredTransactions = []

    this.currentPage = 1
    this.pageSize = 10
    this.totalRecords = 0
    this.totalPages = 0

    this.initializeElements()
    this.bindEvents()
  }

  initializeElements() {
    this.filterToggle = document.getElementById("toggleFilters")
    this.advancedFilters = document.getElementById("advancedFilters")
    this.searchInput = document.getElementById("searchInput")
    this.searchClear = document.getElementById("searchClear")

    this.specificDateInput = document.getElementById("specificDate")
    this.monthSelect = document.getElementById("monthFilter")
    this.cardSelect = document.getElementById("cardFilter")
    this.categorySelect = document.getElementById("categoryFilter")
    this.accountSelect = document.getElementById("accountFilter")
    this.statusSelect = document.getElementById("statusFilter")

    this.applyButton = document.getElementById("applyFilters")
    this.clearButton = document.getElementById("clearFilters")

    this.activeFilters = document.getElementById("activeFilters")
    this.activeFiltersList = document.getElementById("activeFiltersList")
    this.transactionsBody = document.getElementById("transactionsBody")
    this.emptyState = document.getElementById("emptyState")
    this.loadingState = document.getElementById("loadingState")

    this.paginationContainer = document.getElementById("paginationContainer")
    this.paginationInfo = document.getElementById("paginationInfo")
    this.paginationPages = document.getElementById("paginationPages")
    this.firstPageBtn = document.getElementById("firstPageBtn")
    this.prevPageBtn = document.getElementById("prevPageBtn")
    this.nextPageBtn = document.getElementById("nextPageBtn")
    this.lastPageBtn = document.getElementById("lastPageBtn")
    this.pageSizeSelect = document.getElementById("pageSize")
  }

  bindEvents() {
    if (this.filterToggle) {
      this.filterToggle.addEventListener("click", () => {
        this.toggleAdvancedFilters()
      })
    }

    // Pesquisa em tempo real (mas não aplica filtros automaticamente)
    if (this.searchInput) {
      this.searchInput.addEventListener("input", (e) => {
        this.handleSearchInput(e.target.value)
      })
    }

    if (this.searchClear) {
      this.searchClear.addEventListener("click", () => {
        this.clearSearch()
      })
    }

    // Máscara para data específica (sem aplicar filtros automaticamente)
    if (this.specificDateInput) {
      this.specificDateInput.addEventListener("input", (e) => {
        const maskedValue = applyDateMask(e.target.value)
        e.target.value = maskedValue
      })

      this.specificDateInput.addEventListener("blur", (e) => {
        const value = e.target.value
        if (value && value.length === 10) {
          const isoDate = formatDateToISO(value)
          if (!isoDate) {
            alert("Data inválida. Use o formato DD/MM/AAAA")
            e.target.value = ""
          }
        }
      })
    }

    // BOTÃO APLICAR FILTROS
    if (this.applyButton) {
      this.applyButton.addEventListener("click", () => {
        this.applyFilters()
      })
    }

    // Limpar filtros
    if (this.clearButton) {
      this.clearButton.addEventListener("click", () => {
        this.clearAllFilters()
      })
    }

    // Eventos de paginação
    if (this.firstPageBtn) {
      this.firstPageBtn.addEventListener("click", () => this.goToPage(1))
    }

    if (this.prevPageBtn) {
      this.prevPageBtn.addEventListener("click", () => this.goToPage(this.currentPage - 1))
    }

    if (this.nextPageBtn) {
      this.nextPageBtn.addEventListener("click", () => this.goToPage(this.currentPage + 1))
    }

    if (this.lastPageBtn) {
      this.lastPageBtn.addEventListener("click", () => this.goToPage(this.totalPages))
    }

    if (this.pageSizeSelect) {
      this.pageSizeSelect.addEventListener("change", (e) => {
        this.pageSize = Number.parseInt(e.target.value)
        this.totalPages = Math.ceil(this.totalRecords / this.pageSize)
        this.currentPage = 1
        this.renderFilteredTransactions()
        this.updatePagination()
      })
    }
  }

  toggleAdvancedFilters() {
    this.isFiltersExpanded = !this.isFiltersExpanded

    if (this.advancedFilters) {
      if (this.isFiltersExpanded) {
        this.advancedFilters.classList.add("expanded")
        if (this.filterToggle) this.filterToggle.classList.add("active")
      } else {
        this.advancedFilters.classList.remove("expanded")
        if (this.filterToggle) this.filterToggle.classList.remove("active")
      }
    }
  }

  handleSearchInput(value) {
    if (this.searchClear) {
      if (value.length > 0) {
        this.searchClear.style.display = "block"
      } else {
        this.searchClear.style.display = "none"
      }
    }

    // Atualizar o filtro mas não aplicar automaticamente
    this.filters.search = value
  }

  clearSearch() {
    if (this.searchInput) {
      this.searchInput.value = ""
    }
    if (this.searchClear) {
      this.searchClear.style.display = "none"
    }
    this.filters.search = ""
  }

  updateFiltersFromForm() {
    // Atualizar todos os filtros baseado nos valores dos campos
    this.filters.search = this.searchInput?.value || ""
    this.filters.specificDate = this.specificDateInput?.value ? formatDateToISO(this.specificDateInput.value) : ""
    this.filters.month = this.monthSelect?.value || ""
    this.filters.card = this.cardSelect?.value || ""
    this.filters.category = this.categorySelect?.value || ""
    this.filters.account = this.accountSelect?.value || ""
    this.filters.status = this.statusSelect?.value || ""
  }

  applyFilters() {
    console.log("Aplicando filtros localmente...")
    this.updateFiltersFromForm()
    console.log("Filtros coletados:", this.filters)

    this.currentPage = 1
    this.filterTransactionsLocally()
  }

  filterTransactionsLocally() {
    let filtered = [...this.allTransactions]

    // Aplicar filtro de pesquisa
    if (this.filters.search && this.filters.search.trim() !== "") {
      const searchTerm = this.filters.search.toLowerCase()
      filtered = filtered.filter(
        (transaction) =>
          (transaction.titulo || "").toLowerCase().includes(searchTerm) ||
          (transaction.categoriaNome || "").toLowerCase().includes(searchTerm) ||
          (transaction.contaBancariaNome || "").toLowerCase().includes(searchTerm) ||
          (transaction.cartaoNome || "").toLowerCase().includes(searchTerm) ||
          (transaction.formaPagamento || "").toLowerCase().includes(searchTerm),
      )
    }

    // Aplicar filtro de data específica
    if (this.filters.specificDate && this.filters.specificDate.trim() !== "") {
      console.log("Aplicando filtro de data:", this.filters.specificDate)

      filtered = filtered.filter((transaction) => {
        // Extrair apenas a data (YYYY-MM-DD) da movimentação, ignorando horário
        const transactionDateOnly = transaction.dataVencimento.split("T")[0]

        console.log(`Comparando: ${transactionDateOnly} === ${this.filters.specificDate}`)

        // Comparar apenas as datas (YYYY-MM-DD)
        return transactionDateOnly === this.filters.specificDate
      })

      console.log(`Movimentações encontradas para a data ${this.filters.specificDate}:`, filtered.length)
    }

    // Aplicar filtro de mês de referência
    if (this.filters.month && this.filters.month.trim() !== "") {
      const monthId = Number.parseInt(this.filters.month)
      filtered = filtered.filter((transaction) => transaction.mesReferenciaId === monthId)
    }

    // Aplicar filtro de cartão
    if (this.filters.card && this.filters.card.trim() !== "") {
      const cardId = Number.parseInt(this.filters.card)
      filtered = filtered.filter((transaction) => transaction.cartaoId === cardId)
    }

    // Aplicar filtro de categoria
    if (this.filters.category && this.filters.category.trim() !== "") {
      const categoryId = Number.parseInt(this.filters.category)
      filtered = filtered.filter((transaction) => transaction.categoriaId === categoryId)
    }

    // Aplicar filtro de conta
    if (this.filters.account && this.filters.account.trim() !== "") {
      const accountId = Number.parseInt(this.filters.account)
      filtered = filtered.filter((transaction) => transaction.contaBancariaId === accountId)
    }

    // Aplicar filtro de status
    if (this.filters.status && this.filters.status.trim() !== "") {
      filtered = filtered.filter((transaction) => {
        switch (this.filters.status) {
          case "pago":
            return transaction.realizado && transaction.tipo === "Despesa"
          case "recebido":
            return transaction.realizado && transaction.tipo === "Receita"
          case "pendente":
            return !transaction.realizado
          default:
            return true
        }
      })
    }

    this.filteredTransactions = filtered
    this.totalRecords = filtered.length
    this.totalPages = Math.ceil(this.totalRecords / this.pageSize)

    console.log(`Filtros aplicados: ${filtered.length} de ${this.allTransactions.length} movimentações`)

    this.renderFilteredTransactions()
    this.updatePagination()
    this.updateActiveFiltersDisplay()
  }

  renderFilteredTransactions() {
    if (!this.transactionsBody || !this.emptyState) return

    this.transactionsBody.innerHTML = ""

    // Calcular paginação local
    const startIndex = (this.currentPage - 1) * this.pageSize
    const endIndex = startIndex + this.pageSize
    const paginatedTransactions = this.filteredTransactions.slice(startIndex, endIndex)

    if (paginatedTransactions.length === 0) {
      this.emptyState.classList.remove("hidden")
      const hasActiveFilters = Object.values(this.filters).some((filter) => filter !== "")
      const emptyStateTitle = this.emptyState.querySelector("h3")
      const emptyStateText = this.emptyState.querySelector("p")

      if (hasActiveFilters) {
        if (emptyStateTitle) emptyStateTitle.textContent = "Nenhuma movimentação encontrada"
        if (emptyStateText) emptyStateText.textContent = "Nenhuma movimentação encontrada para os filtros aplicados."
      } else {
        if (emptyStateTitle) emptyStateTitle.textContent = "Nenhuma movimentação encontrada"
        if (emptyStateText) emptyStateText.textContent = "Tente ajustar os filtros ou adicionar novas movimentações."
      }
      return
    }

    this.emptyState.classList.add("hidden")

    this.transactionsBody.innerHTML = paginatedTransactions
      .map(
        (transaction) => `
      <tr class="transaction-row-${transaction.tipo && transaction.tipo.toLowerCase() === "despesa" ? "expense" : "income"}">
          <td class="transaction-name">${transaction.titulo || "Sem título"}</td>
          <td>${transaction.mesReferenciaNome || "N/A"}</td>
          <td class="transaction-date">${formatDate(transaction.dataVencimento)}</td>
          <td>
              ${
                transaction.realizado
                  ? `<span class="transaction-status status-completed">
                    ${transaction.tipo === "Despesa" ? "Pago" : "Recebido"}
                  </span>`
                  : `<span class="transaction-status status-pending">
                    Pendente
                  </span>`
              }
          </td>
          <td>${transaction.categoriaNome || "N/A"}</td>
          <td>${transaction.contaBancariaNome || "N/A"}</td>
          <td>${transaction.cartaoNome || "N/A"}</td>
          <td>${transaction.formaPagamento || "N/A"}</td>
          <td>
              <span class="transaction-amount transaction-amount-${transaction.tipo && transaction.tipo.toLowerCase() === "despesa" ? "expense" : "income"}">
                  ${transaction.tipo === "Despesa" ? "-" : "+"} ${formatCurrency(transaction.valor || 0)}
              </span>
          </td>
          <td>
              <button class="transaction-arrow" onclick="openEditModal(${transaction.id})" title="Editar movimentação">
                  <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <path d="M9 18l6-6-6-6"/>
                  </svg>
              </button>
          </td>
      </tr>
    `,
      )
      .join("")
  }

  updateActiveFiltersDisplay() {
    if (!this.activeFilters || !this.activeFiltersList) return

    const activeFilters = []

    if (this.filters.search) {
      activeFilters.push({ key: "search", label: `Pesquisa: "${this.filters.search}"` })
    }

    if (this.filters.specificDate) {
      const brDate = formatDateToBR(this.filters.specificDate)
      activeFilters.push({ key: "specificDate", label: `Data: ${brDate}` })
    }

    if (this.filters.month) {
      const monthSelect = document.getElementById("monthFilter")
      const selectedOption = monthSelect?.querySelector(`option[value="${this.filters.month}"]`)
      const monthName = selectedOption?.textContent || this.filters.month
      activeFilters.push({ key: "month", label: `Mês: ${monthName}` })
    }

    if (this.filters.card) {
      const cardSelect = document.getElementById("cardFilter")
      const selectedOption = cardSelect?.querySelector(`option[value="${this.filters.card}"]`)
      const cardName = selectedOption?.textContent || this.filters.card
      activeFilters.push({ key: "card", label: `Cartão: ${cardName}` })
    }

    if (this.filters.category) {
      const categorySelect = document.getElementById("categoryFilter")
      const selectedOption = categorySelect?.querySelector(`option[value="${this.filters.category}"]`)
      const categoryName = selectedOption?.textContent || this.filters.category
      activeFilters.push({ key: "category", label: `Categoria: ${categoryName}` })
    }

    if (this.filters.account) {
      const accountSelect = document.getElementById("accountFilter")
      const selectedOption = accountSelect?.querySelector(`option[value="${this.filters.account}"]`)
      const accountName = selectedOption?.textContent || this.filters.account
      activeFilters.push({ key: "account", label: `Conta: ${accountName}` })
    }

    if (this.filters.status) {
      const statusLabels = {
        pago: "Pago",
        recebido: "Recebido",
        pendente: "Pendente",
      }
      activeFilters.push({
        key: "status",
        label: `Status: ${statusLabels[this.filters.status] || this.filters.status}`,
      })
    }

    if (activeFilters.length > 0) {
      this.activeFilters.style.display = "flex"
      this.activeFiltersList.innerHTML = activeFilters
        .map(
          (filter) => `
              <div class="filter-tag">
                  ${filter.label}
                  <button class="filter-tag-remove" onclick="window.movementsManager.removeFilter('${filter.key}')">
                      <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                          <line x1="18" y1="6" x2="6" y2="18"></line>
                          <line x1="6" y1="6" x2="18" y2="18"></line>
                      </svg>
                  </button>
              </div>
          `,
        )
        .join("")
    } else {
      this.activeFilters.style.display = "none"
    }
  }

  removeFilter(filterKey) {
    this.filters[filterKey] = ""

    const elementMap = {
      search: this.searchInput,
      specificDate: this.specificDateInput,
      month: this.monthSelect,
      card: this.cardSelect,
      category: this.categorySelect,
      account: this.accountSelect,
      status: this.statusSelect,
    }

    if (elementMap[filterKey]) {
      elementMap[filterKey].value = ""

      if (filterKey === "search" && this.searchClear) {
        this.searchClear.style.display = "none"
      }
    }

    // Aplicar filtros automaticamente quando remover um filtro
    this.applyFilters()
  }

  clearAllFilters() {
    Object.keys(this.filters).forEach((key) => {
      this.filters[key] = ""
    })

    const elements = [
      this.searchInput,
      this.specificDateInput,
      this.monthSelect,
      this.cardSelect,
      this.categorySelect,
      this.accountSelect,
      this.statusSelect,
    ]

    elements.forEach((element) => {
      if (element) element.value = ""
    })

    if (this.searchClear) {
      this.searchClear.style.display = "none"
    }

    // Mostrar todas as movimentações novamente
    this.filteredTransactions = [...this.allTransactions]
    this.totalRecords = this.allTransactions.length
    this.totalPages = Math.ceil(this.totalRecords / this.pageSize)
    this.currentPage = 1

    this.renderFilteredTransactions()
    this.updatePagination()
    this.updateActiveFiltersDisplay()
  }

  showLoading() {
    if (this.loadingState) this.loadingState.classList.remove("hidden")
    if (this.emptyState) this.emptyState.classList.add("hidden")
  }

  hideLoading() {
    if (this.loadingState) this.loadingState.classList.add("hidden")
  }

  async loadTransactions() {
    try {
      console.log(`Carregando todas as movimentações em aberto...`)

      this.showLoading()

      // Sempre carregar TODAS as movimentações em aberto (sem paginação no backend)
      const apiResponse = await fetchTransactionsFromApi(1, 1000) // Buscar muitas para ter todas

      this.allTransactions = apiResponse.movimentacoes || []
      this.filteredTransactions = [...this.allTransactions] // Inicialmente, mostrar todas
      this.totalRecords = this.allTransactions.length
      this.totalPages = Math.ceil(this.totalRecords / this.pageSize)

      console.log(`Carregadas ${this.allTransactions.length} movimentações`)

      this.renderFilteredTransactions()
      await this.loadFilterOptions()
      this.updatePagination()
      this.updateActiveFiltersDisplay()
      this.hideLoading()
    } catch (error) {
      console.error("Erro ao carregar movimentações:", error)
      this.hideLoading()
    }
  }

  goToPage(page) {
    if (page < 1 || page > this.totalPages || page === this.currentPage) return

    console.log(`Navegando para página ${page}`)
    this.currentPage = page
    this.renderFilteredTransactions() // Apenas re-renderizar, não recarregar da API
    this.updatePagination()
  }

  updatePagination() {
    if (!this.paginationContainer) return

    if (this.totalRecords > this.pageSize) {
      this.paginationContainer.style.display = "flex"
    } else {
      this.paginationContainer.style.display = "none"
      return
    }

    const startRecord = (this.currentPage - 1) * this.pageSize + 1
    const endRecord = Math.min(this.currentPage * this.pageSize, this.totalRecords)

    if (this.paginationInfo) {
      this.paginationInfo.textContent = `Mostrando ${startRecord}-${endRecord} de ${this.totalRecords} registros`
    }

    if (this.firstPageBtn) this.firstPageBtn.disabled = this.currentPage === 1
    if (this.prevPageBtn) this.prevPageBtn.disabled = this.currentPage === 1
    if (this.nextPageBtn) this.nextPageBtn.disabled = this.currentPage === this.totalPages
    if (this.lastPageBtn) this.lastPageBtn.disabled = this.currentPage === this.totalPages

    this.renderPageNumbers()

    if (this.pageSizeSelect) {
      this.pageSizeSelect.value = this.pageSize.toString()
    }
  }

  renderPageNumbers() {
    if (!this.paginationPages) return

    const maxVisiblePages = 5
    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2))
    const endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1)

    if (endPage - startPage + 1 < maxVisiblePages) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1)
    }

    let pagesHtml = ""

    for (let i = startPage; i <= endPage; i++) {
      pagesHtml += `
        <button class="pagination-page ${i === this.currentPage ? "active" : ""}" 
                onclick="window.movementsManager.goToPage(${i})">
          ${i}
        </button>
      `
    }

    this.paginationPages.innerHTML = pagesHtml
  }

  async loadFilterOptions() {
    try {
      const [categories, accounts, cards, months] = await Promise.all([
        fetchCategories(),
        fetchAccounts(),
        fetchCards(),
        fetchMonths(),
      ])

      if (this.categorySelect) {
        let categoryHtml = '<option value="">Todas as categorias</option>'
        categories.forEach((category) => {
          categoryHtml += `<option value="${category.id}">${category.nomeCategoria || category.nome}</option>`
        })
        this.categorySelect.innerHTML = categoryHtml
      }

      if (this.accountSelect) {
        let accountHtml = '<option value="">Todas as contas</option>'
        accounts.forEach((account) => {
          accountHtml += `<option value="${account.id}">${account.nomeConta || account.nome}</option>`
        })
        this.accountSelect.innerHTML = accountHtml
      }

      if (this.cardSelect) {
        let cardHtml = '<option value="">Todos os cartões</option>'
        cards.forEach((card) => {
          cardHtml += `<option value="${card.id}">${card.nomeCartao || card.nome}</option>`
        })
        this.cardSelect.innerHTML = cardHtml
      }

      if (this.monthSelect) {
        let monthHtml = '<option value="">Todos os meses</option>'
        months.forEach((month) => {
          monthHtml += `<option value="${month.id}">${month.nomeMes || month.nome}</option>`
        })
        this.monthSelect.innerHTML = monthHtml
      }

      console.log("Filtros carregados com dados da API")
    } catch (error) {
      console.error("Erro ao carregar opções dos filtros:", error)
    }
  }
}

// ===== FUNÇÕES DA MODAL DE EDIÇÃO =====
async function openEditModal(transactionId) {
  try {
    console.log("Abrindo modal de edição para transação:", transactionId)

    const transaction = window.movementsManager.allTransactions.find((t) => t.id === transactionId)

    if (!transaction) {
      alert("Transação não encontrada!")
      return
    }

    currentEditingTransaction = transaction
    console.log("Dados da transação para edição:", transaction)

    await populateEditForm(transaction)

    const modal = document.getElementById("editModalOverlay")
    if (modal) {
      modal.classList.add("active")
      document.body.style.overflow = "hidden"
    }
  } catch (error) {
    console.error("Erro ao abrir modal de edição:", error)
    alert("Erro ao carregar dados da movimentação")
  }
}

async function populateEditForm(transaction) {
  try {
    console.log("Preenchendo formulário com dados:", transaction)

    const [categories, accounts, cards, months] = await Promise.all([
      fetchCategories(),
      fetchAccounts(),
      fetchCards(),
      fetchMonths(),
    ])

    console.log("Dados carregados para selects:", { categories, accounts, cards, months })

    populateSelect("editCategoria", categories, "id", "nomeCategoria", transaction.categoriaId)
    populateSelect("editContaBancaria", accounts, "id", "nomeConta", transaction.contaBancariaId)
    populateSelect("editCartao", cards, "id", "nomeCartao", transaction.cartaoId)
    populateSelect("editMesReferencia", months, "id", "nomeMes", transaction.mesReferenciaId)

    const titleField = document.getElementById("editTitulo")
    const valueField = document.getElementById("editValor")
    const typeField = document.getElementById("editTipo")
    const dateField = document.getElementById("editDataVencimento")
    const paymentMethodField = document.getElementById("editFormaPagamento")
    const completedField = document.getElementById("editRealizado")

    if (titleField) titleField.value = transaction.titulo || ""
    if (valueField) valueField.value = transaction.valor || ""
    if (typeField) typeField.value = transaction.tipo || ""
    if (dateField) dateField.value = formatDateForInput(transaction.dataVencimento)
    if (paymentMethodField) paymentMethodField.value = transaction.formaPagamento || ""
    if (completedField) completedField.checked = transaction.realizado || false

    console.log("Formulário preenchido com sucesso")
  } catch (error) {
    console.error("Erro ao preencher formulário:", error)
    alert("Erro ao carregar dados para edição")
  }
}

function populateSelect(selectId, options, valueField, textField, selectedValue) {
  const select = document.getElementById(selectId)
  if (!select) {
    console.warn(`Select ${selectId} não encontrado`)
    return
  }

  console.log(`Populando select ${selectId} com ${options.length} opções`)

  const firstOption = select.querySelector("option")
  select.innerHTML = ""
  if (firstOption) {
    select.appendChild(firstOption)
  }

  let optionSelected = false
  options.forEach((option) => {
    const optionElement = document.createElement("option")
    optionElement.value = option[valueField]
    optionElement.textContent = option[textField]

    if (option[valueField] === selectedValue) {
      optionElement.selected = true
      optionSelected = true
    }

    select.appendChild(optionElement)
  })

  if (selectedValue && !optionSelected) {
    console.warn(`Valor ${selectedValue} não encontrado nas opções do select ${selectId}`)
  }
}

function closeEditModal() {
  const modal = document.getElementById("editModalOverlay")
  if (modal) {
    modal.classList.remove("active")
    document.body.style.overflow = ""
  }
  currentEditingTransaction = null
}

async function saveTransaction() {
  try {
    if (!currentEditingTransaction) {
      alert("Nenhuma transação selecionada para edição")
      return
    }

    const form = document.getElementById("editTransactionForm")
    const formData = new FormData(form)

    const transactionData = {
      id: currentEditingTransaction.id,
      titulo: formData.get("titulo"),
      valor: Number.parseFloat(formData.get("valor")),
      tipo: formData.get("tipo"),
      dataVencimento: formData.get("dataVencimento"),
      categoriaId: Number.parseInt(formData.get("categoriaId")) || null,
      contaBancariaId: Number.parseInt(formData.get("contaBancariaId")) || null,
      cartaoId: formData.get("cartaoId") ? Number.parseInt(formData.get("cartaoId")) : null,
      formaPagamento: formData.get("formaPagamento") || null,
      mesReferenciaId: Number.parseInt(formData.get("mesReferenciaId")) || null,
      realizado: formData.get("realizado") === "on",
    }

    console.log("Dados para envio:", transactionData)

    if (!transactionData.titulo || !transactionData.valor || !transactionData.tipo) {
      alert("Por favor, preencha todos os campos obrigatórios (título, valor e tipo)")
      return
    }

    if (!transactionData.categoriaId || !transactionData.contaBancariaId || !transactionData.mesReferenciaId) {
      alert("Por favor, selecione categoria, conta bancária e mês de referência")
      return
    }

    await updateTransaction(transactionData)

    closeEditModal()

    await window.movementsManager.loadTransactions()

    alert("Movimentação atualizada com sucesso!")
  } catch (error) {
    console.error("Erro ao salvar transação:", error)
    alert("Erro ao salvar movimentação: " + error.message)
  }
}

// ===== INICIALIZAÇÃO PRINCIPAL =====
document.addEventListener("DOMContentLoaded", async () => {
  console.log("Inicializando aplicação...")

  const token = buscaTokenJwt()
  if (!token) {
    console.warn("Usuário não autenticado, redirecionando para login")
    window.location.replace("login.html")
    return
  }

  // ===== CONFIGURAR MENU VERTICAL =====
  const verticalMenuToggle = document.getElementById("verticalMenuToggle")
  const verticalMenu = document.getElementById("verticalMenu")
  const overlay = document.getElementById("overlay")

  function openVerticalMenu() {
    if (verticalMenu && overlay) {
      verticalMenu.classList.add("active")
      overlay.classList.add("active")
      document.body.style.overflow = "hidden"
    } else {
      console.error("Elementos do menu vertical não encontrados")
    }
  }

  function closeVerticalMenu() {
    if (verticalMenu && overlay) {
      verticalMenu.classList.remove("active")
      overlay.classList.remove("active")
      document.body.style.overflow = ""
    }
  }

  if (verticalMenuToggle) {
    verticalMenuToggle.addEventListener("click", (e) => {
      e.stopPropagation()

      if (verticalMenu && verticalMenu.classList.contains("active")) {
        closeVerticalMenu()
      } else {
        openVerticalMenu()
      }
    })
  }

  if (overlay) {
    overlay.addEventListener("click", () => {
      closeVerticalMenu()
    })
  }

  document.addEventListener("click", (e) => {
    if (
      verticalMenuToggle &&
      verticalMenu &&
      !verticalMenuToggle.contains(e.target) &&
      !verticalMenu.contains(e.target)
    ) {
      closeVerticalMenu()
    }
  })

  document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
      closeVerticalMenu()
      closeEditModal()
    }
  })

  // ===== CONFIGURAR LOGOUT =====
  const logoutBtn = document.querySelector(".logout-btn")
  if (logoutBtn) {
    logoutBtn.addEventListener("click", () => {
      localStorage.removeItem("authToken")
      sessionStorage.clear()
      window.location.replace("login.html")
    })
  }

  // ===== CONFIGURAR TOGGLES DE VISIBILIDADE =====
  const toggleValuesButton = document.getElementById("toggleValues")
  if (toggleValuesButton) {
    toggleValuesButton.addEventListener("click", () => {
      showValues = !showValues
      const eyeIcon = document.getElementById("eyeIcon")
      const eyeSlashIcon = document.getElementById("eyeSlashIcon")
      if (eyeIcon) eyeIcon.classList.toggle("hidden")
      if (eyeSlashIcon) eyeSlashIcon.classList.toggle("hidden")
      updateDashboard()
    })
  }

  const toggleEyeButtons = document.querySelectorAll(".toggle-eye")
  toggleEyeButtons.forEach((button) => {
    button.addEventListener("click", function () {
      const icons = this.querySelectorAll(".eye-icon")
      icons.forEach((icon) => icon.classList.toggle("hidden"))
      showValues = !showValues
      updateDashboard()
    })
  })

  // ===== CONFIGURAR BOTÃO "MOSTRAR POR CONTA" =====
  const toggleAccountDetailsBtn = document.getElementById("toggleAccountDetails")
  const accountSummaryEl = document.getElementById("accountSummary")
  const accountDetailsEl = document.getElementById("accountDetails")

  if (toggleAccountDetailsBtn) {
    toggleAccountDetailsBtn.addEventListener("click", async function () {
      showAccountDetails = !showAccountDetails

      console.log("Toggle account details:", showAccountDetails)

      if (showAccountDetails) {
        if (accountSummaryEl) accountSummaryEl.classList.add("hidden")
        if (accountDetailsEl) accountDetailsEl.classList.remove("hidden")
        this.textContent = "Ocultar detalhes"
        await renderAccountDetails()
      } else {
        if (accountSummaryEl) accountSummaryEl.classList.remove("hidden")
        if (accountDetailsEl) accountDetailsEl.classList.add("hidden")
        this.textContent = "Mostrar por conta"
      }
    })
  }

  // ===== CONFIGURAR MODAL DE EDIÇÃO =====
  const closeModalBtn = document.getElementById("closeEditModal")
  const cancelModalBtn = document.getElementById("cancelEditModal")
  const editForm = document.getElementById("editTransactionForm")

  if (closeModalBtn) {
    closeModalBtn.addEventListener("click", closeEditModal)
  }

  if (cancelModalBtn) {
    cancelModalBtn.addEventListener("click", closeEditModal)
  }

  if (editForm) {
    editForm.addEventListener("submit", (e) => {
      e.preventDefault()
      saveTransaction()
    })
  }

  const modalOverlay = document.getElementById("editModalOverlay")
  if (modalOverlay) {
    modalOverlay.addEventListener("click", (e) => {
      if (e.target === modalOverlay) {
        closeEditModal()
      }
    })
  }

  // ===== INICIALIZAR SISTEMA DE FILTROS =====
  const movementsManager = new MovementsManager()
  window.movementsManager = movementsManager

  // ===== CARREGAR DADOS INICIAIS =====
  try {
    console.log("Carregando dados iniciais...")
    await updateDashboard()
    await movementsManager.loadTransactions() // Carrega SEM filtros inicialmente
    console.log("Aplicação inicializada com sucesso!")
  } catch (error) {
    console.error("Erro ao inicializar aplicação:", error)
  }
})

// ===== FUNÇÕES GLOBAIS =====
function editTransaction(id) {
  openEditModal(id)
}

function refreshDashboard() {
  console.log("Atualizando dashboard...")
  updateDashboard()
  if (window.movementsManager) {
    window.movementsManager.loadTransactions()
  }
}

function exportTransactions() {
  console.log("Exportar transações...")
  alert("Funcionalidade de exportação será implementada")
}

console.log("Dashboard JavaScript carregado com sucesso! 🚀")
