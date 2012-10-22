using System.IO;
using MvcBreadcrumbs;
using MvcBreadcrumbs.Providers;

namespace MvcBreadcrumbsDemo.Breadcrumbs
{
	public class UserSettingsNodeTitleProvider : AbstractTitleProvider
	{
		protected override string GetTitleInternal(Node node, long id)
		{
			//id here is "settingId" - based on it,
			var name = GetUserNameBySettingId(id);
			node.UpdateParentData(parentData => {
				parentData.Title = name;
				parentData.RouteValues["id"] = id;
			});

			return "Settings";
		}

		private string GetUserNameBySettingId(long id)
		{
			return "[" + Path.GetRandomFileName().Replace(".", "") + ", taken from database!]";
		}
	}
}