$(document).ready(function () {

    // rig up contest selector
    var selector = $("#contest-selector");
    selector.change(function () {
        var newValue = selector.children("option:selected").val();
        $.post("/Admin/Home/SetYear", { year: newValue }, function (data) {
            location.reload();
        });
    });
});

/**
 * Creates an event handler function for searching entities that can be assigned to a javascript event.
 * 
 * The event handler will take the value of the linked DOM object and send an AJAX request 
 * for entities of the given type (e.g. Event) that contain the search string in their name.
 * 
 * The class "loading" will be assigned to the linked DOM object while the ajax request runs.
 * 
 * The returned results are displayed in as <li> elements inside the given output DOM element.
 * 
 * When one of the <li> elements is clicked, the form on the same page is filled with data from the chosen element.
 */
var debug;
function create_search_entity_function(type, output_id) {

    var search_timer;
    var url = "/Admin/Api/" + type;
    var output_element = $("#" + output_id);
    
    return (event) => {
        var input_element = event.target;
        var searchString = $(input_element).val();
        var timestamp = $.now();

        var on_element_chosen = (elem) => {
            $("input, select").each((index, input) => {
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
            });
        };

        var on_success = (data) => {

            debug = data;
            $(input_element).removeClass("loading");

            var output_version = output_element.attr("output-version");
            if (output_version !== undefined && output_version > timestamp)
                return;

            $(output_element).show();
            output_element.attr("output-version", timestamp);
            output_element.html("<li disabled>-- Select or search again --</li>");

            data.forEach(elem => {
                var text = elem.name;
                text += " (created at " + elem.createdAt.substring(0, 10) + ")";
                var li = document.createElement("li");
                li.innerHTML = text;
                li.addEventListener("click", () => on_element_chosen(elem));
                output_element.append(li);
            });
        };

        var on_fail = () => {
            $(input_element).removeClass("loading");
            var text = "Error fetching entities via AJAX";
            console.log(text);
            output_element.append("<li>" + text + "</li>");
        };

        clearTimeout(search_timer);
        search_timer = setTimeout(() => {
            $(input_element).addClass("loading");
            $.get(url, { search: searchString }, on_success)
                .fail(on_fail);
        }, 2000);
    };
}