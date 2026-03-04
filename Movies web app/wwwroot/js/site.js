document.addEventListener('DOMContentLoaded', () => {
    // Header Scroll Effect
    const header = document.querySelector('.main-header');

    if (header) {
        window.addEventListener('scroll', () => {
            if (window.scrollY > 50) {
                header.classList.add('scrolled');
            } else {
                header.classList.remove('scrolled');
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
    const menuToggles = document.querySelectorAll('.mobile-menu-toggle');

    if (menuToggles.length > 0) {
        menuToggles.forEach(toggle => {
            // Find the nearest header and its corresponding main-nav
            const header = toggle.closest('.main-header');
            if (!header) return;

            const mainNav = header.querySelector('.main-nav');
            const navLinks = mainNav ? mainNav.querySelectorAll('a') : [];

            // Support both <i class="... mobile-menu-toggle"> and <div class="mobile-menu-toggle"><i>...</div>
            const menuIcon = toggle.tagName.toLowerCase() === 'i' ? toggle : toggle.querySelector('i');

            if (mainNav) {
                toggle.addEventListener('click', () => {
                    mainNav.classList.toggle('active');
                    toggle.classList.toggle('active'); // Add active class to the toggle itself

                    // Toggle body scroll
                    document.body.classList.toggle('no-scroll');

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
                        toggle.classList.remove('active');
                        document.body.classList.remove('no-scroll');
                        menuIcon?.classList.remove('fa-times');
                        menuIcon?.classList.add('fa-bars');
                    });
                });
            }
        });
    }

    // --- ACTIVE LINK HIGHLIGHTING ---
    const currentPath = window.location.pathname;
    const allNavLinks = document.querySelectorAll('.main-nav a, .main-nav .nav-link');
    allNavLinks.forEach(link => {
        if (link.getAttribute('href') && currentPath.includes(link.getAttribute('href'))) {
            // Remove active from all
            allNavLinks.forEach(l => l.classList.remove('active'));
            // Add to current
            link.classList.add('active');
        }
    });

    // --- PROFILE DROPDOWN MENU ---
    const profileTriggers = document.querySelectorAll('.nav-profile-trigger');
    const allDropdowns = document.querySelectorAll('.nav-dropdown');

    if (profileTriggers.length > 0) {
        // Toggle dropdown on click
        profileTriggers.forEach(trigger => {
            trigger.addEventListener('click', (e) => {
                e.preventDefault();
                e.stopPropagation(); // Prevent the body click listener from immediately firing
                const parentDropdown = trigger.closest('.nav-dropdown');

                // Close other open dropdowns first (if multiple exist on a page)
                allDropdowns.forEach(dropdown => {
                    if (dropdown !== parentDropdown) {
                        dropdown.classList.remove('active');
                    }
                });

                if (parentDropdown) {
                    parentDropdown.classList.toggle('active');
                }
            });
        });

        // Close dropdown when clicking anywhere else on the page
        document.addEventListener('click', (e) => {
            allDropdowns.forEach(dropdown => {
                if (!dropdown.contains(e.target)) {
                    dropdown.classList.remove('active');
                }
            });
        });
    }
});
