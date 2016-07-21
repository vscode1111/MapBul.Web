var map;
function onDocumentReady() {
    $("#PopupWindow").hide();
    $("#OpenCalendarButton").click(OpenDataList);
    $("#OpenArticlesButton").click(OpenDataList);

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(InitGoogleMap, InitGoogleMap(null));
    } else {
        InitGoogleMap(null);
    }
    
    $(document).bind("ajaxSend", StartLoadingAnimation).bind("ajaxComplete", StopLoadingAnimation);
}

function StartLoadingAnimation() {
    $("#PopupWindow").hide();
    $("#Loading").show();
}
function StopLoadingAnimation() {
    $("#Loading").hide();
    $("#PopupWindow").show();

}

function InitGoogleMap(position) {
    if (position) {
        map = new window.google.maps.Map(document.getElementById('map'), {
            center: { lat: position.coords.latitude, lng: position.coords.longitude },
            zoom: 9
        });
    } else {
        map = new window.google.maps.Map(document.getElementById('map'), {
            center: { lat: 57.780672, lng: 35.058319 },
            zoom: 3
        });
    }
    map.addListener('click', function() {
        CloseInfo();
        CloseDataLists();
    });
}

function OpenInfo() {
    var id = $(this).attr("data-id");
    var url = $(this).attr("data-url");
    $("body").addClass("item");
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
    $("body").addClass("list");
    $.ajax({
        url: url,
        type: "POST",
        success: function (data) {
            CloseInfo();
            CloseDataLists();
            $("#PopupWindow").html(data);
            $("#PopupWindow").show();
            sender.addClass("active");
            $("body").addClass("list");
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