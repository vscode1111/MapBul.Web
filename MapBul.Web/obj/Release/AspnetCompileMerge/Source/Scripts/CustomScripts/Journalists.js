function OnJournalistsDocumentReady() {
    $("#NewJornalistButton").click(OnNewJournalistClick);
    $(".JournalistDescriptionRow").click(OnJournalistRowClick);
    $('#JournalistsTable table').dataTable({
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

function EditJournalistSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshJournalistsTable();
        ViewNotification("Изменения сохранены", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

function OnJournalistRowClick() {
    var url = "Users/_JournalistInformationPartial";
    var journalistId = $(this).attr("data-id");
    $.ajax({
        url: url,
        type: "POST",
        data: {
            journalistId: journalistId
        },
        success: function (data) {
            $("#ModalContent").html(data);
            $("#Modal").modal("show");
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
}

function OnNewJournalistClick() {
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

function RefreshJournalistsTable() {
    var url = "Users/_JournalistsTablePartial";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#JournalistsTableContainer").html(data);
            OnJournalistsDocumentReady();
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
}

function AddNewJournalistSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshJournalistsTable();
        ViewNotification("Журналист добавлен", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

$(document).ready(OnJournalistsDocumentReady);


