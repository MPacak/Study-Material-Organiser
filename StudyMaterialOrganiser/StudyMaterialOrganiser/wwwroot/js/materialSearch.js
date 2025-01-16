

document.addEventListener('DOMContentLoaded', function () {
    initializeSearchForm();
    initializeTagSelect();
});

function initializeSearchForm() {

    const searchForm = document.getElementById('searchForm');
    const resetButton = document.querySelector('.btn-secondary');
    const searchInput = document.querySelector('input[name="query"]');
    const fileTypeSelect = document.querySelector('select[name="fileType"]');
    const tagSelect = document.querySelector('select[name="tagIds"]');

    let searchTimeout;
    searchInput?.addEventListener('input', function () {
        clearTimeout(searchTimeout);
        searchTimeout = setTimeout(() => {
            searchForm.submit();
        }, 500);
    });


    fileTypeSelect?.addEventListener('change', function () {
        searchForm.submit();
    });


    if (resetButton) {
        resetButton.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            resetForm();
            return false;
        });
    }


    searchForm?.addEventListener('submit', function (e) {
   
        updateUrlWithSearchParams();
    });
}

function initializeTagSelect() {
    const tagSelect = document.querySelector('select[name="tagIds"]');

    if (tagSelect) {
        
        tagSelect.addEventListener('change', function () {
            document.getElementById('searchForm').submit();
        });

      
        addTagSelectionControls(tagSelect);
    }
}

function resetForm() {

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


    window.location.href = window.location.pathname;
}

function updateUrlWithSearchParams() {
    const searchForm = document.getElementById('searchForm');
    const formData = new FormData(searchForm);
    const searchParams = new URLSearchParams();


    for (const [key, value] of formData.entries()) {
        if (value) {
            searchParams.append(key, value);
        }
    }

 
    const newUrl = `${window.location.pathname}?${searchParams.toString()}`;
    window.history.pushState({ path: newUrl }, '', newUrl);
}

function addTagSelectionControls(tagSelect) {

    const controlsDiv = document.createElement('div');
    controlsDiv.className = 'mt-2';


    const selectAllBtn = document.createElement('button');
    selectAllBtn.type = 'button';
    selectAllBtn.className = 'btn btn-sm btn-outline-secondary me-2';
    selectAllBtn.textContent = 'Select All Tags';
    selectAllBtn.onclick = () => {
        Array.from(tagSelect.options).forEach(option => option.selected = true);
        document.getElementById('searchForm').submit();
    };


    const clearAllBtn = document.createElement('button');
    clearAllBtn.type = 'button';
    clearAllBtn.className = 'btn btn-sm btn-outline-secondary';
    clearAllBtn.textContent = 'Clear All Tags';
    clearAllBtn.onclick = () => {
        Array.from(tagSelect.options).forEach(option => option.selected = false);
        document.getElementById('searchForm').submit();
    };

  
    controlsDiv.appendChild(selectAllBtn);
    controlsDiv.appendChild(clearAllBtn);


    tagSelect.parentNode.insertBefore(controlsDiv, tagSelect.nextSibling);
}


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