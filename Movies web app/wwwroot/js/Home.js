document.addEventListener('DOMContentLoaded', () => {
    // --- CAROUSEL LOGIC ---
    const track = document.querySelector('.carousel-track');
    // Guard clause in case carousel HTML is missing
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

    // Pause on hover
    track.addEventListener('mouseenter', stopSlideShow);
    track.addEventListener('mouseleave', startSlideShow);
});
