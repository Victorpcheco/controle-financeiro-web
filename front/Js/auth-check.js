// Aguarda o carregamento completo do conteúdo da página (DOM)
document.addEventListener('DOMContentLoaded', () => {

  // Recupera o token de autenticação salvo no localStorage (caso exista)
  const authToken = localStorage.getItem('authToken');
  
  // Verifica se o token não existe (usuário não autenticado)
  if (!authToken) {
    // Redireciona o usuário para a página de login
    window.location.href = './login.html';
  }
});
