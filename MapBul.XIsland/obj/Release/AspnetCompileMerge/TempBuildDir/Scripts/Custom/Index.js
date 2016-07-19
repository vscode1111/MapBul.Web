function onDocumentReady() {
    $("#PopupWindow").hide();
    $("#OpenCalendarButton").click(OpenDataList);
    $("#OpenArticlesButton").click(OpenDataList);

}

function OpenInfo() {
    var id = $(this).attr("data-id");
    var url = $(this).attr("data-url");
    $.ajax({
        url: url,
        data: { id: id },
        type: "POST",
        success: function (data) {
            CloseInfo();
            CloseDataLists();
            $("#PopupWindow").html(data);
            $("#PopupWindow").show();
            $("body").addClass("item");
        },
        error: function () { }
    });
}

function OpenDataList() {
    var sender = $(this);
    var url = sender.attr("data-url");
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            CloseInfo();
            CloseDataLists();
            $("#PopupWindow").html(data);
            $("#PopupWindow").show();
            $("body").addClass("list");
            sender.addClass("active");
        },
        error: function() {

        }
    });
}

function CloseInfo() {
    $("#PopupWindow").hide();
    $("body").removeClass("item");
}

function CloseDataLists() {
    var calendarButton = $("#OpenCalendarButton");
    var articlesButton = $("#OpenArticlesButton");
    $("#PopupWindow").hide();
    $("body").removeClass("list");
    calendarButton.removeClass("active");
    articlesButton.removeClass("active");

}




$(document).ready(onDocumentReady);