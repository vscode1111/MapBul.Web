function OnGuidesDocumentReady() {
    $("#NewGuideButton").click(OnNewGuideClick);
    $(".GuideDescriptionRow").click(OnGuideRowClick);
    $('#GuidesTable table').dataTable({
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

function OnNewGuideClick() {
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

function AddNewGuideSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshGuidesTable();
        ViewNotification("Гид добавлен", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

function OnGuideRowClick() {
    var url = "Users/_GuideInformationPartial";
    var guideId = $(this).attr("data-id");
    $.ajax({
        url: url,
        type: "POST",
        data: {
            guideId: guideId
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



function EditGuideSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshGuidesTable();
        ViewNotification("Изменения сохранены", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}

function RefreshGuidesTable() {
    var url = "Users/_GuidesTablePartial";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#GuidesTableContainer").html(data);
            OnGuidesDocumentReady();
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
}

$(document).ready(OnGuidesDocumentReady);
