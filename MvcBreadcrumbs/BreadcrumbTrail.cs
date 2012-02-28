using System;
using System.Collections.Generic;
using System.IO;
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

		private BreadcrumbTrail(Node root)
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

		public List<Breadcrumb> Build(RequestContext context)
		{
			var result = new List<Breadcrumb>();

			var controllerContext = new ControllerContext {RequestContext = context};
			var html = new HtmlHelper(new ViewContext(controllerContext, new WebFormView(controllerContext, "some\\path"), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()), new ViewPage());

			var breadcrumbs = BuildInternal(context);

			for (int i = 0; i < breadcrumbs.Count; i++) {
				var data = breadcrumbs[i].Data;
				var link = (i == 0) ? new MvcHtmlString(string.Format("<a href='/'>{0}</a>", data.Title)) : html.ActionLink(data.Title, data.Action, data.Controller, data.RouteValues, null);
				data.RouteValues.Clear();

				result.Add(new Breadcrumb { Name = data.Title, Url = link });
			}
			return result;
		}

		private List<Node> BuildInternal(RequestContext context)
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
