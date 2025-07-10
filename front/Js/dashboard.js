// ===== VARIÁVEIS GLOBAIS =====
let apiAccountsData = null
let apiSaldoTotal = 0
let apiPendingIncomes = 0
let apiPendingExpenses = 0
let showValues = true
let showAccountDetails = false
let currentEditingTransaction = null

// ===== SISTEMA DE NOTIFICAÇÕES =====
class NotificationSystem {
  constructor() {
    this.container = null
    this.notifications = []
    this.init()
  }

  init() {
    // Criar container de notificações se não existir
    this.container = document.querySelector(".notification-container")
    if (!this.container) {
      this.container = document.createElement("div")
      this.container.className = "notification-container"
      document.body.appendChild(this.container)
    }
  }

  show(type, title, message, duration = 5000) {
    const notification = this.createNotification(type, title, message, duration)
    this.container.appendChild(notification)
    this.notifications.push(notification)

    // Trigger animation
    setTimeout(() => {
      notification.classList.add("show")
    }, 100)

    // Auto remove
    if (duration > 0) {
      this.startProgressBar(notification, duration)
      setTimeout(() => {
        this.remove(notification)
      }, duration)
    }

    return notification
  }

  createNotification(type, title, message, duration) {
    const notification = document.createElement("div")
    notification.className = `notification ${type}`

    const icons = {
      success: `<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"></path>
        <polyline points="22,4 12,14.01 9,11.01"></polyline>
      </svg>`,
      error: `<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <circle cx="12" cy="12" r="10"></circle>
        <line x1="15" y1="9" x2="9" y2="15"></line>
        <line x1="9" y1="9" x2="15" y2="15"></line>
      </svg>`,
      warning: `<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"></path>
        <line x1="12" y1="9" x2="12" y2="13"></line>
        <line x1="12" y1="17" x2="12.01" y2="17"></line>
      </svg>`,
      info: `<svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <circle cx="12" cy="12" r="10"></circle>
        <line x1="12" y1="16" x2="12" y2="12"></line>
        <line x1="12" y1="8" x2="12.01" y2="8"></line>
      </svg>`,
    }

    notification.innerHTML = `
      <div class="notification-icon">
        ${icons[type] || icons.info}
      </div>
      <div class="notification-content">
        <h4 class="notification-title">${title}</h4>
        <p class="notification-message">${message}</p>
      </div>
      <button class="notification-close">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <line x1="18" y1="6" x2="6" y2="18"></line>
          <line x1="6" y1="6" x2="18" y2="18"></line>
        </svg>
      </button>
      ${duration > 0 ? '<div class="notification-progress"><div class="notification-progress-bar"></div></div>' : ""}
    `

    // Event listener para fechar
    const closeBtn = notification.querySelector(".notification-close")
    closeBtn.addEventListener("click", () => {
      this.remove(notification)
    })

    return notification
  }

  startProgressBar(notification, duration) {
    const progressBar = notification.querySelector(".notification-progress-bar")
    if (progressBar) {
      progressBar.style.transitionDuration = `${duration}ms`
      setTimeout(() => {
        progressBar.style.transform = "translateX(0)"
      }, 100)
    }
  }

  remove(notification) {
    notification.classList.add("hide")
    setTimeout(() => {
      if (notification.parentNode) {
        notification.parentNode.removeChild(notification)
      }
      const index = this.notifications.indexOf(notification)
      if (index > -1) {
        this.notifications.splice(index, 1)
      }
    }, 400)
  }

  success(title, message, duration = 5000) {
    return this.show("success", title, message, duration)
  }

  error(title, message, duration = 7000) {
    return this.show("error", title, message, duration)
  }

  warning(title, message, duration = 6000) {
    return this.show("warning", title, message, duration)
  }

  info(title, message, duration = 5000) {
    return this.show("info", title, message, duration)
  }
}

// Instância global do sistema de notificações
const notifications = new NotificationSystem()

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
    // Endpoint para movimentações com paginação e filtros
    transactions: "/dashboard/movimentacoes-dashboard",
    // Endpoint para atualizar movimentação
    updateTransaction: "/movimentacoes",
    // Endpoints para dados dos selects
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

