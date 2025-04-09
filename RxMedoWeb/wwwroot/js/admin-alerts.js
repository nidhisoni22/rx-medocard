// Admin Alerts JavaScript

// Auto-hide and make alerts dismissible
$(document).ready(function() {
    // Add a close button to each alert
    $('.alert').append('<button type="button" class="btn-close float-end" data-bs-dismiss="alert" aria-label="Close"></button>');
    
    // Make alerts dismissible
    $('.alert').addClass('alert-dismissible fade show');
    
    // Auto-hide alerts after 5 seconds
    setTimeout(function() {
        $('.alert').fadeOut('slow', function() {
            $(this).remove(); // Remove from DOM after fade out
        });
    }, 5000);
});
