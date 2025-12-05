using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Our.Umbraco.ValidationAttributes.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Web.Common;

namespace Our.Umbraco.ValidationAttributes.Components
{
    public class ValidationAttributesComposer : ComponentComposer<ValidationAttributesComponent> { }

    public class ValidationAttributesComponent : IAsyncComponent
    {
        public IUmbracoHelperAccessor _umbracoHelperAccessor;
        public IConfiguration _configuration;

        public ValidationAttributesComponent(
            IUmbracoHelperAccessor umbracoHelperAccessor,
            IConfiguration configuration
        )
        {
            _umbracoHelperAccessor = umbracoHelperAccessor;
            _configuration = configuration;
        }

        public Task InitializeAsync(bool isRestarting, CancellationToken cancellationToken)
        {
            ValidationAttributesService.Start(_umbracoHelperAccessor, _configuration);
            return Task.CompletedTask;
        }

        public Task TerminateAsync(bool isRestarting, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
