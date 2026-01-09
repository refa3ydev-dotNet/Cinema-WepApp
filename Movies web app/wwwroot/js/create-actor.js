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
});
