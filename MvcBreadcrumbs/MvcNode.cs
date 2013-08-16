using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcBreadcrumbs.Providers;

namespace MvcBreadcrumbs
{
	public class MvcNode<TController> : Node
		where TController : IController
	{
		public MvcNode(string title, Expression<Action<TController>> action, params Node[] children) 
			: base(null, children)
		{
			SetupNodeData(action, title);
		}

		public MvcNode(ITitleProvider titleProvider, Expression<Action<TController>> action, params Node[] children) 
			: base(titleProvider, children)
		{
			SetupNodeData(action, null);
		}

		public override Node FindNode(NodeData searchData)
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

		private void SetupNodeData(Expression<Action<TController>> action, string title)
		{
			var controllerName = typeof(TController).Name.Substring(0, typeof(TController).Name.Length - "Controller".Length).ToLower();
			
			var actionMethod = (action != null) ? ((MethodCallExpression)action.Body).Method : null;
			var actionName = (actionMethod != null) ? actionMethod.Name.ToLower() : "index";
			
			AuthorizeAttribute = (actionMethod != null) ? actionMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true).FirstOrDefault() as AuthorizeAttribute : null;
			if (AuthorizeAttribute == null) {
				AuthorizeAttribute = typeof (TController).GetCustomAttributes(typeof (AuthorizeAttribute), true).FirstOrDefault() as AuthorizeAttribute;
			}
			
			string area = null;
			string controllerFullName = typeof(TController).FullName ?? "";
			int areaNameIndex = controllerFullName.IndexOf("Areas");
			if (areaNameIndex > 0) {
				areaNameIndex += "Areas".Length + 1;
				area = controllerFullName.Substring(areaNameIndex, (controllerFullName.IndexOf(".", areaNameIndex) - areaNameIndex)).ToLower();
			}

			Data = new NodeData(this) {
				Title = title,
				Area = area,
				Controller = controllerName,
				Action = actionName
			};
		}
	}
}