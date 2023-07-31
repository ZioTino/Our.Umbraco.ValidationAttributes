using Our.Umbraco.ValidationAttributes.Helpers;
using Our.Umbraco.ValidationAttributes.Interfaces;
using Our.Umbraco.ValidationAttributes.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Our.Umbraco.ValidationAttributes
{
    /// <summary>
    /// Specifies that a data field value is required.
    /// </summary>
    public sealed class UmbracoRequiredAttribute : RequiredAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "RequiredError";

        public UmbracoRequiredAttribute() : base() {}

        public UmbracoRequiredAttribute(string dictionaryKey) : base()
        {
            DictionaryKey = dictionaryKey;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-required", ErrorMessage);
        }
    }
}
