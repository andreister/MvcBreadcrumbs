using System.Web.Routing;
using MvcBreadcrumbs.Providers.AbstractTitleProviderExtensions;

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

		protected virtual long GetId(NodeData nodeData, RequestContext context)
		{
			string key = "id";
			var result = context.RouteData.Values.GetValue(key) ?? nodeData.RouteValues.GetValue(key);

			return result ?? 0;
		}

		protected virtual string DefaultTitle
		{
			get { return "..."; }
		}
	}

	namespace AbstractTitleProviderExtensions
	{
		internal static class RouteValueDictionaryExtensions
		{
			internal static long? GetValue(this RouteValueDictionary dictionary, string key)
			{
				long result;
				if (dictionary.ContainsKey(key) && long.TryParse(dictionary[key].ToString(), out result)) {
					return result;
				}

				return null;
			}
		}
	}
}
