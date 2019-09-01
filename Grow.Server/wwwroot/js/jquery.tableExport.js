/* CSV Downloader based on: https://jsfiddle.net/gengns/j1jm2tjx/ */
$.fn.tableExport = function () {

    var download_csv = (csv, filename) => {
        var csvFile;
        var downloadLink;

        // CSV FILE
        csvFile = new Blob([csv], {
            type: "data:application/csv;charset=utf-8",
        });

        // Download link
        downloadLink = document.createElement("a");

        // File name
        downloadLink.download = filename;

        // We have to create a link to the file
        downloadLink.href = window.URL.createObjectURL(csvFile);

        // Make sure that the link is not displayed
        downloadLink.style.display = "none";

        // Add the link to your DOM
        document.body.appendChild(downloadLink);

        // Lanzamos
        downloadLink.click();
    };

    var export_table_to_csv = (table, filename) => {
        var csv = [];
        var rows = table.querySelectorAll("tr");

        for (var i = 0; i < rows.length; i++) {
            var row = [], cols = rows[i].querySelectorAll("td, th");

            for (var j = 0; j < cols.length; j++) {
                var cell = cols[j].cloneNode(true);
                var innerLink = cell.querySelector("a");
                if (innerLink !== null && innerLink.hasAttribute("title")) {
                    var newText = document.createElement("span");
                    newText.innerText = innerLink.getAttribute("title");
                    cell.replaceChild(newText, innerLink);
                }
                row.push(cell.innerText.trim());
            }

            var rowStr = row.join(",").trim();
            if (rowStr.charAt(0) === ',') rowStr = rowStr.substr(1);

            csv.push(rowStr);
        }

        var csvStr = "\uFEFF" + csv.join("\n");

        // Download CSV
        download_csv(csvStr, filename);
    };

    this.each((index, element) => {

        var tableId = $(element).attr("data-target");
        var tableName = $(element).attr("data-name");

        element.addEventListener("click", function () {
            var table = document.querySelector(tableId);

            var today = new Date();
            var dd = today.getDate();
            if (dd < 10) dd = '0' + dd;
            var mm = today.getMonth() + 1; //January is 0!
            if (mm < 10) mm = '0' + mm;
            var yy = today.getFullYear();
            export_table_to_csv(table, tableName + "-" + yy + mm + dd + ".csv");
        });
    });
};