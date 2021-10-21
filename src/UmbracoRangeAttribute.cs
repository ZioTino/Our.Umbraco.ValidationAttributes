using Our.Umbraco.DataAnnotations.Helpers;
using Our.Umbraco.DataAnnotations.Interfaces;
using Our.Umbraco.DataAnnotations.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Our.Umbraco.DataAnnotations
{
    public sealed class UmbracoRangeAttribute : RangeAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "RangeError";

        public string WarningMessage { get; set; } = "RangeWarning";

        public UmbracoRangeAttribute(int minimum, int maximum) : base(minimum, maximum) {}

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = DataAnnotationsService.DictionaryValue(DictionaryKey);

            string customConfig = DataAnnotationsService.GetConfigKey(Constants.Configuration.RangeInputType);
            if (!string.IsNullOrEmpty(customConfig))
            {
                AttributeHelper.MergeAttribute(context.Attributes, "type", customConfig.ToLower(), replaceExisting: true);
            }
            else
            {
                AttributeHelper.MergeAttribute(context.Attributes, "type", "range", replaceExisting: true);
            }

            AttributeHelper.MergeAttribute(context.Attributes, "data-val-required", ErrorMessage);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-range", DataAnnotationsService.DictionaryValue(WarningMessage));
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-range-min", Minimum.ToString());
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-range-max", Maximum.ToString());
        }
    }
}
