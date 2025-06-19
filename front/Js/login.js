document.addEventListener('DOMContentLoaded', () => {
  const loginForm = document.getElementById('login-form');
  const submitButton = document.getElementById('submit-button');
  const emailInput = document.getElementById('email');
  const passwordInput = document.getElementById('password');
  const emailError = document.getElementById('email-error');
  const passwordError = document.getElementById('password-error');


  loginForm.addEventListener('submit', async function(e) {
    e.preventDefault();
    
    // Reset error messages
    emailError.textContent = '';
    passwordError.textContent = '';
    emailInput.classList.remove('error');
    passwordInput.classList.remove('error');
    
    // Validate form
    let isValid = true;
    
    if (!emailInput.value) {
      emailError.textContent = 'E-mail é obrigatório';
      emailInput.classList.add('error');
      isValid = false;
    } else if (!/\S+@\S+\.\S+/.test(emailInput.value)) {
      emailError.textContent = 'E-mail inválido';
      emailInput.classList.add('error');
      isValid = false;
    }
    
    if (!passwordInput.value) {
      passwordError.textContent = 'Senha é obrigatória';
      passwordInput.classList.add('error');
      isValid = false;
    } else if (passwordInput.value.length < 6) {
      passwordError.textContent = 'Senha deve ter no mínimo 6 caracteres';
      passwordInput.classList.add('error');
      isValid = false;
    }
    
    if (!isValid) return;
    
    // Process form submission
    submitButton.textContent = 'Entrando...';
    submitButton.disabled = true;
    
    try {
      const response = await fetch('https://localhost:7101/api/usuario/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          email: emailInput.value,
          senhaHash: passwordInput.value
        })
      });
      
      const data = await response.json();
      
      if (!response.ok) {
        throw new Error(data.message || 'Erro durante o login');
      }
      
      // Store tokens in localStorage
      localStorage.setItem('authToken', data.token);
      localStorage.setItem('refreshToken', data.refreshToken);
      
      // Show success toast
      showToast('Login realizado com sucesso!', 'success');
      
      // Redirect to dashboard
      setTimeout(() => {
        window.location.href = './dashboard.html';
      }, 1000);
      
    } catch (error) {
      showToast(error.message || 'Erro durante o login', 'error');
    } finally {
      submitButton.textContent = 'Entrar';
      submitButton.disabled = false;
    }
  });
});