function OnAdminsDocumentReady() {
    $("#NewAdminButton").click(OnNewAdminClick);
    $('#AdminsTable table').dataTable({
        "pageLength": 30,
        "autoWidth": true,
        "language": {
            "lengthMenu": "Показать _MENU_",
            "zeroRecords": "Ничего не найдено",
            "info": "Страница _PAGE_ из _PAGES_",
            "infoEmpty": "Нет записей",
            "infoFiltered": "(Найдено из _MAX_ строк)",
            "search": "Поиск",
            "paginate": {
                "first": "Первая",
                "last": "Последняя",
                "next": "Следующая",
                "previous": "Предыдущая"
            }
        }
    });
}

function OnNewAdminClick() {
    var url = $(this).attr("data-actionurl");
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#ModalContent").html(data);
            $("#Modal").modal("show");
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
}

function RefreshAdminsTable() {
    var url = "Users/_AdminsTablePartial";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#AdminsTableContainer").html(data);
            OnAdminsDocumentReady();
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
}

function AddNewAdminSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshAdminsTable();
        ViewNotification("Администратор добавлен", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

$(document).ready(OnAdminsDocumentReady);
