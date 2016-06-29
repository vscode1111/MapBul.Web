function OnMarkerCategoriesDocumentReady() {
    $('#nestableMarker').nestable({}).on('change', OnMarkerCategoriesStructureChanged);
    $("#NewMarkerCategoryFormSubmit").click(SendNewMarkerCategoryForm);
    $("#EditMarkerCategoryButton").click(OnEditMarkerCategoryClick);
    jQuery.extend(jQuery.validator.messages, {
        required: "Заполните поле"
    });
    $('.colorInput').colorpicker();
}

function OnEditMarkerCategoryDocumentReady() {
    $('.colorInputCategoryEdit').colorpicker();
    $("#EditCategoryFormSubmit").click(SendEditMarkerCategoryForm);
}

function SendEditMarkerCategoryForm() {
    var form = document.getElementById("EditCategoryForm");
    var formData = new FormData(form);
    var file = document.getElementById("EditCategoryIconInput").files[0];
    formData.append("CategoryIcon", file);

    var file = document.getElementById("EditCategoryPinInput").files[0];
    formData.append("CategoryPin", file);

    $.ajax({
        url: "Dictionaries/EditCategory",
        data: formData,
        contentType: false,
        processData: false,
        type: "POST",
        success: function() {
            AddNewMarkerCategorySuccess();
            $("#Modal").modal("hide");
        },
        error:function() {
            ViewNotification("Ошибка","error");
        }
    });
    return false;
}

function OnEditMarkerCategoryClick() {
    var categoryId = $("#EditMarkerCategorySelect").val();
    $.ajax({
        url: "Dictionaries/_EditCategoryModalPartial",
        type: "POST",
        data:{categoryId:categoryId},
        success: function (data) {
            $("#ModalContent").html(data);
            $("#Modal").modal("show");
            OnEditMarkerCategoryDocumentReady();
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
}

function SendNewMarkerCategoryForm() {
    if (!($("#NewMarkerCategoryForm").valid())) {
        ViewNotification("Заполните обязательные поля!", "error");
        return 0;
    }
    var form = document.getElementById("NewMarkerCategoryForm");
    var formData = new FormData(form);
    var file = document.getElementById("NewMarkerCategoryIconInput").files[0];
    formData.append("MarkerCategoryIcon", file);
    file = document.getElementById("NewMarkerCategoryPinInput").files[0];
    formData.append("MarkerCategoryPin", file);
    $.ajax({
        url: "Dictionaries/AddNewCategory",
        data: formData,
        contentType: false,
        processData: false,
        type: "POST",
        success: AddNewMarkerCategorySuccess,
        error: function() {
            ViewNotification("Ошибка", "error");
        }
    });
    return false;
}

function RefreshMarkerCategoriesPage() {
    $.ajax({
        url: "Dictionaries/_MarkerCategoriesPartial",
        type: "POST",
        success: function (data) {
            $("#MarkerCategoriesContainer").html(data);
            OnMarkerCategoriesDocumentReady();
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
}

function AddNewMarkerCategorySuccess() {
    ViewNotification("Категория сохранена", "success");
    RefreshMarkerCategoriesPage();
}

function OnMarkerCategoriesStructureChanged(e) {
    var list = e.length ? e : $(e.target);
    var string = window.JSON.stringify(list.nestable('serialize'));
    SendNewStructure(string);
}

function SendNewStructure(string) {
    var url = "Dictionaries/SaveCategoriesStructure";
    $.ajax({
        url: url,
        type: "POST",
        data: { structure: string },
        success: function () {
            ViewNotification("Изменения сохранены", 'success');
        },
        error: function () {
            ViewNotification("Не удалось сохранить", 'error');
        }
    });
}

$(document).ready(OnMarkerCategoriesDocumentReady);