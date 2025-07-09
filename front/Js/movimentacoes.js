// ===== VARIÁVEIS GLOBAIS =====
let currentEditingTransaction = null
let currentDeletingTransaction = null

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
    // Endpoint para buscar movimentações com paginação e filtros
    allTransactions: "/dashboard/movimentacoes-dashboard",
    // Endpoint para criar nova movimentação
    createTransaction: "/movimentacoes",
    // Endpoint para atualizar movimentação
    updateTransaction: "/movimentacoes",
    // Endpoint para deletar movimentação
    deleteTransaction: "/movimentacoes",
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

// Função para buscar movimentações com paginação e filtros da API
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

    // Construir parâmetros da URL
    const params = new URLSearchParams({
      pagina: pagina.toString(),
      quantidadePorPagina: quantidadePorPagina.toString(),
    })

    // Adicionar filtros aos parâmetros se existirem
    if (filtros.search && filtros.search.trim() !== "") {
      params.append("pesquisa", filtros.search.trim())
    }

    if (filtros.specificDate && filtros.specificDate.trim() !== "") {
      params.append("dataEspecifica", filtros.specificDate.trim())
    }

    if (filtros.month && filtros.month.trim() !== "") {
      params.append("mesReferenciaId", filtros.month.trim())
    }

    if (filtros.type && filtros.type.trim() !== "") {
      params.append("tipo", filtros.type.trim())
    }

    if (filtros.card && filtros.card.trim() !== "") {
      params.append("cartaoId", filtros.card.trim())
    }

    if (filtros.category && filtros.category.trim() !== "") {
      params.append("categoriaId", filtros.category.trim())
    }

    if (filtros.account && filtros.account.trim() !== "") {
      params.append("contaBancariaId", filtros.account.trim())
    }

    if (filtros.status && filtros.status.trim() !== "") {
      params.append("status", filtros.status.trim())
    }

    const url = `${API_CONFIG.baseUrl}${API_CONFIG.endpoints.allTransactions}?${params.toString()}`
    console.log("Buscando movimentações da URL:", url)

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

