// Set active class on navbar items based on current URL
document.addEventListener('DOMContentLoaded', function() {
    // Get current URL path
    const currentPath = window.location.pathname.toLowerCase();
    
    // Get all nav links
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    
    // Loop through nav links and add active class if URL matches
    navLinks.forEach(link => {
        const href = link.getAttribute('href').toLowerCase();
        
        // Skip the Get Started button
        if (link.classList.contains('get-started-btn')) {
            return;
        }
        
        // Check if current path matches the link's href
        if (currentPath === href || 
            (href !== '/' && currentPath.startsWith(href)) || 
            (currentPath === '/' && href === '/home') ||
            (currentPath === '/home' && href === '/')) {
            
            // Add active class to parent li element
            link.parentElement.classList.add('active');
        }
    });
});

// Add scroll effect to navbar
window.addEventListener('scroll', function() {
    const navbar = document.querySelector('.navbar-modern');
    if (window.scrollY > 50) {
        navbar.classList.add('navbar-scrolled');
    } else {
        navbar.classList.remove('navbar-scrolled');
    }
});
