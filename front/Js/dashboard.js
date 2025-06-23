// ===== CONFIGURAÇÕES E DADOS =====
const categories = [
  { id: 1, name: 'Salário', type: 'income' },
  { id: 2, name: 'Freelance', type: 'income' },
  { id: 3, name: 'Investimentos', type: 'income' },
  { id: 4, name: 'Alimentação', type: 'expense' },
  { id: 5, name: 'Transporte', type: 'expense' },
  { id: 6, name: 'Moradia', type: 'expense' },
  { id: 7, name: 'Saúde', type: 'expense' },
  { id: 8, name: 'Educação', type: 'expense' },
  { id: 9, name: 'Lazer', type: 'expense' },
  { id: 10, name: 'Outros', type: 'expense' }
];

const paymentMethods = [
  { id: 1, name: 'Cartão de Crédito' },
  { id: 2, name: 'Débito' },
  { id: 3, name: 'Dinheiro' },
  { id: 4, name: 'PIX' },
  { id: 5, name: 'Transferência' }
];

let transactions = [];

// ===== VARIÁVEIS GLOBAIS =====
let apiAccountsData = null;
let apiSaldoTotal = 0;
let apiPendingIncomes = 0;
let apiPendingExpenses = 0;
let showValues = true;
let showAccountDetails = false;

// ===== CONFIGURAÇÃO DA API =====
const API_CONFIG = {
  baseUrl: 'https://localhost:7101/api',
  endpoints: {
    saldoTotalRotaApi: '/dashboard/saldo-total',
    accountsBalance: '/dashboard/saldo-contas',
    ReceitasTotaisRotaApi: '/dashboard/valor-em-aberto-receitas',
    pendingExpenses: '/dashboard/valor-em-aberto-despesas',
    transactions: '/dashboard/movimentacoes-em-aberto'
  }
};

// ===== FUNÇÕES DE AUTENTICAÇÃO =====
function verificaAutenticacao() {
  const token = buscaTokenJwt();
  
  if (!token) {
    console.warn("Usuário não autenticado, redirecionando para login");
    window.location.replace('/login.html');
    return false;
  }
  
  return true;
}

function buscaTokenJwt() {
  const keys = ['authToken', 'token', 'jwt'];
  for (const key of keys) {
    const token = localStorage.getItem(key);
    if (token) return token;
  }
  return null;
}

// ===== FUNÇÕES DE API =====
async function buscaApiSaldoTotal() {
  try {
    const token = buscaTokenJwt();
    
    if (!token) {
      console.warn('Usuário não autenticado, token não encontrado');
      return null;
    }
    
    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };
    
    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.saldoTotalRotaApi}`, {
      method: 'GET',
      headers: headers
    });
    
    if (!response.ok) {
      console.error(`Erro na API (saldo total): ${response.status} - ${response.statusText}`);
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}`);
    }
    
    const data = await response.json();
    apiSaldoTotal = typeof data === 'number' ? data : data.saldo || data.balance || data.total || 0;
    return apiSaldoTotal;

  } catch (error) {
    console.error('Erro ao buscar saldo total da API:', error.message);
    return null;
  }
}

async function buscaAPiReceitasPendentes() {
  try {
    const token = buscaTokenJwt();
    
    if (!token) {
      console.warn('Usuário não autenticado, token não encontrado');
      return null;
    }
    
    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.ReceitasTotaisRotaApi}`, {
      method: 'GET',
      headers: headers
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`);
    }
    
    const data = await response.json();
    apiPendingIncomes = typeof data === 'number' ? data : data.receitas || data.pendingIncomes || data.total || 0;
    return apiPendingIncomes;
  } catch (error) {
    console.error('Erro ao buscar receitas pendentes da API:', error.message);
    return null;
  }
}

async function fetchPendingExpenses() {
  try {
    const token = buscaTokenJwt();
    
    if (!token) {
      console.warn('Usuário não autenticado, token não encontrado');
      return null;
    }
    
    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.pendingExpenses}`, {
      method: 'GET',
      headers: headers
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Erro na API (despesas): ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`);
    }

    const data = await response.json();
    apiPendingExpenses = typeof data === 'number' ? data : data.despesas || data.pendingExpenses || data.total || 0;
    return apiPendingExpenses;
  } catch (error) {
    console.error('Erro ao buscar despesas pendentes da API:', error.message);
    return null;
  }
}