// Função para criar nova movimentação
async function createTransaction(transactionData) {
  try {
    const token = buscaTokenJwt()
    if (!token) {
      throw new Error("Usuário não autenticado")
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    console.log("Criando nova movimentação:", transactionData)

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.createTransaction}`, {
      method: "POST",
      headers: headers,
      body: JSON.stringify(transactionData),
    })

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
      data = { success: true, message: "Movimentação criada com sucesso" }
    }

    console.log("Movimentação criada com sucesso:", data)
    return data
  } catch (error) {
    console.error("Erro ao criar movimentação:", error.message)
    throw error
  }
}

// Função para atualizar movimentação
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

// Função para deletar movimentação
async function deleteTransaction(transactionId) {
  try {
    const token = buscaTokenJwt()
    if (!token) {
      throw new Error("Usuário não autenticado")
    }

    const headers = {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    }

    console.log("Deletando movimentação ID:", transactionId)

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.deleteTransaction}/${transactionId}`, {
      method: "DELETE",
      headers: headers,
    })

    console.log("Status da resposta:", response.status)

    if (!response.ok) {
      const errorBody = await response.text()
      console.error("Erro na resposta:", errorBody)
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`)
    }

    console.log("Movimentação deletada com sucesso")
    return { success: true, message: "Movimentação deletada com sucesso" }
  } catch (error) {
    console.error("Erro ao deletar movimentação:", error.message)
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

// ===== CLASSE PARA GERENCIAR MOVIMENTAÇÕES COM PAGINAÇÃO DA API =====
class MovementsManager {
  constructor() {
    this.filters = {
      search: "",
      specificDate: "",
      month: "",
      type: "",
      card: "",
      category: "",
      account: "",
      status: "",
    }

    this.isFiltersExpanded = false
    this.searchTimeout = null
    this.currentTransactions = []

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
    this.typeSelect = document.getElementById("typeFilter")
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

      // NOVO: Aplicar filtros ao pressionar Enter
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

    // Mudança no tamanho da página
    if (this.pageSizeSelect) {
      this.pageSizeSelect.addEventListener("change", (e) => {
        this.pageSize = Number.parseInt(e.target.value)
        this.currentPage = 1 // Voltar para primeira página
        this.loadTransactions() // Recarregar com novo tamanho
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
    this.filters.type = this.typeSelect?.value || ""
    this.filters.card = this.cardSelect?.value || ""
    this.filters.category = this.categorySelect?.value || ""
    this.filters.account = this.accountSelect?.value || ""
    this.filters.status = this.statusSelect?.value || ""
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
      console.log("Filtros aplicados:", this.filters)

      this.showLoading()

      // Buscar movimentações da API com paginação e filtros
      const apiResponse = await fetchTransactionsFromApi(this.currentPage, this.pageSize, this.filters)

      this.currentTransactions = apiResponse.movimentacoes || []
      this.totalRecords = apiResponse.total || 0
      this.totalPages = Math.ceil(this.totalRecords / this.pageSize)
      this.currentPage = apiResponse.pagina || this.currentPage

      console.log(`Carregadas ${this.currentTransactions.length} movimentações de ${this.totalRecords} total`)

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
          <td class="transaction-type">${transaction.mesReferenciaNome || "N/A"}</td>
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
              <div class="table-actions">
                  <button class="action-btn action-btn-edit" onclick="openEditModal(${transaction.id})" title="Editar movimentação">
                      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                          <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                          <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                      </svg>
                  </button>
                  <button class="action-btn action-btn-delete" onclick="openDeleteModal(${transaction.id}, '${transaction.titulo}')" title="Excluir movimentação">
                      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                          <path d="M3 6h18l-2 13H5L3 6z"></path>
                          <path d="M8 6V4a2 2 0 012-2h4a2 2 0 012 2v2"></path>
                      </svg>
                  </button>
              </div>
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

    if (this.filters.type) {
      activeFilters.push({ key: "type", label: `Tipo: ${this.filters.type}` })
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
      type: this.typeSelect,
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
      this.typeSelect,
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

// ===== FUNÇÕES DAS MODAIS =====

// Modal de Nova Movimentação
async function openNewMovementModal() {
  try {
    console.log("Abrindo modal de nova movimentação")

    await populateNewMovementForm()

    const modal = document.getElementById("newMovementModalOverlay")
    if (modal) {
      modal.classList.add("active")
      document.body.style.overflow = "hidden"
    }
  } catch (error) {
    console.error("Erro ao abrir modal de nova movimentação:", error)
    notifications.error("Erro", "Erro ao carregar formulário de nova movimentação")
  }
}

async function populateNewMovementForm() {
  try {
    const [categories, accounts, cards, months] = await Promise.all([
      fetchCategories(),
      fetchAccounts(),
      fetchCards(),
      fetchMonths(),
    ])

    populateSelect("newCategoria", categories, "id", "nomeCategoria")
    populateSelect("newContaBancaria", accounts, "id", "nomeConta")
    populateSelect("newCartao", cards, "id", "nomeCartao")
    populateSelect("newMesReferencia", months, "id", "nomeMes")

    console.log("Formulário de nova movimentação preenchido com sucesso")
  } catch (error) {
    console.error("Erro ao preencher formulário de nova movimentação:", error)
    notifications.error("Erro", "Erro ao carregar dados para nova movimentação")
  }
}

function closeNewMovementModal() {
  const modal = document.getElementById("newMovementModalOverlay")
  if (modal) {
    modal.classList.remove("active")
    document.body.style.overflow = ""
  }

  // Limpar formulário
  const form = document.getElementById("newMovementForm")
  if (form) {
    form.reset()
  }
}

async function saveNewMovement() {
  try {
    const form = document.getElementById("newMovementForm")
    const formData = new FormData(form)

    const transactionData = {
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

    console.log("Dados para criação:", transactionData)

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
    const loadingNotification = notifications.info("Criando...", "Criando nova movimentação, aguarde...", 0)

    await createTransaction(transactionData)

    // Remover notificação de carregamento
    notifications.remove(loadingNotification)

    closeNewMovementModal()

    // Recarregar movimentações
    await window.movementsManager.loadTransactions()

    // Mostrar notificação de sucesso
    notifications.success("Sucesso!", "Movimentação criada com sucesso!")
  } catch (error) {
    console.error("Erro ao criar movimentação:", error)
    notifications.error("Erro ao Criar", `Erro ao criar movimentação: ${error.message}`)
  }
}

// Modal de Edição
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
    const paymentMethodField = document.getElementById("editFormaDePagamento")
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

    // Recarregar movimentações
    await window.movementsManager.loadTransactions()

    // Mostrar notificação de sucesso
    notifications.success("Sucesso!", "Movimentação atualizada com sucesso!")
  } catch (error) {
    console.error("Erro ao salvar transação:", error)
    notifications.error("Erro ao Salvar", `Erro ao salvar movimentação: ${error.message}`)
  }
}

// Modal de Exclusão
function openDeleteModal(transactionId, transactionTitle) {
  try {
    console.log("Abrindo modal de exclusão para transação:", transactionId)

    const transaction = window.movementsManager.currentTransactions.find((t) => t.id === transactionId)

    if (!transaction) {
      notifications.error("Erro", "Transação não encontrada!")
      return
    }

    currentDeletingTransaction = transaction

    const titleElement = document.getElementById("deleteMovementTitle")
    if (titleElement) {
      titleElement.textContent = transactionTitle || transaction.titulo || "Movimentação sem título"
    }

    const modal = document.getElementById("deleteModalOverlay")
    if (modal) {
      modal.classList.add("active")
      document.body.style.overflow = "hidden"
    }
  } catch (error) {
    console.error("Erro ao abrir modal de exclusão:", error)
    notifications.error("Erro", "Erro ao abrir modal de exclusão")
  }
}

function closeDeleteModal() {
  const modal = document.getElementById("deleteModalOverlay")
  if (modal) {
    modal.classList.remove("active")
    document.body.style.overflow = ""
  }
  currentDeletingTransaction = null
}

async function confirmDeleteTransaction() {
  try {
    if (!currentDeletingTransaction) {
      notifications.error("Erro", "Nenhuma transação selecionada para exclusão")
      return
    }

    console.log("Confirmando exclusão da transação:", currentDeletingTransaction.id)

    // Mostrar notificação de carregamento
    const loadingNotification = notifications.info("Excluindo...", "Excluindo movimentação, aguarde...", 0)

    await deleteTransaction(currentDeletingTransaction.id)

    // Remover notificação de carregamento
    notifications.remove(loadingNotification)

    closeDeleteModal()

    // Recarregar movimentações
    await window.movementsManager.loadTransactions()

    // Mostrar notificação de sucesso
    notifications.success("Sucesso!", "Movimentação excluída com sucesso!")
  } catch (error) {
    console.error("Erro ao excluir transação:", error)
    notifications.error("Erro ao Excluir", `Erro ao excluir movimentação: ${error.message}`)
  }
}

// Função auxiliar para popular selects
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

// ===== INICIALIZAÇÃO PRINCIPAL =====
document.addEventListener("DOMContentLoaded", async () => {
  console.log("Inicializando aplicação de movimentações...")

  const token = buscaTokenJwt()
  if (!token) {
    console.warn("Usuário não autenticado, redirecionando para login")
    window.location.replace("../login.html")
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
      closeNewMovementModal()
      closeEditModal()
      closeDeleteModal()
    }
  })

  // ===== CONFIGURAR LOGOUT =====
  const logoutBtn = document.getElementById("logoutBtn")
  if (logoutBtn) {
    logoutBtn.addEventListener("click", () => {
      localStorage.removeItem("authToken")
      sessionStorage.clear()
      window.location.replace("../login.html")
    })
  }

  // ===== CONFIGURAR BOTÃO NOVA MOVIMENTAÇÃO =====
  const newMovementBtn = document.getElementById("newMovementBtn")
  if (newMovementBtn) {
    newMovementBtn.addEventListener("click", openNewMovementModal)
  }

  // ===== CONFIGURAR MODAIS =====

  // Modal de Nova Movimentação
  const closeNewMovementBtn = document.getElementById("closeNewMovementModal")
  const cancelNewMovementBtn = document.getElementById("cancelNewMovementModal")
  const newMovementForm = document.getElementById("newMovementForm")

  if (closeNewMovementBtn) {
    closeNewMovementBtn.addEventListener("click", closeNewMovementModal)
  }

  if (cancelNewMovementBtn) {
    cancelNewMovementBtn.addEventListener("click", closeNewMovementModal)
  }

  if (newMovementForm) {
    newMovementForm.addEventListener("submit", (e) => {
      e.preventDefault()
      saveNewMovement()
    })
  }

  const newMovementModalOverlay = document.getElementById("newMovementModalOverlay")
  if (newMovementModalOverlay) {
    newMovementModalOverlay.addEventListener("click", (e) => {
      if (e.target === newMovementModalOverlay) {
        closeNewMovementModal()
      }
    })
  }

  // Modal de Edição
  const closeEditModalBtn = document.getElementById("closeEditModal")
  const cancelEditModalBtn = document.getElementById("cancelEditModal")
  const editForm = document.getElementById("editTransactionForm")

  if (closeEditModalBtn) {
    closeEditModalBtn.addEventListener("click", closeEditModal)
  }

  if (cancelEditModalBtn) {
    cancelEditModalBtn.addEventListener("click", closeEditModal)
  }

  if (editForm) {
    editForm.addEventListener("submit", (e) => {
      e.preventDefault()
      saveTransaction()
    })
  }

  const editModalOverlay = document.getElementById("editModalOverlay")
  if (editModalOverlay) {
    editModalOverlay.addEventListener("click", (e) => {
      if (e.target === editModalOverlay) {
        closeEditModal()
      }
    })
  }

  // Modal de Exclusão
  const closeDeleteModalBtn = document.getElementById("closeDeleteModal")
  const cancelDeleteModalBtn = document.getElementById("cancelDeleteModal")
  const confirmDeleteModalBtn = document.getElementById("confirmDeleteModal")

  if (closeDeleteModalBtn) {
    closeDeleteModalBtn.addEventListener("click", closeDeleteModal)
  }

  if (cancelDeleteModalBtn) {
    cancelDeleteModalBtn.addEventListener("click", closeDeleteModal)
  }

  if (confirmDeleteModalBtn) {
    confirmDeleteModalBtn.addEventListener("click", confirmDeleteTransaction)
  }

  const deleteModalOverlay = document.getElementById("deleteModalOverlay")
  if (deleteModalOverlay) {
    deleteModalOverlay.addEventListener("click", (e) => {
      if (e.target === deleteModalOverlay) {
        closeDeleteModal()
      }
    })
  }

  // ===== INICIALIZAR SISTEMA DE MOVIMENTAÇÕES =====
  const movementsManager = new MovementsManager()
  window.movementsManager = movementsManager

  // ===== CARREGAR DADOS INICIAIS =====
  try {
    console.log("Carregando dados iniciais...")
    await movementsManager.loadTransactions() // Carrega movimentações com paginação da API
    console.log("Aplicação de movimentações inicializada com sucesso!")
  } catch (error) {
    console.error("Erro ao inicializar aplicação:", error)
  }
})

// ===== FUNÇÕES GLOBAIS =====
function editTransaction(id) {
  openEditModal(id)
}

function deleteMovement(id, title) {
  openDeleteModal(id, title)
}

function refreshMovements() {
  console.log("Atualizando movimentações...")
  if (window.movementsManager) {
    window.movementsManager.loadTransactions()
  }
}

console.log("Movimentações JavaScript com paginação da API carregado com sucesso! 🚀")
