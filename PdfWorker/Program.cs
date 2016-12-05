using Akka.Actor;
using CriespPdfGenerator.Test;
using SharedKernel.Actor;

namespace PdfWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("pdfsystem");

            var pdfWorker = system.ActorOf(Props.Create(() => new GeneratePdfService(new TestPdfGenerator())), "pdfworker");

            system.WhenTerminated.Wait();
        }
    }
}
