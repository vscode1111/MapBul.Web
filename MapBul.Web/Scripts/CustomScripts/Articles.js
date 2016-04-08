﻿function OnNewArticleDocumentReady() {
    $("#NewArticleFormSubmit").click(OnNewArticleFormSubmit);
    $('.chosenselect').chosen();
    $('.datepicker').datepicker({language:"ru"});

    jQuery.extend(jQuery.validator.messages, {
        required: "Заполните поле"
    });
}

function OnEditArticleDocumentReady() {
    $("#EditArticleFormSubmit").click(OnEditArticleFormSubmit);
    $('.chosenselect').chosen();
    $('.datepicker').datepicker({ language: "ru" });

    jQuery.extend(jQuery.validator.messages, {
        required: "Заполните поле"
    });
}

function OnArticlesDocumentReady() {
    $(".EditArticleLink").each(function (index, item) {
        $(item).click(OnEditArticleClick);
    });

    $("#ArticlesTable").dataTable({
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
                    ViewNotification("Статус изменен", "success");
                },
                error: function () {
                    ViewNotification('Ошибка', 'error');
                }
            });
        });
    });
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
            ViewNotification('Ошибка', 'error');
        }
    });
}

function OnEditArticleFormSubmit() {
    var form = document.getElementById("EditArticleForm");

    if (!($("#EditArticleForm").valid())) {
        ViewNotification("Заполните обязательные поля!", "error");
        return 0;
    }

    var formData = new FormData(form);
    var file = document.getElementById("EditArticlePhotoInput").files[0];
    formData.append("articlePhoto", file);

    file = document.getElementById("EditArticleTitlePhotoInput").files[0];
    formData.append("articleTitlePhoto", file);

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
            ViewNotification('Ошибка', 'error');
        }
    });
    return false;
}

function OnNewArticleFormSubmit() {
    var form = document.getElementById("NewArticleForm");

    if (!($("#NewArticleForm").valid())) {
        ViewNotification("Заполните обязательные поля!", "error");
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
            ViewNotification('Ошибка', 'error');
        }
    });
    return false;
}

function AddNewArticleSuccess() {
    RefreshArticlesPage();
    ViewNotification("Статья сохранена", "success");
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
            ViewNotification('Ошибка', 'error');
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
            ViewNotification('Ошибка', 'error');
        }
    });
}

$(document).ready(OnArticlesDocumentReady);