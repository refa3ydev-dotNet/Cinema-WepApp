document.addEventListener('DOMContentLoaded', () => {
    // initialize all toggle controls on the page
    const allControls = document.querySelectorAll('.view-controls');

    allControls.forEach(controls => {
        const targetId = controls.dataset.target;
        const storageKey = controls.dataset.storageKey;
        
        if (!targetId) return; // Skip if no target defined

        const grid = document.getElementById(targetId);
        const viewBtns = controls.querySelectorAll('.view-btn');

        if (!grid) return;

        // Load saved preference
        const savedView = localStorage.getItem(storageKey) || 'grid';
        setView(grid, viewBtns, savedView);

        // Add click listeners
        viewBtns.forEach(btn => {
            btn.addEventListener('click', () => {
                const view = btn.getAttribute('data-view');
                setView(grid, viewBtns, view);
                
                // Save preference if key is provided
                if (storageKey) {
                    localStorage.setItem(storageKey, view);
                }
            });
        });
    });

    function setView(gridElement, buttons, view) {
        // Update Grid Class
        if (view === 'list') {
            gridElement.classList.add('list-view');
        } else {
            gridElement.classList.remove('list-view');
        }

        // Update Buttons active state
        buttons.forEach(btn => {
            if (btn.getAttribute('data-view') === view) {
                btn.classList.add('active');
            } else {
                btn.classList.remove('active');
            }
        });
    }
});
