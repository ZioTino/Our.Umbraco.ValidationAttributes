using System;
using Microsoft.Extensions.Configuration;
using Umbraco.Cms.Web.Common;

namespace Our.Umbraco.ValidationAttributes.Services
{
    public static class ValidationAttributesService
    {
        private static IUmbracoHelperAccessor _umbracoHelperAccessor;
        private static IConfiguration _configuration;
        private static UmbracoHelper _umbracoHelper;

        private static string _dictionaryFallbackFormat;

        internal static void Start(
            IUmbracoHelperAccessor umbracoHelperAccessor,
            IConfiguration configuration
            )
        {
            _umbracoHelperAccessor = umbracoHelperAccessor;
            _configuration = configuration;
        }

        internal static string GetConfigKey(string key)
        {
            string appSettingKey = $"Umbraco:{Constants.Configuration.Prefix}:";
            if (key != Constants.Configuration.EmailInputType && key != Constants.Configuration.RangeInputType)
                throw new FormatException($"The key {key} is not a valid configuration constant for Our.Umbraco.ValidationAttributes");

            if (key == Constants.Configuration.EmailInputType)
                appSettingKey += Constants.Configuration.EmailInputType;

            if (key == Constants.Configuration.RangeInputType)
                appSettingKey += Constants.Configuration.RangeInputType;

            return _configuration.GetValue<string>(appSettingKey) ?? string.Empty;
        }

        /// <summary>
        /// Fallback format value if the Dictionary key is not present. (Eg. [{0}])
        /// </summary>
        private static string GetDictionaryFallbackFormat()
        {
            if (_dictionaryFallbackFormat == null)
            {
                string appSettingKey = $"Umbraco:{Constants.Configuration.Prefix}:{Constants.Configuration.DictionaryKeyFallback}";
                _dictionaryFallbackFormat = _configuration.GetValue<string>(appSettingKey) ?? "[{0}]";

                if (!_dictionaryFallbackFormat.Contains("{0}"))
                    throw new FormatException($"'{_dictionaryFallbackFormat}' is not a valid format for '{appSettingKey}'. Format must contain '{{0}}'.");
            }
            return _dictionaryFallbackFormat;
        }
        
        /// <summary>
        /// Gets the value of the Dictionary item with the specified key.
        /// </summary>
        internal static string DictionaryValue(string dictionaryKey)
        {
            if (_umbracoHelperAccessor is null)
                throw new NullReferenceException("Our.Umbraco.ValidationAttributes: cannot access IUmbracoHelperAccessor. Please make sure everything is configured properly.");

            if (!_umbracoHelperAccessor.TryGetUmbracoHelper(out _umbracoHelper))
                throw new NullReferenceException("Our.Umbraco.ValidationAttributes: cannot access UmbracoHelper instance.");

            string key = _umbracoHelper.GetDictionaryValue(dictionaryKey);
            if (!string.IsNullOrEmpty(key))
                return key;

            return string.Format(GetDictionaryFallbackFormat(), dictionaryKey);
        }
    }
}