// Função para exibir uma notificação tipo "toast" na tela
// Recebe a mensagem a ser exibida e o tipo da notificação (default: 'success')
function showToast(message, type = 'success') {
  // Pega o elemento HTML com id 'toast' (container da notificação)
  const toast = document.getElementById('toast');
  
  // Pega o elemento onde a mensagem será exibida, com id 'toast-message'
  const toastMessage = document.getElementById('toast-message');
  
  // Define o texto da mensagem dentro do elemento toast-message
  toastMessage.textContent = message;
  
  // Define as classes CSS para mostrar o toast e aplicar o estilo conforme o tipo ('success', 'error', etc)
  toast.className = 'toast show ' + type;
  
  // Após 3 segundos, remove a classe 'show' para esconder a notificação automaticamente
  setTimeout(() => {
    toast.className = toast.className.replace('show', '');
  }, 3000);
}
