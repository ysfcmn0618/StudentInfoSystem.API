
const form = document.querySelector("form"); // Formu seçiyoruz


form.addEventListener("submit", function (event) {
    event.preventDefault(); // Formun sayfayı yenilemesini engelliyoruz
 

    // Formdan veri alıyoruz
    const studentData = {
        email: document.getElementById("email").value,
        phone: document.getElementById("phone").value,
        studentId: 0,
        firstName: document.getElementById("firstName").value,
        lastName: document.getElementById("lastName").value,
        dateOfBirth: document.getElementById("dateOfBirth").value,
        gender: document.getElementById("gender").value,
        address: document.getElementById("address").value,
        enrollmentDate: document.getElementById("enrollmentDate").value,
        gradeLevel: document.getElementById("gradeLevel").value,
        gpa: parseFloat(document.getElementById("gpa").value),
        courses: [
            "string"
        ],
        isActive: document.getElementById("isActive").checked,
        parentName: document.getElementById("parentName").value,
        parentContact: document.getElementById("parentContact").value,
        photoUrl: document.getElementById("photoUrl").value,

    };
    console.log(studentData);
    // Fetch ile veriyi POST ediyoruz
    fetch("https://localhost:7081/api/Student/AddStudent", {
        method: "POST",
        headers: {            
            "Content-Type": "application/json; charset=utf-8 "
        },
        body: studentData // Form verisini JSON olarak gönderiyoruz

    })
        .then(response => response.json())
        .then(data => {
            console.log("Başarıyla gönderildi", data);
            alert("Öğrenci başarıyla eklendi!");
        })
        .catch(error => {
            console.error("Hata:", error);
            alert("Bir hata oluştu. Lütfen tekrar deneyin.");
        });
});
