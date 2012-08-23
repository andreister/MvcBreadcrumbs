using System.Web.Routing;

namespace MvcBreadcrumbs.Providers
{
	public abstract class AbstractTitleProvider : ITitleProvider
	{
		public string GetTitle(Node node, RequestContext context)
		{
			var contextData = new NodeData(context);
			if (node.Data == contextData) {
				node.Data.CopyQueryString(context);
				var id = GetId(node.Data, context);
				if (id > 0) {
					return GetTitleInternal(node, id);
				}
			}
			return DefaultTitle;
		}

		protected abstract string GetTitleInternal(Node node, long id);

		protected virtual string IdKey
		{
			get { return "id"; }
		}

		private long GetId(NodeData nodeData, RequestContext context)
		{
			long result;
			if (context.RouteData.Values.ContainsKey(IdKey) && context.RouteData.Values[IdKey] != null && long.TryParse(context.RouteData.Values[IdKey].ToString(), out result)) {
				return result;
			}
			if (long.TryParse(context.HttpContext.Request.QueryString.Get(IdKey), out result)) {
				return result;
			}
			if (nodeData.RouteValues.ContainsKey(IdKey) && nodeData.RouteValues[IdKey] != null && long.TryParse(nodeData.RouteValues[IdKey].ToString(), out result)) {
				return result;
			}
			return 0;
		}

		protected virtual string DefaultTitle
		{
			get { return "..."; }
		}
	}
}