async function fetchAccountsBalance() {
  try {
    const token = buscaTokenJwt();
    
    if (!token) {
      console.warn('Usuário não autenticado, token não encontrado');
      return null;
    }
    
    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };

    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.accountsBalance}`, {
      method: 'GET',
      headers: headers
    });

    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`);
    }

    const data = await response.json();
    apiAccountsData = data;
    return data;
  } catch (error) {
    console.error('Erro ao buscar saldos das contas da API:', error.message);
    return null;
  }
}

async function fetchTransactionsFromApi() {
  try {
    const token = buscaTokenJwt();
    if (!token) {
      console.warn('Usuário não autenticado, token não encontrado');
      return [];
    }
    
    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };
    
    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.transactions}`, {
      method: 'GET',
      headers: headers
    });
    
    if (!response.ok) {
      const errorBody = await response.text();
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}. Detalhes: ${errorBody}`);
    }

    const data = await response.json();
    const apiTransactions = Array.isArray(data) ? data : [data];

    return apiTransactions.map(apiT => ({
      id: apiT.id,
      name: apiT.titulo || 'Sem título',
      description: apiT.descricao || '',
      amount: apiT.valor || 0,
      date: apiT.dataVencimento,
      dueDate: apiT.dataVencimento,
      categoryId: apiT.categoriaId,
      accountId: apiT.contaBancariaId,
      paymentMethodId: apiT.cartaoId || null,
      type: apiT.tipo && apiT.tipo.toLowerCase() === 'despesa' ? 'expense' : 'income',
      completed: !!apiT.realizado,
      category: apiT.categoriaNome || 'N/A',
      account: apiT.contaNome || 'N/A'
    }));
  } catch (error) {
    console.error('Erro ao buscar transações da API:', error.message);
    return [];
  }
}

// ===== FUNÇÕES AUXILIARES =====
function formatCurrency(value) {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(value);
}

function formatDate(dateString) {
  if (!dateString) return 'Data inválida';
  return new Date(dateString).toLocaleDateString('pt-BR');
}

function updateValueDisplay(element, value) {
  if (element) {
    if (showValues) {
      element.textContent = formatCurrency(value);
    } else {
      element.textContent = '••••••';
    }
  }
}

