// Add an event listener that waits for a click on the hamburger icon.
document.getElementById("nav-links-collapsed").addEventListener("click", toggleHamburgerContent);

// Define the class name to target for display / hide.
var hClass = document.getElementById("nav-links-mobile").className;

// Toggle visibility of hamburger content.
function toggleHamburgerContent() {
    if (document.getElementById("nav-links-mobile").className === "hide") {
        document.getElementById("nav-links-mobile").className = "show";
    }
    else {
        document.getElementById("nav-links-mobile").className = "hide";
    }
}
