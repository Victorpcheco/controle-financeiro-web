document.addEventListener('DOMContentLoaded', () => {
  const registerForm = document.getElementById('register-form');
  const submitButton = document.getElementById('submit-button');
  const nameInput = document.getElementById('name');
  const emailInput = document.getElementById('email');
  const passwordInput = document.getElementById('password');
  const nameError = document.getElementById('name-error');
  const emailError = document.getElementById('email-error');
  const passwordError = document.getElementById('password-error');
  
  // // Check if user is already logged in
  if (localStorage.getItem('authToken')) {
    window.location.href = './dashboard.html';
  }
  
  registerForm.addEventListener('submit', async function(e) {
    e.preventDefault();
    
    // Reset error messages
    nameError.textContent = '';
    emailError.textContent = '';
    passwordError.textContent = '';
    nameInput.classList.remove('error');
    emailInput.classList.remove('error');
    passwordInput.classList.remove('error');
    
    // Validate form
    let isValid = true;
    
    if (!nameInput.value) {
      nameError.textContent = 'Nome é obrigatório';
      nameInput.classList.add('error');
      isValid = false;
    }
    
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
    submitButton.textContent = 'Registrando...';
    submitButton.disabled = true;
    
    try {
      const response = await fetch('https://localhost:7184/api/v1/users/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          nomeCompleto: nameInput.value,
          email: emailInput.value,
          senhaHash: passwordInput.value
        })
      });
      
      const data = await response.json();
      
      if (!response.ok) {
        throw new Error(data.message || 'Erro durante o registro');
      }
      
      // Store tokens in localStorage
      localStorage.setItem('authToken', data.token);
      localStorage.setItem('refreshToken', data.refreshToken);
      
      // Show success toast
      showToast('Registro realizado com sucesso!', 'success');
      
      // Redirect to dashboard
      setTimeout(() => {
        window.location.href = './dashboard.html';
      }, 1000);
      
    } catch (error) {
      showToast(error.message || 'Erro durante o registro', 'error');
    } finally {
      submitButton.textContent = 'Registrar';
      submitButton.disabled = false;
    }
  });
});