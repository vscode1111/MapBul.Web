function OnNewMarkerDocumentReady() {
    $("#NewMarkerStreetInput").focusout(OnAddressChanged);
    $("#NewMarkerHouseInput").focusout(OnAddressChanged);
    $("#NewMarkerBuildingInput").focusout(OnAddressChanged);
    $("#NewMarkerFormSubmit").click(OnNewMarkerFormSubmit);
    $('.chosenselect').chosen();
    $('.clockpicker').clockpicker();
    MapInit();

    jQuery.extend(jQuery.validator.messages, {
        required: "Заполните поле"
    });
}


function OnNewMarkerFormSubmit() {
    var form = document.getElementById("NewMarkerForm");

    if (!($("#NewMarkerForm").valid())) {
        ViewNotification("Заполните обязательные поля!","error");
        return 0;
    }

    var formData = new FormData(form);
    var file = document.getElementById("NewMarkerPhotoInput").files[0];
    formData.append("markerPhoto", file);

    var openTimesElements = $(".OpenTime");
    var openTimes = [];
    var i;
    for (i = 0; i < openTimesElements.length; i++) {
        openTimes.push({ WeekDayId: $(openTimesElements[i]).attr("data-WeekDayId"), Time: $(openTimesElements[i]).val() });
    }

    var closeTimesElements = $(".CloseTime");
    var closeTimes = [];
    for ( i = 0; i < closeTimesElements.length; i++) {
        closeTimes.push({ WeekDayId: $(closeTimesElements[i]).attr("data-WeekDayId"), Time: $(closeTimesElements[i]).val() });
    }

    formData.append("openTimesString", JSON.stringify(openTimes));
    formData.append("closeTimesString", JSON.stringify(closeTimes));
    formData.append("Lat", ("" + marker.position.lat()).replace(".",","));
    formData.append("Lng", ("" + marker.position.lng()).replace(".", ","));

    $.ajax({
        url: "Markers/AddNewMarker",
        data: formData,
        contentType: false,
        processData: false,
        type: "POST",
        success: function () {
            AddNewMarkerSuccess();
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
    return false;
}


function AddNewMarkerSuccess() {
    RefreshMarkersPage();
    ViewNotification("Маркер сохранен", "success");
    $("#Modal").modal("hide");
}

function RefreshMarkersPage() {
    $.ajax({
        url: "Markers/_MarkersTablePartial",
        type: "POST",
        success: function(data) {
            $("#MarkersContainer").html(data);
            OnMarkersDocumentReady();
        },
        error: function() {
            ViewNotification('Ошибка', 'error');
        }
    });
}


function MapInit() {
    var myLatlng = new google.maps.LatLng(49, 21);
    var mapOptions = {
        zoom: 4,
        center: myLatlng
    }
    window.map = new google.maps.Map(document.getElementById("GMap"), mapOptions);

    var image = 'Images/mapmarker.png';
    window.marker = new google.maps.Marker({
        position: myLatlng,
        map: window.map,
        draggable: true,
        title: "",
        icon:image
    });

}

function OnAddressChanged() {
    var address = $("#NewMarkerHouseInput").val() + ", "+$("#NewMarkerStreetInput").val()+ ", " + $("#NewMarkerCitySelect option:selected").text();
    var url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address;
        $.ajax({
            url: url,
            type: "POST",
            success: function (data) {
                if (data.status !== "OK")
                    return;
                var location = data.results[0].geometry.location;
                var latLng = new google.maps.LatLng(location.lat, location.lng);
                window.marker.setPosition(latLng);
                window.map.setCenter(latLng);
            },
            error: function () {
                ViewNotification('Ошибка', 'error');
            }
        });
}

function OnMarkersDocumentReady() {
    $('.chosenselect').chosen();
    $("#NewMarkerButton").click(OnNewMarkerClick);
    $('#MarkersTable').dataTable({
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

    $(".MarkerSatusSelect").each(function(index,value) {
        $(value).change(function() {
            var markerId = $(this).attr("data-markerid");
            var statusId = $(this).val();
            var url = "Markers/ChangeMarkerStatus";
            $.ajax({
                url: url,
                type: "POST",
                data: {
                    markerId: markerId,
                    statusId: statusId
                },
                success: function () {
                    ViewNotification("Статус изменен","success");
                },
                error: function () {
                    ViewNotification('Ошибка', 'error');
                }
            });
        });
    });
}


function OnNewMarkerClick() {
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


$(document).ready(OnMarkersDocumentReady);