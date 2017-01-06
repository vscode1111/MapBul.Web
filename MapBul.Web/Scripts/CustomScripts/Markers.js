function OnNewMarkerDocumentReady() {
    $("#NewMarkerStreetInput").focusout(OnNewMarkerAddressChanged);
    $("#NewMarkerHouseInput").focusout(OnNewMarkerAddressChanged);
    $("#NewMarkerBuildingInput").focusout(OnNewMarkerAddressChanged);
    $("#NewMarkerFormSubmit").click(OnNewMarkerFormSubmit);
    $('.chosenselect').chosen();
    $("#MarkerCitySelect").chosen().change(OnNewMarkerAddressChanged);
    $('.clockpicker').clockpicker();
    $("#MarkerLatInput").focusout(OnMarkerCoordinateChanged);
    $("#MarkerLngInput").focusout(OnMarkerCoordinateChanged);
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green'
    });
    MapInit();

    jQuery.extend(jQuery.validator.messages, {
        required: "Заполните поле"
    });
}


function OnEditMarkerDocumentReady() {
    $("#EditMarkerStreetInput").focusout(OnEditMarkerAddressChanged);
    $("#EditMarkerHouseInput").focusout(OnEditMarkerAddressChanged);
    $("#EditMarkerBuildingInput").focusout(OnEditMarkerAddressChanged);
    $("#EditMarkerFormSubmit").click(OnEditMarkerFormSubmit); 
    $('.chosenselect').chosen();
    $("#EditMarkerCitySelect").chosen().change(OnEditMarkerAddressChanged);
    $('.clockpicker').clockpicker();
    $("#MarkerLatInput").focusout(OnMarkerCoordinateChanged);
    $("#MarkerLngInput").focusout(OnMarkerCoordinateChanged);

    
    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green'
    });
    MapInit();

    jQuery.extend(jQuery.validator.messages, {
        required: "Заполните поле"
    });
}



function OnMarkerCoordinateChanged() {
    var lat = $("#MarkerLatInput").val();
    var lng = $("#MarkerLngInput").val();
    if (lat.length === 0) {
        lat = window.marker.position.lat();
    }
    if (lng.length === 0) {
        lng = window.marker.position.lng();
    }
    var latlng = new window.google.maps.LatLng(lat, lng);
    marker.setPosition(latlng);
    window.map.setCenter(latlng);
    OnMarkerPositionChanged();
}


