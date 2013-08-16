Type-safe dynamic breadcrumbs for ASP.NET MVC projects.
==============

- Breadcrumbs are defined as C# lambdas (rather than in plain text XML), so the compiler will warn you if a breadcrumb gets out of sync with the code.
- Breadcrumbs are created dynamically, which is especially useful for intranet projects with thousands of pages.
- Title, visibility and clickability can be changed at runtime.

An example of breadcrumbs builder:

	public class BreadcrumbsBuilder
	{
		public Node CreateSiteMap()
		{
			return new MvcNode<HomeController>("Home", x => x.Index(),
				//
				// Admin area
				//
				new MvcNode<DefaultController>("Admin", x => x.Index(),
					new MvcNode<UsersController>("Users", x => x.Index(),
						new MvcNode<UsersController>("Create", x => x.Create()),
						new MvcNode<UsersController>(new UserNodeTitleProvider(), x => x.Display(0),
							new MvcNode<UserSettingsController>(new UserSettingsNodeTitleProvider(), x => x.Index())
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
	
