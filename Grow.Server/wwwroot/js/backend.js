﻿// MAGIC NUMBERS / STRINGS
url_select_year            = "/Admin/Home/SetYear";
url_create_file_prefix     = "/Admin/Api/File?category=";
url_search_entities_prefix = "/Admin/Api/";
url_get_file_prefix        = "/Admin/Api/File/";
var debug;

// ADD GLOBAL EVENT HANDLERS
$(document).ready(function () {

    // rig up contest selector (in Admin navbar)
    var selector = $("#contest-selector");
    selector.change(function () {
        var newValue = selector.children("option:selected").val();
        $.post(url_select_year, { year: newValue }, function (data) {
            location.reload();
        });
    });

    // rig up pagination links
    $(".pagination").each((index, element) => {
        $(element).pagination({
            pages: $(element).attr('max'),
            currentPage: $(element).attr('current')
        });
    });

    // image uploader
    $(".img-selector input[type=file]").change(create_image_upload_function());
    $(".img-selector select").change(create_load_preview_function());
    $(".img-selector select").trigger("change");

    // entity search
    $("#search-box #search-input").keyup(create_search_entity_function());
});

// FUNCTION DEFINITIONS

/** 
 * Helper function to create links with a single query parameter change and the rest kept as-is.
 * 
 * Based on updateURLParameter function by Sujoy (http://stackoverflow.com/a/10997390/11236)
 * 
 * @param {string} param The query parameter name to be changed
 * @param {string} paramVal The new query parameter value
 * @param {string} url The url that should be changed (leave empty to use current url) 
 * @returns {string} The adapted url with a changed parameter
 */
function update_url_parameter(param, paramVal, url = window.location.href) {
    var newAdditionalUrl = "";
    var tempArray = url.split("?");
    var baseUrl = tempArray[0];
    var additionalUrl = tempArray[1];
    var temp = "";
    if (additionalUrl) {
        tempArray = additionalUrl.split("&");
        for (var i = 0; i < tempArray.length; i++) {
            if (tempArray[i].split('=')[0] !== String(param)) {
                newAdditionalUrl += temp + tempArray[i];
                temp = "&";
            }
        }
    }
    if (paramVal === 0 || paramVal) {
        var rows_txt = temp + "" + param + "=" + paramVal;
        return baseUrl + "?" + newAdditionalUrl + rows_txt;
    }
    if (newAdditionalUrl) {
        return baseUrl + "?" + newAdditionalUrl;
    }
    return baseUrl;
}

/**
 * Returns the value for a given url query parameter
 * 
 * @param {string} name Name of the query parameter
 * @returns {string} Value of the query parameter or false if it doesnt exist
 */
function get_url_parameter(name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)')
        .exec(window.location.search);

    return (results !== null) ? results[1] || 0 : false;
}

/**
 * Performs a case insensitive comparison between two values parsed to strings
 * 
 * @param {any} string1 First value
 * @param {any} string2 Second value
 * @returns {bool} Two if both values are equal when parsed to string and compared case-insensitively
 */
function string_compare_case_insensitive(string1, string2) {
    if (string1 === string2)
        return true;

    return String(string1).toLowerCase() === String(string2).toLowerCase();
}

/**
 * Creates a (key) event handler function for searching entities that can be assigned to a javascript event.
 * 
 * The event handler will take the value of the linked DOM object and send an AJAX request 
 * for entities of the given type (e.g. Event) that contain the search string in their name.
 * 
 * The class "loading" will be assigned to the linked DOM object while the ajax request runs.
 * 
 * The returned results are displayed in as <li> elements inside the given output DOM element.
 * 
 * When one of the <li> elements is clicked, the form on the same page is filled with data from the chosen element.
 *
 * The attached input element needs to contain the attributes "dat-type" and "dat-output".
 * 
 * @returns {Function} Handler function that fetches and displays entities
 */
