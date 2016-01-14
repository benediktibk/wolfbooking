using System.IO;
using System.Web;

namespace Facade.Controllers
{
    public static class TemplateEngine
    {
        public static string Parse(string layoutFilePath, string contentFilePath, string title)
        {
            var contentFile = File.ReadAllText(HttpContext.Current.Server.MapPath(contentFilePath));
            var result = File.ReadAllText(HttpContext.Current.Server.MapPath(layoutFilePath));
            result.Replace("@Title", title);
            result.Replace("@Content", contentFile);
            return result;
        }
    }
}