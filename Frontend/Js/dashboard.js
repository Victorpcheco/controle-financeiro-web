document.addEventListener('DOMContentLoaded', () => {
  const logoutButton = document.getElementById('logout-button');
  
  logoutButton.addEventListener('click', () => {
    // Clear tokens from localStorage
    localStorage.removeItem('authToken');
    localStorage.removeItem('refreshToken');
    
    // Show logout toast
    showToast('Logout realizado com sucesso!', 'success');
    
    // Redirect to login page
    setTimeout(() => {
      window.location.href = './login.html';
    }, 1000);
  });
});