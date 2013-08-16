using System.Web.Mvc;

namespace MvcBreadcrumbsDemo.Areas.Admin.Controllers
{
	public class UserSettingsController : Controller
	{
		public ActionResult Index(long id)
		{
			return View(id);
		}
	}
}
