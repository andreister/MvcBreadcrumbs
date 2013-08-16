using MvcBreadcrumbs;
using MvcBreadcrumbs.Providers;

namespace MvcBreadcrumbsDemo.Breadcrumbs
{
	public class UserSettingsNodeTitleProvider : AbstractTitleProvider
	{
		protected override string GetTitleInternal(Node node, long id)
		{
			var user = GetUserBySettingId(id);
			
			node.UpdateParentData(parentData => {
				parentData.Title = user.Name;
				parentData.RouteValues["id"] = user.Id;
			});

			return "Settings";
		}

		/// <summary>
		/// The rule in this sample project is that settingId=userId*10, see "Display.cshtml"
		/// In real app we'd get the user from cache or database.
		/// </summary>
		private User GetUserBySettingId(long id)
		{
			return new User {Id = id/10, Name = "User #" + (id/10) };
		}

		private class User
		{
			public string Name { get; set; } 
			public long Id { get; set; } 
		}
	}
}