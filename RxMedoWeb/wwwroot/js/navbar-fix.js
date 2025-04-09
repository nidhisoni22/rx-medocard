document.addEventListener('DOMContentLoaded', function() {
    // Get current path
    const currentPath = window.location.pathname.toLowerCase();
    
    // Force the toggle button to be visible on mobile
    function forceToggleButtonVisibility() {
        const navbarToggler = document.querySelector('.navbar-toggler');
        
        if (navbarToggler) {
            // Apply inline styles with !important to override any CSS
            const togglerStyles = [
                'display: ' + (window.innerWidth <= 1100 ? 'block' : 'none') + ' !important',
                'visibility: ' + (window.innerWidth <= 1100 ? 'visible' : 'hidden') + ' !important',
                'opacity: ' + (window.innerWidth <= 1100 ? '1' : '0') + ' !important',
                'position: absolute !important',
                'right: 15px !important',
                'top: 50% !important',
                'transform: translateY(-50%) !important',
                'z-index: 1050 !important',
                'background-color: rgba(239, 71, 111, 0.3) !important',
                'padding: 0.7rem !important',
                'border-radius: 8px !important',
                'margin-left: auto !important',
                'width: auto !important',
                'height: auto !important',
                'cursor: pointer !important',
                'pointer-events: auto !important',
                'overflow: visible !important',
                'border: none !important',
                'outline: none !important',
                'box-shadow: none !important'
            ].join(';');
            navbarToggler.style.cssText = togglerStyles;
            
            // Make sure the hamburger icon is visible
            const hamburgerIcon = navbarToggler.querySelector('.hamburger-icon');
            if (hamburgerIcon) {
                const hamburgerStyles = [
                    'width: 30px !important',
                    'height: 22px !important',
                    'position: relative !important',
                    'display: flex !important',
                    'flex-direction: column !important',
                    'justify-content: space-between !important',
                    'visibility: visible !important',
                    'opacity: 1 !important'
                ].join(';');
                hamburgerIcon.style.cssText = hamburgerStyles;
                
                // Make sure all spans in the hamburger icon are visible
                const spans = hamburgerIcon.querySelectorAll('span');
                const spanStyles = [
                    'display: block !important',
                    'width: 100% !important',
                    'height: 3px !important',
                    'background-color: #ef476f !important',
                    'border-radius: 3px !important',
                    'transition: all 0.3s ease !important',
                    'visibility: visible !important',
                    'opacity: 1 !important'
                ].join(';');
                
                spans.forEach(span => {
                    span.style.cssText = spanStyles;
                });
                
                // Ensure the toggle button works correctly
                navbarToggler.addEventListener('click', function(e) {
                    e.preventDefault();
                    e.stopPropagation();
                    
                    // Toggle the collapsed class
                    if (this.classList.contains('collapsed')) {
                        this.classList.remove('collapsed');
                    } else {
                        this.classList.add('collapsed');
                    }
                    
                    // Toggle the show class on navbar-collapse
                    const navbarCollapse = document.querySelector('.navbar-collapse');
                    if (navbarCollapse) {
                        navbarCollapse.classList.toggle('show');
                    }
                });
            }
        }
    }
    
    // Call the function initially
    forceToggleButtonVisibility();
    
    // Call the function whenever the window is resized
    window.addEventListener('resize', forceToggleButtonVisibility);
    
    // Run the fix when page visibility changes
    document.addEventListener('visibilitychange', function() {
        if (!document.hidden) {
            forceToggleButtonVisibility();
        }
    });
});
