// Requires jquery, jquery-validation and jquery-validation-unobtrusive to work.

// Validator for UmbracoMustBeTrueAttribute
$.validator.unobtrusive.adapters.addBool("mustbetrue", "required");

// Validators for UmbracoMaxFileSizeAttribute
$.validator.addMethod("maxfilesize", function (value, element, param) {
    if (value === "") {
        return true;
    }
    var maxBytes = parseInt(param);
    if (element.files !== undefined && element.files[0] !== undefined && element.files[0].size !== undefined) {
        var filesize = parseInt(element.files[0].size);
        return filesize <= maxBytes;
    }
    return true;
});
$.validator.unobtrusive.adapters.add('maxfilesize', ['size'], function (options) {
    options.rules['maxfilesize'] = options.params.size;
    if (options.message) {
        options.messages['maxfilesize'] = options.message;
    }
});

// Validators for UmbracoIFormFileExtensionsAttribute
$.validator.addMethod("filetypes", function(value, element, param) {
    if (value === "") {
        return true;
    }
    var validFileTypes = [];
    if (param.indexOf(',') > -1) {
        validFileTypes = param.split(',');
    } else {
        validFileTypes = [param];
    }
    var currentFileType = value.split('.')[value.split('.').length - 1];
    for (var i = 0; i < validFileTypes.length; i++)
    {
        if (validFileTypes[i] === currentFileType)
        {
            return true;
        }
    }
    return false;
});
$.validator.unobtrusive.adapters.add('filetypes', ['types'], function(options) {
    options.rules['filetypes'] = options.params.types;
    if (options.message) {
        options.messages['filetypes'] = options.message;
    }
});