function OnUsersDocumentReady() {
    
}

function DeleteSuccess(data, refreshFunction) {
    if (data.success) {
        ViewNotification("User deleted", "success");
        $("#Modal").modal("hide");
        refreshFunction();
    }
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
            ViewNotification('Error', 'error');

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
            ViewNotification('Error', 'error');

        }
    });
}


$(document).ready(OnUsersDocumentReady);