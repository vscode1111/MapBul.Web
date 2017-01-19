var geocoder;
//var regionAutocomplete;
var countryAutocomplete;
var cityAutocomplete;
var newCityLat;
var newCityLng;
var newCityPlaceId;
var newCityName;
function OnDictionariesPageReady() {
    $(".chosenselect").chosen();
    $("#NewCountryButton").click(NewCountryButtonClick);
    //$("#NewRegionButton").click(NewRegionButtonClick);
    $("#NewCityButton").click(NewCityButtonClick);
    //$("#NewRegionInput").focusout(OnRegionsInputChanged);
    $("#NewCityInput").focusout(OnCitiesInputChanged);

    $("#CountrySelectForCity").change(OnCountrySelectForCityChanged);

    $(".DeleteCountryButton").click(OnCountryDeleteClick);
    $(".DeleteCityButton").click(OnCityDeleteClick);


    $(".CitiesTable").dataTable({
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
    geocoder = new window.google.maps.Geocoder();
    countryAutocomplete = new window.google.maps.places.Autocomplete(
    (document.getElementById("NewCountryInput")),
    { types: ["(regions)"] });
    /*regionAutocomplete=new window.google.maps.places.Autocomplete(
    (document.getElementById('NewRegionInput')), { types: ['(regions)'] });
    regionAutocomplete.addListener('place_changed', OnRegionsInputChanged);*/

    cityAutocomplete = new window.google.maps.places.Autocomplete(
        document.getElementById("NewCityInput"),
        {
            types: ["(cities)"],
            componentRestrictions: { country: $("#CountrySelectForCity option:selected").attr("data-countrycode") }
        });

    cityAutocomplete.addListener("place_changed", OnCitiesInputChanged);

}

function OnCountryDeleteClick() {
    var id = $(this).attr("data-id");
    $.ajax({
        url: "Dictionaries/DeleteCountry",
        type: "POST",
        data: { countryId: id },
        success: function (data) {
            if (data.success) {
                RefreshCitiesPage();
                ViewNotification("The country is removed", "success");
            }
        },
        error: function () {
            ViewNotification("Error", "error");
        }
    });
}

function OnCityDeleteClick() {
    var id = $(this).attr("data-id");
    $.ajax({
        url: "Dictionaries/DeleteCity",
        type: "POST",
        data: { cityId: id },
        success: function (data) {
            if (data.success) {
                RefreshCitiesPage();
                ViewNotification("City removed", "success");
            }
        },
        error: function () {
            ViewNotification("Error", "error");
        }
    });
}

function OnCountrySelectForCityChanged() {
    var country = $("#CountrySelectForCity option:selected").attr("data-countrycode");
    cityAutocomplete.setComponentRestrictions({ 'country': country });
}

function doGet(e) {

    var sourceText = ''
    if (e.parameter.q) {
        sourceText = e.parameter.q;
    }

    var sourceLang = 'auto';
    if (e.parameter.source) {
        sourceLang = e.parameter.source;
    }

    var targetLang = 'ja';
    if (e.parameter.target) {
        targetLang = e.parameter.target;
    }

    /* Option 1 */

    var translatedText = LanguageApp.translate(sourceText, sourceLang, targetLang);

    /* Option 2 */

    var url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
              + sourceLang + "&tl=" + targetLang + "&dt=t&q=" + encodeURI(sourceText);

    var result = JSON.parse(UrlFetchApp.fetch(url).getContentText());

    translatedText = result[0][0][0];

    var json = {
        'sourceText': sourceText,
        'translatedText': translatedText
    };

    // set JSONP callback
    var callback = 'callback';
    if (e.parameter.callback) {
        callback = e.parameter.callback
    }

    // return JSONP
    return ContentService
             .createTextOutput(callback + '(' + JSON.stringify(json) + ')')
             .setMimeType(ContentService.MimeType.JAVASCRIPT);
}

function RefreshCitiesPage() {
    $.ajax({
        url: "Dictionaries/_CitiesPartial",
        type: "POST",
        success: function (data) {
            $("#CitiesContainer").html(data);
            OnDictionariesPageReady();
        },
        error: function () {
            ViewNotification("Error", "error");
        }
    });
}

function NewCountryButtonClick() {
    var url = $(this).attr("data-actionurl");
    var value = $("#NewCountryInput").val();
    if (value.length === 0) {
        ViewNotification("The country's name can not be empty", "error");
        return;
    }


    window.geocoder.geocode({ 'address': value }, function (results, status) {

        if (status !== "OK") {
            ViewNotification("The country can not be found", "error");
            return;
        }
        var countryName = results[0].address_components[0].long_name;
        var placeId = results[0].place_id;
        var types = results[0].address_components[0].types;
        var code = results[0].address_components[0].short_name;

        if (types.indexOf("country") === -1) {
            ViewNotification("The country can not be found", "error");
            return;
        }

        $.ajax({
            url: url,
            type: "POST",
            data: {
                name: countryName,
                placeId: placeId,
                code: code
            },
            success: function () {
                ViewNotification("Country added", "success");
                RefreshCitiesPage();
            },
            error: function () {
                ViewNotification("Unable to add", "error");
            }
        });
    });
}




/*function NewRegionButtonClick() {
    var url = $(this).attr("data-actionurl");
    var value = $("#NewRegionInput").val();
    var countryId = $("#CountrySelectForRegion").val();
    var countryName = $("#CountrySelectForRegion option:selected").html();
    if (value.length === 0) {
        ViewNotification("Имя региона не может быть пустым", "error");
        return;
    }

    window.geocoder.geocode({ 'address': countryName + ", " + value }, function (results, status) {

        if (status !== "OK") {
            ViewNotification("Регион не найден", 'error');
            return;
        }
        var regionName = results[0].address_components[0].long_name;
        var placeId = results[0].place_id;
        var types = results[0].address_components[0].types;

        if (types.indexOf("political") === -1) {
            ViewNotification("Регион не найден", 'error');
            return;
        }

        $.ajax({
            url: url,
            type: "POST",
            data: {
                name: regionName,
                countryId: countryId,
                placeId: placeId
            },
            success: function(data) {
                ViewNotification("Регион добавлен", 'success');
                RefreshCitiesPage();
            },
            error: function() {
                ViewNotification("Не удалось добавить", 'error');
            }
        });
    });
}*/

function NewCityButtonClick() {
    var url = $(this).attr("data-actionurl");
    var countryId = $("#CountrySelectForCity").val();

    var value = $("#NewCityInput").val();

    if (value.length === 0) {
        ViewNotification("The city's name can not be empty", "error");
        return;
    }
    $.ajax({
        url: url,
        type: "POST",
        data: {
            name: newCityName,
            countryId: countryId,
            placeId: newCityPlaceId,
            lat: newCityLat,
            lng: newCityLng
        },
        success: function () {
            ViewNotification("City added", "success");
            RefreshCitiesPage();
        },
        error: function () {
            ViewNotification("Unable to add", "error");
        }
    });
}


/*function HideCountryForRegionsSelect() {
    $("#CountrySelectForRegion").val("");
    /*$("#CountrySelectForRegion option").each(function (index4, option) {
        $(option).removeAttr("selected");
        $(option).hide();
        $(option).attr("disabled","");
    });#1#
}*/


/*function OnRegionsInputChanged() {
    var activeOptions = [];

    //var regionName = $("#NewRegionInput").val();

    var result = regionAutocomplete.getPlace();

    if (result.types.indexOf("administrative_area_level_1") === -1 && result.types.indexOf("administrative_area_level_2") === -1 && result.types.indexOf("colloquial_area") === -1) {
        HideCountryForRegionsSelect();
        return;
    }
    var fullPlaceAddress = "";
    $(result.address_components.reverse()).each(function(index2, component) {
        if (component.types.indexOf("postal_code") !== -1) {
            return;
        }
        fullPlaceAddress += component.long_name;
        window.geocoder.geocode({ 'address': fullPlaceAddress }, function(results, status) {

            if (status !== "OK") {
                return;
            }
            $(results).each(function(index3, result) {
                var placeId = result.place_id;
                activeOptions.push(placeId);
            });

            HideCountryForRegionsSelect();

            $("#CountrySelectForRegion option").each(function(index4, option) {
                if (activeOptions.indexOf($(option).attr("data-placeid")) !== -1) {
                    //$(option).removeAttr("disabled");
                    $("#CountrySelectForRegion").val($(option).attr("value"));
                    //$(option).show();
                }
            });
        });
        fullPlaceAddress += ", ";
    });

}*/


function HideCountryForCitiesSelect() {
    $("#CountrySelectForCity").val("");
    $("#CountrySelectForCity option").each(function (index4, option) {
        $(option).removeAttr("selected");
        $(option).hide();
        $(option).attr("disabled", "");
    });
}

function OnCitiesInputChanged() {

    var result = cityAutocomplete.getPlace();
    newCityLat = result.geometry.location.lat;
    newCityLng = result.geometry.location.lng;
    newCityPlaceId = result.place_id;
    newCityName = result.address_components[0].long_name;
    /*var activeOptions = [];

    var cityName = $("#NewCityInput").val();
    window.geocoder.geocode({ 'address': cityName }, function(results, status) {
        if (status !== "OK") {
            return;
        }
        $(results).each(function(index1, result) {
            if (result.types.indexOf("locality") === -1) {
                HideCountryForCitiesSelect();
                return;
            }
            var fullPlaceAddress = "";
            $(result.address_components.reverse()).each(function(index2, component) {
                if (component.types.indexOf("postal_code") !== -1) {
                    return;
                }
                fullPlaceAddress += component.long_name;
                window.geocoder.geocode({ 'address': fullPlaceAddress }, function(results, status) {

                    if (status !== "OK") {
                        return;
                    }
                    $(results).each(function(index3, result) {
                        var placeId = result.place_id;
                        activeOptions.push(placeId);
                    });

                    HideCountryForCitiesSelect();

                    $("#CountrySelectForCity option").each(function(index4, option) {
                        if (activeOptions.indexOf($(option).attr("data-placeid")) !== -1) {
                            $(option).removeAttr("disabled");
                            $("#CountrySelectForCity").val($(option).attr("value"));
                            $(option).show();
                        }
                    });
                });
                fullPlaceAddress += ", ";
            });
        });
    });*/
}




$(document).ready(OnDictionariesPageReady);