// Membership Page JavaScript

document.addEventListener('DOMContentLoaded', function () {

    // Animate elements on scroll
    const animatedElements = document.querySelectorAll('.benefit-card');

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
        animatedElements.forEach((element, index) => {
            if (isInViewport(element) && !element.classList.contains('animated')) {
                // Add animated class to prevent re-animation
                element.classList.add('animated');

                // Add animation with delay based on index
                setTimeout(() => {
                    element.style.opacity = '1';
                    element.style.transform = 'translateY(0)';
                }, index * 100);
            }
        });
    }

    // Initial check on page load
    handleScrollAnimation();

    // Check on scroll
    window.addEventListener('scroll', handleScrollAnimation);

    // Add hover effects to benefit cards
    const benefitCards = document.querySelectorAll('.benefit-card');
    benefitCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            const icon = this.querySelector('.benefit-icon');
            if (icon) {
                icon.style.backgroundColor = '#1A237E';
                icon.style.transform = 'scale(1.1) rotate(10deg)';

                const iconElement = icon.querySelector('i');
                if (iconElement) {
                    iconElement.style.color = 'white';
                }
            }
        });

        card.addEventListener('mouseleave', function() {
            const icon = this.querySelector('.benefit-icon');
            if (icon) {
                icon.style.backgroundColor = 'rgba(26, 35, 126, 0.1)';
                icon.style.transform = '';

                const iconElement = icon.querySelector('i');
                if (iconElement) {
                    iconElement.style.color = '#1A237E';
                }
            }
        });
    });

    // Form validation with animation
    const applicationForm = document.querySelector('.application-form');
    if (applicationForm) {
        const formInputs = applicationForm.querySelectorAll('.form-control, .form-select');

        formInputs.forEach(input => {
            input.addEventListener('focus', function() {
                this.parentElement.querySelector('.form-label').style.color = '#1A237E';
            });

            input.addEventListener('blur', function() {
                this.parentElement.querySelector('.form-label').style.color = '';
            });
        });

        // Animate submit button
        const submitBtn = applicationForm.querySelector('.btn-apply-now');
        if (submitBtn) {
            submitBtn.addEventListener('mouseenter', function() {
                this.querySelector('i').style.transform = 'translateX(5px)';
            });

            submitBtn.addEventListener('mouseleave', function() {
                this.querySelector('i').style.transform = 'translateX(0)';
            });
        }
    }
});
