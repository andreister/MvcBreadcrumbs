using System.Collections.Generic;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcBreadcrumbs.Providers;

namespace MvcBreadcrumbs
{
	public abstract class Node
	{
		public Node Parent { get; internal set; }
		public IEnumerable<Node> Children { get; private set; }
		public NodeData Data { get; protected set; }
		public AuthorizeAttribute AuthorizeAttribute { get; set; }

		private bool _isUpdatedByChild;
		private readonly ITitleProvider _titleProvider;
		
		protected Node(ITitleProvider titleProvider, IEnumerable<Node> children)
		{
			_titleProvider = titleProvider;

			Children = children;
			foreach (var child in Children) {
				child.Parent = this;
			}
		}

		public abstract Node FindNode(NodeData searchData);

		public void UpdateParentData(Action<NodeData> update)
		{
			update(Parent.Data);
			Parent._isUpdatedByChild = true;
		}

		public bool IsDynamic
		{
			get { return _titleProvider != null; }
		}

		internal void UpdateTitle(RequestContext context)
		{
			if (context != null && _titleProvider != null && !_isUpdatedByChild) {
				Data.Title = _titleProvider.GetTitle(this, context);
			}
		}
		
		public override string ToString()
		{
			return Data.Title;
		}
	}
}