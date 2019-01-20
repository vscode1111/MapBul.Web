function OnNewArticleDocumentReady() {
    $("#NewArticleFormSubmit").click(OnNewArticleFormSubmit);
    $('.chosenselect').chosen();
    $('.datepicker').datepicker({ language: "en" });

    $("#NewArticleMarkerSelect_chosen").click(OnNewArticleMarkerSelectClick);
    $("#NewArticleCitySelect_chosen").click(OnNewArticleCitySelectClick);
    jQuery.extend(jQuery.validator.messages, {
        required: "Fill in the"
    });
}

function OnEditArticleDocumentReady() {
    $("#EditArticleFormSubmit").click(OnEditArticleFormSubmit);
    $('.chosenselect').chosen();
    $('.datepicker').datepicker({ language: "en" });

    $("#EditArticleMarkerSelect_chosen").click(OnEditArticleMarkerSelectClick);
    $("#EditArticleCitySelect_chosen").click(OnEditArticleCitySelectClick);

    jQuery.extend(jQuery.validator.messages, {
        required: "Fill in the"
    });
}

function OnArticlesDocumentReady() {
    $('.chosenselect').chosen();
    $(".EditArticleLink").each(function (index, item) {
        $(item).click(OnEditArticleClick);
    });

    $(".DeleteArticleButton").click(OnArticleDeleteClick);


    $('.dataTable').each(function (index, item) {
        $(item).dataTable({
            "pageLength": 30,
            "autoWidth": true,
            "language": {
                "lengthMenu": "Show _MENU_",
                "zeroRecords": "Nothing found",
                "info": "Page _PAGE_ from _PAGES_",
                "infoEmpty": "No records",
                "infoFiltered": "(Found from _MAX_)",
                "search": "Search",
                "paginate": {
                    "first": "First",
                    "last": "Last",
                    "next": "Next",
                    "previous": "Previous"
                }
            }
        });
    });

    $("#NewArticleButton").click(OnNewArticleClick);

    $(".ArticleSatusSelect").each(function (index, value) {
        $(value).change(function () {
            var articleId = $(this).attr("data-articleid");
            var statusId = $(this).val();
            var url = "Articles/ChangeArticleStatus";
            $.ajax({
                url: url,
                type: "POST",
                data: {
                    articleId: articleId,
                    statusId: statusId
                },
                success: function () {
                    ViewNotification("Status changed", "success");
                },
                error: function () {
                    ViewNotification('Error', 'error');
                }
            });
        });
    });
}

function OnArticleDeleteClick() {
    var id = $(this).attr("data-id");
    $.ajax({
        url: "Articles/DeleteArticle",
        type: "POST",
        data: { articleId: id },
        success: function (data) {
            if (data) {
                ViewNotification("Article deleted", "success");
                RefreshArticlesPage();
            } else {
                ViewNotification('Error', 'error');
            }
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}

function OnEditArticleMarkerSelectClick() {
    $("#EditArticleMarkerSelect").removeAttr("disabled");
    $("#EditArticleMarkerSelect").trigger("chosen:updated");

    $("#EditArticleCitySelect").val("Not");
    $("#EditArticleCitySelect").attr("disabled", "disabled");
    $("#EditArticleCitySelect").trigger("chosen:updated");
}

function OnEditArticleCitySelectClick() {
    $("#EditArticleCitySelect").removeAttr("disabled");
    $("#EditArticleCitySelect").trigger("chosen:updated");

    $("#EditArticleMarkerSelect").val("Not");
    $("#EditArticleMarkerSelect").attr("disabled", "disabled");
    $("#EditArticleMarkerSelect").trigger("chosen:updated");
}


function OnNewArticleMarkerSelectClick() {
    $("#NewArticleMarkerSelect").removeAttr("disabled");
    $("#NewArticleMarkerSelect").trigger("chosen:updated");

    $("#NewArticleCitySelect").val("Not");
    $("#NewArticleCitySelect").attr("disabled", "disabled");
    $("#NewArticleCitySelect").trigger("chosen:updated");
}

function OnNewArticleCitySelectClick() {
    $("#NewArticleCitySelect").removeAttr("disabled");
    $("#NewArticleCitySelect").trigger("chosen:updated");

    $("#NewArticleMarkerSelect").val("Not");
    $("#NewArticleMarkerSelect").attr("disabled", "disabled");
    $("#NewArticleMarkerSelect").trigger("chosen:updated");
}

function OnEditArticleClick() {
    var articleId = $(this).parent().attr("data-articleid");
    $.ajax({
        url: "Articles/_EditArticleModalPartial",
        type: "POST",
        data: { articleId: articleId },
        success: function (data) {
            $("#Modal").modal("show");
            $("#ModalContent").html(data);
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}

function OnEditArticleFormSubmit() {
    var form = document.getElementById("EditArticleForm");

    if (!($("#EditArticleForm").valid())) {
        ViewNotification("Fill in required fields!", "error");
        return 0;
    }

    var formData = new FormData(form);
    var file = document.getElementById("EditArticlePhotoInput").files[0];
    formData.append("articlePhoto", file);

    var fileTitle = document.getElementById("EditArticleTitlePhotoInput").files[0];
    formData.append("articleTitlePhoto", fileTitle);

    $.ajax({
        url: "Articles/EditArticle",
        data: formData,
        contentType: false,
        processData: false,
        type: "POST",
        success: function () {
            AddNewArticleSuccess();
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
    return false;
}

function OnNewArticleFormSubmit() {
    var form = document.getElementById("NewArticleForm");

    if (!($("#NewArticleForm").valid())) {
        ViewNotification("Fill in required fields!", "error");
        return 0;
    }

    var formData = new FormData(form);
    var file = document.getElementById("NewArticlePhotoInput").files[0];
    formData.append("articlePhoto", file);

    file = document.getElementById("NewArticleTitlePhotoInput").files[0];
    formData.append("articleTitlePhoto", file);

    $.ajax({
        url: "Articles/AddNewArticle",
        data: formData,
        contentType: false,
        processData: false,
        type: "POST",
        success: function () {
            AddNewArticleSuccess();
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
    return false;
}

function AddNewArticleSuccess() {
    RefreshArticlesPage();
    ViewNotification("Article saved", "success");
    $("#Modal").modal("hide");
}

function RefreshArticlesPage() {
    $.ajax({
        url: "Articles/_ArticlesTablePartial",
        type: "POST",
        success: function (data) {
            $("#ArticlesContainer").html(data);
            OnArticlesDocumentReady();
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}

function OnNewArticleClick() {
    var url = $(this).attr("data-actionurl");
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            $("#Modal").modal("show");
            $("#ModalContent").html(data);
        },
        error: function () {
            ViewNotification('Error', 'error');
        }
    });
}

$(document).ready(OnArticlesDocumentReady);