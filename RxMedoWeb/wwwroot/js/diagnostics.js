// Diagnostics Page JavaScript

document.addEventListener('DOMContentLoaded', function () {
    // Animate diagnostic cards on scroll
    const diagnosticCards = document.querySelectorAll('.diagnostic-card');

    // Function to check if an element is in viewport
    function isInViewport(element) {
        const rect = element.getBoundingClientRect();
        return (
            rect.top <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.bottom >= 0
        );
    }

    // Function to handle scroll animation
    function handleScrollAnimation() {
        diagnosticCards.forEach((card, index) => {
            if (isInViewport(card) && !card.classList.contains('animated')) {
                // Add animated class to prevent re-animation
                card.classList.add('animated');

                // Add animation with delay based on index
                setTimeout(() => {
                    card.style.opacity = '1';
                    card.style.transform = 'translateY(0)';
                }, index * 100);
            }
        });
    }

    // Initial check on page load
    handleScrollAnimation();

    // Check on scroll
    window.addEventListener('scroll', handleScrollAnimation);

    // Add hover effect to diagnostic cards
    diagnosticCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.querySelector('.diagnostic-card-btn i').style.transform = 'translateX(5px)';

            // Add a subtle scale effect to the icon on hover
            const icon = this.querySelector('.diagnostic-card-icon');
            if (icon) {
                icon.style.transform = 'scale(1.1) rotate(5deg)';
            }
        });

        card.addEventListener('mouseleave', function() {
            this.querySelector('.diagnostic-card-btn i').style.transform = 'translateX(0)';

            // Reset the icon on mouse leave
            const icon = this.querySelector('.diagnostic-card-icon');
            if (icon) {
                icon.style.transform = 'scale(1) rotate(0deg)';
            }
        });
    });

    // Add parallax effect to hero section
    const heroSection = document.querySelector('.diagnostics-hero');
    if (heroSection) {
        window.addEventListener('scroll', function() {
            const scrollPosition = window.scrollY;
            heroSection.style.backgroundPosition = `center ${scrollPosition * 0.05}px`;
        });
    }

    // Animate hero stats on hover
    const heroStats = document.querySelectorAll('.hero-stat');
    heroStats.forEach(stat => {
        stat.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-10px)';
            this.style.backgroundColor = 'rgba(255, 255, 255, 0.15)';
        });

        stat.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
            this.style.backgroundColor = 'rgba(255, 255, 255, 0.1)';
        });
    });

    // Animate counter for hero stats
    function animateCounter() {
        const statNumbers = document.querySelectorAll('.hero-stat-number');

        statNumbers.forEach(statNumber => {
            // Skip if already animated
            if (statNumber.classList.contains('counted')) return;

            // Check if in viewport
            if (!isInViewport(statNumber)) return;

            // Mark as counted to prevent re-animation
            statNumber.classList.add('counted');

            // Get target count and suffix
            const targetCount = parseInt(statNumber.getAttribute('data-count'));
            const suffix = statNumber.getAttribute('data-suffix') || '';

            // Determine if it's a large number (for different animation speed)
            const isLargeNumber = targetCount > 100;

            // Set animation duration based on number size
            const duration = isLargeNumber ? 2000 : 1500; // milliseconds

            // Calculate increment step
            const incrementStep = isLargeNumber ? Math.ceil(targetCount / 100) : 1;

            // Start counter
            let currentCount = 0;
            const startTime = Date.now();

            // Update counter at regular intervals
            const counterInterval = setInterval(() => {
                const elapsedTime = Date.now() - startTime;
                const progress = Math.min(elapsedTime / duration, 1);

                // Use easeOutQuad for smoother animation
                const easedProgress = 1 - (1 - progress) * (1 - progress);

                // Calculate current value
                currentCount = Math.floor(easedProgress * targetCount);

                // Update the display
                statNumber.textContent = currentCount + suffix;

                // Stop when we reach the target
                if (progress >= 1) {
                    clearInterval(counterInterval);
                    statNumber.textContent = targetCount + suffix;
                }
            }, 16); // ~60fps
        });
    }

    // Run counter animation on page load
    animateCounter();

    // Check for counters on scroll
    window.addEventListener('scroll', animateCounter);

    // Form validation with animation
    const bookingForm = document.querySelector('.booking-form');
    if (bookingForm) {
        const formInputs = bookingForm.querySelectorAll('.form-control, .form-select');

        formInputs.forEach(input => {
            input.addEventListener('focus', function() {
                this.parentElement.querySelector('.form-label').style.color = '#1A237E';
            });

            input.addEventListener('blur', function() {
                this.parentElement.querySelector('.form-label').style.color = '';
            });
        });

        // Animate submit button
        const submitBtn = bookingForm.querySelector('.btn-book-now');
        if (submitBtn) {
            submitBtn.addEventListener('mouseenter', function() {
                this.querySelector('i').style.transform = 'translateX(5px)';
            });

            submitBtn.addEventListener('mouseleave', function() {
                this.querySelector('i').style.transform = 'translateX(0)';
            });
        }
    }

    // Add animation to the booking form container
    const bookingFormContainer = document.querySelector('.booking-form-container');
    if (bookingFormContainer && isInViewport(bookingFormContainer)) {
        bookingFormContainer.style.animation = 'pulse 2s ease-in-out';
    }
});
