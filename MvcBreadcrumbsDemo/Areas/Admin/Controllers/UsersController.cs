using System.Web.Mvc;

namespace MvcBreadcrumbsDemo.Areas.Admin.Controllers
{
	public class UsersController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Create()
		{
			return View();
		}

		public ActionResult Display(long id)
		{
			return View();
		}
	}
}
