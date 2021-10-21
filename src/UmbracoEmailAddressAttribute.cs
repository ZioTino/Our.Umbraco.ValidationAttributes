using Our.Umbraco.DataAnnotations.Helpers;
using Our.Umbraco.DataAnnotations.Interfaces;
using Our.Umbraco.DataAnnotations.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Our.Umbraco.DataAnnotations
{
    /// <summary>
    /// Specifies that a data field value must be a valid Email Address
    /// </summary>
    public sealed class UmbracoEmailAddressAttribute : RegularExpressionAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "EmailError";

        private static new string Pattern { get; set; } = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public UmbracoEmailAddressAttribute() : base(Pattern) {}

        public UmbracoEmailAddressAttribute(string dictionaryKey) : base(Pattern)
        {
            DictionaryKey = dictionaryKey;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = DataAnnotationsService.DictionaryValue(DictionaryKey);

            string customConfig = DataAnnotationsService.GetConfigKey(Constants.Configuration.EmailInputType);
            if (!string.IsNullOrEmpty(customConfig))
            {
                AttributeHelper.MergeAttribute(context.Attributes, "type", customConfig.ToLower(), replaceExisting: true);
            }
            else
            {
                AttributeHelper.MergeAttribute(context.Attributes, "type", "email", replaceExisting: true);
            }

            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-email", ErrorMessage);
        }
    }
}
