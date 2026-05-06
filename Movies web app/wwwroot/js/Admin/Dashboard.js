/* ═══════════════════════════════════════════════════════════
   CINÉMA NOIR — ADMIN DASHBOARD JS
   Dynamic version for ASP.NET Core MVC
   ═══════════════════════════════════════════════════════════ */
document.addEventListener("DOMContentLoaded", () => {
    initCardAnimations();
    initCharts();
});

/* ═══ SIDEBAR MOBILE TOGGLE ═══ */
function initSidebar() {
    const sidebar = document.getElementById("sidebar");
    const overlay = document.getElementById("sidebarOverlay");
    const toggle = document.getElementById("sidebarToggle");

    if (!toggle || !sidebar) return;

    toggle.addEventListener("click", () => {
        sidebar.classList.toggle("open");

        if (overlay) {
            overlay.classList.toggle("active");
        }
    });

    if (overlay) {
        overlay.addEventListener("click", () => {
            sidebar.classList.remove("open");
            overlay.classList.remove("active");
        });
    }
}

/* ═══ ACTIVE NAV LINK ═══ */
function initNavActive() {
    const navItems = document.querySelectorAll(".nav-item");

    navItems.forEach(item => {
        item.addEventListener("click", () => {
            navItems.forEach(n => n.classList.remove("active"));
            item.classList.add("active");

            const sidebar = document.getElementById("sidebar");
            const overlay = document.getElementById("sidebarOverlay");

            if (window.innerWidth <= 768 && sidebar) {
                sidebar.classList.remove("open");

                if (overlay) {
                    overlay.classList.remove("active");
                }
            }
        });
    });
}

/* ═══ CARD ENTRANCE ANIMATIONS ═══ */
function initCardAnimations() {
    //const cards = document.querySelectorAll(".anim-card");

    //const observer = new IntersectionObserver((entries) => {
    //    entries.forEach(entry => {
    //        if (entry.isIntersecting) {
    //            const delay = parseInt(entry.target.dataset.delay || "0", 10) * 80;

    //            setTimeout(() => {
    //                entry.target.classList.add("visible");
    //            }, delay);

    //            observer.unobserve(entry.target);
    //        }
    //    });
    //}, { threshold: 0.1 });

    //cards.forEach(card => observer.observe(card));
}

/* ═══ SEARCH INPUT FOCUS EFFECT ═══ */
function initSearchFocus() {
    const input = document.getElementById("globalSearch");
    if (!input) return;

    input.addEventListener("focus", () => {
        input.parentElement.classList.add("search-focused");
    });

    input.addEventListener("blur", () => {
        input.parentElement.classList.remove("search-focused");
    });
}

/* ═══ DASHBOARD DATA ═══ */
function readDashboardData() {
    const element = document.getElementById("dashboardData");

    if (!element) {
        return {
            revenueChart: [],
            bookingsChart: []
        };
    }

    try {
        return JSON.parse(element.textContent);
    } catch {
        return {
            revenueChart: [],
            bookingsChart: []
        };
    }
}

function getLabels(items) {
    return items.map(x => x.label ?? x.Label);
}

function getValues(items) {
    return items.map(x => x.value ?? x.Value);
}

/* ═══ CHART.JS INITIALIZATION ═══ */
var revenueChartInstance = null;
var bookingsChartInstance = null;

function initCharts() {
    if (typeof Chart === "undefined") return;

    Chart.defaults.font.family = "'Outfit', sans-serif";
    Chart.defaults.color = "#6b7280";

    const dashboardData = readDashboardData();

    createRevenueChart(dashboardData.revenueChart ?? dashboardData.RevenueChart ?? []);
    createBookingsChart(dashboardData.bookingsChart ?? dashboardData.BookingsChart ?? []);
    initPeriodToggles();
}

/* ── Revenue Line Chart ── */
function createRevenueChart(data) {
    const canvas = document.getElementById("revenueChart");
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    const gradient = ctx.createLinearGradient(0, 0, 0, 250);

    gradient.addColorStop(0, "rgba(229, 9, 20, 0.18)");
    gradient.addColorStop(1, "rgba(229, 9, 20, 0.01)");

    revenueChartInstance = new Chart(canvas, {
        type: "line",
        data: {
            labels: getLabels(data),
            datasets: [{
                label: "Revenue",
                data: getValues(data),
                borderColor: "#e50914",
                backgroundColor: gradient,
                borderWidth: 2.5,
                fill: true,
                tension: 0.4,
                pointRadius: 4,
                pointHoverRadius: 7,
                pointBackgroundColor: "#e50914",
                pointBorderColor: "#0a0a0f",
                pointBorderWidth: 2,
                pointHoverBackgroundColor: "#fff",
                pointHoverBorderColor: "#e50914",
                pointHoverBorderWidth: 3
            }]
        },
        options: chartOptions("$")
    });
}

