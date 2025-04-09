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

    // Global unified navbar toggler functionality for all pages
    function setupNavbarToggler() {
        console.log('Setting up navbar toggler');
        // Get the navbar elements
        const navbarToggler = document.querySelector('.navbar-toggler');
        const navbarCollapse = document.querySelector('.navbar-collapse');

        if (navbarToggler && navbarCollapse) {
            console.log('Found navbar elements');
            // Remove any existing event listeners by cloning and replacing
            const newToggler = navbarToggler.cloneNode(true);
            navbarToggler.parentNode.replaceChild(newToggler, navbarToggler);

            // Force the toggle button to be visible on mobile
            if (window.innerWidth <= 1100) {
                newToggler.style.display = 'block';
                newToggler.style.visibility = 'visible';
                newToggler.style.opacity = '1';
                newToggler.style.position = 'absolute';
                newToggler.style.right = '15px';
                newToggler.style.top = '50%';
                newToggler.style.transform = 'translateY(-50%)';
                newToggler.style.zIndex = '1050';
                newToggler.style.backgroundColor = 'rgba(239, 71, 111, 0.3)';
                newToggler.style.padding = '0.7rem';
                newToggler.style.borderRadius = '8px';
                newToggler.style.cursor = 'pointer';
            }

            // Add new event listener with proper event handling
            newToggler.addEventListener('click', function(e) {
                console.log('Toggle button clicked');
                e.preventDefault();
                e.stopPropagation();

                // Toggle the collapsed class
                if (this.classList.contains('collapsed')) {
                    this.classList.remove('collapsed');
                } else {
                    this.classList.add('collapsed');
                }

                // Toggle the show class on navbar-collapse
                navbarCollapse.classList.toggle('show');
            });

            // Close menu when clicking outside
            document.addEventListener('click', function(event) {
                if (navbarCollapse.classList.contains('show') &&
                    !navbarCollapse.contains(event.target) &&
                    !newToggler.contains(event.target)) {
                    newToggler.classList.add('collapsed');
                    navbarCollapse.classList.remove('show');
                }
            });
        } else {
            console.log('Navbar elements not found');
        }
    }

    // Initialize the navbar toggler
    document.addEventListener('DOMContentLoaded', function() {
        console.log('DOM content loaded');
        setupNavbarToggler();
    });

    // Re-initialize on window resize to ensure it works correctly
    window.addEventListener('resize', function() {
        console.log('Window resized');
        setupNavbarToggler();
    });

    // Re-initialize when page visibility changes (helps with page transitions)
    document.addEventListener('visibilitychange', function() {
        if (!document.hidden) {
            console.log('Page visibility changed');
            setupNavbarToggler();
        }
    });

    // Prevent navigation links from closing the menu when clicked
    document.querySelectorAll('.navbar-nav .nav-link').forEach(link => {
        link.addEventListener('click', function(e) {
            // Only prevent default for anchor links (links that start with #)
            const href = this.getAttribute('href');
            if (href && href.startsWith('#')) {
                e.preventDefault();
                const targetId = href;
                if (targetId === '#') return;

                const targetElement = document.querySelector(targetId);
                if (targetElement) {
                    targetElement.scrollIntoView({ behavior: 'smooth' });
                }
            }
            // Don't close the menu for page navigation links
            // The menu will stay open when navigating to a new page
        });
    });
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
