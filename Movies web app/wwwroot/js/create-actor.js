document.addEventListener('DOMContentLoaded', () => {
    // --- CREATE ACTOR UPLOAD LOGIC ---
    const uploadTrigger = document.getElementById('uploadTrigger');
    const fileInput = document.getElementById('headshotUpload');
    const previewImage = document.getElementById('uploadPreview');
    const placeholder = document.querySelector('.upload-placeholder');

    if (uploadTrigger && fileInput) {
        // Trigger file input on circle click
        uploadTrigger.addEventListener('click', () => {
            fileInput.click();
        });

        // Handle file selection
        fileInput.addEventListener('change', (e) => {
            const file = e.target.files[0];
            if (file) {
                const reader = new FileReader();

                reader.onload = (e) => {
                    previewImage.src = e.target.result;
                    previewImage.style.display = 'block';
                    // Hide placeholder text/icon
                    if (placeholder) {
                        placeholder.style.display = 'none';
                    }
                };

                reader.readAsDataURL(file);
            }
        });
    }

    // --- CREATE DIRECTOR UPLOAD LOGIC ---
    const directorInput = document.getElementById('DirectorImage');
    const directorUrlInput = document.getElementById('DirectorImageUrl');
    const directorCircle = document.querySelector('.dashed-upload-circle');
    const directorIcon = document.querySelector('.dashed-upload-circle i');
    const removeLink = document.querySelector('.remove-link');

    // Toggle Elements
    const fileGroup = document.getElementById('fileUploadGroup');
    const urlGroup = document.getElementById('urlInputGroup');
    const btnSwitchToUrl = document.getElementById('btnSwitchToUrl');
    const btnSwitchToFile = document.getElementById('btnSwitchToFile');

    if (directorCircle) {

        // 1. File Upload Logic
        if (directorInput) {
            directorInput.addEventListener('change', (e) => {
                const file = e.target.files[0];
                if (file) {
                    const reader = new FileReader();
                    reader.onload = (e) => {
                        updatePreview(e.target.result);
                    };
                    reader.readAsDataURL(file);
                }
            });
        }

        // 2. URL Input Logic
        if (directorUrlInput) {
            const handleUrlUpdate = (e) => {
                const url = e.target.value.trim();

                // Reset if empty
                if (url.length === 0) {
                    resetPreview();
                    return;
                }

                if (url.length > 5) {
                    // Indicate loading state (optional: yellow border?)
                    directorCircle.style.borderColor = 'rgba(255, 200, 0, 0.5)';

                    const tempImg = new Image();
                    tempImg.onload = () => {
                        updatePreview(url);
                        directorCircle.style.borderColor = 'rgba(255,255,255,0.1)'; // Success: Reset
                    };
                    tempImg.onerror = () => {
                        // Error: Show red border to indicate invalid URL
                        directorCircle.style.borderColor = '#ff4444';
                        // console.log('Image failed to load:', url);
                    };
                    tempImg.src = url;
                }
            };

            // Listen to multiple events for better responsiveness
            ['input', 'change', 'paste'].forEach(evt =>
                directorUrlInput.addEventListener(evt, handleUrlUpdate)
            );
        }

        // 3. Toggle Logic
        if (btnSwitchToUrl && btnSwitchToFile) {
            btnSwitchToUrl.addEventListener('click', () => {
                fileGroup.style.display = 'none';
                urlGroup.style.display = 'flex';
                // Clear file input if switching
                if (directorInput) directorInput.value = '';
            });

            btnSwitchToFile.addEventListener('click', () => {
                urlGroup.style.display = 'none';
                fileGroup.style.display = 'flex';
                // Clear URL input if switching
                if (directorUrlInput) directorUrlInput.value = '';
            });
        }

        // 4. Remove Logic
        if (removeLink) {
            removeLink.addEventListener('click', (e) => {
                e.preventDefault();
                resetPreview();
            });
        }

        // Helper Functions
        function updatePreview(src) {
            directorCircle.style.backgroundImage = `url('${src}')`;
            directorCircle.style.backgroundSize = 'cover';
            directorCircle.style.backgroundPosition = 'center';
            directorCircle.style.border = '2px solid rgba(255,255,255,0.1)';
            if (directorIcon) directorIcon.style.display = 'none';
        }

        function resetPreview() {
            if (directorInput) directorInput.value = '';
            if (directorUrlInput) directorUrlInput.value = '';

            directorCircle.style.backgroundImage = 'none';
            directorCircle.style.border = '2px dashed rgba(255,255,255,0.2)';
            if (directorIcon) directorIcon.style.display = 'block';
        }
    }

    // --- CHARACTER COUNTER LOGIC ---
    const bioTextarea = document.getElementById('DirectorBio');
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
});
