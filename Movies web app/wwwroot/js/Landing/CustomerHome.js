document.addEventListener("DOMContentLoaded", () => {
    // === 1. تعريف المتغيرات الأساسية ===
    const topbar = document.getElementById("topbar");
    const modal = document.getElementById("bookingModal");
    const modalHero = document.getElementById("modalHero");
    const modalTitle = document.getElementById("modalTitle");
    const modalRating = document.getElementById("modalRating");
    const modalGenre = document.getElementById("modalGenre");
    const modalRuntime = document.getElementById("modalRuntime");
    const modalDescription = document.getElementById("modalDescription");
    const scheduleList = document.getElementById("scheduleList");
    const modalCloseButton = modal.querySelector("[data-close-modal]");
    const laneTracks = Array.from(document.querySelectorAll(".lane-track"));

    let lastFocusedElement = null;

    // === 2. دوال تأثيرات الصفحة العادية (Scroll, Drag, Nav) ===
    function syncTopbar() {
        topbar.classList.toggle("is-scrolled", window.scrollY > 18);
    }

    function getFormatLabel(hallName) {
        if (!hallName) return "Standard";
        if (hallName.includes("IMAX")) return "IMAX";
        if (hallName.includes("Dolby")) return "Dolby";
        if (hallName.includes("4DX")) return "4DX";
        if (hallName.includes("VIP") || hallName.includes("Luxe")) return "VIP Luxe";
        return "Standard";
    }

    // === 3. 🚨 بناء قائمة الحفلات بناءً على داتا الـ C# (JSON) ===
    function renderSchedules(schedules) {
        // لو مفيش حفلات راجعة من الباك إند
        if (!schedules || schedules.length === 0) {
            scheduleList.innerHTML = `<div class="schedule-row" style="justify-content:center; color: #9ba1a6;">No active schedules available right now.</div>`;
            return;
        }

        // بناء الـ HTML لكل حفلة
        scheduleList.innerHTML = schedules.map((schedule) => {
            const format = getFormatLabel(schedule.roomName);

            // هنا بنستخدم السعر اللي ثبتناه في الكنترولر (Base_Price و VIP_Price)
            // لو القاعة VIP هنعرض السعر الـ VIP، لو عادية هنعرض الأساسي
            const finalPrice = format === "VIP Luxe" ? schedule.viP_Price : schedule.base_Price;

            return `
                <div class="schedule-row">
                <div class="schedule-slot">
                    <span class="schedule-date">${schedule.dateTimeText.split('|')[0].trim()}</span>
                    <div class="schedule-subline">
                    <span>${schedule.dateTimeText.split('|')[1].trim()}</span>
                    <span class="schedule-dot" aria-hidden="true">•</span>
                    <span>${schedule.roomName}</span>
                    </div>
                </div>
                <div class="schedule-meta">
                    <span class="schedule-format">${format}</span>
                    <span class="schedule-seat-note">Available</span>
                </div>
                <div class="schedule-price-block">
                    <span class="schedule-price-label">From</span>
                    <span class="schedule-price">${finalPrice} EGP</span>
                </div>
                <a href="/Customer/SelectSeats?scheduleId=${schedule.id}" class="seat-btn" style="text-decoration: none; display: inline-flex; align-items: center; justify-content: center;">Book Seats</a>
                </div>
            `;
        }).join("");
    }

    // === 4. 🚨 فتح المودال وسحب الداتا (AJAX) ===
    window.openBookingModal = function (cardElement) {
        if (!cardElement) return;

        lastFocusedElement = document.activeElement;

        // 1. سحب بيانات الفيلم من الكارت (data-attributes)
        const movieId = cardElement.getAttribute("data-movie-id");
        modalTitle.textContent = cardElement.getAttribute("data-title");
        modalRating.textContent = `IMDb ${cardElement.getAttribute("data-rating")}`;
        modalGenre.textContent = cardElement.getAttribute("data-genre");
        modalRuntime.textContent = cardElement.getAttribute("data-runtime");
        modalDescription.textContent = cardElement.getAttribute("data-description");

        const posterUrl = cardElement.getAttribute("data-poster");
        modalHero.style.backgroundImage = `
            linear-gradient(90deg, rgba(5, 5, 7, 0.94) 0%, rgba(5, 5, 7, 0.66) 42%, rgba(5, 5, 7, 0.18) 100%),
            linear-gradient(180deg, rgba(5, 5, 7, 0.08), rgba(5, 5, 7, 0.92)),
            url('${posterUrl}')
        `;

        // 2. إظهار المودال بوضعية "التحميل"
        scheduleList.innerHTML = `<div class="schedule-row" style="justify-content:center; color: #fff;"><i class="fa-solid fa-spinner fa-spin"></i> Loading schedules...</div>`;
        modal.classList.add("is-open");
        modal.setAttribute("aria-hidden", "false");
        document.body.classList.add("modal-open");
        modalCloseButton.focus();

        // 3. جلب الحفلات من الـ C# AgentController أو CustomerController
        // (تأكد إن مسار الـ API صح حسب اللي عندك)
        fetch(`/Agent/GetSchedules?movieId=${movieId}`)
            .then(response => response.json())
            .then(data => {
                renderSchedules(data); // رسم الحفلات بالداتا اللي رجعت
            })
            .catch(error => {
                console.error("Error fetching schedules:", error);
                scheduleList.innerHTML = `<div class="schedule-row" style="justify-content:center; color: #ff0f39;">Failed to load schedules. Please try again.</div>`;
            });
    };

    // === 5. إغلاق المودال ===
    function closeModal() {
        modal.classList.remove("is-open");
        modal.setAttribute("aria-hidden", "true");
        document.body.classList.remove("modal-open");

        if (lastFocusedElement && typeof lastFocusedElement.focus === "function") {
            lastFocusedElement.focus();
        }
    }

    modal.addEventListener("click", (event) => {
        const clickedClose = event.target.closest("[data-close-modal]");
        if (clickedClose || event.target === modal) {
            closeModal();
        }
    });

    document.addEventListener("keydown", (event) => {
        if (event.key === "Escape" && modal.classList.contains("is-open")) {
            closeModal();
        }
    });

    // === 6. السكرول العرضي (Swimlanes) ===
    function updateLaneButtons(track) {
        const swimlane = track.closest(".swimlane");
        const prevButton = swimlane.querySelector('.lane-button[data-direction="prev"]');
        const nextButton = swimlane.querySelector('.lane-button[data-direction="next"]');
        const maxScroll = Math.max(0, track.scrollWidth - track.clientWidth - 2);

        if (prevButton) prevButton.disabled = track.scrollLeft <= 2;
        if (nextButton) nextButton.disabled = track.scrollLeft >= maxScroll;
    }

    document.querySelectorAll(".lane-button").forEach((button) => {
        button.addEventListener("click", () => {
            const swimlane = button.closest(".swimlane");
            const track = swimlane.querySelector(".lane-track");
            const direction = button.dataset.direction === "next" ? 1 : -1;
            const amount = track.clientWidth * 0.85;

            track.scrollBy({
                left: amount * direction,
                behavior: "smooth"
            });
        });
    });

    laneTracks.forEach((track) => {
        let isPointerDown = false;
        let startX = 0;
        let startScrollLeft = 0;
        let dragged = false;

        track.dataset.justDragged = "false";
        updateLaneButtons(track);

        track.addEventListener("scroll", () => { updateLaneButtons(track); });

        track.addEventListener("pointerdown", (event) => {
            isPointerDown = true;
            dragged = false;
            startX = event.clientX;
            startScrollLeft = track.scrollLeft;
            track.classList.add("is-grabbing");
            if (typeof track.setPointerCapture === "function") track.setPointerCapture(event.pointerId);
        });

        track.addEventListener("pointermove", (event) => {
            if (!isPointerDown) return;
            const distance = event.clientX - startX;
            if (Math.abs(distance) > 8) dragged = true;
            track.scrollLeft = startScrollLeft - distance;
        });

        function endDrag() {
            if (!isPointerDown) return;
            isPointerDown = false;
            track.classList.remove("is-grabbing");
            if (dragged) {
                track.dataset.justDragged = "true";
                window.setTimeout(() => { track.dataset.justDragged = "false"; }, 140);
            }
        }

        track.addEventListener("pointerup", endDrag);
        track.addEventListener("pointercancel", endDrag);
        track.addEventListener("pointerleave", (event) => {
            if (isPointerDown && event.buttons === 0) endDrag();
        });
    });

    window.addEventListener("resize", () => {
        laneTracks.forEach((track) => updateLaneButtons(track));
    });

    window.addEventListener("scroll", syncTopbar, { passive: true });
    syncTopbar();
});