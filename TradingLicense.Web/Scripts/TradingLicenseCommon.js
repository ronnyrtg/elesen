var messsageIntervalId;
var divMessage = "divMessage";
var divErrorMessage = "divErrorMessage";

(function (window, document, $) {
    'use strict';
    var $html = $('html');
    var $body = $('body');

    $(document).ready(function () {

    });

})(window, document, jQuery);

function clearMessage() {
    $('#' + divMessage).html("");
}

function clearMessageFadeOut() {
    $('#' + divMessage).fadeOut();
}

function clearErrorMessage() {
    $('#' + divErrorMessage).html("");
}

function isNumberOnly(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function getQueryStringByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function messageSetup(mess) {
    if (messsageIntervalId) {
        clearTimeout(messsageIntervalId);
        messsageIntervalId = null;
    }

    $('#' + divMessage).fadeIn();
    $('#' + divMessage).html($.parseHTML(unescape(mess)));

    messsageIntervalId = setTimeout(clearMessageFadeOut, 10000);

    $('#' + divMessage).hover(function () {
        clearTimeout(messsageIntervalId);
    }, function () {
        messsageIntervalId = setTimeout(clearMessageFadeOut, 10000);
    });
}

function messageSetup(mess) {
    if (messsageIntervalId) {
        clearTimeout(messsageIntervalId);
        messsageIntervalId = null;
    }

    $('#' + divMessage).fadeIn();
    $('#' + divMessage).html($.parseHTML(unescape(mess)));

    messsageIntervalId = setTimeout(clearMessageFadeOut, 10000);

    $('#' + divMessage).hover(function () {
        clearTimeout(messsageIntervalId);
    }, function () {
        messsageIntervalId = setTimeout(clearMessageFadeOut, 10000);
    });
}

function errorMessage(message, divId) {
    if (divId == null || divId == undefined || divId == 'undefined' || divId == 'null' || divId == '') {
        divId = "divMessage";
    }
    divMessage = divId;
    $('#' + divMessage).removeAttr('style');
    var mess = "<div class='alert alert-danger'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>" + message + "</div>";
    messageSetup(mess);
}

function clearMessageFadeOut() {
    $('#' + divMessage).fadeOut();
}

function successMessage(message, divId) {
    if (divId == null || divId == undefined || divId == 'undefined' || divId == 'null' || divId == '') {
        divId = "divMessage";
    }
    divMessage = divId;
    $('#' + divMessage).removeAttr('style');
    var mess = "<div class='alert alert-success'><a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>" + message + "</div>";
    messageSetup(mess);
}




function renderDate(d) {
    if (d != null) {
        return moment(d).format('DD-MMM-YYYY');
    }
    return "";
}

function renderDateTime(d) {
    if (d != null) {
        return moment(d).format("DD-MMM-YYYY HH:mm:ss");
    }
    return "";
}
