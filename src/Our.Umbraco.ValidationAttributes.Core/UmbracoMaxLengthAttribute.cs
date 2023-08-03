using Our.Umbraco.ValidationAttributes.Helpers;
using Our.Umbraco.ValidationAttributes.Interfaces;
using Our.Umbraco.ValidationAttributes.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Our.Umbraco.ValidationAttributes
{
    /// <summary>
    /// Specifies the maximum length of array or string data allowed in a property.
    /// </summary>
    public sealed class UmbracoMaxLengthAttribute : MaxLengthAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "MaxLengthError";

        public UmbracoMaxLengthAttribute(int length) : base(length) {}

        public UmbracoMaxLengthAttribute(int length, string dictionaryKey) : base(length)
        {
            DictionaryKey = dictionaryKey;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-maxlength", ErrorMessage);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-maxlength-max", Length.ToString());
        }
    }
}
