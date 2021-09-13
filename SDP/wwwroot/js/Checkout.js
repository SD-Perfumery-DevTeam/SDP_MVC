

function performCheckout(_this)
{
    /**
     * Line 8 - 11: Changes color of button for .1 milisec duration
     * */
    _this.style.backgroundColor = "#026612";
    setTimeout(function () {
        _this.style.backgroundColor = "#06cf27";
    }, 100);
}