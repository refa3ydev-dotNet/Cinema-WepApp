   <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>


        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: "btn btn-success",
                cancelButton: "btn btn-danger"
            },
            buttonsStyling: false
        });

        // أربط كل فورم حذف بكود SweetAlert
        document.querySelectorAll('.delete-form').forEach(form => {
            form.addEventListener('submit', function (e) {
                e.preventDefault(); // امنع الإرسال المؤقتًا

                swalWithBootstrapButtons.fire({
                    title: "Are you sure?",
                    text: "You won't be able to revert this!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Yes, delete it!",
                    cancelButtonText: "No, cancel!",
                    reverseButtons: true
                }).then((result) => {
                    if (result.isConfirmed) {
                        form.submit(); // ابعت الفورم فعليًا
                    } else if (result.dismiss === Swal.DismissReason.cancel) {
                        swalWithBootstrapButtons.fire({
                            title: "Cancelled",
                            text: "Your actor is safe :)",
                            icon: "error"
                        });
                    }
                });
            });
        });