// ===== FUNÇÃO PARA BUSCAR MOVIMENTAÇÕES COM PAGINAÇÃO E FILTROS DA API =====
async function fetchTransactionsFromApi(pagina = 1, quantidadePorPagina = 10, filtros = {}) {
  try {
    const token = buscaTokenJwt()
    if (!token) {
      console.warn("Usuário não autenticado, token não encontrado")
      return { movimentacoes: [], total: 0, pagina: 1, quantidade: quantidadePorPagina }
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    // Construir parâmetros da URL - APENAS PAGINAÇÃO
    const params = new URLSearchParams({
      pagina: pagina.toString(),
      quantidadePorPagina: quantidadePorPagina.toString(),
    })

    console.log("Buscando dados da API sem filtros (apenas paginação)")

    const url = `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.transactions}?${params.toString()}`
    console.log("URL completa da requisição:", url)

    const response = await fetch(url, {
      method: "GET",
      headers: headers,
    })

    if (!response.ok) {
      const errorBody = await response.text()
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`)
    }

    const data = await response.json()
    console.log("Dados recebidos da API:", data)

    // Verificar se a resposta tem a estrutura esperada
    if (!data.movimentacoes || !Array.isArray(data.movimentacoes)) {
      console.warn("Estrutura de dados inesperada:", data)
      return { movimentacoes: [], total: 0, pagina: 1, quantidade: quantidadePorPagina }
    }

    return {
      movimentacoes: data.movimentacoes,
      total: data.total || 0,
      pagina: data.pagina || pagina,
      quantidade: data.quantidade || quantidadePorPagina,
    }
  } catch (error) {
    console.error("Erro ao buscar movimentações da API:", error.message)
    return { movimentacoes: [], total: 0, pagina: 1, quantidade: quantidadePorPagina }
  }
}

// ===== FUNÇÃO PARA ATUALIZAR MOVIMENTAÇÃO =====
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
    console.log(
      "URL da requisição:",
      `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.updateTransaction}/${transactionData.id}`,
    )

    // Preparar dados no formato correto para o backend
    const requestData = {
      titulo: transactionData.titulo,
      valor: transactionData.valor,
      tipo: transactionData.tipo,
      dataVencimento: transactionData.dataVencimento,
      categoriaId: transactionData.categoriaId,
      contaBancariaId: transactionData.contaBancariaId,
      cartaoId: transactionData.cartaoId,
      formaDePagamento: transactionData.formaDePagamento,
      mesReferenciaId: transactionData.mesReferenciaId,
      realizado: transactionData.realizado,
    }

    if (requestData.cartaoId == "") {
      console.log("Cartão não selecionado, removendo campo cartaoId")
    }

    console.log("Dados formatados para envio:", requestData)

    const response = await fetch(
      `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.updateTransaction}/${transactionData.id}`,
      {
        method: "PUT",
        headers: headers,
        body: JSON.stringify(requestData),
      },
    )

    console.log("Status da resposta:", response.status)

    if (!response.ok) {
      const errorBody = await response.text()
      console.error("Erro na resposta:", errorBody)
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`)
    }

    // Verificar se há conteúdo na resposta
    const contentType = response.headers.get("content-type")
    let data = null

    if (contentType && contentType.includes("application/json")) {
      data = await response.json()
    } else {
      // Se não há JSON, considerar sucesso se status é 200-299
      data = { success: true, message: "Movimentação atualizada com sucesso" }
    }

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

