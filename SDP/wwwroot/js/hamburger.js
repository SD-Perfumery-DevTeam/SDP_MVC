// Add an event listener that waits for a click on the hamburger icon.
document.getElementById("nav-links-collapsed").addEventListener("click", toggleHamburgerContent);

// Target the content to toggle when clicked.
var h = document.getElementById("nav-links-mobile");

// Toggle visibility of hamburger content.
function toggleHamburgerContent() {
    h.style.display === "none" ? h.style.display = "block" : h.style.display = "none";
}