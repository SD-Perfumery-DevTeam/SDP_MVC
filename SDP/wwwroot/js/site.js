// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.




$("body").on("submit", "#popup", function () {
    return confirm("Do you want to Delete?");
});

   

$("body").on("submit", "#popupConfrimSend", function () {
    return confirm("Are you sure you want to send out this promotion");
});

