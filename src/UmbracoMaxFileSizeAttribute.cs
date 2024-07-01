using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Our.Umbraco.ValidationAttributes.Helpers;
using Our.Umbraco.ValidationAttributes.Interfaces;
using Our.Umbraco.ValidationAttributes.Services;

namespace Our.Umbraco.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class UmbracoMaxFileSizeAttribute : ValidationAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "MaxFileSizeError";

        public int MaxFileSize { get; set; }

        public UmbracoMaxFileSizeAttribute(int maxFileSize)
        {
            MaxFileSize = maxFileSize;
        }

        public UmbracoMaxFileSizeAttribute(int maxFileSize, string dictionaryKey)
        {
            DictionaryKey = dictionaryKey;
            MaxFileSize = maxFileSize;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);
            ErrorMessage = FormatErrorMessage(GetMaxFileSizeInMB());
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-maxfilesize", ErrorMessage);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-maxfilesize-size", MaxFileSize.ToString());
        }

        public override bool IsValid(object value)
        {
            IFormFile file = value as IFormFile;
            bool isValid = true;

            if (file != null)
            {
                isValid = file.Length <= MaxFileSize;
            }

            return isValid;
        }

        private string GetMaxFileSizeInMB()
        {
            return (MaxFileSize % 1048576M == 0) ? (MaxFileSize / 1048576).ToString() : Math.Round(MaxFileSize / 1048576M, 3).ToString();
        }
    }
}