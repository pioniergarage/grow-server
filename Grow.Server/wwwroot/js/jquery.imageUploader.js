/* MAGIC NUMBERS */
let file_api_url = "/Api/File";

/**
 * Execute on an input[type=file] to add AJAX functionality to upload files to the server.
 *
 * After a triggered change event it will take the the linked file and send an AJAX request
 * to upload the selected file in a given category (e.g. TeamLogos).
 *
 * The class "loading" will be assigned to the output DOM object while the ajax request runs.
 *
 * The created file is (optionally) displayed as a <option> element inside a given output <select> element.
 * 
 * Data attributes:
 * dat-category - Category name in which to upload the file (required)
 * dat-output - DOM Id of the output element to add the uploaded file to (optional)
 * 
 * @returns {IQuery<HTMLElement>} Input selector
 */
$.fn.imageUploader = function () {

    this.on("change", (event) => {
        var input_element = event.target;
        var category = $(input_element).attr("dat-category");
        var output_element = $("#" + $(input_element).attr("dat-output"));

        if (!category)
            return;

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
                output_element.trigger("change");
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
            url: file_api_url + "?category=" + category,
            type: "POST",
            dataType: "json",
            data: formData,
            contentType: false,
            processData: false,
            success: on_success,
            error: on_fail
        });
    });
    return this;
};

/**
 * Execute on a <select> to add AJAX functionality to request a selected file and preview it.
 *
 * After a triggered change event the event handler will take the value of the selected image <option>, interpret it as an entity id 
 * and request the corresponding file from the server to display it.
 *
 * Data attributes:
 * dat-preview - DOM Id of the output element in which to display the image
 * 
 * @returns {IQuery<HTMLElement>} Input selector
 */
$.fn.imagePreviewer = function() {

    this.on("change", (event) => {
        // get selected image
        var input_element = $(event.target);
        var preview_element = $("#" + input_element.attr("dat-preview"));
        var image_id = $(input_element).val();
        if (!image_id || !preview_element)
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
        $.getJSON(file_api_url + "/" + image_id)
            .done(on_success)
            .error(on_fail);
    });
    return this;
}