/**
 * Returns the value for a given url query parameter
 * 
 * @param {string} name Name of the query parameter
 * @returns {string} Value of the query parameter or false if it doesnt exist
 */
$.prototype.getUrlParameter = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)')
        .exec(window.location.search);

    return (results !== null) ? results[1] || 0 : false;
};

/**
 * Performs a case insensitive comparison between two values parsed to strings
 * 
 * @param {any} string1 First value
 * @param {any} string2 Second value
 * @returns {bool} Two if both values are equal when parsed to string and compared case-insensitively
 */
$.prototype.stringCompareInsensitive = function (string1, string2) {
    if (string1 === string2)
        return true;

    return String(string1).toLowerCase() === String(string2).toLowerCase();
};

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
$.prototype.updateUrlParameter = function(param, paramVal, url = window.location.href) {
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
