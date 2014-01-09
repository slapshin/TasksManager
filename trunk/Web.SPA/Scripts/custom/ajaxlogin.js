$(function () {
    var getValidationSummaryErrors = function ($form) {
        return $form.find('.validation-summary-errors, .validation-summary-valid');
    };

    var displayErrors = function (form, errors) {
        var errorSummary = getValidationSummaryErrors(form)
            .removeClass('validation-summary-valid')
            .addClass('validation-summary-errors');

        var items = $.map(errors, function (error) {
            return '<li>' + error + '</li>';
        }).join('');

        errorSummary.find('ul').empty().append(items);
    };

    var formSubmitHandler = function (e) {
        var $form = $(this);

        // We check if jQuery.validator exists on the form
        if (!$form.valid || $form.valid()) {
            $.post('/Token',
                {
                    grant_type: "password",
                    username: $form.find('#username')[0].value,
                    password: $form.find('#password')[0].value
                })
                .done(function (data) {
                    if (data.userName && data.access_token) {
                        localStorage["accessToken"] = data.access_token;
                        window.location = location.href;                        
                    } else {
                        displayErrors($form, ["An unknown error occurred."]);                        
                    }                   
                    //json = json || {};

                    //// In case of success, we redirect to the provided URL or the same page.
                    //if (json.success) {
                    //    window.location = json.redirect || location.href;
                    //} else if (json.errors) {
                    //    displayErrors($form, json.errors);
                    //}
                })
                .error(function (data) {
                    displayErrors($form, [data.error_description]);
                });
        }

        // Prevent the normal behavior since we opened the dialog
        e.preventDefault();
    };

    $("#loginForm").submit(formSubmitHandler);
});