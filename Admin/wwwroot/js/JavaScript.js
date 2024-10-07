// Gender Bar Chart
const genderCtx = document.getElementById('genderChart').getContext('2d');
const genderChart = new Chart(genderCtx, {
    type: 'bar',
    data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        datasets: [
            {
                label: 'Male',
                data: [400, 300, 600, 700, 800, 600, 700, 800, 900, 1000, null, null],
                backgroundColor: 'rgba(54, 162, 235, 0.6)',
            },
            {
                label: 'Female',
                data: [300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, null, null],
                backgroundColor: 'rgba(75, 192, 192, 0.6)',
            },
            {
                label: 'Children',
                data: [200, 100, 300, 400, 500, 600, 700, 800, 900, 1000, null, null],
                backgroundColor: 'rgba(255, 206, 86, 0.6)',
            },
        ],
    },
    options: {
        responsive: true,
        scales: {
            y: {
                beginAtZero: true,
            },
        },
    },
});

// Department Doughnut Chart
const departmentCtx = document.getElementById('departmentChart').getContext('2d');
const departmentChart = new Chart(departmentCtx, {
    type: 'doughnut',
    data: {
        labels: ['General', 'Pediatrics', 'Orthopedics', 'Cardiology', 'Dermatology'],
        datasets: [{
            data: [50, 40, 30, 60, 69],
            backgroundColor: ['#36A2EB', '#FFCE56', '#FF6384', '#4BC0C0', '#9966FF'],
        }],
    },
    options: {
        responsive: true,
    },
});