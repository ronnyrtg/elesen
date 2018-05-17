function bindSortingArrow() {
    var spanSorting = '<span class="arrow-hack sort">&nbsp;&nbsp;&nbsp;</span>',
        spanAsc = '<span class="arrow-hack asc">&nbsp;&nbsp;&nbsp;</span>',
        spanDesc = '<span class="arrow-hack desc">&nbsp;&nbsp;&nbsp;</span>';
    $(".dataTable thead th").each(function (i, th) {
        $(th).find('.arrow-hack').remove();
        var html = $(th).html(),
            cls = $(th).attr('class');
        if (cls.indexOf('sorting_disabled') == -1) {
            switch (cls) {
                case 'sorting_asc':
                    $(th).html(html + spanAsc); break;
                case 'sorting_desc':
                    $(th).html(html + spanDesc); break;
                default:
                    $(th).html(html + spanSorting); break;
            }
        }
    });
}


// Toaster massage
function ShowMessageToastr(type, message, IsConfirmation, YesResponseMethod, NoResponseMethod) {
    // If message is not confirmation type
    if (IsConfirmation != true) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-top-center",
            "preventDuplicates": false,
            "showDuration": "300",
            "hideDuration": "100",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }

        if (type == null || type == 'undefined') {
            type = 'success';
        }

        if (message != null && message != 'undefined' || message != '') {
            if (type.toLowerCase() == 'danger') {
                toastr.error(message);
            }
            else if (type == 'success') {
                toastr.success(message);
            }
            else if (type == 'warning') {
                toastr.warning(message);
            }
            else {
                toastr.info(message);
            }

            $('#toast-container').addClass('nopacity');
        }
    } else {
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": true,
            "progressBar": false,
            "positionClass": "toast-top-center",
            "preventDuplicates": false,
            "showMethod": "fadeIn",
            "timeOut": "0",
            "extendedTimeOut": "0",
            "showEasing": "swing",
            "onclick": null,
            "tapToDismiss": false,
            "hideMethod": 'noop',
            allowHtml: true,
            onShown: function (toast) {
                $("#confirmationRevertYes").click(function () {
                    eval(YesResponseMethod);
                    $('.toast').remove();
                });

                $("#confirmationRevertNo").click(function () {
                    eval(NoResponseMethod);
                    $('.toast').remove();
                });
            }
        }

        if (type == null || type == 'undefined') {
            type = 'success';
        }
        if (message != null && message != 'undefined' || message != '') {
            if (type.toLowerCase() == 'danger') {
                toastr.error(message + "<br /><br /><button type='button' id='confirmationRevertYes' style='margin-left:30px;' class='btn btn-primary btn-md'>Yes</button><button type='button' id='confirmationRevertNo' style='padding-left:15px;margin-left:10px;' class='btn btn-default btn-md'>No</button>");
            }
            else if (type == 'success') {
                toastr.success(message + "<br /><br /><button type='button' id='confirmationRevertYes' style='margin-left:30px;' class='btn btn-primary btn-md'>Yes</button><button type='button' id='confirmationRevertNo' style='padding-left:15px;margin-left:10px;' class='btn btn-default btn-md'>No</button>");
            }
            else if (type == 'warning') {
                toastr.warning(message + "<br /><br /><button type='button' id='confirmationRevertYes' style='margin-left:30px;' class='btn btn-primary btn-md'>Yes</button><button type='button' id='confirmationRevertNo' style='padding-left:15px;margin-left:10px' class='btn btn-default btn-md'>No</button>");
            }
            else {
                toastr.info(message + "<br /><br /><button type='button' id='confirmationRevertYes' style='margin-left:30px;' class='btn btn-primary btn-md'>Yes</button><button type='button' id='confirmationRevertNo' style='padding-left:15px;margin-left:10px' class='btn btn-default btn-md'>No</button>");
            }

            $('#toast-container').addClass('nopacity');
        }
    }
}

// Password Validator

function checkStrength(password) {
    var strength = 0;

    if (password.length > 7) strength += 1
    if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1
    if (password.match(/([A-Z])/) && password.match(/([0-9])/)) strength += 1
    if (password.match(/([!,%,&,@@,#,$,^,*,?,_,~])/)) strength += 1
    if (password.match(/(.*[!,%,&,@@,#,$,^,*,?,_,~].*[!,",%,&,@@,#,$,^,*,?,_,~])/)) strength += 1

    return strength;
}


function checkforValidUploadedfiles(files, _validFileExtensions) {
    var blnValidP = false;
    if (files && files[0]) {
        var sFileName = files[0].name.toLowerCase();
        if (sFileName.length > 0) {
            for (var j = 0; j < _validFileExtensions.length; j++) {
                var sCurExtension = _validFileExtensions[j];
                if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                    blnValidP = true;
                    break;
                }
                else {
                    blnValidP = false;
                }
            }
        }
    }
    else {
        blnValidP = false;
    }
    return blnValidP;
}
