using System.Web.Routing;

namespace MvcBreadcrumbs.Providers
{
	public interface IClickabilityProvider
	{
		bool IsClickable(Node node, RequestContext context);
	}
}
