const doctors = [
    { id: 1, image: 'https://via.placeholder.com/50', name: 'Dr. Andrio Maksim', gender: 'Male', position: 'Senior Doctor', speciality: 'Cardiologist', status: 'Full-time' },
    { id: 2, image: 'https://via.placeholder.com/50', name: 'Dr. Emily Clark', gender: 'Female', position: 'Junior Doctor', speciality: 'Neurologist', status: 'Part-time' },
    { id: 3, image: 'https://via.placeholder.com/50', name: 'Dr. John Doe', gender: 'Male', position: 'Consultant', speciality: 'Dermatologist', status: 'Full-time' },
    { id: 4, image: 'https://via.placeholder.com/50', name: 'Dr. Jane Smith', gender: 'Female', position: 'Surgeon', speciality: 'Orthopedic', status: 'Full-time' },
    { id: 5, image: 'https://via.placeholder.com/50', name: 'Dr. Lisa Ray', gender: 'Female', position: 'Senior Doctor', speciality: 'Pediatrician', status: 'Part-time' },
    { id: 6, image: 'https://via.placeholder.com/50', name: 'Dr. Michael Green', gender: 'Male', position: 'Consultant', speciality: 'Oncologist', status: 'Full-time' },
    { id: 7, image: 'https://via.placeholder.com/50', name: 'Dr. Sarah Brown', gender: 'Female', position: 'Junior Doctor', speciality: 'Ophthalmologist', status: 'Full-time' },
    { id: 8, image: 'https://via.placeholder.com/50', name: 'Dr. Peter Black', gender: 'Male', position: 'Surgeon', speciality: 'ENT Specialist', status: 'Full-time' },
    { id: 9, image: 'https://via.placeholder.com/50', name: 'Dr. Nancy White', gender: 'Female', position: 'Consultant', speciality: 'Psychiatrist', status: 'Part-time' },
    { id: 10, image: 'https://via.placeholder.com/50', name: 'Dr. David Lee', gender: 'Male', position: 'Senior Doctor', speciality: 'General Practitioner', status: 'Full-time' },
];

let currentPage = 1;
const doctorsPerPage = 5;
const doctorListEl = document.getElementById('doctor-list');
const currentPageEl = document.getElementById('current-page');
const totalPagesEl = document.getElementById('total-pages');
const totalDoctors = doctors.length;
const totalPages = Math.ceil(totalDoctors / doctorsPerPage);

totalPagesEl.textContent = totalPages;

function renderDoctors() {
    const startIndex = (currentPage - 1) * doctorsPerPage;
    const currentDoctors = doctors.slice(startIndex, startIndex + doctorsPerPage);

    doctorListEl.innerHTML = '';
    currentDoctors.forEach((doctor) => {
        const row = document.createElement('tr');
        row.innerHTML = `
        <td><img src="${doctor.image}" alt="${doctor.name}" width="50"></td>
        <td>${doctor.name}</td>
        <td>${doctor.gender}</td>
        <td>${doctor.position}</td>
        <td>${doctor.speciality}</td>
        <td>${doctor.status}</td>
        <td>
            <button onclick="location.href='/Home/DoctorDetails?id=${doctor.id}'" class="edit-btn btn btn-primary">More details</button>
            <form action="/Doctors/Delete" method="post" onsubmit="return confirm('Are you sure you want to delete this doctor?');">
                <input type="hidden" name="id" value="${doctor.id}">
                <button type="submit" class="delete-btn btn btn-danger">Delete</button>
            </form>
        </td>
    `;
        doctorListEl.appendChild(row);
    });


    currentPageEl.textContent = currentPage;
}

document.getElementById('prev-page').addEventListener('click', () => {
    if (currentPage > 1) {
        currentPage--;
        renderDoctors();
    }
});

document.getElementById('next-page').addEventListener('click', () => {
    if (currentPage < totalPages) {
        currentPage++;
        renderDoctors();
    }
});

renderDoctors();
