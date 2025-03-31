using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Our.Umbraco.ValidationAttributes.Interfaces;
using Our.Umbraco.ValidationAttributes.Services;

namespace Our.Umbraco.ValidationAttributes
{
    /// <summary>
    /// A RemoteAttributeBase for controllers which configures Unobtrusive validation to send an Ajax request to the web site. The invoked action should return JSON indicating whether the value is valid.
    /// Does no server-side validation of the final form submission.
    ///
    /// UmbracoRemoteAttribute: you can pass a dictionary key into the ErrorMessage parameter to specify a custom error message.
    /// NOTE: If you're using POST, remember to add AdditionalFields = "__RequestVerificationToken" to your property and [HttpPost] and [ValidateAntiForgeryToken] to your action method.
    /// </summary>
    public sealed class UmbracoRemoteAttribute : RemoteAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; }

        public UmbracoRemoteAttribute(string routeName) : base(routeName) { }
        public UmbracoRemoteAttribute(string action, string controller) : base(action, controller) { }
        public UmbracoRemoteAttribute(string action, string controller, string areaName) : base(action, controller, areaName) { }

        public override void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);
            base.AddValidation(context);
        }
    }
}