# Our.Umbraco.DataAnotations
Contains model validation attributes to for your properties, by using umbraco dictionary as the resource for error messages.

This branch is exclusively for Umbraco 9.
This project is a port for Umbraco 9, taken from the [original Our.Umbraco.DataAnnotations](http://github.com/rasmuseeg/Our.Umbraco.DataAnnotations) by [rasmuseeg](https://github.com/rasmuseeg).  

[Looking for Umbraco 8?](https://github.com/rasmuseeg/Our.Umbraco.DataAnnotations/tree/dev-v8)  
[Looking for Umbraco 7?](https://github.com/rasmuseeg/Our.Umbraco.DataAnnotations/tree/dev-v7)

## Installation

NuGet:
```
PM > Install-Package Our.Umbraco.DataAnnotations
```

Build the project and start website.

## Client Validation
Include the following scripts in your layout.cshtml file, or in your master page:

```
<body>
    @RenderBody()

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js" referrerpolicy="no-referrer"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.3/jquery.validate.min.js" integrity="sha512-37T7leoNS06R80c8Ulq7cdCDU5MNQBwlYoy1TX/WUsLFC2eYNqtKlV0QjH7r8JpG/S0GUMZwebnVFLPd6SU5yg==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.12/jquery.validate.unobtrusive.min.js" referrerpolicy="no-referrer"></script>

    <script src="~\App_Plugins\Our.Umbraco.DataAnnotations\Scripts\jquery.validation.custom.js">
</body>
```

The above is just a sample, you may use any method you like to include the scripts.  
**NOTE: *jquery.validation.custom.js* is required to ensure that the UmbracoMustBeTrue attribute is working.**  
**As an alternative you can include yourself its content with any method you like.**  

The end result for a page with validation could look like:
```cshtml
@model LoginModel
@using MyWebsite.Web.Models;
@using MyWebsite.Web.Controllers;
@using (Html.BeginUmbracoForm<MemberController>("HandleLogin", null, new { @role="form", @class="" }, FormMethod.Post))
{
    @Html.ValidationSummary("loginModel", true)

    <div class="form-group">
        @Html.LabelFor(m=> m.Username, new { @class="control-label" })
        @Html.TextBoxFor(m => m.Username, new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.Username)
    </div>

    <div class="form-group">
        @Html.LabelFor(m=> m.Password, new { @class="control-label" })
        @Html.PasswordFor(m => m.Password, new {
            @class = "form-control form-control-appended",
            @placeholder = Umbraco.GetDictionaryValue("EnterYourPassword", "Enter your password")
        })
        @Html.ValidationMessageFor(m => m.Password)
    </div>

    @Html.HiddenFor(m=> m.RedirectUrl)

    <button type="submit" role="button">@Umbraco.GetDictionaryValue("SignIn", "Sign in")</button>
}
```

### 

## Attributes
Decorate your properties with the following attributes

 * UmbracoCompare
 * UmbracoDisplayName
 * UmbracoEmailAddress
 * UmbracoMaxLength
 * UmbracoMinLength
 * UmbracoMustBeTrue
 * UmbracoRange
 * UmbracoRegularExpression
 * UmbracoRemote
 * UmbracoRequired
 * UmbracoStringLength

**How to use:**
```C#
[UmbracoRequired]
public string MyProperty { get; set; } 
```

### UmbracoCompareAttribute

| Umbraco Dictionary Key | Default |
| -- | -- |
| `EqualToError` | Must be created by your self. |


Example:
```C#
[UmbracoDisplayName(nameof(Password))]
[DataType(DataType.Password)]
public string Password { get; set; }

[UmbracoDisplayName(nameof(ConfirmPassword))]
[UmbracoRequired]
[UmbracoCompare(nameof(Password))]
[DataType(DataType.Password)]
public string ConfirmPassword { get; set; }
```

### UmbracoDisplayName

| Key | Default |
| -- | -- |
| Provied key | Must be created by your self. |

Example:
```C#
[UmbracoDisplayName(nameof(Username))]
[UmbracoRequired]
public string Username { get; set; }
```

### UmbracoEmailAddress

| Key | Default |
| -- | -- |
| EmailError | Must be created by your self. |

Example:
```C#
[UmbracoEmailAddress]
public string Email { get; set; }
```

### UmbracoMinLength

| Key | Default |
| -- | -- |
| MinLengthError | Must be created by your self. |

Example:
```C#
[UmbracoMinLength(20)]
public string MyProperty { get; set; }
```

### UmbracoMaxLength

| Key | Default |
| -- | -- |
| MaxLengthError | Must be created by your self. |

Example:
```C#
[UmbracoMaxLength(120)]
public string MyProperty { get; set; }
```

### UmbracoStringLength

| Key | Default
| -- | -- |
| MinMaxLengthError | Must be created by your self. |

Examples:
```C#
[UmbracoStringLength(120)]
public string Message { get; set; }

[UmbracoStringLength(120, MinimumLength = 30)]
public string Message { get; set; }
```

### UmbracoMustBeTrue
| Key | Default |
| -- | -- |
| MustBeTrueError | Must be created by your self. |

Example:
```C#
[UmbracoMustBeTrue]
public bool Consent { get; set; }
```

### UmbracoRegularExpression

There are no default keys for this attribute, since each regex validation is unique.

Example:
```C#
[UmbracoRegularExpression("^([0-9]{4})$", DictionaryKey = "MyCustomKey")]
public string Password { get; set; }
```

### UmbracoRequired

Example:
```C#
[UmbracoRequired]
public string MyProperty { get; set; }
```

## Custom dictionary keys
Each Attribute, has a public property `DictionaryKey` which can be set like this:
```C#
[UmbracoRequired(DictionaryKey = "MyCustomKey")]
public string MyProperty { get; set; }
```

Not setting a custom key, will fallback to the default dictionary key.  
**You have to create Dictionary Keys manually, taking example from this documentation.**