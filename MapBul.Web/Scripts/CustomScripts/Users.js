function OnUsersDocumentReady() {
    
}


function AjaxRequestWithoutParams(url) {
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            if (data.success) {
                ViewNotification(data.message, 'success');
                RefreshClientsPage();
            } else
                ViewNotification(data.message, 'error');
        },
        error: function () {
            ViewNotification('Ошибка', 'error');

        }
    });
}

function AjaxRequest(selectedIds, url) {
    $.ajax({
        url: url,
        type: "POST",
        data: {
            ids: JSON.stringify(selectedIds)
        },
        success: function (data) {
            if (data.success) {
                ViewNotification(data.message, 'success');
                RefreshClientsPage();
            } else
                ViewNotification(data.message, 'error');
        },
        error: function () {
            ViewNotification('Ошибка', 'error');

        }
    });
}


$(document).ready(OnUsersDocumentReady);