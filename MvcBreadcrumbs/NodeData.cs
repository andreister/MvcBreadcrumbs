﻿using System.Web.Routing;

namespace MvcBreadcrumbs
{
	public class NodeData
	{
		public string Title { get; set; }
		internal bool IsUpdatedByChild { get; set; }

		public string Area { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
		public RouteValueDictionary RouteValues { get; private set; }

		internal NodeData(string title)
		{
			Title = title;
			RouteValues = new RouteValueDictionary();
		}
		
		public NodeData(RequestContext context) : this((string)null)
		{
			var routeData = context.RouteData.Values;
			var dataTokens = context.RouteData.DataTokens;

			Area = dataTokens.ContainsKey("area") ? dataTokens["area"].ToString() : null;
			Controller = routeData["controller"].ToString();
			Action = routeData["action"].ToString();
		}

		#region Equals/GethashCode

		public bool Equals(NodeData other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return string.Compare(Area, other.Area, true) == 0 &&
			       string.Compare(Controller, other.Controller, true) == 0 &&
			       string.Compare(Action, other.Action, true) == 0;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (NodeData)) return false;
			return Equals((NodeData) obj);
		}

		public override int GetHashCode()
		{
			unchecked {
				int result = (Area != null ? Area.GetHashCode() : 0);
				result = (result*397) ^ (Controller != null ? Controller.GetHashCode() : 0);
				result = (result*397) ^ (Action != null ? Action.GetHashCode() : 0);
				return result;
			}
		}

		public static bool operator ==(NodeData a, NodeData b)
		{
			if (ReferenceEquals(a, b)) return true;
			if (((object)a == null) || ((object)b == null)) return false;

			return a.Equals(b);
		}

		public static bool operator !=(NodeData a, NodeData b)
		{
			return !(a == b);
		}

		#endregion
	}
}