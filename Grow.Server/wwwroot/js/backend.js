/*
 * Hint: Required scripts (e.g. grow.helpers.js) are including via bundling (bundleconfig.json) into backend.min.js
 */

// MAGIC NUMBERS / STRINGS
url_select_year = "/Admin/Home/SetYear";

var debug;

// RIG UP PLUGINS AND GLOBAL EVENT HANDLERS
$(document).ready(function () {

    // rig up contest selector (in Admin navbar)
    var selector = $("#contest-selector");
    selector.change(function () {
        var newValue = selector.children("option:selected").val();
        $.post(url_select_year, { year: newValue }, function (data) {
            location.reload();
        });
    });

    // pagination links
    $(".pagination").each((index, element) => {
        $(element).pagination({
            pages: $(element).attr('max'),
            currentPage: $(element).attr('current')
        });
    });

    // datepicker
    $("input[type='date']").flatpickr({
        dateFormat: "d.m.Y"
    });
    $("input[type='datetime-local']").flatpickr({
        altInput: true,
        altFormat: "d.m.Y H:i",
        enableTime: true,
        time_24hr: true
    });

    // conditional collapsibles
    $(".conditional").conditionalCollapse();

    // image uploader
    $(".img-selector input[type=file]").imageUploader();
    $(".img-selector select").imagePreviewer();
    $(".img-selector select").trigger("change");

    // entity search
    $("#copy-entity-box .search-input").entitySearch(createFillFormAction());
    $(".search-entity-box").each((index, element) => {
        var search_id = $(element).attr("dat-id");
        var search_input = $(element).find(".search-input");
        search_input.entitySearch(createSetInputAction(search_id, search_input.attr("id")));
    });
});
