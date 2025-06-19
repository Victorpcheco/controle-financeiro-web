// Mock Data - Removido mock de contas, mantido apenas categorias e métodos de pagamento
const categories = [
  { id: 1, name: 'Salário', type: 'income' },
];

const paymentMethods = [
  { id: 1, name: 'Cartão de Crédito' },
  { id: 2, name: 'Débito' },
];

let transactions = [
  {
    id: 8,
    name: 'Consulta Médica',
    description: 'Consulta de rotina',
    amount: 200,
    date: '2025-05-25',
    dueDate: '2025-05-25',
    categoryId: 7,
    accountId: 2,
    paymentMethodId: 4,
    type: 'expense',
    completed: false,
  },
  {
    id: 9,
    name: 'Freelance Programação',
    description: 'Desenvolvimento de site',
    amount: 3000,
    date: '2025-05-30',
    dueDate: '2025-05-30',
    categoryId: 2,
    accountId: 3,
    paymentMethodId: 4,
    type: 'income',
    completed: false,
  },
];

// Variáveis para armazenar dados da API
let apiAccountsData = null;
let apiTotalBalance = 0;

// Configurações da API
const API_CONFIG = {
  baseUrl: 'https://localhost:7101/api',
  endpoints: {
    totalBalance: '/financeiro/saldo-total',
    accountsBalance: '/financeiro/saldo-conta'
  }
};

// Busca o token de autenticação do localStorage
function getAuthToken() {
  const Key = ['authToken'];

  for (const key of Key) {
    const token = localStorage.getItem(key);
      return token;
  }
  return null;
}

// Busca saldo total da API
async function fetchTotalBalance() {
  try {
    const token = getAuthToken();
    
    if (!token) {
      console.warn('❌ Token de autenticação não encontrado');
      return null;
    }
    
    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };

    console.log('🔄 Buscando saldo total da API...');
    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.totalBalance}`, {
      method: 'GET',
      headers: headers
    });

    if (!response.ok) {
      let errorDetails = '';
      try {
        const errorBody = await response.text();
        errorDetails = errorBody;
        console.error('❌ Corpo da resposta de erro:', errorBody);
      } catch (e) {
        console.error('❌ Não foi possível ler o corpo da resposta de erro');
      }
      console.error(`❌ Erro na API (saldo total): ${response.status} - ${response.statusText}`);
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}${errorDetails ? '. Detalhes: ' + errorDetails : ''}`);
    }

    const data = await response.json();
    console.log('✅ Saldo total recebido da API:', data);
    
    apiTotalBalance = typeof data === 'number' ? data : data.saldo || data.balance || data.total || 0;
    console.log('💰 Saldo total processado:', apiTotalBalance);
    return apiTotalBalance;
  } catch (error) {
    console.error('❌ Erro ao buscar saldo total da API:', error.message);
    return null;
  }
}

// Busca saldos das contas da API
async function fetchAccountsBalance() {
  try {
    const token = getAuthToken();
    
    if (!token) {
      console.warn('❌ Token de autenticação não encontrado para buscar contas');
      return null;
    }
    
    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };

    console.log('🔄 Buscando saldos das contas da API...');
    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.accountsBalance}`, {
      method: 'GET',
      headers: headers
    });

    if (!response.ok) {
      let errorDetails = '';
      try {
        const errorBody = await response.text();
        errorDetails = errorBody;
        console.error('❌ Corpo da resposta de erro (contas):', errorBody);
      } catch (e) {
        console.error('❌ Não foi possível ler o corpo da resposta de erro das contas');
      }
      console.error(`❌ Erro na API (contas): ${response.status} - ${response.statusText}`);
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}${errorDetails ? '. Detalhes: ' + errorDetails : ''}`);
    }

    const data = await response.json();
    console.log('✅ Dados das contas recebidos da API:', data);
    
    apiAccountsData = data;
    return data;
  } catch (error) {
    console.error('❌ Erro ao buscar saldos das contas da API:', error.message);
    return null;
  }
}

