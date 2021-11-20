

$(document).ready(function () {
    $('.custom-file-input').on("change", function () {
        var filename = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(filename);
    });
});


$('textarea.message_area').on('keyup', function () {
    var maxlen = $(this).attr('maxlength');

    var length = $(this).val().length;
    if (length > (maxlen - 10)) {
        $('.textarea_message').text('max length ' + maxlen + ' characters only!')
    }
    else {
        $('.textarea_message').text('');
    }
});