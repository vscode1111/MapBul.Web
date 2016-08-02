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
        data:{countryId:id},
        success: function (data) {
            if (data.success) {
                RefreshCitiesPage();
                ViewNotification("Страна удалена", "success");
            }
        },
        error: function () {
            ViewNotification("Ошибка", "error");
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
                ViewNotification("Город удален", "success");
            }
        },
        error: function () {
            ViewNotification("Ошибка", "error");
        }
    });
}

function OnCountrySelectForCityChanged() {
    var country = $("#CountrySelectForCity option:selected").attr("data-countrycode");
    cityAutocomplete.setComponentRestrictions({ 'country': country });
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
            ViewNotification("Ошибка", "error");
        }
    });
}

function NewCountryButtonClick() {
    var url = $(this).attr("data-actionurl");
    var value = $("#NewCountryInput").val();
    if (value.length === 0) {
        ViewNotification("Имя страны не может быть пустым","error");
        return;
    }


    window.geocoder.geocode({ 'address': value }, function(results, status) {

        if (status !== "OK") {
            ViewNotification("Страна не найдена", "error");
            return;
        }
        var countryName = results[0].address_components[0].long_name;
        var placeId = results[0].place_id;
        var types = results[0].address_components[0].types;
        var code = results[0].address_components[0].short_name;

        if (types.indexOf("country") === -1) {
            ViewNotification("Страна не найдена", "error");
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
            success: function() {
                ViewNotification("Страна добавлена", "success");
                RefreshCitiesPage();
            },
            error: function() {
                ViewNotification("Не удалось добавить", "error");
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
        ViewNotification("Имя города не может быть пустым", "error");
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
        success: function() {
            ViewNotification("Город добавлен", "success");
            RefreshCitiesPage();
        },
        error: function() {
            ViewNotification("Не удалось добавить", "error");
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