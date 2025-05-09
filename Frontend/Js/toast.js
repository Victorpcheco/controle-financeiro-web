// Toast notification functionality
function showToast(message, type = 'success') {
  const toast = document.getElementById('toast');
  const toastMessage = document.getElementById('toast-message');
  
  // Set message and type
  toastMessage.textContent = message;
  toast.className = 'toast show ' + type;
  
  // Hide toast after 3 seconds
  setTimeout(() => {
    toast.className = toast.className.replace('show', '');
  }, 3000);
}