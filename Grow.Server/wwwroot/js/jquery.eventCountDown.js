var timeinterval;

$.fn.eventCountDown = function () {

	var targetDateString = $(this).attr("asp-datetime");
    if (!targetDateString)
        return;

    var targetDate = new Date(targetDateString);
    targetDate.setTime(targetDate.getTime() + targetDate.getTimezoneOffset() * 60 * 1000);

    initializeClock(this, targetDate);

    return this;
};

function getTimeRemaining(endtime) {
    var t = endtime.getTime() - Date.now();
    var seconds = Math.floor((t / 1000) % 60);
    var minutes = Math.floor((t / 1000 / 60) % 60);
    var hours = Math.floor((t / (1000 * 60 * 60)) % 24);
    var days = Math.floor(t / (1000 * 60 * 60 * 24));
    return {
        'total': t,
        'days': days,
        'hours': hours,
        'minutes': minutes,
        'seconds': seconds
    };
}

function initializeClock(clock, endtime) {
    if (!clock || !endtime) return;
    var daysSpan = clock.querySelector('.days');
    var hoursSpan = clock.querySelector('.hours');
    var minutesSpan = clock.querySelector('.minutes');
    var secondsSpan = clock.querySelector('.seconds');

    function updateClock() {
        var t = getTimeRemaining(endtime);

        daysSpan.innerHTML = t.days;
        hoursSpan.innerHTML = ('0' + t.hours).slice(-2);
        minutesSpan.innerHTML = ('0' + t.minutes).slice(-2);
        secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);

        if (t.total <= 0) {
            clearInterval(timeinterval);
		}
	}

	updateClock();
    var timeinterval = setInterval(updateClock, 1000);
}