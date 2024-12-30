

document.addEventListener('DOMContentLoaded', function () {
    initializeSearchForm();
    initializeTagSelect();
});

function initializeSearchForm() {
    // Get form elements
    const searchForm = document.getElementById('searchForm');
    const resetButton = document.querySelector('.btn-secondary');
    const searchInput = document.querySelector('input[name="query"]');
    const fileTypeSelect = document.querySelector('select[name="fileType"]');
    const tagSelect = document.querySelector('select[name="tagIds"]');

    // Add debounce to search input
    let searchTimeout;
    searchInput?.addEventListener('input', function () {
        clearTimeout(searchTimeout);
        searchTimeout = setTimeout(() => {
            searchForm.submit();
        }, 500);
    });

    // Handle file type change
    fileTypeSelect?.addEventListener('change', function () {
        searchForm.submit();
    });

    // Handle reset button
    if (resetButton) {
        resetButton.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            resetForm();
            return false;
        });
    }

    // Handle form submission
    searchForm?.addEventListener('submit', function (e) {
        // Optional: Add any validation or preprocessing here
        updateUrlWithSearchParams();
    });
}

function initializeTagSelect() {
    const tagSelect = document.querySelector('select[name="tagIds"]');

    if (tagSelect) {
        // Add change event listener for tag selection
        tagSelect.addEventListener('change', function () {
            document.getElementById('searchForm').submit();
        });

        // Optional: Add "Select All" and "Clear All" functionality
        addTagSelectionControls(tagSelect);
    }
}

function resetForm() {
    // Clear all form inputs
    const searchForm = document.getElementById('searchForm');
    const inputs = searchForm.querySelectorAll('input, select');

    inputs.forEach(input => {
        if (input.type === 'text') {
            input.value = '';
        } else if (input.type === 'select-one' || input.type === 'select-multiple') {
            input.selectedIndex = 0;
            if (input.type === 'select-multiple') {
                Array.from(input.options).forEach(option => option.selected = false);
            }
        }
    });

    // Reset URL and reload page
    window.location.href = window.location.pathname;
}

function updateUrlWithSearchParams() {
    const searchForm = document.getElementById('searchForm');
    const formData = new FormData(searchForm);
    const searchParams = new URLSearchParams();

    // Add non-empty parameters to URL
    for (const [key, value] of formData.entries()) {
        if (value) {
            searchParams.append(key, value);
        }
    }

    // Update URL without reloading the page
    const newUrl = `${window.location.pathname}?${searchParams.toString()}`;
    window.history.pushState({ path: newUrl }, '', newUrl);
}

function addTagSelectionControls(tagSelect) {
    // Create control buttons container
    const controlsDiv = document.createElement('div');
    controlsDiv.className = 'mt-2';

    // Add "Select All" button
    const selectAllBtn = document.createElement('button');
    selectAllBtn.type = 'button';
    selectAllBtn.className = 'btn btn-sm btn-outline-secondary me-2';
    selectAllBtn.textContent = 'Select All Tags';
    selectAllBtn.onclick = () => {
        Array.from(tagSelect.options).forEach(option => option.selected = true);
        document.getElementById('searchForm').submit();
    };

    // Add "Clear All" button
    const clearAllBtn = document.createElement('button');
    clearAllBtn.type = 'button';
    clearAllBtn.className = 'btn btn-sm btn-outline-secondary';
    clearAllBtn.textContent = 'Clear All Tags';
    clearAllBtn.onclick = () => {
        Array.from(tagSelect.options).forEach(option => option.selected = false);
        document.getElementById('searchForm').submit();
    };

    // Add buttons to controls container
    controlsDiv.appendChild(selectAllBtn);
    controlsDiv.appendChild(clearAllBtn);

    // Insert controls after the tag select
    tagSelect.parentNode.insertBefore(controlsDiv, tagSelect.nextSibling);
}

// Utility function for debouncing
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}