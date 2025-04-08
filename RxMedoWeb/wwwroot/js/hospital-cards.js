// Hospital Cards JavaScript

document.addEventListener('DOMContentLoaded', function () {
    // Animate hospital cards on scroll
    const hospitalCards = document.querySelectorAll('.hospital-card');
    const hospitalNetwork = document.querySelector('.hospital-network');
    const hospitalHeader = document.querySelector('.hospitals-header');
    const hospitalFeatures = document.querySelectorAll('.hospital-card-features li');

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
        hospitalCards.forEach((card, index) => {
            if (isInViewport(card) && !card.classList.contains('animated')) {
                // Add animated class to prevent re-animation
                card.classList.add('animated');

                // Add animation with delay based on index
                setTimeout(() => {
                    card.style.opacity = '1';
                    card.style.transform = 'translateY(0)';
                }, index * 200);
            }
        });
    }

    // Initial check on page load
    handleScrollAnimation();

    // Check on scroll
    window.addEventListener('scroll', handleScrollAnimation);

    // Add hover effect to hospital cards
    hospitalCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.querySelector('.hospital-card-btn i').style.transform = 'translateX(5px)';

            // Add a subtle scale effect to the features on hover
            const features = this.querySelectorAll('.hospital-card-features li');
            features.forEach((feature, index) => {
                setTimeout(() => {
                    feature.style.transform = 'translateY(-3px)';
                }, index * 100);
            });
        });

        card.addEventListener('mouseleave', function() {
            this.querySelector('.hospital-card-btn i').style.transform = 'translateX(0)';

            // Reset the features on mouse leave
            const features = this.querySelectorAll('.hospital-card-features li');
            features.forEach((feature) => {
                feature.style.transform = 'translateY(0)';
            });
        });
    });

    // Add parallax effect to hospital network section
    if (hospitalNetwork) {
        window.addEventListener('scroll', function() {
            const scrollPosition = window.scrollY;
            hospitalNetwork.style.backgroundPosition = `center ${scrollPosition * 0.05}px`;
        });
    }

    // Filter functionality (if needed in the future)
    const filterButtons = document.querySelectorAll('.hospital-filter-btn');
    if (filterButtons.length > 0) {
        filterButtons.forEach(button => {
            button.addEventListener('click', function() {
                // Remove active class from all buttons
                filterButtons.forEach(btn => btn.classList.remove('active'));

                // Add active class to clicked button
                this.classList.add('active');

                // Get filter value
                const filterValue = this.getAttribute('data-filter');

                // Filter cards
                hospitalCards.forEach(card => {
                    if (filterValue === 'all') {
                        card.style.display = 'block';
                    } else {
                        const cardCategory = card.getAttribute('data-category');
                        if (cardCategory === filterValue) {
                            card.style.display = 'block';
                        } else {
                            card.style.display = 'none';
                        }
                    }
                });
            });
        });
    }
});
