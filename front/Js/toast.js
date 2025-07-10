function showToast(message, type = 'success') {
  const toast = document.getElementById('toast');
  const toastMessage = document.getElementById('toast-message');
  toastMessage.textContent = message;
  
  toast.className = 'toast show ' + type;
  
  setTimeout(() => {
    toast.className = toast.className.replace('show', '');
  }, 3000);
}
