$.fn.conditionalCollapse = function () {
    this.each((index, element) => {

        // get data properties
        var propertyName = $(element).attr("data-if");
        var dataEquals = $(element).attr("data-eq");
        var dataNotEquals = $(element).attr("data-neq");

        // helper variables
        var comparisonShouldBe = dataEquals !== undefined
            ? true
            : false;
        var comparisonValue = dataEquals !== undefined
            ? dataEquals
            : dataNotEquals;
        var deciderElement = $("[name='" + propertyName + "']");

        // input validity check
        if (comparisonValue === undefined || !deciderElement)
            return;

        // add conditional handler to collapse/extend
        deciderElement.on("change", event => {
            var inputValue = $(event.target).is(":checkbox")
                ? ($(event.target).is(":checked") ? "true" : "false")
                : $(event.target).val();

            if ((inputValue === comparisonValue) === comparisonShouldBe) {
                $(element).collapse('show');
            } else {
                $(element).collapse('hide');
            }
        });

        // start out collapsed?
        var inputValue = $(element).is(":checkbox")
            ? ($(element).is(":checked") ? "true" : "false")
            : $(element).val();
        if ((inputValue === comparisonValue) !== comparisonShouldBe) {
            $(element).addClass("collapse show");
        }
    });
};