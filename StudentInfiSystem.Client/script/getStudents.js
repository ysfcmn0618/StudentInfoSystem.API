

fetch('https://localhost:7081/api/Student/AllStudents')
  .then(response => {
    if (!response.ok) {
      document.getElementById("noStudent").style.display = "block";
    }
    return response.json();
  })
  .then(data => {
    //console.log(JSON.stringify(data, null, 2));

    const tableBody = document.getElementById("tableBody"); // Doğru seçici

    data.forEach(element => {
      // Yeni bir satır oluştur
      const newRow = document.createElement('tr');

      // Yeni hücreler oluştur
      const firstName = document.createElement('td');
      const lastName = document.createElement('td');
      const eMail = document.createElement('td');
      const phone = document.createElement('td');
      const gender = document.createElement('td');
      const address = document.createElement('td');
      const gpa = document.createElement('td');
      const isActive = document.createElement('td');
      const actions = document.createElement('td');

      // Hücrelere fetch'den gelen verileri atama
      firstName.textContent = element.firstName || "No Data";
      lastName.textContent = element.lastName || "No Data";
      eMail.textContent = element.email || "No Data";
      phone.textContent = element.phone || "No Data";
      gender.textContent = element.gender || "No Data";
      address.textContent = element.address || "No Data";
      gpa.textContent = element.gpa || "No Data";
      isActive.textContent = element.isActive ? "Active" : "Inactive";
      actions.textContent = "Edit | Delete"; // Örnek işlemler

      // Hücreleri satıra ekleme
      newRow.appendChild(firstName);
      newRow.appendChild(lastName);
      newRow.appendChild(eMail);
      newRow.appendChild(phone);
      newRow.appendChild(gender);
      newRow.appendChild(address);
      newRow.appendChild(gpa);
      newRow.appendChild(isActive);
      newRow.appendChild(actions);

      // Satırı tablo gövdesine ekleme
      tableBody.appendChild(newRow);
    });
  })
  .catch(error => {
    console.error('Fetch error:', error);
    document.getElementById("noStudent").style.display = "block";
  });


