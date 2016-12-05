using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using CriespPdfGenerator.HelloMigraDoc;

namespace Pdf.Test
{
    [TestClass]
    public class PdfMigraDocPerformanceTest
    {
        [TestMethod, Timeout(5000)]
        public void ShouldGenerateDocument()
        {
            // Create a MigraDoc document
            var document = Documents.CreateDocument();

            //string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");

            var renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always) {Document = document};

            renderer.RenderDocument();

            // Save the document...
            var filename = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
            renderer.PdfDocument.Save(filename);
        }
    }
}