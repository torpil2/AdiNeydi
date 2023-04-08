function sanitizeInput(input) {
    // özel karakterleri temizle
    var sanitizedInput = input.replace(/[^\w\s]/gi, '');
    // SQL kodlarını engelle
    sanitizedInput = sanitizedInput.replace(/SELECT|INSERT|UPDATE|DELETE|FROM|WHERE/gi, '');
    return sanitizedInput;
  }
  

function submitForm() {
    // Form alanlarını al
    var userName = document.getElementById('username').value;
    var password = document.getElementById('password').value;

    
    // Verileri temizle
    userName = sanitizeInput(userName);
    password = sanitizeInput(password);

    
    // Form verisi oluştur
    var formData = {
        userName: userName,
        password: password,
    
    };
    formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

    sendData(formData);
    // Form verisini gönder
  ; // Bu kısımı kendi gereksinimlerinize göre değiştirebilirsiniz
    return false; // Sayfa yenileme önlemek için formu göndermez
  }
  

  function sendData(formData) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Account/Login', true);
    xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
    xhr.onreadystatechange = function() {
      if(xhr.readyState === 4 && xhr.status === 200) {
        console.log(xhr.responseText);
      }
    };
    xhr.send(formData);
  }
  