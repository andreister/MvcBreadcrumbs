using System;
using System.Linq.Expressions;
using MvcBreadcrumbs.Providers;

namespace MvcBreadcrumbs
{
	public class MvcNode<TController> : Node
	{
		public MvcNode(string title, Expression<Action<TController>> action, params Node[] children)
			: base(children)
		{
			var controllerName = GetControllerName();
			var actionName = GetActionName(action);
			var area = GetAreaName();

			Data = new NodeData(title) {
				Area = area, 
				Controller = controllerName, 
				Action = actionName
			};
		}

		public MvcNode(ITitleProvider titleProvider, Expression<Action<TController>> action, params Node[] children)
			: this((string)null, action, children)
		{
			TitleProvider = titleProvider;
		}

		internal override Node FindNode(NodeData searchData)
		{
			if (Data == searchData) 
				return this;

			foreach (var child in Children) {
				var node = child.FindNode(searchData);
				if (node != null) {
					return node;
				}
			}

			return null;
		}

		private static string GetControllerName()
		{
			return typeof(TController).Name.Substring(0, typeof(TController).Name.Length - "Controller".Length).ToLower();
		}

		private static string GetActionName(Expression<Action<TController>> action)
		{
			return (action != null) ? ((MethodCallExpression)action.Body).Method.Name.ToLower() : "index";
		}

		private static string GetAreaName()
		{
			string area = null;
			string controllerFullName = typeof(TController).FullName ?? "";
			int areaNameIndex = controllerFullName.IndexOf("Areas");
			if (areaNameIndex > 0) {
				areaNameIndex += "Areas".Length + 1;
				area = controllerFullName.Substring(areaNameIndex, (controllerFullName.IndexOf(".", areaNameIndex) - areaNameIndex)).ToLower();
			}
			return area;
		}
	}
}