// Elementos do DOM
// ---------------------------------
document.addEventListener('DOMContentLoaded', function() {
  // Mobile menu toggle
  const mobileMenuToggle = document.getElementById('mobileMenuToggle');
  const mobileMenu = document.getElementById('mobileMenu');
  
  // Cards and visibility toggles
  const toggleValuesButton = document.getElementById('toggleValues');
  const eyeIcon = document.getElementById('eyeIcon');
  const eyeSlashIcon = document.getElementById('eyeSlashIcon');
  const toggleEyeButtons = document.querySelectorAll('.toggle-eye');
  const totalBalanceEl = document.getElementById('totalBalance');
  const pendingIncomesEl = document.getElementById('pendingIncomes');
  const pendingExpensesEl = document.getElementById('pendingExpenses');
  
  // Account details
  const toggleAccountDetailsBtn = document.getElementById('toggleAccountDetails');
  const accountSummaryEl = document.getElementById('accountSummary');
  const accountDetailsEl = document.getElementById('accountDetails');
  
  // Filters
  const toggleFiltersBtn = document.getElementById('toggleFilters');
  const filterBodyEl = document.getElementById('filterBody');
  const startDateInput = document.getElementById('startDate');
  const endDateInput = document.getElementById('endDate');
  const categoryFilterEl = document.getElementById('categoryFilter');
  const accountFilterEl = document.getElementById('accountFilter');
  const applyFiltersBtn = document.getElementById('applyFilters');
  const clearFiltersBtn = document.getElementById('clearFilters');
  
  // Transactions
  const transactionsBodyEl = document.getElementById('transactionsBody');
  const emptyStateEl = document.getElementById('emptyState');
  
  let showValues = true;
  let showAccountDetails = false;
  let expandedTransaction = null;
  let filteredTransactions = [...transactions];
  let activeFilters = {
    startDate: '',
    endDate: '',
    categoryId: null,
    accountId: null
  };
  
  // Utilidades
  // ---------------------------------
  
  // Formatar moeda
  function formatCurrency(value) {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value);
  }
  
  // Formatar data
  function formatDate(dateString) {
    const options = { day: '2-digit', month: '2-digit', year: 'numeric' };
    return new Date(dateString).toLocaleDateString('pt-BR', options);
  }
  
  // Obter nome da categoria pelo ID
  function getCategoryName(categoryId) {
    const category = categories.find(c => c.id === categoryId);
    return category ? category.name : 'N/A';
  }
  
  // Obter nome da conta pelo ID - Agora usa dados da API
  function getAccountName(accountId) {
    if (apiAccountsData && apiAccountsData.sucesso && Array.isArray(apiAccountsData.dados)) {
      const account = apiAccountsData.dados.find(a => a.id === accountId || a.contaId === accountId);
      return account ? (account.nomeConta || account.name) : 'Conta não encontrada';
    }
    return 'Dados não disponíveis';
  }
  
  // Obter nome do método de pagamento pelo ID
  function getPaymentMethodName(methodId) {
    const method = paymentMethods.find(m => m.id === methodId);
    return method ? method.name : 'N/A';
  }
  
  // Calcular saldo total - Usa apenas a API
  async function getTotalBalance() {
    const apiBalance = await fetchTotalBalance();
    
    if (apiBalance !== null) {
      return apiBalance;
    }
    
    console.warn('⚠️ Não foi possível obter o saldo total da API. Retornando 0.');
    return 0;
  }
  
  // Calcular receitas pendentes
  function getPendingIncomes() {
    return transactions
      .filter(t => t.type === 'income' && !t.completed)
      .reduce((total, t) => total + t.amount, 0);
  }
  
  // Calcular despesas pendentes
  function getPendingExpenses() {
    return transactions
      .filter(t => t.type === 'expense' && !t.completed)
      .reduce((total, t) => total + t.amount, 0);
  }
  
  // Marcar transação como concluída - Removida atualização de saldo local
  function completeTransaction(transactionId) {
    const transaction = transactions.find(t => t.id === transactionId);
    
    if (!transaction || transaction.completed) return;
  
    transaction.completed = true;
    
    console.log(`✅ Transação ${transactionId} marcada como concluída`);
    
    // Atualizar a interface (o saldo será atualizado na próxima busca da API)
    updateDashboard();
    renderTransactions();
  }
  
  // Filtrar transações
  function filterTransactions() {
    filteredTransactions = transactions.filter(transaction => {
      // Filtro por data
      if (activeFilters.startDate && new Date(transaction.dueDate) < new Date(activeFilters.startDate)) {
        return false;
      }
      if (activeFilters.endDate && new Date(transaction.dueDate) > new Date(activeFilters.endDate)) {
        return false;
      }
      // Filtro por categoria
      if (activeFilters.categoryId && transaction.categoryId !== activeFilters.categoryId) {
        return false;
      }
      // Filtro por conta
      if (activeFilters.accountId && transaction.accountId !== activeFilters.accountId) {
        return false;
      }
      return true;
    });
    
    renderTransactions();
  }
  
  // Iniciar Aplicação
  // ---------------------------------
  
  // Inicializar a interface
  function initApp() {
    populateCategoryFilter();
    populateAccountFilter();
    updateDashboard();
    renderTransactions();
    setupEventListeners();
  }
  
  // Atualizar o dashboard
  async function updateDashboard() {
    // Atualizar card de saldo total (vem da API)
    const totalBalance = await getTotalBalance();
    updateValueDisplay(totalBalanceEl, totalBalance);
    
    // Manter os outros cards com dados locais
    updateValueDisplay(pendingIncomesEl, getPendingIncomes());
    updateValueDisplay(pendingExpensesEl, getPendingExpenses());
    
    // Atualizar detalhes das contas
    renderAccountDetails();
  }
  
  // Atualizar exibição de valores com base na visibilidade
  function updateValueDisplay(element, value) {
    if (showValues) {
      element.textContent = formatCurrency(value);
    } else {
      element.textContent = '••••••';
    }
  }
  
  // Renderizar detalhes das contas - Usa apenas dados da API
  async function renderAccountDetails() {
    if (!showAccountDetails) return;
    
    console.log('🔄 Renderizando detalhes das contas...');
    
    // Buscar dados da API
    const apiAccountsData = await fetchAccountsBalance();
    
    let html = '';
    
    if (apiAccountsData && apiAccountsData.sucesso && Array.isArray(apiAccountsData.dados)) {
      console.log('✅ Usando dados das contas da API');
      apiAccountsData.dados.forEach(account => {
        html += `
          <div class="account-item">
            <span>${account.nomeConta}</span>
            <span>${showValues ? formatCurrency(account.saldoAtual) : '••••••'}</span>
          </div>
        `;
      });
    } else {
      console.error('❌ Falha ao carregar dados das contas da API');
      html = `
        <div class="account-item error">
          <span>Erro ao carregar contas</span>
          <span>-</span>
        </div>
      `;
    }
    
    // Para o total, usa o saldo total da API
    const totalBalance = await getTotalBalance();
    html += `
      <div class="account-total">
        <span>Total</span>
        <span>${showValues ? formatCurrency(totalBalance) : '••••••'}</span>
      </div>
    `;
    
    accountDetailsEl.innerHTML = html;
  }
  
  // Preencher filtro de categorias
  function populateCategoryFilter() {
    let html = '<option value="">Todas as categorias</option>';
    
    categories.forEach(category => {
      html += `
        <option value="${category.id}">
          ${category.name} (${category.type === 'income' ? 'Receita' : 'Despesa'})
        </option>
      `;
    });
    
    categoryFilterEl.innerHTML = html;
  }
  
  // Preencher filtro de contas - Usa dados da API
  async function populateAccountFilter() {
    let html = '<option value="">Todas as contas</option>';
    
    // Buscar contas da API
    const apiAccountsData = await fetchAccountsBalance();
    
    if (apiAccountsData && apiAccountsData.sucesso && Array.isArray(apiAccountsData.dados)) {
      apiAccountsData.dados.forEach(account => {
        html += `<option value="${account.id || account.contaId}">${account.nomeConta}</option>`;
      });
    } else {
      console.warn('⚠️ Não foi possível carregar contas para o filtro');
      html += '<option value="" disabled>Erro ao carregar contas</option>';
    }
    
    accountFilterEl.innerHTML = html;
  }
  
  // Renderizar transações
  function renderTransactions() {
    if (filteredTransactions.length === 0) {
      transactionsBodyEl.innerHTML = '';
      emptyStateEl.classList.remove('hidden');
      return;
    }
    
    emptyStateEl.classList.add('hidden');
    
    let html = '';
    filteredTransactions.forEach(transaction => {
      const isExpanded = expandedTransaction === transaction.id;
      
      html += `
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
            <button
              class="details-button"
              data-id="${transaction.id}"
            >
              ${isExpanded ? 'Ocultar' : 'Detalhes'}
            </button>
          </td>
        </tr>
        ${isExpanded ? renderTransactionDetails(transaction) : ''}
      `;
    });
    
    transactionsBodyEl.innerHTML = html;
    
    // Adicionar listeners para os botões de detalhes
    document.querySelectorAll('.details-button').forEach(button => {
      button.addEventListener('click', function() {
        const id = parseInt(this.getAttribute('data-id'), 10);
        toggleExpand(id);
      });
    });
    
    // Adicionar listeners para os botões de marcar como concluído
    document.querySelectorAll('.complete-transaction').forEach(button => {
      button.addEventListener('click', function() {
        const id = parseInt(this.getAttribute('data-id'), 10);
        completeTransaction(id);
      });
    });
  }
  
  // Renderizar detalhes da transação
  function renderTransactionDetails(transaction) {
    return `
      <tr>
        <td colspan="6">
          <div class="transaction-details">
            <div class="details-grid">
              <div class="details-item">
                <p class="details-label">Descrição:</p>
                <p class="details-value">${transaction.description}</p>
              </div>
              <div class="details-item">
                <p class="details-label">Categoria:</p>
                <p class="details-value">${getCategoryName(transaction.categoryId)}</p>
              </div>
              <div class="details-item">
                <p class="details-label">Conta:</p>
                <p class="details-value">${getAccountName(transaction.accountId)}</p>
              </div>
              <div class="details-item">
                <p class="details-label">Forma de Pagamento:</p>
                <p class="details-value">${getPaymentMethodName(transaction.paymentMethodId)}</p>
              </div>
              <div class="details-item">
                <p class="details-label">Data de Criação:</p>
                <p class="details-value">${formatDate(transaction.date)}</p>
              </div>
              <div class="details-item">
                <p class="details-label">Data de Vencimento:</p>
                <p class="details-value">${formatDate(transaction.dueDate)}</p>
              </div>
            </div>
            
            ${!transaction.completed ? `
              <div class="details-actions">
                <button
                  class="button button-primary complete-transaction"
                  data-id="${transaction.id}"
                >
                  Marcar como ${transaction.type === 'expense' ? 'Pago' : 'Recebido'}
                </button>
              </div>
            ` : ''}
          </div>
        </td>
      </tr>
    `;
  }
  
  // Alternar expansão dos detalhes da transação
  function toggleExpand(id) {
    expandedTransaction = expandedTransaction === id ? null : id;
    renderTransactions();
  }
  
  // Configurar event listeners
  function setupEventListeners() {
    // Mobile menu
    mobileMenuToggle.addEventListener('click', function() {
      mobileMenu.classList.toggle('active');
      
      // Mudar o ícone do botão
      const menuIcon = this.querySelector('.menu-icon');
      if (mobileMenu.classList.contains('active')) {
        menuIcon.textContent = '×';
      } else {
        menuIcon.textContent = '☰';
      }
    });
    
    // Toggle de visibilidade dos valores
    toggleValuesButton.addEventListener('click', function() {
      showValues = !showValues;
      eyeIcon.classList.toggle('hidden');
      eyeSlashIcon.classList.toggle('hidden');
      
      // Atualizar a dashboard com a nova configuração
      updateDashboard();
    });
    
    // Toggle de visibilidade para outros botões de olho
    toggleEyeButtons.forEach(button => {
      button.addEventListener('click', function() {
        const icons = this.querySelectorAll('.eye-icon');
        icons.forEach(icon => icon.classList.toggle('hidden'));
        
        showValues = !showValues;
        updateDashboard();
      });
    });
    
    // Toggle de detalhes da conta
    toggleAccountDetailsBtn.addEventListener('click', async function() {
      showAccountDetails = !showAccountDetails;
      
      if (showAccountDetails) {
        accountSummaryEl.classList.add('hidden');
        accountDetailsEl.classList.remove('hidden');
        this.textContent = 'Ocultar detalhes';
        await renderAccountDetails();
      } else {
        accountSummaryEl.classList.remove('hidden');
        accountDetailsEl.classList.add('hidden');
        this.textContent = 'Mostrar por conta';
      }
    });
    
    // Toggle de filtros
    toggleFiltersBtn.addEventListener('click', function() {
      filterBodyEl.classList.toggle('active');
    });
    
    // Aplicar filtros
    applyFiltersBtn.addEventListener('click', function() {
      activeFilters.startDate = startDateInput.value;
      activeFilters.endDate = endDateInput.value;
      activeFilters.categoryId = categoryFilterEl.value ? parseInt(categoryFilterEl.value, 10) : null;
      activeFilters.accountId = accountFilterEl.value ? parseInt(accountFilterEl.value, 10) : null;
      
      filterTransactions();
    });
    
    // Limpar filtros
    clearFiltersBtn.addEventListener('click', function() {
      startDateInput.value = '';
      endDateInput.value = '';
      categoryFilterEl.value = '';
      accountFilterEl.value = '';
      
      activeFilters = {
        startDate: '',
        endDate: '',
        categoryId: null,
        accountId: null
      };
      
      filteredTransactions = [...transactions];
      renderTransactions();
    });
  }
  
  // Iniciar a aplicação
  initApp();
});