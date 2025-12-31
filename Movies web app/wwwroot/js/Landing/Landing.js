document.addEventListener('DOMContentLoaded', () => {
    // Header Scroll Effect
    const header = document.querySelector('.main-header');

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

    // --- CAROUSEL LOGIC ---
    const track = document.querySelector('.carousel-track');
    // Guard clause in case carousel HTML is missing or class names changed
    if (!track) return;

    const slides = Array.from(track.children);
    const nextButton = document.querySelector('.carousel-button--right');
    const prevButton = document.querySelector('.carousel-button--left');
    const dotsNav = document.querySelector('.carousel-nav');
    const dots = Array.from(dotsNav.children);

    let currentSlideIndex = 0;

    // Initialize slides background and state
    const updateSlidePosition = (slide, index) => {
        const bg = slide.getAttribute('data-bg');
        if (bg) {
            slide.style.setProperty('--bg-image', `url('${bg}')`);
        }
    };

    slides.forEach(updateSlidePosition);

    const updateDots = (targetIndex) => {
        dots.forEach(dot => dot.classList.remove('current-slide'));
        dots[targetIndex].classList.add('current-slide');
    };

    const moveToSlide = (targetIndex) => {
        const currentSlide = slides[currentSlideIndex];
        const targetSlide = slides[targetIndex];

        currentSlide.classList.remove('current-slide'); // Fade out current
        targetSlide.classList.add('current-slide');    // Fade in target

        updateDots(targetIndex);

        currentSlideIndex = targetIndex;
    };

    // Auto Play
    let slideInterval;
    const startSlideShow = () => {
        stopSlideShow(); // Clear existing to prevent duplicates
        slideInterval = setInterval(() => {
            const nextIndex = (currentSlideIndex + 1) % slides.length;
            moveToSlide(nextIndex);
        }, 5000); // 5 seconds
    };

    const stopSlideShow = () => {
        if (slideInterval) clearInterval(slideInterval);
    };

    // Event Listeners
    if (nextButton) {
        nextButton.addEventListener('click', () => {
            stopSlideShow();
            const nextIndex = (currentSlideIndex + 1) % slides.length;
            moveToSlide(nextIndex);
            startSlideShow();
        });
    }

    if (prevButton) {
        prevButton.addEventListener('click', () => {
            stopSlideShow();
            const prevIndex = (currentSlideIndex - 1 + slides.length) % slides.length;
            moveToSlide(prevIndex);
            startSlideShow();
        });
    }

    dots.forEach((dot, index) => {
        dot.addEventListener('click', () => {
            stopSlideShow();
            moveToSlide(index);
            startSlideShow();
        });
    });

    // Start auto play
    startSlideShow();

    // Pause on hover (Optional, usually good UX)
    track.addEventListener('mouseenter', stopSlideShow);
    track.addEventListener('mouseleave', startSlideShow);

    // Simple Hover Animations for Cards
    const cards = document.querySelectorAll('.movie-card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', () => {
            card.style.zIndex = '10';
        });
        card.addEventListener('mouseleave', () => {
            setTimeout(() => {
                card.style.zIndex = '1';
            }, 300);
        });
    });

    // Search Bar Focus Effect
    const searchInput = document.querySelector('.search-bar input');
    const searchContainer = document.querySelector('.search-bar');

    if (searchInput) {
        searchInput.addEventListener('focus', () => {
            searchContainer.style.boxShadow = '0 0 0 2px var(--primary-red), 0 10px 30px rgba(0,0,0,0.5)';
        });

        searchInput.addEventListener('blur', () => {
            searchContainer.style.boxShadow = '0 10px 30px rgba(0,0,0,0.3)';
        });
    }

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
    const menuIcon = menuToggle.querySelector('i');

    if (menuToggle) {
        menuToggle.addEventListener('click', () => {
            mainNav.classList.toggle('active');
            // Toggle icon
            if (mainNav.classList.contains('active')) {
                menuIcon.classList.remove('fa-bars');
                menuIcon.classList.add('fa-times');
            } else {
                menuIcon.classList.remove('fa-times');
                menuIcon.classList.add('fa-bars');
            }
        });

        // Close menu when a link is clicked
        navLinks.forEach(link => {
            link.addEventListener('click', () => {
                mainNav.classList.remove('active');
                menuIcon.classList.remove('fa-times');
                menuIcon.classList.add('fa-bars');
            });
        });
    }
});
