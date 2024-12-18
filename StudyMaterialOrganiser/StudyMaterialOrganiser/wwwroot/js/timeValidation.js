document.addEventListener('DOMContentLoaded', (event) => {
    const currentDateTime = new Date().toISOString().slice(0, 16);
    const startDateInput = document.getElementById('StartDate');
    const endDateInput = document.getElementById('EndDate');
    const form = document.getElementById('projectForm');

    startDateInput.setAttribute('min', currentDateTime);
    startDateInput.value = currentDateTime; // Set initial value to current date and time
    endDateInput.value= currentDateTime; // Set initial value to current date and time
    startDateInput.addEventListener('change', function () {
        const startTime = this.value;
        endDateInput.setAttribute('min', startTime);
    });

    form.addEventListener('submit', function (e) {
        const startTime = new Date(startDateInput.value);
        const endTime = new Date(endDateInput.value);

        if (endTime <= startTime) {
            e.preventDefault();
            alert('End time must be after the start time.');
        }
    });
});
