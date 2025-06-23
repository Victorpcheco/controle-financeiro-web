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
let apiPendingIncomes = 0;
let apiPendingExpenses = 0;

// Configurações da API
const API_CONFIG = {
  baseUrl: 'https://localhost:7101/api',
  endpoints: {
    totalBalance: '/dashboard/saldo-total',
    accountsBalance: '/dashboard/saldo-contas',
    pendingIncomes: '/dashboard/valor-em-aberto-receitas',
    pendingExpenses: '/dashboard/valor-em-aberto-despesas',
    transactions: '/dashboard/movimentacoes-em-aberto'
  }
};

function checkAuthentication() {
  const token = getAuthToken();
  
  if (!token) {
    console.warn(" Usuário não autenticado, redirecionando para login");
    window.location.replace('/.login.html');
    return false;
  }
  
  return true;
}

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

// Busca receitas pendentes da API
async function fetchPendingIncomes() {
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

    console.log('🔄 Buscando receitas pendentes da API...');
    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.pendingIncomes}`, {
      method: 'GET',
      headers: headers
    });

    if (!response.ok) {
      let errorDetails = '';
      try {
        const errorBody = await response.text();
        errorDetails = errorBody;
        console.error('❌ Corpo da resposta de erro (receitas pendentes):', errorBody);
      } catch (e) {
        console.error('❌ Não foi possível ler o corpo da resposta de erro das receitas pendentes');
      }
      console.error(`❌ Erro na API (receitas pendentes): ${response.status} - ${response.statusText}`);
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}${errorDetails ? '. Detalhes: ' + errorDetails : ''}`);
    }

    const data = await response.json();
    console.log('✅ Receitas pendentes recebidas da API:', data);
    
    apiPendingIncomes = typeof data === 'number' ? data : data.receitas || data.pendingIncomes || data.total || 0;
    console.log('💰 Receitas pendentes processadas:', apiPendingIncomes);
    return apiPendingIncomes;
  } catch (error) {
    console.error('❌ Erro ao buscar receitas pendentes da API:', error.message);
    return null;
  }
}

