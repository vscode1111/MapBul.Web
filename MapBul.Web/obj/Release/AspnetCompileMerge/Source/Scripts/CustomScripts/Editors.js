function OnEditorsDocumentReady() {
    $("#NewEditorButton").click(OnNewEditorClick);
    $(".EditorDescriptionRow").click(OnEditorRowClick);
    $('#EditorsTable table').dataTable({
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


function EditEditorSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshEditorsTable();
        ViewNotification("Изменения сохранены", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}


function OnEditorRowClick() {
    var url = "Users/_EditorInformationPartial";
    var editorId = $(this).attr("data-id");
    $.ajax({
        url: url,
        type: "POST",
        data: {
            editorId:editorId
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

function OnNewEditorClick() {
    var url = $(this).attr("data-actionurl");
    $.ajax({
        url: url,
        type: "POST",
        success: function(data) {
            $("#ModalContent").html(data);
            $("#Modal").modal("show");
        },
        error: function() {
            ViewNotification('Ошибка', 'error');
        }
    });
}

function RefreshEditorsTable() {
    var url = "Users/_EditorsTablePartial";
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#EditorsTableContainer").html(data);
            OnEditorsDocumentReady();
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
}

function AddNewEditorSuccess(data) {
    if (data.success) {
        $("#Modal").modal("hide");
        RefreshEditorsTable();
        ViewNotification("Редактор добавлен", "success");
    } else {
        ViewNotification(data.errorReason, "error");
    }
}



$(document).ready(OnEditorsDocumentReady);