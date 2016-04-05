function OnDictionariesPageReady() {
    $('.chosenselect').chosen();
    $("#NewCountryButton").click(NewCountryButtonClick);
    $("#NewRegionButton").click(NewRegionButtonClick);
    $("#NewCityButton").click(NewCityButtonClick);

    $('.CitiesTable').dataTable({
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
            ViewNotification('Ошибка', 'error');
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
    $.ajax({
        url: url,
        type: "POST",
        data: {name:value},
        success: function(data) {
            ViewNotification("Страна добавлена", 'success');
            RefreshCitiesPage();
        },
        error: function () {
            ViewNotification("Не удалось добавить", 'error');
        }
    });
}

function NewRegionButtonClick() {
    var url = $(this).attr("data-actionurl");
    var value = $("#NewRegionInput").val();
    var country = $("#CountrySelectForRegion").val();
    if (value.length === 0) {
        ViewNotification("Имя региона не может быть пустым", "error");
        return;
    }
    $.ajax({
        url: url,
        type: "POST",
        data: {
            name: value,
            countryId:country
        },
        success: function (data) {
            ViewNotification("Регион добавлен", 'success');
            RefreshCitiesPage();
        },
        error: function () {
            ViewNotification("Не удалось добавить", 'error');
        }
    });
}

function NewCityButtonClick() {
    var url = $(this).attr("data-actionurl");
    var value = $("#NewCityInput").val();
    var region = $("#RegionSelectForCity").val();

    if (value.length === 0) {
        ViewNotification("Имя города не может быть пустым", "error");
        return;
    }
    $.ajax({
        url: url,
        type: "POST",
        data: {
            name: value,
            regionId: region
        },
        success: function (data) {
            ViewNotification("Город добавлен", 'success');
            RefreshCitiesPage();
        },
        error: function () {
            ViewNotification("Не удалось добавить", 'error');
        }
    });
}


$(document).ready(OnDictionariesPageReady);