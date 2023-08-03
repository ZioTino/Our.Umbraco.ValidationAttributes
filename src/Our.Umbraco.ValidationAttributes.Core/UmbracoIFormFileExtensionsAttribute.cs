using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using J2N.Collections.Generic;
using Lucene.Net.Analysis.Hunspell;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Our.Umbraco.ValidationAttributes.Helpers;
using Our.Umbraco.ValidationAttributes.Interfaces;
using Our.Umbraco.ValidationAttributes.Services;

namespace Our.Umbraco.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class UmbracoIFormFileExtensionsAttribute : ValidationAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey {get; set;} = "FormFileExtensionsError";

        public string[] ValidFileTypes { get; set; }

        public UmbracoIFormFileExtensionsAttribute(string fileTypes)
        {
            ValidFileTypes = ParseFileTypes(fileTypes);
        }

        public UmbracoIFormFileExtensionsAttribute(string fileTypes, string dictionaryKey)
        {
            DictionaryKey = dictionaryKey;
            ValidFileTypes = ParseFileTypes(fileTypes);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);
            ErrorMessage = FormatErrorMessage(string.Join(", ", ValidFileTypes));
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-filetypes", ErrorMessage);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-filetypes-types", string.Join(',', ValidFileTypes));

            // input type="file" accept attribute 
            System.Collections.Generic.List<string> validExtensions = new System.Collections.Generic.List<string>();
            foreach (string type in ValidFileTypes)
            {
                validExtensions.Add($".{type}");
            }
            AttributeHelper.MergeAttribute(context.Attributes, "accept", string.Join(',', validExtensions));
        }

        public override bool IsValid(object value)
        {
            IFormFile file = value as IFormFile;
            bool isValid = true;
            
            if (file != null)
            {
                isValid = ValidFileTypes.Any(x => file.FileName.EndsWith(x));
            }

            return isValid;
        }

        private string[] ParseFileTypes(string fileTypes)
        {
            return fileTypes.ToLower().Split(',');
        }
    }
}