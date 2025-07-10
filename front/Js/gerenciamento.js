// ===== VARIÁVEIS GLOBAIS =====
let currentEditingItem = null
let currentDeletingItem = null
let currentEntityType = null

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

// ===== CONFIGURAÇÃO DA API =====
const API_CONFIG = {
  baseUrl: "https://localhost:7101/api",
  endpoints: {
    categories: "/categoria",
    months: "/MesReferencia",
    accounts: "/contabancaria",
    cards: "/cartao",
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

// ===== FUNÇÕES AUXILIARES =====
function formatCurrency(value) {
  return new Intl.NumberFormat("pt-BR", {
    style: "currency",
    currency: "BRL",
  }).format(value)
}

// ===== CLASSE BASE PARA GERENCIAMENTO DE ENTIDADES =====
class EntityManager {
  constructor(entityType, config) {
    this.entityType = entityType
    this.config = config
    this.filters = { search: "" }
    this.searchTimeout = null
    this.currentItems = []
    this.allItems = []

    // Configurações de paginação
    this.currentPage = 1
    this.pageSize = 10
    this.totalRecords = 0
    this.totalPages = 0

    this.initializeElements()
    this.bindEvents()
  }

  initializeElements() {
    const prefix = this.entityType

    this.searchInput = document.getElementById(`${prefix}SearchInput`)
    this.searchClear = document.getElementById(`${prefix}SearchClear`)
    this.activeFilters = document.getElementById(`${prefix}ActiveFilters`)
    this.activeFiltersList = document.getElementById(`${prefix}ActiveFiltersList`)
    this.tableBody = document.getElementById(`${prefix}TableBody`)
    this.emptyState = document.getElementById(`${prefix}EmptyState`)
    this.loadingState = document.getElementById(`${prefix}LoadingState`)

    this.paginationContainer = document.getElementById(`${prefix}PaginationContainer`)
    this.paginationInfo = document.getElementById(`${prefix}PaginationInfo`)
    this.paginationPages = document.getElementById(`${prefix}PaginationPages`)
    this.firstPageBtn = document.getElementById(`${prefix}FirstPageBtn`)
    this.prevPageBtn = document.getElementById(`${prefix}PrevPageBtn`)
    this.nextPageBtn = document.getElementById(`${prefix}NextPageBtn`)
    this.lastPageBtn = document.getElementById(`${prefix}LastPageBtn`)
    this.pageSizeSelect = document.getElementById(`${prefix}PageSize`)
  }

  bindEvents() {
    // Pesquisa em tempo real
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
        this.currentPage = 1
        this.loadItems()
      })
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

    this.filters.search = value?.trim() || ""

    if (this.searchTimeout) {
      clearTimeout(this.searchTimeout)
    }

    this.searchTimeout = setTimeout(() => {
      this.applyFrontendFilters()
      this.renderItems()
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
    this.currentPage = 1
    this.loadItems()
  }

  async loadItems() {
    try {
      this.showLoading()

      const apiResponse = await this.fetchFromApi(this.currentPage, this.pageSize)
      this.allItems = apiResponse.items || []

      this.applyFrontendFilters()

      this.totalRecords = apiResponse.total || 0
      this.totalPages = Math.ceil(this.totalRecords / this.pageSize)
      this.currentPage = apiResponse.pagina || this.currentPage

      this.renderItems()
      this.updatePagination()
      this.updateActiveFiltersDisplay()
      this.hideLoading()
    } catch (error) {
      console.error(`Erro ao carregar ${this.entityType}:`, error)
      this.hideLoading()
      notifications.error("Erro", `Erro ao carregar ${this.config.displayName}`)
    }
  }

  applyFrontendFilters() {
    let filteredItems = this.allItems || []

    if (this.filters.search && this.filters.search.trim() !== "") {
      const searchTerm = this.filters.search.toLowerCase().trim()
      filteredItems = filteredItems.filter((item) => {
        return this.config.searchFields.some((field) => {
          const value = item[field]
          return value && value.toString().toLowerCase().includes(searchTerm)
        })
      })
    }

    this.currentItems = filteredItems
  }

  renderItems() {
    if (!this.tableBody || !this.emptyState) return

    this.tableBody.innerHTML = ""

    if (this.currentItems.length === 0) {
      this.emptyState.classList.remove("hidden")
      return
    }

    this.emptyState.classList.add("hidden")
    this.tableBody.innerHTML = this.currentItems.map((item) => this.config.renderRow(item)).join("")
  }

  updateActiveFiltersDisplay() {
    if (!this.activeFilters || !this.activeFiltersList) return

    const activeFilters = []

    if (this.filters.search) {
      activeFilters.push({ key: "search", label: `Pesquisa: "${this.filters.search}"` })
    }

    if (activeFilters.length > 0) {
      this.activeFilters.style.display = "flex"
      this.activeFiltersList.innerHTML = activeFilters
        .map(
          (filter) => `
              <div class="filter-tag">
                  ${filter.label}
                  <button class="filter-tag-remove" onclick="window.${this.entityType}Manager.removeFilter('${filter.key}')">
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

    if (filterKey === "search") {
      if (this.searchInput) this.searchInput.value = ""
      if (this.searchClear) this.searchClear.style.display = "none"
    }

    this.loadItems()
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

    this.currentPage = page
    await this.loadItems()
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
                onclick="window.${this.entityType}Manager.goToPage(${i})">
          ${i}
        </button>
      `
    }

    this.paginationPages.innerHTML = pagesHtml
  }

  async fetchFromApi(pagina = 1, quantidadePorPagina = 10) {
    try {
      const token = buscaTokenJwt()
      if (!token) {
        throw new Error("Usuário não autenticado")
      }

      const headers = {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      }

      const params = new URLSearchParams({
        pagina: pagina.toString(),
        quantidadePorPagina: quantidadePorPagina.toString(),
      })

      const url = `${API_CONFIG.baseUrl}${this.config.endpoint}?${params.toString()}`
      const response = await fetch(url, { method: "GET", headers })

      if (!response.ok) {
        throw new Error(`Erro na API: ${response.status} - ${response.statusText}`)
      }

      const data = await response.json()

      return {
        items: data[this.config.dataKey] || data.data || data || [],
        total: data.total || 0,
        pagina: data.pagina || pagina,
      }
    } catch (error) {
      console.error(`Erro ao buscar ${this.entityType}:`, error)
      throw error
    }
  }

  async createItem(itemData) {
    try {
      const token = buscaTokenJwt()
      if (!token) {
        throw new Error("Usuário não autenticado")
      }

      const headers = {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      }

      const response = await fetch(`${API_CONFIG.baseUrl}${this.config.endpoint}`, {
        method: "POST",
        headers,
        body: JSON.stringify(itemData),
      })

      if (!response.ok) {
        const errorBody = await response.text()
        throw new Error(`Erro na API: ${response.status} - ${response.statusText}. ${errorBody}`)
      }

      return response.status === 204 ? { success: true } : await response.json()
    } catch (error) {
      console.error(`Erro ao criar ${this.entityType}:`, error)
      throw error
    }
  }

  async updateItem(itemId, itemData) {
    try {
      const token = buscaTokenJwt()
      if (!token) {
        throw new Error("Usuário não autenticado")
      }

      const headers = {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      }

      const response = await fetch(`${API_CONFIG.baseUrl}${this.config.endpoint}/${itemId}`, {
        method: "PUT",
        headers,
        body: JSON.stringify(itemData),
      })

      if (!response.ok) {
        const errorBody = await response.text()
        throw new Error(`Erro na API: ${response.status} - ${response.statusText}. ${errorBody}`)
      }

      return response.status === 204 ? { success: true } : await response.json()
    } catch (error) {
      console.error(`Erro ao atualizar ${this.entityType}:`, error)
      throw error
    }
  }

  async deleteItem(itemId) {
    try {
      const token = buscaTokenJwt()
      if (!token) {
        throw new Error("Usuário não autenticado")
      }

      const headers = {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      }

      const response = await fetch(`${API_CONFIG.baseUrl}${this.config.endpoint}/${itemId}`, {
        method: "DELETE",
        headers,
      })

      if (!response.ok) {
        const errorBody = await response.text()
        throw new Error(`Erro na API: ${response.status} - ${response.statusText}. ${errorBody}`)
      }

      return { success: true }
    } catch (error) {
      console.error(`Erro ao deletar ${this.entityType}:`, error)
      throw error
    }
  }
}

// ===== CONFIGURAÇÕES DAS ENTIDADES =====
const entityConfigs = {
  categories: {
    endpoint: "/categoria",
    dataKey: "categorias",
    displayName: "categorias",
    searchFields: ["nomeCategoria", "tipo"],
    renderRow: (item) => `
      <tr>
        <td>${item.nomeCategoria || "Sem nome"}</td>
        <td>
          <span class="category-type category-type-${item.tipo?.toLowerCase() || "default"}">
            ${item.tipo || "N/A"}
          </span>
        </td>
        <td>
          <div class="table-actions">
            <button class="action-btn action-btn-edit" onclick="openEditModal('categories', ${item.id}, '${item.nomeCategoria}')" title="Editar categoria">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
              </svg>
            </button>
            <button class="action-btn action-btn-delete" onclick="openDeleteModal('categories', ${item.id}, '${item.nomeCategoria}')" title="Excluir categoria">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M3 6h18l-2 13H5L3 6z"></path>
                <path d="M8 6V4a2 2 0 012-2h4a2 2 0 012 2v2"></path>
              </svg>
            </button>
          </div>
        </td>
      </tr>
    `,
  },
  months: {
    endpoint: "/MesReferencia",
    dataKey: "meses",
    displayName: "meses",
    searchFields: ["nomeMes"],
    renderRow: (item) => `
      <tr>
        <td>${item.nomeMes || "Sem nome"}</td>
        <td>
          <div class="table-actions">
            <button class="action-btn action-btn-edit" onclick="openEditModal('months', ${item.id}, '${item.nomeMes}')" title="Editar mês">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
              </svg>
            </button>
            <button class="action-btn action-btn-delete" onclick="openDeleteModal('months', ${item.id}, '${item.nomeMes}')" title="Excluir mês">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M3 6h18l-2 13H5L3 6z"></path>
                <path d="M8 6V4a2 2 0 012-2h4a2 2 0 012 2v2"></path>
              </svg>
            </button>
          </div>
        </td>
      </tr>
    `,
  },
  accounts: {
    endpoint: "/contabancaria",
    dataKey: "contas",
    displayName: "contas bancárias",
    searchFields: ["nomeConta"],
    renderRow: (item) => `
    <tr>
      <td>${item.nomeConta || "Sem nome"}</td>
      <td>${formatCurrency(item.saldoInicial || 0)}</td>
      <td>
        <div class="table-actions">
          <button class="action-btn action-btn-edit" onclick="openEditModal('accounts', ${item.id}, '${item.nomeConta}')" title="Editar conta">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
              <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
            </svg>
          </button>
          <button class="action-btn action-btn-delete" onclick="openDeleteModal('accounts', ${item.id}, '${item.nomeConta}')" title="Excluir conta">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M3 6h18l-2 13H5L3 6z"></path>
              <path d="M8 6V4a2 2 0 012-2h4a2 2 0 012 2v2"></path>
            </svg>
          </button>
        </div>
      </td>
    </tr>
  `,
  },
  cards: {
    endpoint: "/cartao",
    dataKey: "cartoes",
    displayName: "cartões",
    searchFields: ["nomeCartao"],
    renderRow: (item) => `
      <tr>
        <td>${item.nomeCartao || "Sem nome"}</td>
        <td>
          <div class="table-actions">
            <button class="action-btn action-btn-edit" onclick="openEditModal('cards', ${item.id}, '${item.nomeCartao}')" title="Editar cartão">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
              </svg>
            </button>
            <button class="action-btn action-btn-delete" onclick="openDeleteModal('cards', ${item.id}, '${item.nomeCartao}')" title="Excluir cartão">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M3 6h18l-2 13H5L3 6z"></path>
                <path d="M8 6V4a2 2 0 012-2h4a2 2 0 012 2v2"></path>
              </svg>
            </button>
          </div>
        </td>
      </tr>
    `,
  },
}

// ===== SISTEMA DE ABAS =====
class TabSystem {
  constructor() {
    this.currentTab = "categories"
    this.initializeElements()
    this.bindEvents()
  }

  initializeElements() {
    this.tabButtons = document.querySelectorAll(".tab-button")
    this.tabContents = document.querySelectorAll(".tab-content")
  }

  bindEvents() {
    this.tabButtons.forEach((button) => {
      button.addEventListener("click", () => {
        const tabId = button.getAttribute("data-tab")
        this.switchTab(tabId)
      })
    })
  }

  switchTab(tabId) {
    // Atualizar botões
    this.tabButtons.forEach((button) => {
      if (button.getAttribute("data-tab") === tabId) {
        button.classList.add("active")
      } else {
        button.classList.remove("active")
      }
    })

    // Atualizar conteúdo
    this.tabContents.forEach((content) => {
      if (content.id === tabId) {
        content.classList.add("active")
      } else {
        content.classList.remove("active")
      }
    })

    this.currentTab = tabId

    // Carregar dados da aba ativa
    if (window[`${tabId}Manager`]) {
      window[`${tabId}Manager`].loadItems()
    }
  }
}

// ===== FUNÇÕES DAS MODAIS =====
function openNewModal(entityType) {
  console.log("Abrindo modal para:", entityType)

  currentEntityType = entityType
  currentEditingItem = null

  const modalId =
    entityType === "categories"
      ? "categoryModalOverlay"
      : entityType === "months"
        ? "monthModalOverlay"
        : entityType === "accounts"
          ? "accountModalOverlay"
          : entityType === "cards"
            ? "cardModalOverlay"
            : null

  console.log("Modal ID:", modalId)

  const modal = document.getElementById(modalId)
  console.log("Modal element:", modal)

  if (!modal) {
    console.error("Modal não encontrada:", modalId)
    notifications.error("Erro", "Modal não encontrada")
    return
  }

  const titleId =
    entityType === "categories"
      ? "categoryModalTitle"
      : entityType === "months"
        ? "monthModalTitle"
        : entityType === "accounts"
          ? "accountModalTitle"
          : entityType === "cards"
            ? "cardModalTitle"
            : null

  const formId =
    entityType === "categories"
      ? "categoryForm"
      : entityType === "months"
        ? "monthForm"
        : entityType === "accounts"
          ? "accountForm"
          : entityType === "cards"
            ? "cardForm"
            : null

  const title = document.getElementById(titleId)
  const form = document.getElementById(formId)

  if (title) {
    const displayNames = {
      categories: "Nova Categoria",
      months: "Novo Mês",
      accounts: "Nova Conta Bancária",
      cards: "Novo Cartão",
    }
    title.textContent = displayNames[entityType] || "Novo Item"
  }

  if (form) {
    form.reset()

    // Para contas bancárias, mostrar campo de saldo inicial na criação
    if (entityType === "accounts") {
      const initialBalanceGroup = document.getElementById("initialBalanceGroup")
      if (initialBalanceGroup) {
        initialBalanceGroup.style.display = "block"
      }
    }
  }

  modal.classList.add("active")
  document.body.style.overflow = "hidden"

  console.log("Modal aberta com sucesso")
}

function openEditModal(entityType, itemId, itemName) {
  currentEntityType = entityType

  const manager = window[`${entityType}Manager`]
  const item = manager.currentItems.find((i) => i.id === itemId)

  if (!item) {
    notifications.error("Erro", "Item não encontrado!")
    return
  }

  currentEditingItem = item

  const modalId =
    entityType === "categories"
      ? "categoryModalOverlay"
      : entityType === "months"
        ? "monthModalOverlay"
        : entityType === "accounts"
          ? "accountModalOverlay"
          : entityType === "cards"
            ? "cardModalOverlay"
            : null

  const modal = document.getElementById(modalId)

  const titleId =
    entityType === "categories"
      ? "categoryModalTitle"
      : entityType === "months"
        ? "monthModalTitle"
        : entityType === "accounts"
          ? "accountModalTitle"
          : entityType === "cards"
            ? "cardModalTitle"
            : null

  const formId =
    entityType === "categories"
      ? "categoryForm"
      : entityType === "months"
        ? "monthForm"
        : entityType === "accounts"
          ? "accountForm"
          : entityType === "cards"
            ? "cardForm"
            : null

  const title = document.getElementById(titleId)
  const form = document.getElementById(formId)

  if (title) {
    const displayNames = {
      categories: "Editar Categoria",
      months: "Editar Mês",
      accounts: "Editar Conta Bancária",
      cards: "Editar Cartão",
    }
    title.textContent = displayNames[entityType] || "Editar Item"
  }

  if (form) {
    // Preencher formulário baseado no tipo de entidade
    if (entityType === "categories") {
      document.getElementById("categoryName").value = item.nomeCategoria || ""
      document.getElementById("categoryType").value = item.tipo || ""
    } else if (entityType === "months") {
      document.getElementById("monthName").value = item.nomeMes || ""
    } else if (entityType === "accounts") {
      document.getElementById("accountName").value = item.nomeConta || ""
      // Ocultar campo de saldo inicial na edição
      const initialBalanceGroup = document.getElementById("initialBalanceGroup")
      if (initialBalanceGroup) {
        initialBalanceGroup.style.display = "none"
      }
    } else if (entityType === "cards") {
      document.getElementById("cardName").value = item.nomeCartao || ""
    }
  }

  if (modal) {
    modal.classList.add("active")
    document.body.style.overflow = "hidden"
  }
}

function closeModal(entityType) {
  const modalId =
    entityType === "categories"
      ? "categoryModalOverlay"
      : entityType === "months"
        ? "monthModalOverlay"
        : entityType === "accounts"
          ? "accountModalOverlay"
          : entityType === "cards"
            ? "cardModalOverlay"
            : null

  const modal = document.getElementById(modalId)
  if (modal) {
    modal.classList.remove("active")
    document.body.style.overflow = ""
  }
  currentEditingItem = null
  currentEntityType = null
}

async function saveItem(entityType) {
  try {
    if (!currentEntityType) {
      notifications.error("Erro", "Tipo de entidade não definido")
      return
    }

    const formId =
      entityType === "categories"
        ? "categoryForm"
        : entityType === "months"
          ? "monthForm"
          : entityType === "accounts"
            ? "accountForm"
            : entityType === "cards"
              ? "cardForm"
              : null

    const form = document.getElementById(formId)
    const formData = new FormData(form)

    let itemData = {}

    // Preparar dados baseado no tipo de entidade
    if (entityType === "categories") {
      itemData = {
        nomeCategoria: formData.get("nomeCategoria"),
        tipo: formData.get("tipo"),
      }
    } else if (entityType === "months") {
      itemData = { nomeMes: formData.get("nomeMes") }
    } else if (entityType === "accounts") {
      if (currentEditingItem) {
        // Na edição, enviar apenas o nome da conta
        itemData = {
          nomeConta: formData.get("nomeConta"),
        }
      } else {
        // Na criação, enviar nome e saldo inicial
        itemData = {
          nomeConta: formData.get("nomeConta"),
          saldoInicial: formData.get("saldoInicial") ? Number.parseFloat(formData.get("saldoInicial")) : 0,
        }
      }
    } else if (entityType === "cards") {
      itemData = { nomeCartao: formData.get("nomeCartao") }
    }

    // Validação básica
    const requiredFields = {
      categories: ["nomeCategoria", "tipo"],
      months: ["nomeMes"],
      accounts: ["nomeConta"],
      cards: ["nomeCartao"],
    }

    const required = requiredFields[entityType] || []
    for (const field of required) {
      if (!itemData[field] || itemData[field].trim() === "") {
        notifications.error("Campo Obrigatório", "Por favor, preencha todos os campos obrigatórios")
        return
      }
    }

    const manager = window[`${entityType}Manager`]
    const loadingNotification = notifications.info(
      currentEditingItem ? "Salvando..." : "Criando...",
      currentEditingItem ? "Atualizando item, aguarde..." : "Criando novo item, aguarde...",
      0,
    )

    if (currentEditingItem) {
      await manager.updateItem(currentEditingItem.id, itemData)
    } else {
      await manager.createItem(itemData)
    }

    notifications.remove(loadingNotification)
    closeModal(entityType)
    await manager.loadItems()

    notifications.success("Sucesso!", currentEditingItem ? "Item atualizado com sucesso!" : "Item criado com sucesso!")
  } catch (error) {
    console.error("Erro ao salvar item:", error)
    notifications.error("Erro ao Salvar", `Erro ao salvar item: ${error.message}`)
  }
}

function openDeleteModal(entityType, itemId, itemName) {
  currentEntityType = entityType

  const manager = window[`${entityType}Manager`]
  const item = manager.currentItems.find((i) => i.id === itemId)

  if (!item) {
    notifications.error("Erro", "Item não encontrado!")
    return
  }

  currentDeletingItem = item

  const nameElement = document.getElementById("deleteItemName")
  if (nameElement) {
    nameElement.textContent = itemName || "Item sem nome"
  }

  const modal = document.getElementById("deleteModalOverlay")
  if (modal) {
    modal.classList.add("active")
    document.body.style.overflow = "hidden"
  }
}

function closeDeleteModal() {
  const modal = document.getElementById("deleteModalOverlay")
  if (modal) {
    modal.classList.remove("active")
    document.body.style.overflow = ""
  }
  currentDeletingItem = null
  currentEntityType = null
}

async function confirmDelete() {
  try {
    if (!currentDeletingItem || !currentEntityType) {
      notifications.error("Erro", "Nenhum item selecionado para exclusão")
      return
    }

    const manager = window[`${currentEntityType}Manager`]
    const loadingNotification = notifications.info("Excluindo...", "Excluindo item, aguarde...", 0)

    await manager.deleteItem(currentDeletingItem.id)

    notifications.remove(loadingNotification)
    closeDeleteModal()
    await manager.loadItems()

    notifications.success("Sucesso!", "Item excluído com sucesso!")
  } catch (error) {
    console.error("Erro ao excluir item:", error)
    notifications.error("Erro ao Excluir", `Erro ao excluir item: ${error.message}`)
  }
}

// ===== INICIALIZAÇÃO PRINCIPAL =====
document.addEventListener("DOMContentLoaded", async () => {
  console.log("Inicializando aplicação de gerenciamento...")

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
    overlay.addEventListener("click", closeVerticalMenu)
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
      closeModal(currentEntityType)
      closeDeleteModal()
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

  // ===== INICIALIZAR SISTEMA DE ABAS =====
  const tabSystem = new TabSystem()

  // ===== INICIALIZAR GERENCIADORES DE ENTIDADES =====
  window.categoriesManager = new EntityManager("categories", entityConfigs.categories)
  window.monthsManager = new EntityManager("months", entityConfigs.months)
  window.accountsManager = new EntityManager("accounts", entityConfigs.accounts)
  window.cardsManager = new EntityManager("cards", entityConfigs.cards)

  // ===== CONFIGURAR BOTÕES DE NOVA ENTIDADE =====
  const newCategoryBtn = document.getElementById("newCategoryBtn")
  if (newCategoryBtn) {
    console.log("Botão Nova Categoria encontrado")
    newCategoryBtn.addEventListener("click", (e) => {
      console.log("Clique no botão Nova Categoria")
      e.preventDefault()
      openNewModal("categories")
    })
  } else {
    console.error("Botão Nova Categoria não encontrado")
  }

  document.getElementById("newMonthBtn")?.addEventListener("click", () => openNewModal("months"))
  document.getElementById("newAccountBtn")?.addEventListener("click", () => openNewModal("accounts"))
  document.getElementById("newCardBtn")?.addEventListener("click", () => openNewModal("cards"))

  // ===== CONFIGURAR MODAIS =====
  const modalTypes = [
    { type: "category", entityType: "categories" },
    { type: "month", entityType: "months" },
    { type: "account", entityType: "accounts" },
    { type: "card", entityType: "cards" },
  ]

  modalTypes.forEach(({ type, entityType }) => {
    // Botões de fechar
    document
      .getElementById(`close${type.charAt(0).toUpperCase() + type.slice(1)}Modal`)
      ?.addEventListener("click", () => closeModal(entityType))
    document
      .getElementById(`cancel${type.charAt(0).toUpperCase() + type.slice(1)}Modal`)
      ?.addEventListener("click", () => closeModal(entityType))

    // Formulários
    document.getElementById(`${type}Form`)?.addEventListener("submit", (e) => {
      e.preventDefault()
      saveItem(entityType)
    })

    // Overlay
    document.getElementById(`${type}ModalOverlay`)?.addEventListener("click", (e) => {
      if (e.target.id === `${type}ModalOverlay`) {
        closeModal(entityType)
      }
    })
  })

  // Modal de exclusão
  document.getElementById("closeDeleteModal")?.addEventListener("click", closeDeleteModal)
  document.getElementById("cancelDeleteModal")?.addEventListener("click", closeDeleteModal)
  document.getElementById("confirmDeleteModal")?.addEventListener("click", confirmDelete)
  document.getElementById("deleteModalOverlay")?.addEventListener("click", (e) => {
    if (e.target.id === "deleteModalOverlay") {
      closeDeleteModal()
    }
  })

  // ===== CARREGAR DADOS INICIAIS =====
  try {
    console.log("Carregando dados iniciais...")
    await window.categoriesManager.loadItems() // Carregar primeira aba
    console.log("Aplicação de gerenciamento inicializada com sucesso!")
  } catch (error) {
    console.error("Erro ao inicializar aplicação:", error)
  }
})

console.log("Gerenciamento JavaScript carregado com sucesso! 🚀")
