document.addEventListener('DOMContentLoaded', () => {
    // Header Scroll Effect
    const header = document.querySelector('.main-header');

    if (header) {
        window.addEventListener('scroll', () => {
            if (window.scrollY > 50) {
                header.style.background = 'rgba(5, 7, 10, 0.95)';
                header.style.padding = '1rem 3rem';
                header.style.boxShadow = '0 5px 20px rgba(0,0,0,0.5)';
            } else {
                header.style.background = 'linear-gradient(to bottom, rgba(0,0,0,0.8), transparent)';
                header.style.padding = '1.5rem 3rem';
                header.style.boxShadow = 'none';
            }
        });
    }

    // Smooth Scroll
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');
            if (targetId === '#') return;

            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });

    // --- SCROLL ANIMATIONS ---
    const observerOptions = {
        threshold: 0.1,
        rootMargin: "0px 0px -50px 0px"
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('active');
                observer.unobserve(entry.target); // Only animate once
            }
        });
    }, observerOptions);

    // Elements to reveal
    const revealElements = document.querySelectorAll('.reveal-up, .reveal-fade');
    revealElements.forEach(el => observer.observe(el));

    // --- MOBILE MENU ---
    const menuToggle = document.querySelector('.mobile-menu-toggle');
    const mainNav = document.querySelector('.main-nav');
    const navLinks = document.querySelectorAll('.main-nav a');
    const menuIcon = menuToggle ? menuToggle.querySelector('i') : null;

    if (menuToggle && mainNav) {
        menuToggle.addEventListener('click', () => {
            mainNav.classList.toggle('active');
            // Toggle icon
            if (mainNav.classList.contains('active')) {
                menuIcon?.classList.remove('fa-bars');
                menuIcon?.classList.add('fa-times');
            } else {
                menuIcon?.classList.remove('fa-times');
                menuIcon?.classList.add('fa-bars');
            }
        });

        // Close menu when a link is clicked
        navLinks.forEach(link => {
            link.addEventListener('click', () => {
                mainNav.classList.remove('active');
                menuIcon?.classList.remove('fa-times');
                menuIcon?.classList.add('fa-bars');
            });
        });
    }
});

// --- MOBILE MENU ---
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
