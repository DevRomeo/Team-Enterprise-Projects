using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PDFGeneration
{
    public class PdfViewController : Controller
    {
        private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;

        public PdfViewController()
        {
            this.htmlViewRenderer = new HtmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        //Return pdf byte[] for download
        protected byte[] BufferPdf(string pageTitle, string viewName, object model)
        {
            // Render the view html to a string.
            string htmlText = this.htmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.
            byte[] buffer = standardPdfRenderer.Render(htmlText, pageTitle);

            // Return the PDF as a binary stream to the client.            
            return buffer;
        }

        //Return PDF on browser
        protected ActionResult ViewPdf(string pageTitle, string viewName, object model)
        {            
            // Return the PDF as a binary stream to the client.            
            return new BinaryContentResult(this.BufferPdf(pageTitle, viewName, model), "application/pdf");
        }

        protected ActionResult JournalPdf(string pageTitle, string viewName, object model)
        {
            // Return the PDF as a binary stream to the client.            
            return new BinaryContentResult(this.BufferPdf(pageTitle, viewName, model), "application/pdf");
        }
    }
}