// ===== FUNÇÃO PARA RENDERIZAR DETALHES DAS CONTAS =====
async function renderAccountDetails() {
  const accountDetailsEl = document.getElementById('accountDetails');
  
  if (!showAccountDetails || !accountDetailsEl) return;
  
  console.log('Renderizando detalhes das contas...'); // Debug
  
  const accountsData = await fetchAccountsBalance();
  let html = '';
  
  if (accountsData && Array.isArray(accountsData)) {
    console.log('Dados das contas:', accountsData); // Debug
    
    accountsData.forEach(account => {
      html += `
        <div class="account-item">
          <div class="account-info">
            <span class="account-name">${account.nomeConta || account.name || 'Conta sem nome'}</span>
            <span class="account-type">${account.tipoConta || account.type || ''}</span>
          </div>
          <div class="account-balance">
            <span class="balance-value ${(account.saldoAtual || account.balance || 0) >= 0 ? 'positive' : 'negative'}">
              ${showValues ? formatCurrency(account.saldoAtual || account.balance || 0) : '••••••'}
            </span>
          </div>
        </div>
      `;
    });
  } else {
    console.error('Falha ao carregar dados das contas da API');
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
    `;
  }
  
  // Adicionar total geral
  const totalBalance = await buscaApiSaldoTotal();
  html += `
    <div class="account-total">
      <div class="account-info">
        <span class="account-name">Total Geral</span>
      </div>
      <div class="account-balance">
        <span class="balance-value total ${totalBalance >= 0 ? 'positive' : 'negative'}">
          ${showValues ? formatCurrency(totalBalance) : '••••••'}
        </span>
      </div>
    </div>
  `;
  
  accountDetailsEl.innerHTML = html;
}

// ===== FUNÇÃO PARA ATUALIZAR DASHBOARD =====
async function updateDashboard() {
  console.log('Atualizando dashboard...'); // Debug
  
  const totalBalanceEl = document.getElementById('totalBalance');
  const pendingIncomesEl = document.getElementById('pendingIncomes');
  const pendingExpensesEl = document.getElementById('pendingExpenses');
  
  try {
    // Buscar saldo total
    const totalBalance = await buscaApiSaldoTotal();
    if (totalBalance !== null) {
      updateValueDisplay(totalBalanceEl, totalBalance);
    }
    
    // Buscar receitas pendentes
    const pendingIncomes = await buscaAPiReceitasPendentes();
    if (pendingIncomes !== null) {
      updateValueDisplay(pendingIncomesEl, pendingIncomes);
    }
    
    // Buscar despesas pendentes
    const pendingExpenses = await fetchPendingExpenses();
    if (pendingExpenses !== null) {
      updateValueDisplay(pendingExpensesEl, pendingExpenses);
    }
    
    // Atualizar detalhes das contas se estiver visível
    if (showAccountDetails) {
      await renderAccountDetails();
    }
    
    console.log('Dashboard atualizado com sucesso'); // Debug
  } catch (error) {
    console.error('Erro ao atualizar dashboard:', error);
  }
}

// ===== CLASSE PARA GERENCIAR FILTROS E MOVIMENTAÇÕES =====
class MovementsManager {
    constructor() {
        this.filters = {
            search: '',
            startDate: '',
            endDate: '',
            category: '',
            account: '',
            type: '',
            status: ''
        };
        
        this.isFiltersExpanded = false;
        this.searchTimeout = null;
        this.allTransactions = [];
        this.filteredTransactions = [];
        
        this.initializeElements();
        this.bindEvents();
    }
    
    initializeElements() {
        // Elementos do sistema de filtros
        this.filterToggle = document.getElementById('toggleFilters');
        this.advancedFilters = document.getElementById('advancedFilters');
        this.searchInput = document.getElementById('searchInput');
        this.searchClear = document.getElementById('searchClear');
        
        // Filtros
        this.startDateInput = document.getElementById('startDate');
        this.endDateInput = document.getElementById('endDate');
        this.categorySelect = document.getElementById('categoryFilter');
        this.accountSelect = document.getElementById('accountFilter');
        this.typeSelect = document.getElementById('typeFilter');
        this.statusSelect = document.getElementById('statusFilter');
        
        // Botões
        this.applyButton = document.getElementById('applyFilters');
        this.clearButton = document.getElementById('clearFilters');
        
        // Containers
        this.activeFilters = document.getElementById('activeFilters');
        this.activeFiltersList = document.getElementById('activeFiltersList');
        this.transactionsBody = document.getElementById('transactionsBody');
        this.emptyState = document.getElementById('emptyState');
        this.loadingState = document.getElementById('loadingState');
    }
    
    bindEvents() {
        // Toggle dos filtros avançados
        if (this.filterToggle) {
            this.filterToggle.addEventListener('click', () => {
                this.toggleAdvancedFilters();
            });
        }
        
        // Pesquisa em tempo real
        if (this.searchInput) {
            this.searchInput.addEventListener('input', (e) => {
                this.handleSearchInput(e.target.value);
            });
        }
        
        // Limpar pesquisa
        if (this.searchClear) {
            this.searchClear.addEventListener('click', () => {
                this.clearSearch();
            });
        }
        
        // Aplicar filtros
        if (this.applyButton) {
            this.applyButton.addEventListener('click', () => {
                this.applyFilters();
            });
        }
        
        // Limpar filtros
        if (this.clearButton) {
            this.clearButton.addEventListener('click', () => {
                this.clearAllFilters();
            });
        }
        
        // Filtros individuais
        const filterElements = [
            this.startDateInput, this.endDateInput, this.categorySelect, 
            this.accountSelect, this.typeSelect, this.statusSelect
        ].filter(el => el !== null);
        
        filterElements.forEach(element => {
            element.addEventListener('change', () => {
                this.updateFilterFromElement(element);
                this.applyFilters();
            });
        });
    }
    
    toggleAdvancedFilters() {
        this.isFiltersExpanded = !this.isFiltersExpanded;
        
        if (this.advancedFilters) {
            if (this.isFiltersExpanded) {
                this.advancedFilters.classList.add('expanded');
                if (this.filterToggle) this.filterToggle.classList.add('active');
            } else {
                this.advancedFilters.classList.remove('expanded');
                if (this.filterToggle) this.filterToggle.classList.remove('active');
            }
        }
    }
    
    handleSearchInput(value) {
        if (this.searchClear) {
            if (value.length > 0) {
                this.searchClear.style.display = 'block';
            } else {
                this.searchClear.style.display = 'none';
            }
        }
        
        clearTimeout(this.searchTimeout);
        this.searchTimeout = setTimeout(() => {
            this.filters.search = value;
            this.applyFilters();
        }, 300);
    }
    
    clearSearch() {
        if (this.searchInput) {
            this.searchInput.value = '';
        }
        if (this.searchClear) {
            this.searchClear.style.display = 'none';
        }
        this.filters.search = '';
        this.applyFilters();
    }
    
    updateFilterFromElement(element) {
        if (!element) return;
        
        const filterMap = {
            'startDate': 'startDate',
            'endDate': 'endDate',
            'categoryFilter': 'category',
            'accountFilter': 'account',
            'typeFilter': 'type',
            'statusFilter': 'status'
        };
        
        const filterKey = filterMap[element.id];
        if (filterKey) {
            this.filters[filterKey] = element.value;
        }
    }
    
    applyFilters() {
        console.log('Aplicando filtros:', this.filters);
        
        this.showLoading();
        
        setTimeout(() => {
            this.filterTransactions();
            this.updateActiveFiltersDisplay();
            this.hideLoading();
        }, 200);
    }
    
    filterTransactions() {
        this.filteredTransactions = this.allTransactions.filter(transaction => {
            // Filtro de pesquisa
            if (this.filters.search) {
                const searchTerm = this.filters.search.toLowerCase();
                const matchesSearch = 
                    transaction.name.toLowerCase().includes(searchTerm) ||
                    (transaction.description && transaction.description.toLowerCase().includes(searchTerm)) ||
                    (transaction.category && transaction.category.toLowerCase().includes(searchTerm)) ||
                    Math.abs(transaction.amount).toString().includes(searchTerm);
                
                if (!matchesSearch) return false;
            }
            
            // Filtro de data inicial
            if (this.filters.startDate && transaction.dueDate < this.filters.startDate) {
                return false;
            }
            
            // Filtro de data final
            if (this.filters.endDate && transaction.dueDate > this.filters.endDate) {
                return false;
            }
            
            // Filtro de categoria
            if (this.filters.category && transaction.categoryId.toString() !== this.filters.category) {
                return false;
            }
            
            // Filtro de conta
            if (this.filters.account && transaction.accountId.toString() !== this.filters.account) {
                return false;
            }
            
            // Filtro de tipo
            if (this.filters.type) {
                const filterType = this.filters.type === 'receita' ? 'income' : 'expense';
                if (transaction.type !== filterType) return false;
            }
            
            // Filtro de status
            if (this.filters.status) {
                const isCompleted = this.filters.status === 'pago';
                if (transaction.completed !== isCompleted) return false;
            }
            
            return true;
        });
        
        this.renderTransactions();
    }
    
    renderTransactions() {
        if (!this.transactionsBody || !this.emptyState) return;
        
        if (this.filteredTransactions.length === 0) {
            this.transactionsBody.innerHTML = '';
            this.emptyState.classList.remove('hidden');
            return;
        }
        
        this.emptyState.classList.add('hidden');
        
        this.transactionsBody.innerHTML = this.filteredTransactions.map(transaction => `
            <tr class="transaction-row-${transaction.type}">
                <td class="transaction-name">${transaction.name}</td>
                <td>
                    <span class="transaction-amount transaction-amount-${transaction.type}">
                        ${transaction.type === 'expense' ? '-' : '+'} ${formatCurrency(transaction.amount)}
                    </span>
                </td>
                <td class="transaction-type">
                    ${transaction.type === 'expense' ? 'Despesa' : 'Receita'}
                </td>
                <td class="transaction-date">
                    ${formatDate(transaction.dueDate)}
                </td>
                <td>
                    ${transaction.completed 
                        ? `<span class="transaction-status status-completed">
                            ${transaction.type === 'expense' ? 'Pago' : 'Recebido'}
                          </span>`
                        : `<span class="transaction-status status-pending">
                            Pendente
                          </span>`
                    }
                </td>
                <td>
                    <button class="btn-action" onclick="editTransaction(${transaction.id})" title="Editar">
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                            <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                        </svg>
                    </button>
                    <button class="btn-action btn-danger" onclick="deleteTransaction(${transaction.id})" title="Excluir">
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <polyline points="3,6 5,6 21,6"></polyline>
                            <path d="M19,6v14a2,2,0,0,1-2,2H7a2,2,0,0,1-2-2V6m3,0V4a2,2,0,0,1,2-2h4a2,2,0,0,1,2,2V6"></path>
                        </svg>
                    </button>
                </td>
            </tr>
        `).join('');
    }
    
    updateActiveFiltersDisplay() {
        if (!this.activeFilters || !this.activeFiltersList) return;
        
        const activeFilters = [];
        
        if (this.filters.search) {
            activeFilters.push({ key: 'search', label: `Pesquisa: "${this.filters.search}"` });
        }
        
        if (this.filters.startDate) {
            activeFilters.push({ key: 'startDate', label: `De: ${formatDate(this.filters.startDate)}` });
        }
        
        if (this.filters.endDate) {
            activeFilters.push({ key: 'endDate', label: `Até: ${formatDate(this.filters.endDate)}` });
        }
        
        if (this.filters.category) {
            const categoryName = this.getCategoryName(parseInt(this.filters.category));
            activeFilters.push({ key: 'category', label: `Categoria: ${categoryName}` });
        }
        
        if (this.filters.account) {
            const accountName = this.getAccountName(parseInt(this.filters.account));
            activeFilters.push({ key: 'account', label: `Conta: ${accountName}` });
        }
        
        if (this.filters.type) {
            activeFilters.push({ key: 'type', label: `Tipo: ${this.filters.type === 'receita' ? 'Receita' : 'Despesa'}` });
        }
        
        if (this.filters.status) {
            activeFilters.push({ key: 'status', label: `Status: ${this.filters.status === 'pago' ? 'Pago' : 'Pendente'}` });
        }
        
        if (activeFilters.length > 0) {
            this.activeFilters.style.display = 'flex';
            this.activeFiltersList.innerHTML = activeFilters.map(filter => `
                <div class="filter-tag">
                    ${filter.label}
                    <button class="filter-tag-remove" onclick="movementsManager.removeFilter('${filter.key}')">
                        <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <line x1="18" y1="6" x2="6" y2="18"></line>
                            <line x1="6" y1="6" x2="18" y2="18"></line>
                        </svg>
                    </button>
                </div>
            `).join('');
        } else {
            this.activeFilters.style.display = 'none';
        }
    }
    
    removeFilter(filterKey) {
        this.filters[filterKey] = '';
        
        const elementMap = {
            'search': this.searchInput,
            'startDate': this.startDateInput,
            'endDate': this.endDateInput,
            'category': this.categorySelect,
            'account': this.accountSelect,
            'type': this.typeSelect,
            'status': this.statusSelect
        };
        
        if (elementMap[filterKey]) {
            elementMap[filterKey].value = '';
            
            if (filterKey === 'search' && this.searchClear) {
                this.searchClear.style.display = 'none';
            }
        }
        
        this.applyFilters();
    }
    
    clearAllFilters() {
        Object.keys(this.filters).forEach(key => {
            this.filters[key] = '';
        });
        
        const elements = [
            this.searchInput, this.startDateInput, this.endDateInput,
            this.categorySelect, this.accountSelect, this.typeSelect, this.statusSelect
        ];
        
        elements.forEach(element => {
            if (element) element.value = '';
        });
        
        if (this.searchClear) {
            this.searchClear.style.display = 'none';
        }
        
        this.applyFilters();
    }
    
    showLoading() {
        if (this.loadingState) this.loadingState.classList.remove('hidden');
        if (this.emptyState) this.emptyState.classList.add('hidden');
    }
    
    hideLoading() {
        if (this.loadingState) this.loadingState.classList.add('hidden');
    }
    
    getCategoryName(categoryId) {
        const category = categories.find(c => c.id === categoryId);
        return category ? category.name : 'N/A';
    }
    
    getAccountName(accountId) {
        if (apiAccountsData && Array.isArray(apiAccountsData)) {
            const account = apiAccountsData.find(a => a.id === accountId);
            return account ? account.nomeConta || account.name : 'Conta não encontrada';
        }
        return 'Dados não disponíveis';
    }
    
    async loadTransactions() {
        try {
            console.log('Carregando transações...');
            this.showLoading();
            this.allTransactions = await fetchTransactionsFromApi();
            this.filteredTransactions = [...this.allTransactions];
            await this.loadFilterOptions();
            this.applyFilters();
            console.log('Transações carregadas:', this.allTransactions.length);
        } catch (error) {
            console.error('Erro ao carregar transações:', error);
            this.hideLoading();
        }
    }
    
    async loadFilterOptions() {
        // Carregar categorias
        if (this.categorySelect) {
            let categoryHtml = '<option value="">Todas as categorias</option>';
            categories.forEach(category => {
                categoryHtml += `<option value="${category.id}">${category.name}</option>`;
            });
            this.categorySelect.innerHTML = categoryHtml;
        }
        
        // Carregar contas
        if (this.accountSelect) {
            let accountHtml = '<option value="">Todas as contas</option>';
            const accountsData = await fetchAccountsBalance();
            
            if (accountsData && Array.isArray(accountsData)) {
                accountsData.forEach(account => {
                    accountHtml += `<option value="${account.id}">${account.nomeConta || account.name}</option>`;
                });
            }
            this.accountSelect.innerHTML = accountHtml;
        }
    }
}

// ===== INICIALIZAÇÃO PRINCIPAL =====
document.addEventListener('DOMContentLoaded', async function() {
    console.log('Inicializando aplicação...');
    
    // Verificar autenticação
    const token = buscaTokenJwt();
    if (!token) {
        console.warn("Usuário não autenticado, redirecionando para login");
        window.location.replace('login.html'); 
        return; 
    }

    // ===== CONFIGURAR MENU VERTICAL =====
    const verticalMenuToggle = document.getElementById("verticalMenuToggle");
    const verticalMenu = document.getElementById("verticalMenu");
    const overlay = document.getElementById("overlay");
    
    function openVerticalMenu() {
        if (verticalMenu && overlay) {
            verticalMenu.classList.add("active");
            overlay.classList.add("active");
            document.body.style.overflow = "hidden";
        } else {
            console.error("Elementos do menu vertical não encontrados");
        }
    }

    function closeVerticalMenu() {
        if (verticalMenu && overlay) {
            verticalMenu.classList.remove("active");
            overlay.classList.remove("active");
            document.body.style.overflow = "";
        }
    }

    if (verticalMenuToggle) {
        verticalMenuToggle.addEventListener("click", (e) => {
            e.stopPropagation();
            
            if (verticalMenu && verticalMenu.classList.contains("active")) {
                closeVerticalMenu();
            } else {
                openVerticalMenu();
            }
        });
    }

    if (overlay) {
        overlay.addEventListener("click", () => {
            closeVerticalMenu();
        });
    }

    document.addEventListener("click", (e) => {
        if (verticalMenuToggle && verticalMenu && 
            !verticalMenuToggle.contains(e.target) && 
            !verticalMenu.contains(e.target)) {
            closeVerticalMenu();
        }
    });

    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape") {
            closeVerticalMenu();
        }
    });

    // ===== CONFIGURAR LOGOUT =====
    const logoutBtn = document.querySelector(".logout-btn");
    if (logoutBtn) {
        logoutBtn.addEventListener("click", () => {
            localStorage.removeItem('authToken');
            sessionStorage.clear();
            window.location.replace('login.html'); 
        });
    }

    // ===== CONFIGURAR TOGGLES DE VISIBILIDADE =====
    
    // Toggle principal de visibilidade dos valores
    const toggleValuesButton = document.getElementById('toggleValues');
    if (toggleValuesButton) {
        toggleValuesButton.addEventListener('click', function() {
            showValues = !showValues;
            const eyeIcon = document.getElementById('eyeIcon');
            const eyeSlashIcon = document.getElementById('eyeSlashIcon');
            if (eyeIcon) eyeIcon.classList.toggle('hidden');
            if (eyeSlashIcon) eyeSlashIcon.classList.toggle('hidden');
            updateDashboard();
        });
    }

    // Outros botões de toggle de visibilidade
    const toggleEyeButtons = document.querySelectorAll('.toggle-eye');
    toggleEyeButtons.forEach(button => {
        button.addEventListener('click', function() {
            const icons = this.querySelectorAll('.eye-icon');
            icons.forEach(icon => icon.classList.toggle('hidden'));
            showValues = !showValues;
            updateDashboard();
        });
    });

    // ===== CONFIGURAR BOTÃO "MOSTRAR POR CONTA" =====
    const toggleAccountDetailsBtn = document.getElementById('toggleAccountDetails');
    const accountSummaryEl = document.getElementById('accountSummary');
    const accountDetailsEl = document.getElementById('accountDetails');
    
    if (toggleAccountDetailsBtn) {
        toggleAccountDetailsBtn.addEventListener('click', async function() {
            showAccountDetails = !showAccountDetails;
            
            console.log('Toggle account details:', showAccountDetails);
            
            if (showAccountDetails) {
                // Mostrar detalhes das contas
                if (accountSummaryEl) accountSummaryEl.classList.add('hidden');
                if (accountDetailsEl) accountDetailsEl.classList.remove('hidden');
                this.textContent = 'Ocultar detalhes';
                await renderAccountDetails();
            } else {
                // Mostrar resumo
                if (accountSummaryEl) accountSummaryEl.classList.remove('hidden');
                if (accountDetailsEl) accountDetailsEl.classList.add('hidden');
                this.textContent = 'Mostrar por conta';
            }
        });
    }

    // ===== INICIALIZAR SISTEMA DE FILTROS =====
    window.movementsManager = new MovementsManager();
    
    // ===== CARREGAR DADOS INICIAIS =====
    try {
        console.log('Carregando dados iniciais...');
        await updateDashboard();
        await movementsManager.loadTransactions();
        console.log('Aplicação inicializada com sucesso!');
    } catch (error) {
        console.error('Erro ao inicializar aplicação:', error);
    }
});

// ===== FUNÇÕES GLOBAIS PARA AÇÕES =====
function editTransaction(id) {
    console.log('Editar transação:', id);
    // TODO: Implementar modal de edição
    alert(`Funcionalidade de edição será implementada para a transação ${id}`);
}

function deleteTransaction(id) {
    if (confirm('Tem certeza que deseja excluir esta transação?')) {
        console.log('Excluir transação:', id);
        // TODO: Implementar chamada para API de exclusão
        alert(`Funcionalidade de exclusão será implementada para a transação ${id}`);
        
        // Após excluir, recarregar as transações
        if (window.movementsManager) {
            window.movementsManager.loadTransactions();
        }
    }
}

function completeTransaction(id) {
    console.log('Marcar transação como concluída:', id);
    // TODO: Implementar chamada para API de atualização
    alert(`Funcionalidade de conclusão será implementada para a transação ${id}`);
    
    // Após completar, recarregar as transações
    if (window.movementsManager) {
        window.movementsManager.loadTransactions();
    }
}

// ===== FUNÇÕES DE UTILIDADE =====
function refreshDashboard() {
    console.log('Atualizando dashboard...');
    updateDashboard();
    if (window.movementsManager) {
        window.movementsManager.loadTransactions();
    }
}

function exportTransactions() {
    console.log('Exportar transações...');
    // TODO: Implementar funcionalidade de exportação
    alert('Funcionalidade de exportação será implementada');
}

// ===== LOG DE INICIALIZAÇÃO =====
console.log('Dashboard JavaScript carregado com sucesso! 🚀');