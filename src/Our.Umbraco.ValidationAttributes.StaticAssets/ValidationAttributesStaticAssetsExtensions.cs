using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.ValidationAttributes.UI;

public static class ValidationAttributesStaticAssetsExtensions
{
    public static IUmbracoBuilder AddValidationAttributesStaticAssets(this IUmbracoBuilder builder)
    {
        // don't add if the filter is already there .
        if (builder.ManifestFilters().Has<ValidationAttributesAssetManifestFilter>())
            return builder;

        // add the package manifest programatically. 
        builder.ManifestFilters().Append<ValidationAttributesAssetManifestFilter>();

        return builder;
    }
}