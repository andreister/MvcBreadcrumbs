using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MvcBreadcrumbs;
using NUnit.Framework;

namespace MvcBreadcrumbsTests
{
	[TestFixture]
	public class BreadcrumbTrailTests
	{
		private readonly Node _root = 
			new MvcNode<HomeController>("Home", x => x.Index(),
				new MvcNode<AdminController>("Admin", x => x.Index(),
					new MvcNode<AdminController>("Admin Nested 1", x => x.Nested1()),
					new MvcNode<AdminController>("Admin Nested 2", x => x.Nested2())
				),
				new MvcNode<ReportsController>("Reports", x => x.Index(),
					new MvcNode<ReportsController>("Reports Nested 1", x => x.Nested1()),
					new MvcNode<ReportsController>("Reports Nested 2", x => x.Nested2())
				)
			);

		[Test]
		public void Home()
		{
			var trail = BreadcrumbTrail.GetInstance(() => _root);
			var context = GetMockedContext("home", "index");

			var breadcrumbs = trail.Build(context);

			Assert.That(breadcrumbs.Count, Is.EqualTo(1));
			Assert.That(breadcrumbs[0].Name, Is.EqualTo("Home"));
		}
	
		[Test]
		public void Home_Admin()
		{
			var trail = BreadcrumbTrail.GetInstance(() => _root);
			var context = GetMockedContext("admin", "index");

			var breadcrumbs = trail.Build(context);

			Assert.That(breadcrumbs.Count, Is.EqualTo(2));
			Assert.That(breadcrumbs[0].Name, Is.EqualTo("Home"));
			Assert.That(breadcrumbs[1].Name, Is.EqualTo("Admin"));
		}

		[Test]
		public void Home_Admin_AdminNested1()
		{
			var trail = BreadcrumbTrail.GetInstance(() => _root);
			var context = GetMockedContext("admin", "nested1");

			var breadcrumbs = trail.Build(context);

			Assert.That(breadcrumbs.Count, Is.EqualTo(3));
			Assert.That(breadcrumbs[0].Name, Is.EqualTo("Home"));
			Assert.That(breadcrumbs[1].Name, Is.EqualTo("Admin"));
			Assert.That(breadcrumbs[2].Name, Is.EqualTo("Admin Nested 1"));
		}

		[Test]
		public void Home_Reports_ReportsNested2()
		{
			var trail = BreadcrumbTrail.GetInstance(() => _root);
			var context = GetMockedContext("reports", "nested2");

			var breadcrumbs = trail.Build(context);

			Assert.That(breadcrumbs.Count, Is.EqualTo(3));
			Assert.That(breadcrumbs[0].Name, Is.EqualTo("Home"));
			Assert.That(breadcrumbs[1].Name, Is.EqualTo("Reports"));
			Assert.That(breadcrumbs[2].Name, Is.EqualTo("Reports Nested 2"));
		}

		private RequestContext GetMockedContext(string controller, string action)
		{
			var context = new Mock<RequestContext>();
			var routeData = new RouteData();
			routeData.Values.Add("controller", controller);
			routeData.Values.Add("action", action);
			context.SetupGet(x => x.RouteData).Returns(routeData);

			var httpContext = new Mock<HttpContextBase>();
			context.SetupGet(x => x.HttpContext).Returns(httpContext.Object);
			return context.Object;
		}

		#region Controllers

		class HomeController : Controller
		{
			public void Index() { }
		}

		class AdminController : Controller
		{
			public void Index() {}
			public void Nested1() {}
			public void Nested2() {}
		}

		class ReportsController : Controller
		{
			public void Index() { }
			public void Nested1() { }
			public void Nested2() { }
		}

		#endregion
	}
}