function OnEditMarkerFormSubmit() {
    var form = document.getElementById("EditMarkerForm");

    if (!($("#EditMarkerForm").valid())) {
        ViewNotification("Заполните обязательные поля!", "error");
        return 0;
    }

    var formData = new FormData(form);
    var photo = document.getElementById("EditMarkerPhotoInput").files[0];
    formData.append("markerPhoto", photo);
    var logo = document.getElementById("EditMarkerLogoInput").files[0];
    formData.append("markerLogo", logo);

    var openTimesElements = $(".OpenTime");
    var openTimes = [];
    var i;
    for (i = 0; i < openTimesElements.length; i++) {
        openTimes.push({ WeekDayId: $(openTimesElements[i]).attr("data-WeekDayId"), Time: $(openTimesElements[i]).val() });
    }

    var closeTimesElements = $(".CloseTime");
    var closeTimes = [];
    for (i = 0; i < closeTimesElements.length; i++) {
        closeTimes.push({ WeekDayId: $(closeTimesElements[i]).attr("data-WeekDayId"), Time: $(closeTimesElements[i]).val() });
    }

    formData.append("openTimesString", JSON.stringify(openTimes));
    formData.append("closeTimesString", JSON.stringify(closeTimes));
    formData.append("Lat", ("" + marker.position.lat()).replace(".", ","));
    formData.append("Lng", ("" + marker.position.lng()).replace(".", ","));

    $.ajax({
        url: "Markers/EditMarker",
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

function OnEditMarkerClick() {
    var markerId = $(this).parent().attr("data-markerid");
    $.ajax({
        url: "Markers/_EditMarkerModalPartial",
        type: "POST",
        data: { markerId: markerId },
        success: function (data) {
            $("#Modal").modal("show");
            $("#ModalContent").html(data);
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
}


function OnNewMarkerFormSubmit() {
    var form = document.getElementById("NewMarkerForm");

    if (!($("#NewMarkerForm").valid())) {
        ViewNotification("Заполните обязательные поля!","error");
        return 0;
    }

    var formData = new FormData(form);
    
    var photos = document.getElementById("NewMarkerPhotoInput").files;
    var x;
    if (photos.length < 10) {
        for (x = 0; x < photos.length; x++) {
            formData.append("markerPhotos", photos[x]);
        }
    } else {
        for (x = 0; x < 10; x++) {
            formData.append("markerPhotos", photos[x]);
        }
        
    }
    var logo = document.getElementById("NewMarkerLogoInput").files[0];
    formData.append("markerLogo", logo);

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
    //formData.append("Lat", ("" + marker.position.lat()).replace(".",","));
    //formData.append("Lng", ("" + marker.position.lng()).replace(".", ","));

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
    var myLatlng;
    if (isNaN(window.markerLat) || isNaN(window.markerLng))
        myLatlng = new google.maps.LatLng(49, 21);
    else
        myLatlng = new google.maps.LatLng(window.markerLat, window.markerLng);

    var mapOptions = {
        zoom: 4,
        center: myLatlng
    }
    window.map = new google.maps.Map(document.getElementById("GMap"), mapOptions);
    window.geocoder = new google.maps.Geocoder;


    var image = 'Images/mapmarker.png';
    window.marker = new google.maps.Marker({
        position: myLatlng,
        map: window.map,
        draggable: true,
        title: "",
        icon: image
    });
    marker.addListener('dragend', OnMarkerPositionChanged);
    map.addListener('click',OnMapClick);

}

function OnMapClick(e) {
    var latLng = e.latLng;
    marker.setPosition(latLng);
    OnMarkerPositionChanged();
}


function ChangeCitySelect(result) {
    $("#MarkerCitySelect option").each(function (index2, option) {
        $("#MarkerCitySelect").val("");
    });
    var found = false;
    var fullAddress = "";
    var localities = 0;
    $(result.address_components.reverse()).each(function (index1, component) {
        if (component.types.indexOf("postal_code") !== -1) {
            return;
        }
        fullAddress += component.long_name;
        if (component.types.indexOf("locality") !== -1 || component.types.indexOf('administrative_area_level_3')!==-1) {
            localities++;
            window.geocoder.geocode({ 'address': fullAddress }, function (results, status) {
                if (status === "OK") {
                    $("#MarkerCitySelect option").each(function(index2, option) {
                        if ($(option).attr("data-city-placeid") === results[0].place_id) {
                            $("#MarkerCitySelect").val($(option).attr("value")).trigger("chosen:updated");
                            found = true;
                        }
                        if ((index2 === ($("#MarkerCitySelect option").length - 1)) && !found) {
                            ViewNotification("Указанный город не найден в справочнике, сначала добавьте город", "error");
                            $("#MarkerCitySelect").val("").trigger("chosen:updated");
                        }
                    });
                }
            });
        }
        fullAddress += ", ";
        if ((index1 === ($(result.address_components).length - 1)) && localities === 0) {
            ViewNotification("Указанный город не найден в справочнике, сначала добавьте город", "error");
            $("#MarkerCitySelect").val("").trigger("chosen:updated");
        }
    });
}


function OnMarkerPositionChanged() {
    var latLng = { lat: window.marker.position.lat(), lng: window.marker.position.lng() };

    $("#MarkerLatInput").val(window.marker.position.lat());
    $("#MarkerLngInput").val(window.marker.position.lng());

    window.geocoder.geocode({ 'location': latLng }, function (results, status) {
        if (status === window.google.maps.GeocoderStatus.OK) {
            if (results[0]) {

                ChangeCitySelect(results[0]);

                $("input[name='House']").val("");
                $("input[name='Buliding']").val("");
                $(results[0].address_components).each(function(index, item) {
                    if (item.types.indexOf("street_number") >= 0) {
                        var house;
                        var building = "";

                        if (item.short_name.indexOf('с') >= 0) {
                            house = item.short_name.substring(0, item.short_name.indexOf('с'));
                            building = item.short_name.substr(item.short_name.indexOf('с') + 1);
                        } else {
                            house = item.short_name;
                        }
                        $("input[name='House']").val(house);
                        $("input[name='Buliding']").val(building);
                    }
                    if (item.types.indexOf("route") >= 0) {
                        var street = item.short_name;
                        $("input[name='Street']").val(street);
                    }
                });
            }
        }
    });
}



function OnEditMarkerAddressChanged() {    
    var address = $("#EditMarkerHouseInput").val() + ", " + $("#EditMarkerStreetInput").val() + ", " + $("#MarkerCitySelect option:selected").text();

    window.geocoder.geocode({ 'address': address }, function(results, status) {
        if (status !== "OK")
            return;
        var location = results[0].geometry.location;
        var latLng = new google.maps.LatLng(location.lat(), location.lng());
        window.marker.setPosition(latLng);
        window.map.setCenter(latLng);
        window.map.setZoom(11);
        $("#MarkerLatInput").val(location.lat());
        $("#MarkerLngInput").val(location.lng());
    });

}

function OnNewMarkerAddressChanged() {
    var address = $("#NewMarkerHouseInput").val() + ", "+$("#NewMarkerStreetInput").val()+ ", " + $("#MarkerCitySelect option:selected").text();
    window.geocoder.geocode({ 'address': address }, function(results, status) {
        if (status !== "OK") {
            ViewNotification("Проверьте корректность адреса", "error");
            return;

        }
        var location = results[0].geometry.location;
        var latLng = new google.maps.LatLng(location.lat(), location.lng());
        window.marker.setPosition(latLng);
        window.map.setCenter(latLng);
    });

}

function OnMarkersDocumentReady() {
    $('.chosenselect').chosen();
    $("#NewMarkerButton").click(OnNewMarkerClick);
    $(".DeleteMarkerButton").click(OnMarkerDeleteClick);
    $(".EditMarkerLink").each(function(index, item) {
        $(item).click(OnEditMarkerClick);
    });
    $(".MarkerSatusSelect").each(function (index, value) {
        $(value).change(function () {
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
                    ViewNotification("Статус изменен", "success");
                },
                error: function () {
                    ViewNotification('Ошибка', 'error');
                }
            });
        });
    });
    $('.dataTable').each(function(index, item) {
        $(item).dataTable({
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




function OnMarkerDeleteClick() {
    var id = $(this).attr("data-id");
    $.ajax({
        url: "Markers/DeleteMarker",
        type: "POST",
        data: { markerId: id },
        success: function (data) {
            if (data) {
                ViewNotification("Маркер удален", "success");
                RefreshMarkersPage();
            } else {
                ViewNotification('Ошибка', 'error');
            }
        },
        error: function () {
            ViewNotification('Ошибка', 'error');
        }
    });
}



$(document).ready(OnMarkersDocumentReady);

