using System.IO;
using MvcBreadcrumbs;
using MvcBreadcrumbs.Providers;

namespace MvcBreadcrumbsDemo.Breadcrumbs
{
	public class UserNodeTitleProvider : AbstractTitleProvider
	{
		protected override string GetTitleInternal(Node node, long id)
		{
			var name = GetUserNameByUserId(id);
			return name;
		}

		private string GetUserNameByUserId(long id)
		{
			return "[" + Path.GetRandomFileName().Replace(".", "") + ", taken from database!]";
		}
	}
}