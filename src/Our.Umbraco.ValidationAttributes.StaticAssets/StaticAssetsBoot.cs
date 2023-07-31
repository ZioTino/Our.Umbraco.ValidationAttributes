using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.ValidationAttributes.UI;

public class StaticAssetsBoot : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.AddValidationAttributesStaticAssets();
    }
}