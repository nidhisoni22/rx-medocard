// RxMedo Website JavaScript

// Wait for the DOM to be fully loaded
document.addEventListener('DOMContentLoaded', function () {
    // Clean navbar - no scroll effect needed

    // Set active nav item based on current page
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-link');

    navLinks.forEach(link => {
        const href = link.getAttribute('href');
        if (href) {
            // Extract the controller name from the href
            const hrefParts = href.split('/');
            const controller = hrefParts[1] || '';

            // Extract the controller name from the current path
            const pathParts = currentPath.split('/');
            const currentController = pathParts[1] || '';

            if (controller && currentController && controller.toLowerCase() === currentController.toLowerCase()) {
                link.closest('.nav-item')?.classList.add('active');
            } else if (currentPath === '/' && (href === '/' || controller.toLowerCase() === 'home')) {
                link.closest('.nav-item')?.classList.add('active');
            }
        }
    });

    // Fix for navbar toggler
    const navbarToggler = document.querySelector('.navbar-toggler');
    if (navbarToggler) {
        navbarToggler.addEventListener('click', function() {
            this.classList.toggle('collapsed');
        });
    }
    // Initialize Bootstrap tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Form validation
    const forms = document.querySelectorAll('.needs-validation');
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            } else {
                // If this is a booking or appointment form, show success message
                if (form.classList.contains('booking-form') || form.classList.contains('appointment-form')) {
                    event.preventDefault();
                    showSuccessMessage(form);
                }
            }
            form.classList.add('was-validated');
        }, false);
    });

    // Function to show success message after form submission
    function showSuccessMessage(form) {
        // Hide the form
        form.style.display = 'none';

        // Create success message
        const successDiv = document.createElement('div');
        successDiv.className = 'alert alert-success';
        successDiv.innerHTML = '<h4 class="alert-heading">Success!</h4>' +
            '<p>Your request has been submitted successfully. We will contact you shortly.</p>' +
            '<hr>' +
            '<p class="mb-0">Thank you for choosing RxMedical Trust.</p>';

        // Insert success message before the form
        form.parentNode.insertBefore(successDiv, form);

        // Scroll to success message
        successDiv.scrollIntoView({ behavior: 'smooth' });

        // Reset and show form after 5 seconds
        setTimeout(() => {
            form.reset();
            successDiv.remove();
            form.style.display = 'block';
            form.classList.remove('was-validated');
        }, 5000);
    }

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');
            if (targetId === '#') return;

            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                targetElement.scrollIntoView({ behavior: 'smooth' });
            }
        });
    });
});
