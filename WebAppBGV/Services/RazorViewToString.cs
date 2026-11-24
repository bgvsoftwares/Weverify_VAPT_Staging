using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Office.Interop.Outlook;
using HtmlAgilityPack;

namespace WebAppBGV.Services
{
    public static class RazorViewToString
    {

        public static string RenderRazorViewToString(ControllerContext context,  string viewPath, object model)
        {
            var viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);
            if (viewEngineResult.View == null)
                throw new FileNotFoundException($"View {viewPath} cannot be found.");

            context.Controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(context, viewEngineResult.View,
                    context.Controller.ViewData, context.Controller.TempData, sw);
                viewEngineResult.View.Render(viewContext, sw);
                return sw.ToString();
            }
                        
        }

    }


}