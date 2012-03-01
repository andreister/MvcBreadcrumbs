using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Linq;
using MvcBreadcrumbs.GeneratorExtensions;

namespace MvcBreadcrumbs
{
	public class Generator
	{
		private readonly Node _root;
		private static Generator _instance;
		private static readonly object _lock = new object();

		private Generator(Node root)
		{
			_root = root;
		}

		public static Generator GetInstance(Func<Node> getRoot)
		{
			if (_instance == null) {
				lock (_lock) {
					if (_instance == null) {
						var root = getRoot();
						_instance = new Generator(root);
					}
				}
			}
			return _instance;
		}

		public List<Breadcrumb> BuildBreadcrumbs(RequestContext context)
		{
			var result = new List<Breadcrumb>();
			var html = GetHtmlHelper(context);

			var breadcrumbs = BuildTree(context);
			for (int i = 0; i < breadcrumbs.Count; i++) {
				var data = breadcrumbs[i].Data;
				if (!data.IsVisible) {
					break;
				}
				var link = (i == 0) ? new MvcHtmlString(string.Format("<a href='/'>{0}</a>", data.Title)) : html.ActionLink(data.Title, data.Action, data.Controller, data.RouteValues, null);
				data.RouteValues.Clear();

				result.Add(new Breadcrumb {
					Name = data.Title, 
					Url = data.IsClickable ? link : null
				});
			}
			return result;
		}

		public List<Node> BuildTree(RequestContext context)
		{
			var result = new List<Node>();

			var searchData = new NodeData(context);
			var node = _root.FindNode(searchData);
			if (node != null) {
				result.Insert(node, context);

				node = node.Parent;
				while (node != null) {
					result.Insert(node, context);
					node = node.Parent;
				}
			}

			if (!result.Any()) {
				const string template = "Node was not found in the tree (area={0}, controller={1}, action={2})";
				var values = context.RouteData.Values;
				throw new MvcBreadcrumbsException(string.Format(template, values.GetValue("area"), values.GetValue("controller"), values.GetValue("action")));
			}

			return result;
		}

		private static HtmlHelper GetHtmlHelper(RequestContext context)
		{
			var controllerContext = new ControllerContext { RequestContext = context };
			return new HtmlHelper(new ViewContext(controllerContext, new WebFormView(controllerContext, "some\\path"), new ViewDataDictionary(), new TempDataDictionary(), new StringWriter()), new ViewPage());
		}
	}

	namespace GeneratorExtensions
	{
		internal static class RouteValueDictionaryExtensions
		{
			internal static string GetValue(this RouteValueDictionary values, string key)
			{
				if (values.ContainsKey(key)) {
					var result = values[key];
					return (result != null) ? result.ToString() : "<null>";
				}
				return "";
			}
		}

		internal static class ListExtensions
		{
			internal static void Insert(this List<Node> list, Node node, RequestContext context)
			{
				node.Resolve(context);
				list.Insert(0, node);
			}
		}
	}
}
