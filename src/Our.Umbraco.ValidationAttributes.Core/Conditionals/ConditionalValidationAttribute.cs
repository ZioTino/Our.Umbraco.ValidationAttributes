using Our.Umbraco.ValidationAttributes.Helpers;
using Our.Umbraco.ValidationAttributes.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Our.Umbraco.ValidationAttributes.Conditionals
{
    /// <summary>
    /// Abstract class to implement custom conditional validation, with support to Umbraco Dictionary items.
    /// The implementer must inherit from this class and implement it's own javascript unobtrusive validation.
    ///
    /// To pass extra validation parameters the implementer must override the method GetExtraValidationParameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class ConditionalValidationAttribute : ValidationAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        protected readonly ValidationAttribute InnerAttribute;
        public string DictionaryKey { get; set; }
        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }
        protected abstract string ValidationName { get; }

        protected virtual IDictionary<string, string> GetExtraValidationParameters()
        {
            return new Dictionary<string, string>();
        }

        protected ConditionalValidationAttribute(ValidationAttribute innerAttribute, string dependentProperty, object targetValue)
        {
            this.InnerAttribute = innerAttribute;
            this.DependentProperty = dependentProperty;
            this.TargetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(this.DependentProperty);
            if (field != null)
            {
                // get the value of the dependent property
                var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);

                // compare the value against the target value
                if ((dependentvalue == null && this.TargetValue == null) || (dependentvalue != null && dependentvalue.Equals(this.TargetValue)))
                {
                    // match => means we should try validating this field
                    if (!InnerAttribute.IsValid(value))
                    {
                        // validation failed - return an error
                        return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                    }
                }
            }
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            string depProp = BuildDependentPropertyId(context);
            string targetValue = (this.TargetValue ?? "").ToString();
            if (this.TargetValue.GetType() == typeof(bool))
                targetValue = targetValue.ToLower();

            AttributeHelper.MergeAttribute(context.Attributes, $"data-val-{ValidationName}-dependentproperty", depProp);
            AttributeHelper.MergeAttribute(context.Attributes, $"data-val-{ValidationName}-targetvalue", targetValue);
            foreach (var param in GetExtraValidationParameters())
            {
                AttributeHelper.MergeAttribute(context.Attributes, param.Key, param.Value, replaceExisting: true);
            }
        }

        private string BuildDependentPropertyId(ClientModelValidationContext context)
        {
            var viewContext = context.ActionContext as ViewContext;
            string depProp = TagBuilder.CreateSanitizedId(viewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(this.DependentProperty), "_");
            // This will have the name of the current field appended to the beginning, because the TemplateInfo's context has had this fieldname appended to it.
            var thisField = context.ModelMetadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
            {
                depProp = depProp.Substring(thisField.Length);
            }
            return depProp;
        }
    }
}
