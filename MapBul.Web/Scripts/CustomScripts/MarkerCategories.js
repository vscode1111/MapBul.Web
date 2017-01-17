function OnMarkerCategoriesDocumentReady() {
    $('#nestableMarker').nestable({}).on('change', OnMarkerCategoriesStructureChanged);
    $("#NewMarkerCategoryFormSubmit").click(SendNewMarkerCategoryForm);
    $("#EditMarkerCategoryButton").click(OnEditMarkerCategoryClick);
    jQuery.extend(jQuery.validator.messages, {
        required: "Fill out the field"
    });
    $('.colorInput').colorpicker();
}

function OnEditMarkerCategoryDocumentReady() {
    $('.colorInputCategoryEdit').colorpicker();
    $("#EditCategoryFormSubmit").click(SendEditMarkerCategoryForm);
    $("#DeleteCategoryButton").click(OnDeleteMarkerCategoryClick);

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
            ViewNotification("Error", "error");
        }
    });
    return false;
}

function OnDeleteMarkerCategoryClick() {
    var categoryId = $("#EditCategoryForm input[name=Id]").val();
    $.ajax({
        url: "Dictionaries/DeleteCategory",
        type: "POST",
        data: { categoryId: categoryId },
        success: function (data) {
            if (data.success) {
                $("#Modal").modal("hide");
                RefreshMarkerCategoriesPage();
            }
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
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
            ViewNotification('Error', 'error');
        }
    });
}

function SendNewMarkerCategoryForm() {
    if (!($("#NewMarkerCategoryForm").valid())) {
        ViewNotification("Fill in required fields!", "error");
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
            ViewNotification("Error", "error");
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
            ViewNotification('Error', 'error');
        }
    });
}

function AddNewMarkerCategorySuccess() {
    ViewNotification("Category saved", "success");
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
            ViewNotification("Changes saved", 'success');
        },
        error: function () {
            ViewNotification("Could not save", 'error');
        }
    });
}

$(document).ready(OnMarkerCategoriesDocumentReady);