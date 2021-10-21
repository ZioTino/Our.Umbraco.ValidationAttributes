using Microsoft.Extensions.Configuration;
using Our.Umbraco.DataAnnotations.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Dictionary;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;

namespace Our.Umbraco.DataAnnotations.Components
{
    public class DataAnnotationsComposer : ComponentComposer<DataAnnotationsComponent> { }
    public class DataAnnotationsComponent : IComponent
    {
        public IUmbracoHelperAccessor _umbracoHelperAccessor;
        public IConfiguration _configuration;
        public DataAnnotationsComponent(
            IUmbracoHelperAccessor umbracoHelperAccessor,
            IConfiguration configuration
            )
        {
            _umbracoHelperAccessor = umbracoHelperAccessor;
            _configuration = configuration;
        }

        public void Initialize() => DataAnnotationsService.Start(_umbracoHelperAccessor, _configuration);
        public void Terminate() {}
    }
}