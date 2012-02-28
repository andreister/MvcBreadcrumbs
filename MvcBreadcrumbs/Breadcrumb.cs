using System.Web.Mvc;

namespace MvcBreadcrumbs
{
	public class Breadcrumb
	{
		public string Name { get; set; }
		public MvcHtmlString Url { get; set; }
	}
}
