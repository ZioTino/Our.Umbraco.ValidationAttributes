using Our.Umbraco.ValidationAttributes.Interfaces;
using Our.Umbraco.ValidationAttributes.Services;
using System.ComponentModel;

namespace Our.Umbraco.ValidationAttributes
{
    public sealed class UmbracoDisplayNameAttribute : DisplayNameAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; }

        public UmbracoDisplayNameAttribute(string dictionaryKey) : base()
        {
            DictionaryKey = dictionaryKey;
        }

        public override string DisplayName => ValidationAttributesService.DictionaryValue(DictionaryKey);
    }
}
