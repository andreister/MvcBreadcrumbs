using System.Web.Routing;

namespace MvcBreadcrumbs.Providers
{
	public interface ITitleProvider
	{
		string GetTitle(Node node, RequestContext context);
	}
}
