using System.Collections.Generic;
using System.Web.Routing;
using MvcBreadcrumbs.Providers;

namespace MvcBreadcrumbs
{
	public abstract class Node
	{
		public Node Parent { get; internal set; }
		public IEnumerable<Node> Children { get; private set; }
		public NodeData Data { get; protected set; }

		public ITitleProvider TitleProvider { get; set; }
		
		protected Node(IEnumerable<Node> children)
		{
			Children = children;
			foreach (var child in Children) {
				child.Parent = this;
			}
		}

		internal abstract Node FindNode(NodeData searchData);

		internal void ResolveTitle(RequestContext context)
		{
			if (!Data.IsUpdatedByChild && TitleProvider != null && context != null) {
				Data.Title = TitleProvider.GetTitle(this, context);
			}
		}

		public void UpdateParentData(string title, long id)
		{
			Parent.Data.RouteValues["id"] = id;
			Parent.Data.Title = title;
			Parent.Data.IsUpdatedByChild = true;
		}

		public override string ToString()
		{
			return Data.Title;
		}
	}
}