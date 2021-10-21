using Our.Umbraco.DataAnnotations.Helpers;
using Our.Umbraco.DataAnnotations.Interfaces;
using Our.Umbraco.DataAnnotations.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Our.Umbraco.DataAnnotations
{
    /// <summary>
    /// Specified that two properties data field value must match.
    /// </summary>
    public sealed class UmbracoCompareAttribute : CompareAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "EqualToError";

        public new string ErrorMessageString { get; set; }
        public new string OtherPropertyDisplayName { get; set; }

        public UmbracoCompareAttribute(string otherProperty) : base(otherProperty) {}

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessageString = DataAnnotationsService.DictionaryValue(DictionaryKey);

            if (context.ModelMetadata.ContainerType != null)
            {
                if (OtherPropertyDisplayName == null)
                {
                    var otherPropertyMetadata = context.MetadataProvider.GetMetadataForProperty(context.ModelMetadata.ContainerType, OtherProperty);

                    // TODO: Check if there's a better way to get the formatted display name to input into the 'data-val-equalto-other' attribute
                    // The original CompareAttribute sets the 'data-val-equalto-other' attribute with a prepending '*.', but I noticed that it works without it too
                    OtherPropertyDisplayName = $"*.{otherPropertyMetadata.GetDisplayName()}";
                }
            }
            
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-equalto", ErrorMessageString);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-equalto-other", OtherPropertyDisplayName);
        }
    }
}
