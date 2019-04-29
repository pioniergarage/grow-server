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