/* ── Bookings Bar Chart ── */
function createBookingsChart(data) {
    const canvas = document.getElementById("bookingsChart");
    if (!canvas) return;

    const values = getValues(data);
    const maxVal = values.length ? Math.max(...values) : 0;

    bookingsChartInstance = new Chart(canvas, {
        type: "bar",
        data: {
            labels: getLabels(data),
            datasets: [{
                label: "Bookings",
                data: values,
                backgroundColor: values.map(v =>
                    v === maxVal ? "rgba(229, 9, 20, 0.8)" : "rgba(229, 9, 20, 0.22)"
                ),
                borderColor: "rgba(229, 9, 20, 0.45)",
                borderWidth: 1,
                borderRadius: 6,
                borderSkipped: false,
                hoverBackgroundColor: "rgba(229, 9, 20, 0.9)"
            }]
        },
        options: chartOptions("")
    });
}

/* ── Shared Chart Options ── */
function chartOptions(prefix) {
    return {
        responsive: true,
        maintainAspectRatio: false,
        interaction: {
            mode: "index",
            intersect: false
        },
        plugins: {
            legend: {
                display: false
            },
            tooltip: {
                backgroundColor: "rgba(19, 21, 31, 0.95)",
                borderColor: "rgba(255, 255, 255, 0.08)",
                borderWidth: 1,
                titleFont: {
                    size: 13,
                    weight: "600"
                },
                bodyFont: {
                    size: 12
                },
                padding: 12,
                cornerRadius: 8,
                displayColors: false,
                callbacks: {
                    label: (ctx) => `${prefix}${Number(ctx.parsed.y).toLocaleString()}`
                }
            }
        },
        scales: {
            x: {
                grid: {
                    color: "rgba(255, 255, 255, 0.03)",
                    drawBorder: false
                },
                ticks: {
                    font: {
                        size: 11,
                        weight: "500"
                    }
                }
            },
            y: {
                grid: {
                    color: "rgba(255, 255, 255, 0.03)",
                    drawBorder: false
                },
                ticks: {
                    font: {
                        size: 11
                    },
                    callback: (val) => {
                        if (prefix === "$") {
                            return val >= 1000 ? "$" + (val / 1000).toFixed(1) + "K" : "$" + val;
                        }

                        return val;
                    }
                },
                beginAtZero: true
            }
        }
    };
}

/* ── Period Toggle: calls /Admin/DashboardCharts ── */
function initPeriodToggles() {
    document.querySelectorAll(".period-toggle").forEach(toggle => {
        toggle.querySelectorAll(".period-btn").forEach(btn => {
            btn.addEventListener("click", async function () {
                toggle.querySelectorAll(".period-btn").forEach(b => b.classList.remove("active"));
                this.classList.add("active");

                const days = parseInt(this.dataset.days || "7", 10);

                await reloadCharts(days);
            });
        });
    });
}

async function reloadCharts(days) {
    try {
        const response = await fetch(`/Admin/DashboardCharts?days=${days}`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (!response.ok) return;

        const data = await response.json();

        const revenueData = data.revenueChart ?? data.RevenueChart ?? [];
        const bookingsData = data.bookingsChart ?? data.BookingsChart ?? [];

        updateRevenueChart(revenueData);
        updateBookingsChart(bookingsData);
    } catch {
        // Keep current chart data if request fails
    }
}

function updateRevenueChart(data) {
    if (!revenueChartInstance) return;

    revenueChartInstance.data.labels = getLabels(data);
    revenueChartInstance.data.datasets[0].data = getValues(data);
    revenueChartInstance.update("active");
}

function updateBookingsChart(data) {
    if (!bookingsChartInstance) return;

    const values = getValues(data);
    const maxVal = values.length ? Math.max(...values) : 0;

    bookingsChartInstance.data.labels = getLabels(data);
    bookingsChartInstance.data.datasets[0].data = values;
    bookingsChartInstance.data.datasets[0].backgroundColor = values.map(v =>
        v === maxVal ? "rgba(229, 9, 20, 0.8)" : "rgba(229, 9, 20, 0.22)"
    );

    bookingsChartInstance.update("active");
}
