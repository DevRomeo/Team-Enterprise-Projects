
namespace PDFGeneration
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    public class FakeView : IView
    {
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
