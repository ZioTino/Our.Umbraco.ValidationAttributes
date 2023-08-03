using System.Diagnostics;
using Umbraco.Cms.Core.Manifest;

namespace Our.Umbraco.ValidationAttributes.UI;

internal class ValidationAttributesAssetManifestFilter : IManifestFilter
{
    public void Filter(List<PackageManifest> manifests)
    {
        var assembly = typeof(ValidationAttributesAssetManifestFilter).Assembly;
        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        string version = fileVersionInfo.ProductVersion;

        manifests.Add(new PackageManifest
        {
            PackageName = "OurUmbracoValidationAttributes",

            BundleOptions = BundleOptions.None,
            Scripts = new string[]
            {
                $"App_Plugins/Our.Umbraco.ValidationAttributes/scripts/jquery.validation.custom.js"
            },
            Stylesheets = new string[]
            {
            }
        });
    }
}