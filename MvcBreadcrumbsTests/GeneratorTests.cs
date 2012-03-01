using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MvcBreadcrumbs;
using NUnit.Framework;

namespace MvcBreadcrumbsTests
{
	[TestFixture]
	public class GeneratorTests
	{
		#region Setup/Teardown and helpers

		private Generator _generator;
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

		[TestFixtureSetUp]
		public void Setup()
		{
			_generator = Generator.GetInstance(() => _root);
		}

		private static RequestContext GetMockedContext(string controller, string action)
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

		class HomeController : Controller
		{
			public void Index() { }
		}

		class AdminController : Controller
		{
			public void Index() { }
			public void Nested1() { }
			public void Nested2() { }
		}

		class ReportsController : Controller
		{
			public void Index() { }
			public void Nested1() { }
			public void Nested2() { }
		}

		#endregion

		[Test, ExpectedException(typeof(MvcBreadcrumbsException))]
		public void MissingNode()
		{
			var context = GetMockedContext("unknowncontroller", "unknownaction");
			_generator.BuildBreadcrumbs(context);
		}

		[Test, TestCaseSource("Scenarios")]
		public void ExistingNode(string controller, string action, List<Breadcrumb> expectedBreadcrumbs)
		{
			var context = GetMockedContext(controller, action);
			var actualBreadcrumbs = _generator.BuildBreadcrumbs(context);

			Assert.That(actualBreadcrumbs.Count, Is.EqualTo(expectedBreadcrumbs.Count));
			for (int i = 0; i < expectedBreadcrumbs.Count; i++) {
				Assert.That(actualBreadcrumbs[i].Name, Is.EqualTo(expectedBreadcrumbs[i].Name));
			}
		}

		public IEnumerable<TestCaseData> Scenarios
		{
			get
			{
				return new List<TestCaseData> {
					new TestCaseData("home", "index", new List<Breadcrumb> {new Breadcrumb {Name = "Home"}}).SetName("Home"),
					new TestCaseData("admin", "index", new List<Breadcrumb> {new Breadcrumb {Name = "Home"}, new Breadcrumb {Name = "Admin"}}).SetName("Home > Admin"),
					new TestCaseData("admin", "nested1", new List<Breadcrumb> {new Breadcrumb {Name = "Home"}, new Breadcrumb {Name = "Admin"}, new Breadcrumb {Name = "Admin Nested 1"}}).SetName("Home > Admin > Admin Nested 1"),
					new TestCaseData("reports", "nested2", new List<Breadcrumb> {new Breadcrumb {Name = "Home"}, new Breadcrumb {Name = "Reports"}, new Breadcrumb {Name = "Reports Nested 2"}}).SetName("Home > Reports > Reports Nested 2"),
				};
			}
		}
	}
}
