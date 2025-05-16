// Mock Data
// ---------------------------------

// Contas bancárias simuladas
const accounts = [
  { id: 1, name: 'NuBank', balance: 3500 },
  { id: 2, name: 'Banco do Brasil', balance: 7200 },
  { id: 3, name: 'Caixa Econômica', balance: 1450 },
];

// Categorias simuladas
const categories = [
  { id: 1, name: 'Salário', type: 'income' },
  { id: 2, name: 'Freelance', type: 'income' },
  { id: 3, name: 'Alimentação', type: 'expense' },
  { id: 4, name: 'Transporte', type: 'expense' },
  { id: 5, name: 'Lazer', type: 'expense' },
  { id: 6, name: 'Moradia', type: 'expense' },
  { id: 7, name: 'Saúde', type: 'expense' },
  { id: 8, name: 'Educação', type: 'expense' },
];

// Formas de pagamento simuladas
const paymentMethods = [
  { id: 1, name: 'Cartão de Crédito' },
  { id: 2, name: 'Débito' },
  { id: 3, name: 'Dinheiro' },
  { id: 4, name: 'PIX' },
  { id: 5, name: 'Transferência' },
];

// Transações simuladas
let transactions = [
  {
    id: 1,
    name: 'Salário Mensal',
    description: 'Salário referente ao mês de maio',
    amount: 4500,
    date: '2025-05-05',
    dueDate: '2025-05-10',
    categoryId: 1,
    accountId: 1,
    paymentMethodId: 5,
    type: 'income',
    completed: true,
  },
  {
    id: 2,
    name: 'Freelance Design',
    description: 'Projeto de design para cliente XYZ',
    amount: 1200,
    date: '2025-05-08',
    dueDate: '2025-05-08',
    categoryId: 2,
    accountId: 1,
    paymentMethodId: 4,
    type: 'income',
    completed: false,
  },
  {
    id: 3,
    name: 'Supermercado',
    description: 'Compras do mês',
    amount: 650.75,
    date: '2025-05-12',
    dueDate: '2025-05-12',
    categoryId: 3,
    accountId: 2,
    paymentMethodId: 1,
    type: 'expense',
    completed: true,
  },
  {
    id: 4,
    name: 'Aluguel',
    description: 'Aluguel do apartamento',
    amount: 1800,
    date: '2025-05-10',
    dueDate: '2025-05-15',
    categoryId: 6,
    accountId: 2,
    paymentMethodId: 5,
    type: 'expense',
    completed: false,
  },
  {
    id: 5,
    name: 'Gasolina',
    description: 'Abastecimento do carro',
    amount: 250.43,
    date: '2025-05-07',
    dueDate: '2025-05-07',
    categoryId: 4,
    accountId: 3,
    paymentMethodId: 2,
    type: 'expense',
    completed: true,
  },
  {
    id: 6,
    name: 'Cinemark',
    description: 'Ingressos para cinema',
    amount: 75.80,
    date: '2025-05-16',
    dueDate: '2025-05-16',
    categoryId: 5,
    accountId: 1,
    paymentMethodId: 1,
    type: 'expense',
    completed: false,
  },
  {
    id: 7,
    name: 'Curso Programação',
    description: 'Curso de JavaScript Avançado',
    amount: 399.90,
    date: '2025-05-20',
    dueDate: '2025-05-20',
    categoryId: 8,
    accountId: 1,
    paymentMethodId: 1,
    type: 'expense',
    completed: false,
  },
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
  
  // Obter nome da conta pelo ID
  function getAccountName(accountId) {
    const account = accounts.find(a => a.id === accountId);
    return account ? account.name : 'N/A';
  }
  
  // Obter nome do método de pagamento pelo ID
  function getPaymentMethodName(methodId) {
    const method = paymentMethods.find(m => m.id === methodId);
    return method ? method.name : 'N/A';
  }
  
  // Calcular saldo total
  function getTotalBalance() {
    return accounts.reduce((total, account) => total + account.balance, 0);
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
  
  // Marcar transação como concluída
  function completeTransaction(transactionId) {
    const transaction = transactions.find(t => t.id === transactionId);
    
    if (!transaction || transaction.completed) return;
  
    transaction.completed = true;
    
    // Atualizar o saldo da conta
    const account = accounts.find(a => a.id === transaction.accountId);
    if (account) {
      if (transaction.type === 'income') {
        account.balance += transaction.amount;
      } else {
        account.balance -= transaction.amount;
      }
    }
    
    // Atualizar a interface
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
  function updateDashboard() {
    // Atualizar cards de resumo
    updateValueDisplay(totalBalanceEl, getTotalBalance());
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
  
  // Renderizar detalhes das contas
  function renderAccountDetails() {
    if (!showAccountDetails) return;
    
    let html = '';
    accounts.forEach(account => {
      html += `
        <div class="account-item">
          <span>${account.name}</span>
          <span>${showValues ? formatCurrency(account.balance) : '••••••'}</span>
        </div>
      `;
    });
    
    html += `
      <div class="account-total">
        <span>Total</span>
        <span>${showValues ? formatCurrency(getTotalBalance()) : '••••••'}</span>
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
  
  // Preencher filtro de contas
  function populateAccountFilter() {
    let html = '<option value="">Todas as contas</option>';
    
    accounts.forEach(account => {
      html += `<option value="${account.id}">${account.name}</option>`;
    });
    
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
    toggleAccountDetailsBtn.addEventListener('click', function() {
      showAccountDetails = !showAccountDetails;
      
      if (showAccountDetails) {
        accountSummaryEl.classList.add('hidden');
        accountDetailsEl.classList.remove('hidden');
        this.textContent = 'Ocultar detalhes';
      } else {
        accountSummaryEl.classList.remove('hidden');
        accountDetailsEl.classList.add('hidden');
        this.textContent = 'Mostrar por conta';
      }
      
      renderAccountDetails();
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