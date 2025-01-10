

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
      // console.log(element);
      // Yeni bir satır oluştur
      const newRow = document.createElement('tr');
      const editButton = document.createElement("button");
      editButton.className = "btn btn-primary";
      editButton.textContent = "Düzenle |";
      editButton.type = "button";
      editButton.id = "editButton";
      const deleteButton = document.createElement("button");
      deleteButton.className = "btn btn-danger";
      deleteButton.textContent = "| Sil";
      deleteButton.type = "button";
      deleteButton.id = "deleteButton";


      // Yeni hücreler oluştur
      const id = element.studentId;
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
      isActive.style.color = element.isActive ? "green" : "red";
      actions.appendChild(editButton);
      actions.appendChild(deleteButton);
      // actions.textContent = "Edit | Delete"; // Örnek işlemler

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
      deleteButton.addEventListener("click", async () => {
        const confirmed = confirm(`Silmek istediğinize emin misiniz: ${element.firstName + " " + element.lastName}?`);
        if (confirmed) {
          try {
            // API'ye DELETE isteği gönder
            const response = await fetch(`https://localhost:7081/api/Student/DeleteStudent/${element.studentId}`, {
              method: "DELETE",
            });

            if (response.ok) {
              alert("Başarıyla silindi.");
              newRow.remove();
            } else {
              const errorData = await response.json();
              alert(`Silme işlemi başarısız: ${errorData.message}`);
            }
          } catch (error) {
            console.error("Silme sırasında hata oluştu:", error);
            alert("Silme işlemi sırasında bir hata oluştu.");
          }
        }
      });
      editButton.addEventListener("click", () => {
        document.getElementById("firstName").value = element.firstName || "";
        document.getElementById("lastName").value = element.lastName || "";
        document.getElementById("photoUrl").value = element.photoUrl || "";
        document.getElementById("email").value = element.email || "";
        document.getElementById("phone").value = element.phone || "";
        document.getElementById("dateOfBirth").value = element.dateOfBirth || "";
        document.getElementById("gender").value = element.gender || "Erkek";
        document.getElementById("address").value = element.address || "";
        document.getElementById("enrollmentDate").value = element.enrollmentDate || "";
        document.getElementById("gradeLevel").value = element.gradeLevel || "1";
        document.getElementById("gpa").value = element.gpa || 0;
        document.getElementById("isActive").checked = element.isActive || false;
        document.getElementById("parentName").value = element.parentName || "";
        document.getElementById("parentContact").value = element.parentContact || "";

        // Modal'ı aç
        const editStudentModal = new bootstrap.Modal(document.getElementById("editStudentModal"));
        editStudentModal.show();

        document.getElementById("editStudentForm").addEventListener("submit", async (event) => {
          event.preventDefault();

          const updatedStudent = {
            firstName: document.getElementById("firstName").value,
            lastName: document.getElementById("lastName").value,
            photoUrl: document.getElementById("photoUrl").value,
            email: document.getElementById("email").value,
            phone: document.getElementById("phone").value,
            dateOfBirth: document.getElementById("dateOfBirth").value,
            gender: document.getElementById("gender").value,
            address: document.getElementById("address").value,
            enrollmentDate: document.getElementById("enrollmentDate").value,
            gradeLevel: document.getElementById("gradeLevel").value,
            gpa: document.getElementById("gpa").value,
            isActive: document.getElementById("isActive").checked,
            parentName: document.getElementById("parentName").value,
            parentContact: document.getElementById("parentContact").value,
          };

          try {
            const response = await fetch(`https://localhost:7081/api/Student/UpdateStudent/${element.studentId}`, {
              method: "PUT",
              headers: {
                "Content-Type": "application/json",
              },
              body: JSON.stringify(updatedStudent),
            });

            if (response.ok) {
              alert("Öğrenci bilgileri başarıyla güncellendi.");
              location.reload(); // Sayfayı yenile
            } else {
              const errorData = await response.json();
              alert(`Güncelleme başarısız: ${errorData.message}`);
            }
          } catch (error) {
            console.error("Güncelleme sırasında hata oluştu:", error);
            alert("Güncelleme sırasında bir hata oluştu.");
          }
        });
      });


    });
  })
  .catch(error => {
    console.error('Fetch error:', error);
    document.getElementById("noStudent").style.display = "block";
  });


