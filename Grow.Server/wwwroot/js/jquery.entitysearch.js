// MAGIC NUMBERS
url_search_entities_prefix = "/Api/";

/**
 * Execute on a input[type=text] to add AJAX functionality to search for entities on the server.
 * 
 * The event handler will take the value of the linked DOM object and send an AJAX request 
 * for entities of the given type (e.g. Event) that contain the search string in their name.
 * 
 * The class "loading" will be assigned to the linked DOM object while the ajax request runs.
 * 
 * The returned results are displayed in as <li> elements inside the given output DOM element.
 * 
 * When one of the <li> elements is clicked, the given function onElementChosen(element) is executed.
 *
 * Data attributes:
 * dat-type - Entity type for which to search for (required)
 * dat-output - DOM Id of the output element to add the found entities to (optional)
 * dat-filter-name - Add additional parameter to the search REST call (optional)
 * dat-filter-value - Add additional parameter to the search REST call (optional)
 *
 * @param {Function} onElementChosen The Function that will be executed when an element is selected
 * 
 * @returns {IQuery<HTMLElement>} Input selector
 */
$.fn.entitySearch = function (onElementChosen) {

    var search_timer;

    if (!onElementChosen)
        onElementChosen = (element) => { };
    
    this.on("keyup", (event) => {
        var input_element = event.target;
        var type = $(input_element).attr("dat-type");
        var filterName = $(input_element).attr("dat-filter-name");
        var filterValue = $(input_element).attr("dat-filter-value");
        var url = url_search_entities_prefix + type;
        var output_id = $(input_element).attr("dat-output");
        var output_element = $("#" + output_id);

        var searchString = $(input_element).val();
        var timestamp = $.now();
        
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
            var counter = 0;
            for (var i = 0; i < data.length && i < 6; i++) {
                let elem = data[i];
                let text = elem.name;
                if (elem.contestId)
                    text += " (" + contest_years[elem.contestId] + ")";
                else if (elem.createdAt)
                    text += " (created at " + elem.createdAt.substring(0, 10) + ")";
                let li = document.createElement("li");
                li.innerHTML = text;
                li.addEventListener("click", () => {
                    let clicked_elem = elem;
                    $(output_element).hide();
                    onElementChosen(clicked_elem);
                });
                output_element.append(li);
            }
            if (data.length > 6)
                output_element.append("<li disabled>...</li>");
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

            var payload = { search: searchString };
            if (filterName !== undefined && filterValue !== undefined)
                payload[filterName] = filterValue;

            $.get(url, payload, on_success)
                .fail(on_fail);
        }, 2000);
    });
    return this;
};

function createFillFormAction() {
    return (elem) => {
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
}

function createSetInputAction(inputForId, inputForName) {
    return (element) => {
        $("#" + inputForId).val(element.id);
        if (inputForName)
            $("#" + inputForName).val(element.name);
    };
}
