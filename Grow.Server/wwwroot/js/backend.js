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
    $("input[type='date'],input[type='datetime-local']").on("change", (evnt) => {
        var fp = evnt.target._flatpickr;
        var newVal = $(evnt.target).val();
        if (fp && newVal)
            fp.setDate(newVal);
    });
    $("input[type='date']").flatpickr({
        dateFormat: "d.m.Y"
    });
    $("input[type='datetime-local']").flatpickr({
        altInput: true,
        altFormat: "d.m.Y H:i",
        enableTime: true,
        time_24hr: true
    });

    // Code to show selected file on custom file inputs
    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });

    // csv export buttons
    $(".export[data-target][data-name]").tableExport();

    // conditional collapsibles
    $(".conditional").conditionalCollapse();

    // image uploader
    $(".file-uploader").fileUploader();
    $(".file-uploader").imagePreviewer();

    // entity search
    $("#copy-entity-box .search-input").entitySearch(createFillFormAction());
    $(".search-entity-box").each((index, element) => {
        var search_id = $(element).attr("dat-id");
        var search_input = $(element).find(".search-input");
        search_input.entitySearch(createSetInputAction(search_id, search_input.attr("id")));
    });
});
