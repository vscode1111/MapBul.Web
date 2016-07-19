function EventsListDocumentReady() {
    $('.datepick').pickmeup({
        format: 'd.m.Y',
        hide_on_select: true,
        locale: {
            days: ['Воскресенье', 'Понедельник', 'Вторник', 'Среда', 'Четверг', 'Пятница', 'Суббота', 'Воскресенье'],
            daysShort: ['Воск', 'Пон', 'Втор', 'Сред', 'Четв', 'Пят', 'Суб', 'Воск'],
            daysMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб', 'Вс'],
            months: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
            monthsShort: ['Янв', 'Фев', 'Март', 'Апр', 'Май', 'Июнь', 'Июль', 'Авг', 'Сен', 'Окт', 'Нов', 'Дек']
        },
        show: function() {
            $('.datepick_field').addClass("opened");
        },
        hide: function() {
            $('.datepick_field').removeClass("opened");
        }
    });


    $('.list_link').click(OpenInfo);
}


