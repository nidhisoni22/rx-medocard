// Toggle Button Fix for All Pages
// This script ensures the toggle button is properly initialized and visible on mobile

document.addEventListener('DOMContentLoaded', function() {
    console.log('Toggle button fix script loaded');
    
    // Function to fix toggle button visibility
    function fixToggleButton() {
        // Get the current window width
        const windowWidth = window.innerWidth;
        
        // Get the toggle button
        const toggleButton = document.querySelector('.navbar-toggler');
        
        if (toggleButton) {
            console.log('Toggle button found');
            
            // Set display based on window width
            if (windowWidth <= 1100) {
                console.log('Window width <= 1100px, showing toggle button');
                toggleButton.style.display = 'block';
                toggleButton.style.visibility = 'visible';
                toggleButton.style.opacity = '1';
            } else {
                console.log('Window width > 1100px, hiding toggle button');
                toggleButton.style.display = 'none';
                toggleButton.style.visibility = 'hidden';
                toggleButton.style.opacity = '0';
            }
            
            // Ensure the toggle button works correctly
            toggleButton.addEventListener('click', function(e) {
                console.log('Toggle button clicked');
                e.preventDefault();
                e.stopPropagation();
                
                // Toggle the collapsed class
                this.classList.toggle('collapsed');
                
                // Toggle the show class on navbar-collapse
                const navbarCollapse = document.querySelector('.navbar-collapse');
                if (navbarCollapse) {
                    navbarCollapse.classList.toggle('show');
                }
            });
        } else {
            console.log('Toggle button not found');
        }
    }
    
    // Fix toggle button on page load
    fixToggleButton();
    
    // Fix toggle button on window resize
    window.addEventListener('resize', fixToggleButton);
    
    // Fix toggle button when page visibility changes
    document.addEventListener('visibilitychange', function() {
        if (!document.hidden) {
            fixToggleButton();
        }
    });
    
    // Fix toggle button when navigating between pages
    window.addEventListener('pageshow', function(event) {
        if (event.persisted) {
            fixToggleButton();
        }
    });
});
