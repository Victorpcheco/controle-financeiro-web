// Check if user is authenticated
document.addEventListener('DOMContentLoaded', () => {
  const authToken = localStorage.getItem('authToken');
  
  if (!authToken) {
    // User is not logged in, redirect to login page
    window.location.href = './login.html';
  }
});