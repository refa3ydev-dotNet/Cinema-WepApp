document.addEventListener("DOMContentLoaded", () => {
    initAdminSidebar();
    initAdminProfileMenu();
    initAdminSearch();
});

function initAdminSidebar() {
    const sidebar = document.getElementById("sidebar");
    const toggleBtn = document.getElementById("sidebarToggle");
    const overlay = document.getElementById("sidebarOverlay");
    const navItems = document.querySelectorAll(".sidebar-nav .nav-item");

    if (!sidebar || !toggleBtn) return;

    const closeSidebar = () => {
        sidebar.classList.remove("open");
        overlay?.classList.remove("active");
        toggleBtn.setAttribute("aria-expanded", "false");
        document.body.classList.remove("admin-sidebar-open");
    };

    const openSidebar = () => {
        sidebar.classList.add("open");
        overlay?.classList.add("active");
        toggleBtn.setAttribute("aria-expanded", "true");
        document.body.classList.add("admin-sidebar-open");
    };

    toggleBtn.setAttribute("aria-expanded", "false");

    toggleBtn.addEventListener("click", () => {
        if (sidebar.classList.contains("open")) {
            closeSidebar();
        } else {
            openSidebar();
        }
    });

    overlay?.addEventListener("click", closeSidebar);

    navItems.forEach(item => {
        item.addEventListener("click", () => {
            if (window.innerWidth <= 768) {
                closeSidebar();
            }
        });
    });

    window.addEventListener("resize", () => {
        if (window.innerWidth > 768) {
            closeSidebar();
        }
    });

    document.addEventListener("keydown", event => {
        if (event.key === "Escape") {
            closeSidebar();
        }
    });
}

function initAdminProfileMenu() {
    const button = document.getElementById("profileMenuBtn");
    const menu = document.getElementById("profileMenu");

    if (!button || !menu) return;

    const closeMenu = () => {
        menu.classList.remove("open");
        menu.setAttribute("aria-hidden", "true");
        button.setAttribute("aria-expanded", "false");
    };

    button.setAttribute("aria-expanded", "false");
    button.setAttribute("aria-controls", "profileMenu");

    button.addEventListener("click", event => {
        event.stopPropagation();
        const isOpen = menu.classList.toggle("open");
        menu.setAttribute("aria-hidden", String(!isOpen));
        button.setAttribute("aria-expanded", String(isOpen));
    });

    menu.addEventListener("click", event => {
        event.stopPropagation();
    });

    document.addEventListener("click", closeMenu);
    document.addEventListener("keydown", event => {
        if (event.key === "Escape") {
            closeMenu();
        }
    });
}

function initAdminSearch() {
    const input = document.getElementById("globalSearch");

    if (!input) return;

    input.addEventListener("focus", () => {
        input.parentElement?.classList.add("search-focused");
    });

    input.addEventListener("blur", () => {
        input.parentElement?.classList.remove("search-focused");
    });

    input.addEventListener("keydown", event => {
        if (event.key !== "Enter") return;

        const query = input.value.trim();
        if (!query) return;

        window.location.href = `/Movies/SearchTmdb?query=${encodeURIComponent(query)}`;
    });
}
