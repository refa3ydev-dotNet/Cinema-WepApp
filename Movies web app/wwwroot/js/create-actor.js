document.addEventListener('DOMContentLoaded', () => {
    // --- GENERIC IMAGE UPLOAD LOGIC ---
    function setupImageUpload(fileInputId, urlInputId) {
        const fileInput = document.getElementById(fileInputId);
        const urlInput = document.getElementById(urlInputId);

        // Common elements (assumes only one upload section per page)
        const circle = document.querySelector('.dashed-upload-circle');
        const icon = document.querySelector('.dashed-upload-circle i');
        const removeLink = document.querySelector('.remove-link');

        const fileGroup = document.getElementById('fileUploadGroup');
        const urlGroup = document.getElementById('urlInputGroup');
        const btnSwitchToUrl = document.getElementById('btnSwitchToUrl');
        const btnSwitchToFile = document.getElementById('btnSwitchToFile');

        if (fileInput && circle) {

            // 1. File Upload
            fileInput.addEventListener('change', (e) => {
                const file = e.target.files[0];
                if (file) {
                    const reader = new FileReader();
                    reader.onload = (e) => updatePreview(e.target.result);
                    reader.readAsDataURL(file);
                }
            });

            // 2. URL Input
            if (urlInput) {
                const handleUrlUpdate = (e) => {
                    const url = e.target.value.trim();
                    if (url.length === 0) {
                        resetPreview();
                        return;
                    }
                    if (url.length > 5) {
                        circle.style.borderColor = 'rgba(255, 200, 0, 0.5)'; // Loading
                        const tempImg = new Image();
                        tempImg.onload = () => {
                            updatePreview(url);
                            circle.style.borderColor = 'rgba(255,255,255,0.1)';
                        };
                        tempImg.onerror = () => {
                            circle.style.borderColor = '#ff4444'; // Error
                        };
                        tempImg.src = url;
                    }
                };
                ['input', 'change', 'paste'].forEach(evt =>
                    urlInput.addEventListener(evt, handleUrlUpdate)
                );
            }

            // 3. Toggle Buttons
            if (btnSwitchToUrl && btnSwitchToFile) {
                btnSwitchToUrl.addEventListener('click', () => {
                    fileGroup.style.display = 'none';
                    urlGroup.style.display = 'flex';
                    fileInput.value = '';
                });
                btnSwitchToFile.addEventListener('click', () => {
                    urlGroup.style.display = 'none';
                    fileGroup.style.display = 'flex';
                    if (urlInput) urlInput.value = '';
                });
            }

            // 4. Remove
            if (removeLink) {
                removeLink.addEventListener('click', (e) => {
                    e.preventDefault();
                    resetPreview();
                });
            }

            // Helpers
            function updatePreview(src) {
                circle.style.backgroundImage = `url('${src}')`;
                circle.style.backgroundSize = 'cover';
                circle.style.backgroundPosition = 'center';
                circle.style.border = '2px solid rgba(255,255,255,0.1)';
                if (icon) icon.style.display = 'none';
            }

            function resetPreview() {
                fileInput.value = '';
                if (urlInput) urlInput.value = '';
                circle.style.backgroundImage = 'none';
                circle.style.border = '2px dashed rgba(255,255,255,0.2)';
                if (icon) icon.style.display = 'block';
            }
        }
    }

    // Initialize for Director
    setupImageUpload('DirectorImage', 'DirectorImageUrl');
    // Initialize for Actor
    setupImageUpload('ActorImage', 'ActorImageUrl');


    // --- CHARACTER COUNTER LOGIC ---
    const bioTextarea = document.getElementById('Biography');
    const charCountDisplay = document.querySelector('.char-count');

    if (bioTextarea && charCountDisplay) {
        bioTextarea.addEventListener('input', () => {
            const length = bioTextarea.value.length;
            charCountDisplay.textContent = `${length}/500 characters`;

            if (length > 500) {
                charCountDisplay.style.color = '#ff4444'; // Red warning
            } else {
                charCountDisplay.style.color = '#666';
            }
        });
    }

    // --- DECEASED TOGGLE LOGIC ---
    function setupDeceasedToggle(checkboxId, groupId) {
        const checkbox = document.getElementById(checkboxId);
        const group = document.getElementById(groupId);

        if (checkbox && group) {
            checkbox.addEventListener('change', () => {
                if (checkbox.checked) {
                    group.style.display = 'block'; // Or 'flex' depending on layout
                    // For input-with-icon we probably want block or flex
                    group.style.display = 'block';
                } else {
                    group.style.display = 'none';
                    // Clear date if unchecked? Optional.
                    const dateInput = group.querySelector('input');
                    if (dateInput) dateInput.value = '';
                }
            });
        }
    }

    // Initialize for Director
    setupDeceasedToggle('IsDeceased', 'DeathDateGroup');

    // Initialize for Actor
    setupDeceasedToggle('ActorIsDeceased', 'ActorDeathDateGroup');

    // --- NATIONALITY DROPDOWN LOGIC ---
    const nationalitySelect = document.getElementById('Nationality');

    if (nationalitySelect) {
        fetch('https://restcountries.com/v3.1/all?fields=name')
            .then(response => response.json())
            .then(data => {
                // Sort by common name
                const countries = data.sort((a, b) =>
                    a.name.common.localeCompare(b.name.common)
                );

                countries.forEach(country => {
                    const option = document.createElement('option');
                    option.value = country.name.common; // Value is country name suitable for text field in DB
                    option.textContent = country.name.common;
                    nationalitySelect.appendChild(option);
                });
            })
            .catch(error => {
                console.error('Error fetching countries:', error);
                // Fallback or leave as "Select a country"
            });
    }

});
