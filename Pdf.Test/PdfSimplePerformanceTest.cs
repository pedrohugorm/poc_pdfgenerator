using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Pdf.Test
{
    [TestClass]
    public class PdfSimplePerformanceTest
    {
        /// <summary>
        /// http://www.pdfsharp.net/wiki/HelloWorld-sample.ashx
        /// </summary>
        [TestMethod, Timeout(1000)]
        public void ShouldGenerateHelloWordPdfAndSaveIt()
        {
            var doc = new PdfDocument();
            doc.Info.Title = $"{nameof(ShouldGenerateHelloWordPdfAndSaveIt)} - {DateTime.Now:D}";

            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var font = new XFont("Verdana", 12, XFontStyle.Regular);

            var pageRect = new XRect(0, 0, page.Width, page.Height);

            gfx.DrawString("Hello World", font, XBrushes.Black, pageRect, XStringFormats.Center);

            doc.Save($"{nameof(ShouldGenerateHelloWordPdfAndSaveIt)}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.pdf");
        }

        public class LayoutHelper
        {
            private readonly PdfDocument _document;
            private readonly XUnit _topPosition;
            private readonly XUnit _bottomMargin;
            private XUnit _currentPosition;

            public LayoutHelper(PdfDocument document, XUnit topPosition, XUnit bottomMargin)
            {
                _document = document;
                _topPosition = topPosition;
                _bottomMargin = bottomMargin;
                // Set a value outside the page - a new page will be created on the first request.
                _currentPosition = bottomMargin + 10000;
            }

            public XUnit GetLinePosition(XUnit requestedHeight)
            {
                return GetLinePosition(requestedHeight, -1f);
            }

            public XUnit GetLinePosition(XUnit requestedHeight, XUnit requiredHeight)
            {
                XUnit required = requiredHeight == -1f ? requestedHeight : requiredHeight;
                if (_currentPosition + required > _bottomMargin)
                    CreatePage();
                XUnit result = _currentPosition;
                _currentPosition += requestedHeight;
                return result;
            }

            public XGraphics Gfx { get; private set; }
            public PdfPage Page { get; private set; }

            void CreatePage()
            {
                Page = _document.AddPage();
                Page.Size = PageSize.A4;
                Gfx = XGraphics.FromPdfPage(Page);
                _currentPosition = _topPosition;
            }
        }

        /// <summary>
        /// http://www.pdfsharp.net/wiki/MultiplePages-sample.ashx
        /// </summary>
        [TestMethod, Timeout(1000)]
        public void ShouldGenerateMultiplePagesExamplePdfAndSaveIt()
        {
            var document = new PdfDocument();

            // Sample uses DIN A4, page height is 29.7 cm. We use margins of 2.5 cm.
            var helper = new LayoutHelper(document, XUnit.FromCentimeter(2.5), XUnit.FromCentimeter(29.7 - 2.5));
            var left = XUnit.FromCentimeter(2.5);

            // Random generator with seed value, so created document will always be the same.
            var rand = new Random(42);

            const int headerFontSize = 20;
            const int normalFontSize = 10;

            var fontHeader = new XFont("Verdana", headerFontSize, XFontStyle.BoldItalic);
            var fontNormal = new XFont("Verdana", normalFontSize, XFontStyle.Regular);

            const int totalLines = 666;
            var washeader = false;
            for (var line = 0; line < totalLines; ++line)
            {
                var isHeader = line == 0 || !washeader && line < totalLines - 1 && rand.Next(15) == 0;
                washeader = isHeader;
                // We do not want a single header at the bottom of the page, so if we have a header we require space for header and a normal text line.
                var top = helper.GetLinePosition(isHeader ? headerFontSize + 5 : normalFontSize + 2, isHeader ? headerFontSize + 5 + normalFontSize : normalFontSize);

                helper.Gfx.DrawString(isHeader ? "Sed massa libero, semper a nisi nec" : "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    isHeader ? fontHeader : fontNormal, XBrushes.Black, left, top, XStringFormats.TopLeft);
            }

            // Save the document... 
            document.Save($"{nameof(ShouldGenerateMultiplePagesExamplePdfAndSaveIt)}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.pdf");
        }
    }
}
