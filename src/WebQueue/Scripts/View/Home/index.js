$(document).ready(() => {
    const calendarElement = document.querySelector('#datePicker');
    const timePickerElement = $('#timePicker');
    let selectedDate = new Date().toLocaleDateString('ru');
    let selectedTime;

    const makeDateUnselectable = (dateElem, should) => {
        if (should) {
            dateElem.classList.remove("jsCalendar-unselectable");
        } else {
            dateElem.classList.add("jsCalendar-unselectable");
        }
    };

    const getTimeFromDate = (date) => date.toLocaleTimeString("Ru", {hour: '2-digit', minute: '2-digit'});

    const dateWorkTimes = {}; // { DateToLocale: {startTime, endTime} }

    const calendar = jsCalendar.new({
        target: calendarElement,
        firstDayOfTheWeek: 2,
        navigatorPosition: "right",
        language: 'ru',
        dayFormat: 'DD',
        min: new Date(new Date().setDate(new Date().getDate() - 1)),
        onMonthRender: function (month_index, month_element, month_info) {
            $(".jsCalendar-title-name").append(" " + month_info.start.getFullYear())
        },
        onDateRender: (date, date_element, date_info) => {
            days.forEach(day => {
                let shortDate = date.toLocaleDateString('ru');
                
                if (date > Date.now() && day.DayOfWeek == date.getDay()) {
                    if (day.WorkStartTime && day.WorkEndTime) {
                        dateWorkTimes[shortDate] = {
                            start: getTimeFromDate(day.WorkStartTime),
                            end: getTimeFromDate(day.WorkEndTime)
                        }
                    }
                    makeDateUnselectable(date_element, day.IsWorkTime)
                }

                if (day.ExactDate && day.ExactDate.toLocaleDateString('ru') === shortDate) {
                    if (day.WorkStartTime && day.WorkEndTime) {
                        dateWorkTimes[day.ExactDate.toLocaleDateString('ru')] = {
                            start: getTimeFromDate(day.WorkStartTime),
                            end: getTimeFromDate(day.WorkEndTime)
                        }
                    }
                    makeDateUnselectable(date_element, day.IsWorkTime)
                }
            })
        }
    });

    updateTimePicker();

    calendar.onDateClick((event, date) => {
        if (event.target.classList.contains('jsCalendar-unselectable')) {
            return alert("На выбранную дату нельзя записаться.");
        }
        calendar.set(date);

        selectedDate = date.toLocaleDateString('ru');
        updateTimePicker();
    });


    function updateTimePicker() {
        timePickerElement.val("");
        timePickerElement.attr("placeholder", "Загрузка...");
        $.ajax(`/QueuePositions/GetBusyPositions?selectedDate=${selectedDate}`)
            .done((data) => {
                if (!data) return timePickerElement.attr("placeholder", "Ошибка загрузки");
                timePickerElement.attr("placeholder", "Нажмите для выбора времени");
                const dateList = JSON.parse(data);
                const disabledTime = dateList.map(el => {
                    let date = new Date(el);
                    let startTime = `${date.getHours()}:${date.getMinutes()}`;
                    let date2 = new Date(date.setMinutes(date.getMinutes() + 30));
                    let endTime = `${date2.getHours()}:${date2.getMinutes()}`;
                    return [startTime, endTime];
                });

                timePickerElement.timepicker({
                    scrollDefault: 'now',
                    timeFormat: 'H:i',
                    minTime: dateWorkTimes[selectedDate]?.start ?? '9:00',
                    maxTime: dateWorkTimes[selectedDate]?.end ?? '17:00',
                    forceRoundTime: true,
                    disableTimeRanges: disabledTime
                });
            })
            .fail(() => {
                timePickerElement.attr("placeholder", "Ошибка загрузки");
            })

        timePickerElement.on('changeTime', function () {
            selectedTime = $(this).val();
        });
    }

    $("#submitBtn").click(() => {
        if (!selectedDate) return alert("Дата в календаре не выбрана!");
        if (!selectedTime) return alert("Время записи не выбрано!");

        $("#submitBtn").addClass("disabled");
        window.location.href = `/Confirmations/EnqueuePosition?selectedDate=${selectedDate} ${selectedTime}`;
    });
})