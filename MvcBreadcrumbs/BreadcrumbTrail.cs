using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MvcBreadcrumbs
{
	public class BreadcrumbTrail
	{
		private readonly Node _root;
		private static BreadcrumbTrail _instance;
		private static readonly object _lock = new object();

		public BreadcrumbTrail(Node root)
		{
			_root = root;
		}

		public static BreadcrumbTrail GetInstance(Func<Node> getRoot)
		{
			if (_instance == null) {
				lock (_lock) {
					if (_instance == null) {
						var root = getRoot();
						_instance = new BreadcrumbTrail(root);
					}
				}
			}
			return _instance;
		}

		public MvcHtmlString RenderHtml(RequestContext context, HtmlHelper html)
		{
			var result = new StringBuilder();

			var breadcrumbs = Build(context);

			result.Append("<div id='breadcrumbs'>");
			for (int i = 0; i < breadcrumbs.Count; i++) {
				var data = breadcrumbs[i].Data;

				if (i == breadcrumbs.Count - 1) {
					result.AppendFormat("<span class='selected'>{0}</span>", data.Title);
				}
				else if (i == 0) {
					result.AppendFormat("<a href='/'>{0}</a><span>&gt;</span>", data.Title);
				}
				else {
					var link = html.ActionLink(data.Title, data.Action, data.Controller, data.RouteValues, null).ToHtmlString();
					result.AppendFormat("{0}<span>&gt;</span>", link);
				}
				data.RouteValues.Clear();
			}
			result.Append("</div>");
			return new MvcHtmlString(result.ToString());
		}

		private List<Node> Build(RequestContext context)
		{
			var result = new List<Node>();

			var searchData = new NodeData(context);
			var node = _root.FindNode(searchData);
			if (node != null) {
				node.ResolveTitle(context);
				result.Insert(0, node);
				
				node = node.Parent;
				while (node != null) {
					node.ResolveTitle(context);
					result.Insert(0, node);
					node = node.Parent;
				}
			}
			return result;
		}
	}
}
