using MvcBreadcrumbs;
using MvcBreadcrumbsDemo.Areas.Admin.Controllers;
using MvcBreadcrumbsDemo.Controllers;

namespace MvcBreadcrumbsDemo.Breadcrumbs
{
	public class Builder
	{
		public static Node CreateSiteMap()
		{
			return new MvcNode<HomeController>("Home", x => x.Index(),
				//
				// Admin area
				//
				new MvcNode<DefaultController>("Admin", x => x.Index(),
					new MvcNode<UsersController>("Users", x => x.Index(),
						new MvcNode<UsersController>("Create", x => x.Create()),
						new MvcNode<UsersController>(new UserNodeTitleProvider(), x => x.Display(0),
							new MvcNode<UserSettingsController>(new UserSettingsNodeTitleProvider(), x => x.Index(0))
						)
					)
				),
				//
				// About us
				//
				new MvcNode<AboutController>("About", x => x.Index())
			);
		}
	}
}