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

		private bool _isUpdatedByChild;
		private readonly ITitleProvider _titleProvider;
		private readonly IVisibilityProvider _visibilityProvider;
		private readonly IClickabilityProvider _clickabilityProvider;
		
		protected Node(ITitleProvider titleProvider, IVisibilityProvider visibilityProvider, IClickabilityProvider clickabilityProvider, IEnumerable<Node> children)
		{
			_titleProvider = titleProvider;
			_visibilityProvider = visibilityProvider;
			_clickabilityProvider = clickabilityProvider;

			Children = children;
			foreach (var child in Children) {
				child.Parent = this;
			}
		}

		internal abstract Node FindNode(NodeData searchData);

		public void UpdateParentData(string title, long id)
		{
			Parent.Data.RouteValues["id"] = id;
			Parent.Data.Title = title;
			Parent._isUpdatedByChild = true;
		}

		public bool IsDynamic
		{
			get { return _titleProvider != null; }
		}

		internal void ApplyProviders(RequestContext context)
		{
			if (context == null)
				return;

			if (_titleProvider != null && !_isUpdatedByChild) {
				Data.Title = _titleProvider.GetTitle(this, context);
			}
			if (_visibilityProvider != null) {
				Data.IsVisible = _visibilityProvider.IsVisible(this, context);
			}
			if (_clickabilityProvider != null) {
				Data.IsClickable = _clickabilityProvider.IsClickable(this, context);
			}
		}
		
		public override string ToString()
		{
			return Data.Title;
		}
	}
}