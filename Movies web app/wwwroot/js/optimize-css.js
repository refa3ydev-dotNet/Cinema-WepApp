// Optimized CSS Loading Script
// This script defers non-critical CSS loading to improve initial page load time

document.addEventListener('DOMContentLoaded', function() {
    // Lazy load images below the fold
    const lazyImages = document.querySelectorAll('img[data-src]');
    
    if ('IntersectionObserver' in window) {
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src;
                    img.removeAttribute('data-src');
                    imageObserver.unobserve(img);
                }
            });
        });
        
        lazyImages.forEach(img => imageObserver.observe(img));
    } else {
        // Fallback for browsers that don't support IntersectionObserver
        lazyImages.forEach(img => {
            img.src = img.dataset.src;
            img.removeAttribute('data-src');
        });
    }
    
    // Preload next page CSS if hover detected
    const preloadLinks = document.querySelectorAll('link[rel="preload"][as="style"]');
    preloadLinks.forEach(link => {
        link.addEventListener('mouseenter', () => {
            link.rel = 'stylesheet';
        });
    });
});

// Remove unused CSS after page load
window.addEventListener('load', function() {
    // Remove loading attribute from stylesheets
    const stylesheets = document.querySelectorAll('link[rel="stylesheet"][media="print"]');
    stylesheets.forEach(link => {
        link.removeAttribute('media');
    });
});

// Critical CSS injection for faster rendering
(function() {
    const criticalCSS = `
        /* Inline critical CSS for immediate rendering */
        :root { --bg-dark: #0b0f15; --primary-red: #e50914; }
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { background-color: var(--bg-dark); color: #fff; font-family: 'Outfit', sans-serif; }
        .btn { display: inline-flex; align-items: center; padding: 0.8rem 1.5rem; border-radius: 4px; font-weight: 600; cursor: pointer; border: none; transition: all 0.3s ease; }
        .btn-primary { background-color: var(--primary-red); color: white; }
    `;
    
    const style = document.createElement('style');
    style.textContent = criticalCSS;
    document.head.insertBefore(style, document.head.firstChild);
})();