// Busca despesas pendentes da API
async function fetchPendingExpenses() {
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

    console.log('🔄 Buscando despesas pendentes da API...');
    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.pendingExpenses}`, {
      method: 'GET',
      headers: headers
    });

    if (!response.ok) {
      let errorDetails = '';
      try {
        const errorBody = await response.text();
        errorDetails = errorBody;
        console.error('❌ Corpo da resposta de erro (despesas pendentes):', errorBody);
      } catch (e) {
        console.error('❌ Não foi possível ler o corpo da resposta de erro das despesas pendentes');
      }
      console.error(`❌ Erro na API (despesas pendentes): ${response.status} - ${response.statusText}`);
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}${errorDetails ? '. Detalhes: ' + errorDetails : ''}`);
    }

    const data = await response.json();
    console.log('✅ Despesas pendentes recebidas da API:', data);
    
    apiPendingExpenses = typeof data === 'number' ? data : data.despesas || data.pendingExpenses || data.total || 0;
    console.log('💰 Despesas pendentes processadas:', apiPendingExpenses);
    return apiPendingExpenses;
  } catch (error) {
    console.error('❌ Erro ao buscar despesas pendentes da API:', error.message);
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

// Função para buscar transações da API e converter para o formato do front
async function fetchTransactionsFromApi() {
  try {
    const token = getAuthToken();
    if (!token) {
      console.warn('❌ Token de autenticação não encontrado');
      return [];
    }
    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };
    console.log('🔄 Buscando transações da API...');
    const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.transactions}`, {
      method: 'GET',
      headers: headers
    });
    if (!response.ok) {
      let errorDetails = '';
      try {
        const errorBody = await response.text();
        errorDetails = errorBody;
        console.error('❌ Corpo da resposta de erro (transações):', errorBody);
      } catch (e) {
        console.error('❌ Não foi possível ler o corpo da resposta de erro das transações');
      }
      throw new Error(`Erro na API: ${response.status} - ${response.statusText}${errorDetails ? '. Detalhes: ' + errorDetails : ''}`);
    }
    const data = await response.json();
    const apiTransactions = Array.isArray(data) ? data : [data];
    return apiTransactions.map(apiT => ({
      id: apiT.id,
      name: apiT.titulo,
      description: '',
      amount: apiT.valor,
      date: apiT.dataVencimento,
      dueDate: apiT.dataVencimento,
      categoryId: apiT.categoriaId,
      accountId: apiT.contaBancariaId,
      paymentMethodId: apiT.cartaoId || null,
      type: apiT.tipo && apiT.tipo.toLowerCase() === 'despesa' ? 'expense' : 'income',
      completed: !!apiT.realizado
    }));
  } catch (error) {
    console.error('❌ Erro ao buscar transações da API:', error.message);
    return [];
  }
}

// Elementos do DOM e inicialização
document.addEventListener('DOMContentLoaded', async function() {

  // ✅ ADICIONE ESTAS LINHAS NO INÍCIO
  // Verificar se o usuário está autenticado
  const token = getAuthToken();
  if (!token) {
    console.warn("⚠️ Usuário não autenticado, redirecionando para login");
    window.location.replace('login.html'); // MUDE PARA O NOME DA SUA PÁGINA DE LOGIN
    return; // Para a execução
  }

  // ELEMENTOS DO MENU VERTICAL
  const verticalMenuToggle = document.getElementById("verticalMenuToggle");
  const verticalMenu = document.getElementById("verticalMenu");
  const overlay = document.getElementById("overlay");
  
  // FUNÇÕES DO MENU VERTICAL (DENTRO DO DOMContentLoaded PARA TER ACESSO AOS ELEMENTOS)
  function openVerticalMenu() {
    if (verticalMenu && overlay) {
      verticalMenu.classList.add("active");
      overlay.classList.add("active");
      document.body.style.overflow = "hidden";
      console.log("Menu vertical aberto");
    } else {
      console.error("Elementos do menu vertical não encontrados");
    }
  }

  function closeVerticalMenu() {
    if (verticalMenu && overlay) {
      verticalMenu.classList.remove("active");
      overlay.classList.remove("active");
      document.body.style.overflow = "";
      console.log("Menu vertical fechado");
    }
  }

  // EVENT LISTENERS DO MENU VERTICAL
  if (verticalMenuToggle) {
    console.log("✅ Botão do menu vertical encontrado, adicionando event listener");
    verticalMenuToggle.addEventListener("click", (e) => {
      console.log("🔄 Clique no botão do menu vertical detectado");
      e.stopPropagation();
      
      if (verticalMenu && verticalMenu.classList.contains("active")) {
        console.log("Menu está aberto, fechando...");
        closeVerticalMenu();
      } else {
        console.log("Menu está fechado, abrindo...");
        openVerticalMenu();
      }
    });
  } else {
    console.error("❌ Botão do menu vertical não encontrado! Verifique se o ID 'verticalMenuToggle' existe no HTML");
  }

  if (overlay) {
    overlay.addEventListener("click", () => {
      console.log("Clique no overlay detectado, fechando menu");
      closeVerticalMenu();
    });
  }

  // Event listener para fechar o menu quando clicar fora dele
  document.addEventListener("click", (e) => {
    if (verticalMenuToggle && verticalMenu && 
        !verticalMenuToggle.contains(e.target) && 
        !verticalMenu.contains(e.target)) {
      closeVerticalMenu();
    }
  });

  // Fechar menu com a tecla ESC
  document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
      closeVerticalMenu();
    }
  });

  // Event listener para o botão de logout
  const logoutBtn = document.querySelector(".logout-btn");
    if (logoutBtn) {
      logoutBtn.addEventListener("click", () => {
     {
      // 1. Remover token de autenticação
      localStorage.removeItem('authToken');
      
      // 2. Limpar outros dados se houver
      sessionStorage.clear();
      
      // 3. Redirecionar SEM permitir voltar
      window.location.replace('login.html'); // ✅ MUDE PARA O NOME DA SUA PÁGINA DE LOGIN
      
      console.log("✅ Logout realizado com segurança");
      }
    });
  } 

  // Adicionar efeito hover nos links do menu
  const menuLinks = document.querySelectorAll(".vertical-menu-link, .vertical-submenu-link");
  console.log(`✅ Encontrados ${menuLinks.length} links do menu`);
  menuLinks.forEach((link) => {
    link.addEventListener("click", function (e) {
      e.preventDefault();
      closeVerticalMenu();
      console.log("Navegando para:", this.textContent.trim());
    });
  });
  
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
  let filteredTransactions = [];
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
  
  // Obter nome da conta pelo ID
  function getAccountName(accountId) {
    if (apiAccountsData && Array.isArray(apiAccountsData)) {
      const account = apiAccountsData.find(a => a.id === accountId);
      return account ? account.nomeConta : 'Conta não encontrada';
    }
    return 'Dados não disponíveis';
  }
  
  // Obter nome do método de pagamento pelo ID
  function getPaymentMethodName(methodId) {
    const method = paymentMethods.find(m => m.id === methodId);
    return method ? method.name : 'N/A';
  }
  
  // Calcular saldo total
  async function getTotalBalance() {
    const apiBalance = await fetchTotalBalance();
    if (apiBalance !== null) {
      return apiBalance;
    }
    console.warn('⚠️ Não foi possível obter o saldo total da API. Retornando 0.');
    return 0;
  }
  
  // Calcular receitas pendentes
  async function getPendingIncomes() {
    const apiIncomes = await fetchPendingIncomes();
    if (apiIncomes !== null) {
      return apiIncomes;
    }
    console.warn('⚠️ Não foi possível obter receitas pendentes da API. Usando dados locais.');
    return transactions
      .filter(t => t.type === 'income' && !t.completed)
      .reduce((total, t) => total + t.amount, 0);
  }
  
  // Calcular despesas pendentes
  async function getPendingExpenses() {
    const apiExpenses = await fetchPendingExpenses();
    if (apiExpenses !== null) {
      return apiExpenses;
    }
    console.warn('⚠️ Não foi possível obter despesas pendentes da API. Usando dados locais.');
    return transactions
      .filter(t => t.type === 'expense' && !t.completed)
      .reduce((total, t) => total + t.amount, 0);
  }
  
  // Marcar transação como concluída
  function completeTransaction(transactionId) {
    const transaction = transactions.find(t => t.id === transactionId);
    if (!transaction || transaction.completed) return;
    transaction.completed = true;
    console.log(`✅ Transação ${transactionId} marcada como concluída`);
    updateDashboard();
    renderTransactions();
  }
  
  // Filtrar transações
  function filterTransactions() {
    filteredTransactions = transactions.filter(transaction => {
      if (activeFilters.startDate && new Date(transaction.dueDate) < new Date(activeFilters.startDate)) {
        return false;
      }
      if (activeFilters.endDate && new Date(transaction.dueDate) > new Date(activeFilters.endDate)) {
        return false;
      }
      if (activeFilters.categoryId && transaction.categoryId !== activeFilters.categoryId) {
        return false;
      }
      if (activeFilters.accountId && transaction.accountId !== activeFilters.accountId) {
        return false;
      }
      return true;
    });
    renderTransactions();
  }
  
  // Inicializar a interface
  async function initApp() {
    transactions = await fetchTransactionsFromApi();
    filteredTransactions = [...transactions];
    populateCategoryFilter();
    await populateAccountFilter();
    await updateDashboard();
    renderTransactions();
    setupEventListeners();
  }
  
  // Atualizar o dashboard
  async function updateDashboard() {
    const totalBalance = await getTotalBalance();
    updateValueDisplay(totalBalanceEl, totalBalance);
    
    const pendingIncomes = await getPendingIncomes();
    updateValueDisplay(pendingIncomesEl, pendingIncomes);
    
    const pendingExpenses = await getPendingExpenses();
    updateValueDisplay(pendingExpensesEl, pendingExpenses);
    
    renderAccountDetails();
  }
  
  // Atualizar exibição de valores com base na visibilidade
  function updateValueDisplay(element, value) {
    if (element) {
      if (showValues) {
        element.textContent = formatCurrency(value);
      } else {
        element.textContent = '••••••';
      }
    }
  }
  
  // Renderizar detalhes das contas
  async function renderAccountDetails() {
    if (!showAccountDetails || !accountDetailsEl) return;
    
    console.log('🔄 Renderizando detalhes das contas...');
    const accountsData = await fetchAccountsBalance();
    let html = '';
    
    if (accountsData && Array.isArray(accountsData)) {
      console.log('✅ Usando dados das contas da API');
      accountsData.forEach(account => {
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
    if (!categoryFilterEl) return;
    
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
  
  // Preencher filtro de contas
  async function populateAccountFilter() {
    if (!accountFilterEl) return;
    
    let html = '<option value="">Todas as contas</option>';
    const accountsData = await fetchAccountsBalance();
    
    if (accountsData && Array.isArray(accountsData)) {
      accountsData.forEach(account => {
        html += `<option value="${account.id}">${account.nomeConta}</option>`;
      });
    } else {
      console.warn('⚠️ Não foi possível carregar contas para o filtro');
      html += '<option value="" disabled>Erro ao carregar contas</option>';
    }
    
    accountFilterEl.innerHTML = html;
  }
  
  // Renderizar transações
  function renderTransactions() {
    if (!transactionsBodyEl || !emptyStateEl) return;
    
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
    if (mobileMenuToggle && mobileMenu) {
      mobileMenuToggle.addEventListener('click', function() {
        mobileMenu.classList.toggle('active');
        const menuIcon = this.querySelector('.menu-icon');
        if (mobileMenu.classList.contains('active')) {
          menuIcon.textContent = '×';
        } else {
          menuIcon.textContent = '☰';
        }
      });
    }
    
    // Toggle de visibilidade dos valores
    if (toggleValuesButton) {
      toggleValuesButton.addEventListener('click', function() {
        showValues = !showValues;
        if (eyeIcon) eyeIcon.classList.toggle('hidden');
        if (eyeSlashIcon) eyeSlashIcon.classList.toggle('hidden');
        updateDashboard();
      });
    }
    
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
    if (toggleAccountDetailsBtn) {
      toggleAccountDetailsBtn.addEventListener('click', async function() {
        showAccountDetails = !showAccountDetails;
        
        if (showAccountDetails) {
          if (accountSummaryEl) accountSummaryEl.classList.add('hidden');
          if (accountDetailsEl) accountDetailsEl.classList.remove('hidden');
          this.textContent = 'Ocultar detalhes';
          await renderAccountDetails();
        } else {
          if (accountSummaryEl) accountSummaryEl.classList.remove('hidden');
          if (accountDetailsEl) accountDetailsEl.classList.add('hidden');
          this.textContent = 'Mostrar por conta';
        }
      });
    }
    
    // Toggle de filtros
    if (toggleFiltersBtn && filterBodyEl) {
      toggleFiltersBtn.addEventListener('click', function() {
        filterBodyEl.classList.toggle('active');
      });
    }
    
    // Aplicar filtros
    if (applyFiltersBtn) {
      applyFiltersBtn.addEventListener('click', function() {
        activeFilters.startDate = startDateInput ? startDateInput.value : '';
        activeFilters.endDate = endDateInput ? endDateInput.value : '';
        activeFilters.categoryId = categoryFilterEl && categoryFilterEl.value ? parseInt(categoryFilterEl.value, 10) : null;
        activeFilters.accountId = accountFilterEl && accountFilterEl.value ? parseInt(accountFilterEl.value, 10) : null;
        filterTransactions();
      });
    }
    
    // Limpar filtros
    if (clearFiltersBtn) {
      clearFiltersBtn.addEventListener('click', function() {
        if (startDateInput) startDateInput.value = '';
        if (endDateInput) endDateInput.value = '';
        if (categoryFilterEl) categoryFilterEl.value = '';
        if (accountFilterEl) accountFilterEl.value = '';
        
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
  }
  
  initApp();
  
});