using System.Web.Routing;

namespace MvcBreadcrumbs.Providers
{
	public abstract class AbstractTitleProvider : ITitleProvider
	{
		public string GetTitle(Node node, RequestContext context)
		{
			var contextData = new NodeData(context);
			if (node.Data == contextData) {
				var id = GetId(node.Data, context);
				if (id > 0) {
					return GetTitleInternal(node, id);
				}
			}
			return DefaultTitle;
		}

		protected abstract string GetTitleInternal(Node node, long id);

		protected long GetId(NodeData data, RequestContext context)
		{
			var contextValues = context.RouteData.Values;
			var nodeValues = data.RouteValues;
			long result;
			if (contextValues.ContainsKey("id") && long.TryParse(contextValues["id"].ToString(), out result)) {
				return result;
			}
			if (nodeValues.ContainsKey("id") && long.TryParse(nodeValues["id"].ToString(), out result)) {
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
