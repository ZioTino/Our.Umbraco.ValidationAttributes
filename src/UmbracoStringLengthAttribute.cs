using Our.Umbraco.DataAnnotations.Helpers;
using Our.Umbraco.DataAnnotations.Interfaces;
using Our.Umbraco.DataAnnotations.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Our.Umbraco.DataAnnotations
{
    /// <summary>
    /// Specifies the minimum and maximum length of charactors that are allowed in a data field. 
    /// </summary>
    public sealed class UmbracoStringLengthAttribute : StringLengthAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "MinMaxLengthError";

        public UmbracoStringLengthAttribute(int maximumLength) : base(maximumLength) {}

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = DataAnnotationsService.DictionaryValue(DictionaryKey);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-length", ErrorMessage);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-length-max", MaximumLength.ToString());
        }
    }
}
