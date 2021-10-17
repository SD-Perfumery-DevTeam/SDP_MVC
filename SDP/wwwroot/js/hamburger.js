// Add an event listener that waits for a click on the hamburger icon.
document.getElementById("nav-links-collapsed").addEventListener("click", toggleHamburgerContent);

// Target the content to toggle when clicked.
var h = document.getElementById("nav-links-mobile");
// This variable avoids the conditional statement from evaluating incorrectly
// on the first click. Another workaround is to add display: none as inline
// styling (prefer to avoid this).
var hContentVisible = false;

// Toggle visibility of hamburger content.
function toggleHamburgerContent() {
    if (!hContentVisible) {
        h.style.display = "block";
        hContentVisible = true;
    }
    else {
        h.style.display = "none";
        hContentVisible = false;
    }
}
