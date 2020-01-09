/* MAGIC NUMBERS */
let file_api_url = "/Api/File";

/**
 * Execute on an input[type=file] to add AJAX functionality to upload files to the server.
 *
 * After a triggered change event it will take the the linked file and send an AJAX request
 * to upload the selected file in a given category (e.g. TeamLogos).
 *
 * A linked search bar allows to search for preexisting files by name.
 * 
 * Data attributes:
 * dat-category - Category name in which to upload the file (required)
 * dat-output - DOM Id of the output element to update with the uploaded file id (optional)
 * 
 * @returns {IQuery<HTMLElement>} Input selector
 */
$.fn.fileUploader = function () {
    
    this.each((index, element) => {

        // handler to upload images
        $(element).find("[id^=file-upload-]").on("change", (event) => {
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
                $(element).removeClass("loading");

                if (output_element) {
                    output_element.val(data[0].id);
                    output_element.trigger("change");
                }
            };

            // function to alert in case of error
            var on_fail = (xhr, status, error) => {
                $(element).removeClass("loading");

                switch (xhr.status) {
                    case 409:
                        alert("Error: A file with this name has already been uploaded.");
                        break;
                    case 404:
                        alert("Misconfiguration: Trying to save file in a nonexistent category.");
                        break;
                    case 500:
                        alert("Error: No storage connection could be established.");
                        break;
                }

                var text = "Error fetching entities via AJAX";
                console.log(text);
            };

            // Make API call to create image
            $(element).addClass("loading");
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

        // handler to search for preexisting files
        $(element).find(".search-input").entitySearch(file => {
            $(element).find(".file-id-input").val(file.id);
            $(element).find(".file-name-input").val(file.name);
            $(element).find(".file-id-input").trigger("change");
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
 * @returns {IQuery<HTMLElement>} Input selector
 */
$.fn.imagePreviewer = function() {

    this.each((index, element) =>
        $(element).find(".file-id-input").on("change", (event) => {
            // get selected image
            var input_element = $(event.target);
            var preview_element = $(element).find("[id^=file-preview-]");
            var name_element = $(element).find(".file-name-input");
            var image_id = $(input_element).val();
            if (!image_id || !preview_element)
                return;

            // function to process received image
            var on_success = (data) => {
                var image_url = data.url;
                var isImage = false;
                ["gif", "png", "jpg", "jpeg", "svg"].forEach((val) => {
                    if (image_url.endsWith(val))
                        isImage = true;
                });

                if (isImage) {
                    preview_element.attr("src", image_url);
                    preview_element.css("display", "block");
                }

                // check that name has changed too
                if (name_element.val() !== data.name) {
                    name_element.val(data.name);
                }
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
        })
    );

    this.find(".file-id-input").trigger("change");

    return this;
}