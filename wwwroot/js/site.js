import * as $ from 'jquery'; // Import jQuery

$(function () {
    try {
        // Function to show the loading modal
        function showLoadingModal() {
            var ModProgress = document.getElementById('ProgressModal');
            ModProgress.style.display = "block";
        }

        // Function to hide the loading modal
        function hideLoadingModal() {
            var ModProgress = document.getElementById('ProgressModal');
            ModProgress.style.display = "none";
        }

        // Show loading modal before initializing the DataTable
        showLoadingModal();

        $("#datatable-buttons").DataTable({
            responsive: true,
            lengthChange: true,
            lengthMenu: [[10, 25, 50], [10, 25, 50]],
            pageLength: 25,
            autoWidth: false,
            dom: '<"row"<"col-md-6"B><"col-md-6"f>>rt<"row"<"col-md-6"l><"col-md-6"p>>',
            buttons: [
                {
                    extend: 'excelHtml5',
                    text: 'Excel',
                    className: 'btn btn-success btn-sm waves-effect waves-light',
                    filename: 'test_filename'
                },
                {
                    extend: 'pdfHtml5',
                    text: 'PDF',
                    className: 'btn btn-danger btn-sm waves-effect waves-light',
                    orientation: 'landscape',
                    pageSize: 'LETTER'
                }
            ],
            language: {
                search: "Table Filter:"
            },
            // Callbacks to show/hide loading modal
            preInit: function () {
                showLoadingModal();
            },
            drawCallback: function () {
                hideLoadingModal(); // Hide the loading modal when the table is drawn
            }
        }).buttons().container().appendTo("#datatable-buttons_wrapper .col-md-6:eq(0)");

        $(".dataTables_length select").addClass("form-select form-select-sm");
    } catch (error) {
        console.error("An error occurred:", error);
    }
});
