using System.Web.Routing;

namespace MvcBreadcrumbs.Providers
{
	public interface IVisibilityProvider
	{
		bool IsVisible(Node node, RequestContext context);
	}
}
