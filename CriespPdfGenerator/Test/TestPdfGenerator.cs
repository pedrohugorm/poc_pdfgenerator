using System;
using CriespPdfGenerator.HelloMigraDoc;
using MigraDoc.Rendering;
using SharedKernel.Criesp;
using SharedKernel.Document;

namespace CriespPdfGenerator.Test
{
    public class TestPdfGenerator: IGenerateFile<CriespPdfParameters>
    {
        public FileGenerationResponse Create(CriespPdfParameters parameters)
        {
            // Create a MigraDoc document
            var document = Documents.CreateDocument();

            //string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");

            var renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always) { Document = document };

            renderer.RenderDocument();

            // Save the document...
            var filename = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
            renderer.PdfDocument.Save(filename);

            return new FileGenerationResponse
            {
                IsSuccess = true,
                FileName = filename
            };
        }
    }
}
