function OnIndexDocumentReady() {
    IndexMapInit();
}

function IndexMapInit() {
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


    $(markers).each(function (index, item) {
        var contentString = "<h3>" + item.Name + "</h3>";
        var infowindow = new window.google.maps.InfoWindow({
            content: contentString
        });
        var position = new window.google.maps.LatLng(item.Lat, item.Lng);
        var marker = new window.google.maps.Marker({
            position: position,
            map: window.map,
            icon: markerImage
        });
        marker.addListener('click', function () {
            infowindow.open(map, marker);
        });
    });
    
}

$(document).ready(OnIndexDocumentReady);