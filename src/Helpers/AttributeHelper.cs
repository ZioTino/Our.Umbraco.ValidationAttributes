using System.Collections.Generic;

namespace Our.Umbraco.ValidationAttributes.Helpers
{
    public static class AttributeHelper
    {
        public static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value, bool replaceExisting = false)
        {
            if (attributes.ContainsKey(key))
            {
                if (replaceExisting)
                {
                    attributes[key] = value;
                    return true;
                }
                else return false;
            }

            attributes.Add(key, value);
            return true;
        }
    }
}