// ===== CLASSE PARA GERENCIAR MOVIMENTAÇÕES COM PAGINAÇÃO DA API =====
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
    this.currentTransactions = []
    this.allTransactions = [] // Nova propriedade para armazenar todos os dados

    // Configurações de paginação
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

    // CORREÇÃO 1: Pesquisa em tempo real com debounce
    if (this.searchInput) {
      this.searchInput.addEventListener("input", (e) => {
        this.handleSearchInput(e.target.value)
      })

      // Aplicar filtros ao pressionar Enter
      this.searchInput.addEventListener("keydown", (e) => {
        if (e.key === "Enter") {
          e.preventDefault()
          this.applyFilters()
        }
      })
    }

    if (this.searchClear) {
      this.searchClear.addEventListener("click", () => {
        this.clearSearch()
      })
    }

    // Máscara para data específica
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
            notifications.error("Data Inválida", "Use o formato DD/MM/AAAA")
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

    // CORREÇÃO 2: Mudança no tamanho da página com log para debug
    if (this.pageSizeSelect) {
      this.pageSizeSelect.addEventListener("change", (e) => {
        const newPageSize = Number.parseInt(e.target.value)
        console.log(`Mudando tamanho da página de ${this.pageSize} para ${newPageSize}`)

        this.pageSize = newPageSize
        this.currentPage = 1 // Voltar para primeira página

        console.log(`Novo pageSize definido: ${this.pageSize}`)

        // Recarregar com novo tamanho
        this.loadTransactions()
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

  // CORREÇÃO 1: Implementar busca com debounce para aplicar automaticamente
  handleSearchInput(value) {
    console.log("Valor de pesquisa recebido:", value)

    if (this.searchClear) {
      if (value.length > 0) {
        this.searchClear.style.display = "block"
      } else {
        this.searchClear.style.display = "none"
      }
    }

    // Atualizar o filtro de pesquisa
    this.filters.search = value?.trim() || ""
    console.log("Filtro de pesquisa atualizado para:", this.filters.search)

    // Aplicar todos os filtros no frontend
    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout)
    }

    this.searchTimeout = setTimeout(() => {
      console.log("Aplicando todos os filtros no frontend")
      this.applyAllFrontendFilters()
      this.renderTransactions()
      this.updateActiveFiltersDisplay()
    }, 300)
  }

  clearSearch() {
    if (this.searchInput) {
      this.searchInput.value = ""
    }
    if (this.searchClear) {
      this.searchClear.style.display = "none"
    }
    this.filters.search = ""

    // Aplicar filtros automaticamente após limpar
    this.currentPage = 1
    this.loadTransactions()
  }

  updateFiltersFromForm() {
    // Log antes de atualizar os filtros
    console.log("Atualizando filtros do formulário...")
    console.log("Valor atual do searchInput:", this.searchInput?.value)

    // Atualizar todos os filtros baseado nos valores dos campos
    this.filters.search = this.searchInput?.value?.trim() || ""
    this.filters.specificDate = this.specificDateInput?.value ? formatDateToISO(this.specificDateInput.value) : ""
    this.filters.month = this.monthSelect?.value || ""
    this.filters.card = this.cardSelect?.value || ""
    this.filters.category = this.categorySelect?.value || ""
    this.filters.account = this.accountSelect?.value || ""
    this.filters.status = this.statusSelect?.value || ""

    // Log após atualizar os filtros
    console.log("Filtros atualizados:", this.filters)
  }

  async applyFilters() {
    console.log("Aplicando filtros e recarregando da API...")
    this.updateFiltersFromForm()
    console.log("Filtros coletados:", this.filters)

    this.currentPage = 1 // Voltar para primeira página ao aplicar filtros
    await this.loadTransactions()
  }

  async loadTransactions() {
    try {
      console.log(`Carregando movimentações - Página: ${this.currentPage}, Tamanho: ${this.pageSize}`)
      console.log("Filtros que serão aplicados no frontend:", this.filters)

      this.showLoading()

      // Buscar da API sem nenhum filtro (apenas paginação)
      const apiResponse = await fetchTransactionsFromApi(this.currentPage, this.pageSize, {})

      this.allTransactions = apiResponse.movimentacoes || []

      // Aplicar TODOS os filtros no frontend
      this.applyAllFrontendFilters()

      this.totalRecords = apiResponse.total || 0
      this.totalPages = Math.ceil(this.totalRecords / this.pageSize)
      this.currentPage = apiResponse.pagina || this.currentPage

      console.log(`Exibindo ${this.currentTransactions.length} movimentações de ${this.totalRecords} total`)

      this.renderTransactions()
      await this.loadFilterOptions()
      this.updatePagination()
      this.updateActiveFiltersDisplay()
      this.hideLoading()
    } catch (error) {
      console.error("Erro ao carregar movimentações:", error)
      this.hideLoading()
      notifications.error("Erro", "Erro ao carregar movimentações")
    }
  }

  applyAllFrontendFilters() {
    let filteredTransactions = this.allTransactions || []
    console.log(`Aplicando filtros no frontend. Total de registros: ${filteredTransactions.length}`)

    // Filtro de pesquisa (texto)
    if (this.filters.search && this.filters.search.trim() !== "") {
      const searchTerm = this.filters.search.toLowerCase().trim()
      filteredTransactions = filteredTransactions.filter((transaction) => {
        return (
          (transaction.titulo && transaction.titulo.toLowerCase().includes(searchTerm)) ||
          (transaction.categoriaNome && transaction.categoriaNome.toLowerCase().includes(searchTerm)) ||
          (transaction.contaBancariaNome && transaction.contaBancariaNome.toLowerCase().includes(searchTerm)) ||
          (transaction.cartaoNome && transaction.cartaoNome.toLowerCase().includes(searchTerm)) ||
          (transaction.formaDePagamento && transaction.formaDePagamento.toLowerCase().includes(searchTerm)) ||
          (transaction.valor && transaction.valor.toString().includes(searchTerm))
        )
      })
      console.log(`Filtro de pesquisa aplicado: ${filteredTransactions.length} registros restantes`)
    }

    // Filtro de data específica
    if (this.filters.specificDate && this.filters.specificDate.trim() !== "") {
      filteredTransactions = filteredTransactions.filter((transaction) => {
        if (!transaction.dataVencimento) return false
        const transactionDate = transaction.dataVencimento.split("T")[0] // YYYY-MM-DD
        return transactionDate === this.filters.specificDate
      })
      console.log(`Filtro de data específica aplicado: ${filteredTransactions.length} registros restantes`)
    }

    // Filtro de mês de referência
    if (this.filters.month && this.filters.month.trim() !== "") {
      const monthId = Number.parseInt(this.filters.month)
      filteredTransactions = filteredTransactions.filter((transaction) => {
        return transaction.mesReferenciaId === monthId
      })
      console.log(`Filtro de mês aplicado: ${filteredTransactions.length} registros restantes`)
    }

    // Filtro de cartão
    if (this.filters.card && this.filters.card.trim() !== "") {
      const cardId = Number.parseInt(this.filters.card)
      filteredTransactions = filteredTransactions.filter((transaction) => {
        return transaction.cartaoId === cardId
      })
      console.log(`Filtro de cartão aplicado: ${filteredTransactions.length} registros restantes`)
    }

    // Filtro de categoria
    if (this.filters.category && this.filters.category.trim() !== "") {
      const categoryId = Number.parseInt(this.filters.category)
      filteredTransactions = filteredTransactions.filter((transaction) => {
        return transaction.categoriaId === categoryId
      })
      console.log(`Filtro de categoria aplicado: ${filteredTransactions.length} registros restantes`)
    }

    // Filtro de conta bancária
    if (this.filters.account && this.filters.account.trim() !== "") {
      const accountId = Number.parseInt(this.filters.account)
      filteredTransactions = filteredTransactions.filter((transaction) => {
        return transaction.contaBancariaId === accountId
      })
      console.log(`Filtro de conta aplicado: ${filteredTransactions.length} registros restantes`)
    }

    // Filtro de status
    if (this.filters.status && this.filters.status.trim() !== "") {
      filteredTransactions = filteredTransactions.filter((transaction) => {
        const status = this.filters.status.toLowerCase()
        if (status === "pendente") {
          return !transaction.realizado
        } else if (status === "pago" || status === "recebido") {
          return transaction.realizado
        }
        return true
      })
      console.log(`Filtro de status aplicado: ${filteredTransactions.length} registros restantes`)
    }

    this.currentTransactions = filteredTransactions
    console.log(`Total final após todos os filtros: ${this.currentTransactions.length} registros`)
  }

  // Modificar o método handleSearchInput para usar o novo método:

  handleSearchInput(value) {
    console.log("Valor de pesquisa recebido:", value)

    if (this.searchClear) {
      if (value.length > 0) {
        this.searchClear.style.display = "block"
      } else {
        this.searchClear.style.display = "none"
      }
    }

    // Atualizar o filtro de pesquisa
    this.filters.search = value?.trim() || ""
    console.log("Filtro de pesquisa atualizado para:", this.filters.search)

    // Aplicar todos os filtros no frontend
    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout)
    }

    this.searchTimeout = setTimeout(() => {
      console.log("Aplicando todos os filtros no frontend")
      this.applyAllFrontendFilters()
      this.renderTransactions()
      this.updateActiveFiltersDisplay()
    }, 300)
  }

  renderTransactions() {
    if (!this.transactionsBody || !this.emptyState) return

    this.transactionsBody.innerHTML = ""

    if (this.currentTransactions.length === 0) {
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

    this.transactionsBody.innerHTML = this.currentTransactions
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
          <td>${transaction.formaDePagamento || "N/A"}</td>
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

  async removeFilter(filterKey) {
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
    await this.applyFilters()
  }

  async clearAllFilters() {
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

    // Recarregar sem filtros
    this.currentPage = 1
    await this.loadTransactions()
  }

  showLoading() {
    if (this.loadingState) this.loadingState.classList.remove("hidden")
    if (this.emptyState) this.emptyState.classList.add("hidden")
  }

  hideLoading() {
    if (this.loadingState) this.loadingState.classList.add("hidden")
  }

  async goToPage(page) {
    if (page < 1 || page > this.totalPages || page === this.currentPage) return

    console.log(`Navegando para página ${page}`)
    this.currentPage = page
    await this.loadTransactions() // Recarregar da API
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

    // CORREÇÃO 2: Garantir que o select mostre o valor correto
    if (this.pageSizeSelect) {
      this.pageSizeSelect.value = this.pageSize.toString()
      console.log(`Select de paginação atualizado para: ${this.pageSize}`)
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

    const transaction = window.movementsManager.currentTransactions.find((t) => t.id === transactionId)

    if (!transaction) {
      notifications.error("Erro", "Transação não encontrada!")
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
    notifications.error("Erro", "Erro ao carregar dados da movimentação")
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
    const paymentMethodField = document.getElementById("editformaDePagamento")
    const completedField = document.getElementById("editRealizado")

    if (titleField) titleField.value = transaction.titulo || ""
    if (valueField) valueField.value = transaction.valor || ""
    if (typeField) typeField.value = transaction.tipo || ""
    if (dateField) dateField.value = formatDateForInput(transaction.dataVencimento)
    if (paymentMethodField) paymentMethodField.value = transaction.formaDePagamento || ""
    if (completedField) completedField.checked = transaction.realizado || false

    console.log("Formulário preenchido com sucesso")
  } catch (error) {
    console.error("Erro ao preencher formulário:", error)
    notifications.error("Erro", "Erro ao carregar dados para edição")
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
      notifications.error("Erro", "Nenhuma transação selecionada para edição")
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
      formaDePagamento: formData.get("formaDePagamento") || null,
      mesReferenciaId: Number.parseInt(formData.get("mesReferenciaId")) || null,
      realizado: formData.get("realizado") === "on",
    }

    console.log("Dados para envio:", transactionData)

    if (!transactionData.titulo || !transactionData.valor || !transactionData.tipo) {
      notifications.error(
        "Campos Obrigatórios",
        "Por favor, preencha todos os campos obrigatórios (título, valor e tipo)",
      )
      return
    }

    if (!transactionData.categoriaId || !transactionData.contaBancariaId || !transactionData.mesReferenciaId) {
      notifications.error("Campos Obrigatórios", "Por favor, selecione categoria, conta bancária e mês de referência")
      return
    }

    // Mostrar notificação de carregamento
    const loadingNotification = notifications.info("Salvando...", "Atualizando movimentação, aguarde...", 0)

    await updateTransaction(transactionData)

    // Remover notificação de carregamento
    notifications.remove(loadingNotification)

    closeEditModal()

    // Recarregar movimentações e dashboard
    await window.movementsManager.loadTransactions()
    await updateDashboard()

    // Mostrar notificação de sucesso
    notifications.success("Sucesso!", "Movimentação atualizada com sucesso!")
  } catch (error) {
    console.error("Erro ao salvar transação:", error)
    notifications.error("Erro ao Salvar", `Erro ao salvar movimentação: ${error.message}`)
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

  // ===== INICIALIZAR SISTEMA DE MOVIMENTAÇÕES =====
  const movementsManager = new MovementsManager()
  window.movementsManager = movementsManager

  // ===== CARREGAR DADOS INICIAIS =====
  try {
    console.log("Carregando dados iniciais...")
    await updateDashboard()
    await movementsManager.loadTransactions() // Carrega com paginação da API
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
  notifications.info("Em Desenvolvimento", "Funcionalidade de exportação será implementada em breve")
}

console.log("Dashboard JavaScript com correções aplicadas! 🚀")