function create_search_entity_function() {

    var search_timer;

    // Return the handler function
    return (event) => {
        var input_element = event.target;
        var type = $(input_element).attr("dat-type");
        var url = url_search_entities_prefix + type;
        var output_id = $(input_element).attr("dat-output");
        var output_element = $("#" + output_id);

        var searchString = $(input_element).val();
        var timestamp = $.now();

        // function handler when search result item is selected
        var on_element_chosen = (elem) => {
            $("input, select").each((index, input) => {
                // fill values on current form from selected entity
                var name = $(input).attr("name");
                if (!name) return;
                name = name.substring(0, 1).toLowerCase() + name.substring(1);
                // dont change "isActive"
                if (name === "isActive")
                    return;
                // text box, select
                if (elem[name])
                    $(input).val(elem[name]);
                // check box
                if ($(input).attr("type") === "checkbox")
                    $(input).prop("checked", elem[name]);
                // trigger change event
                $(input).trigger("change");
            });
        };

        // function to process search results
        var on_success = (data) => {
            $(input_element).removeClass("loading");

            // versioning of results to avoid older requests to overwrite newer ones
            var output_version = output_element.attr("output-version");
            if (output_version !== undefined && output_version > timestamp)
                return;

            // base setup for output element
            $(output_element).show();
            output_element.attr("output-version", timestamp);
            output_element.html("<li disabled>-- Select or search again --</li>");

            // add search results (as <li>)
            data.forEach(elem => {
                var text = elem.name;
                if (elem.contestId)
                    text += " (" + contest_years[elem.contestId] + ")";
                else
                    text += " (created at " + elem.createdAt.substring(0, 10) + ")";
                var li = document.createElement("li");
                li.innerHTML = text;
                li.addEventListener("click", () => on_element_chosen(elem));
                output_element.append(li);
            });
        };

        // error function
        var on_fail = () => {
            $(input_element).removeClass("loading");
            var text = "Error fetching entities via AJAX";
            console.log(text);
            output_element.append("<li>" + text + "</li>");
        };

        clearTimeout(search_timer);
        if (searchString === "") {
            output_element.hide();
            return;
        }

        // delayed ajax to execute search (to avoid constant updates while typing)
        search_timer = setTimeout(() => {
            $(input_element).addClass("loading");
            $.get(url, { search: searchString }, on_success)
                .fail(on_fail);
        }, 2000);
    };
}

/**
 * Creates a (change) event handler function for uploading files to the server.
 *
 * The event handler will take the value of the linked input[type=file] and send an AJAX request
 * to upload the selected type in a given category (e.g. TeamLogos).
 *
 * The class "loading" will be assigned to the output DOM object while the ajax request runs.
 *
 * The create file is (optionally) displayed as a <option> element inside a given output <select> element.
 * 
 * The attached input element needs to contain the attributes "dat-category" and "dat-output".
 * 
 * @returns {Function} Handler function that uploads file and adds result to given <select>
 */
function create_image_upload_function() {
    
    return (event) => {
        var input_element = event.target;
        var category = $(input_element).attr("dat-category");
        var output_element = $("#" + $(input_element).attr("dat-output"));
        
        var files = input_element.files;
        var formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            formData.append("fileData", files[i]);
        }

        // function to process created image
        var on_success = (data) => {
            output_element.removeClass("loading");
            
            if (output_element.length > 0) {
                var option;
                // write new image to select list
                for (var i = 0; i < data.length; i++) {
                    var file = data[i];
                    option = document.createElement("option");
                    option.innerText = file.name;
                    option.value = file.id;
                    output_element.append(option);
                }
                // select last uploaded image
                if (option)
                    option.selected = true;
            }
        };

        // function to alert in case of error
        var on_fail = (xhr, status, error) => {
            output_element.removeClass("loading");
            
            switch (xhr.status) {
                case 409:
                    alert("Error: A file with this name has already been uploaded.");
                    break;
                case 404:
                    alert("Misconfiguration: Trying to save file to a nonexistent folder.");
                    break;
                case 500:
                    alert("Error: No storage connection could be established.");
                    break;
            }

            var text = "Error fetching entities via AJAX";
            console.log(text);
        };

        // Make API call to create image
        output_element.addClass("loading");
        $.ajax({
            url: url_create_file_prefix + category,
            type: "POST",
            dataType: "json",
            data: formData,
            contentType: false,
            processData: false,
            success: on_success,
            error: on_fail
        });
    };
}

/**
 * Creates a (change) event handler function for displaying a preview of selected images.
 *
 * The event handler will take the id of the selected image <option> and request the file from the server to display it.
 *
 * @returns {Function} Handler function that fetches a selected file and previews it
 */
function create_load_preview_function() {

    return (event) => {
        // get selected element
        var select_element = $(event.target);
        var option_element = select_element.find(":selected");
        var preview_element = $("#" + select_element.attr("dat-preview"));
        var image_id = option_element.val();
        if (!image_id)
            return;

        // function to process received image
        var on_success = (data) => {
            var image_url = data.url;
            preview_element.attr("src", image_url);
            preview_element.css("display", "block");
        };

        // function to log error
        var on_fail = (xhr, status, error) => {
            var text = "Error fetching selected image via AJAX: " + xhr.status;
            console.log(text);
        };

        // selection cleared => remove img
        if (!image_id) {
            preview_element.css("display", "none");
            return;
        }

        // send request to fetch image
        $.getJSON(url_get_file_prefix + image_id)
            .done(on_success)
            .error(on_fail);
    };
}