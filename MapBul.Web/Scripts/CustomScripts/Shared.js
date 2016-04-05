function ViewNotification(message, type) {
    var shortCutFunction = true;
    var msg = message;
    toastr.options = {
        closeButton: $('#closeButton').prop('checked'),
        debug: $('#debugInfo').prop('checked'),
        progressBar: $('#progressBar').prop('checked'),
        positionClass: $('#positionGroup input:radio:checked').val() || 'toast-top-right',
        onclick: null
    };
    if ($('#addBehaviorOnToastClick').prop('checked')) {
        toastr.options.onclick = function () {
            alert('You can perform some custom action after a toast goes away');
        };
    }
    toastr.options.showDuration = 1000;

    toastr.options.hideDuration = 1000;

    toastr.options.timeOut = 2000;

    toastr.options.extendedTimeOut = 2000;

    toastr.options.showEasing = 'swing';

    toastr.options.hideEasing = 'linear';

    toastr.options.showMethod = 'fadeIn';

    toastr.options.hideMethod = 'fadeOut';

    $("#toastrOptions").text("Command: toastr["
        + shortCutFunction
        + "](\""
        + msg
        + "\")\n\ntoastr.options = "
        + JSON.stringify(toastr.options, null, 2)
    );
    var $toast = toastr[type](msg); // Wire up an event handler to a button in the toast, if it exists
    $toastlast = $toast;
};