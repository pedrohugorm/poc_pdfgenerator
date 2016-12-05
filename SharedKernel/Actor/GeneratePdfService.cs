using System;
using System.Threading.Tasks;
using Akka.Actor;
using SharedKernel.Actor.Exception;
using SharedKernel.Criesp;
using SharedKernel.Document;
using SharedKernel.Extension;
using SharedKernel.Message;

namespace SharedKernel.Actor
{
    public class GeneratePdfService : ReceiveActor
    {
        private readonly IGenerateFile<CriespPdfParameters> _generateFile;

        public GeneratePdfService(IGenerateFile<CriespPdfParameters> generateFile)
        {
            _generateFile = generateFile;
            ReceiveAsync<GeneratePdf>(GeneratePdf);
        }

        private Task GeneratePdf(GeneratePdf pdf)
        {
            try
            {
                var response = _generateFile.Create(pdf.DocumetInfo);

                var receivedMessage = $"Received message {pdf}";
                Console.WriteLine(receivedMessage);
                Sender?.SendSimpleMessage(receivedMessage);
                Sender?.SendSimpleMessage($"My Current working DIR = {Environment.CurrentDirectory}");

                if (Sender == null)
                    throw new PdfWorkerError(PdfGenerationError.NoSenderIdentified, pdf);

                if (response.IsSuccess)
                    Sender?.Tell(new SuccessGeneratePdf
                    {
                        //FileName = TODO 
                    });
                else
                    Sender?.Tell(new ErrorGeneratePdf
                    {
                        Code = PdfGenerationError.Unknown, //TODO
                        Message = response.Message
                    });
            }
            catch (PdfWorkerError pdfWorkerError)
            {
                Sender?.Tell(new ErrorGeneratePdf
                {
                    Message = pdfWorkerError.Message,
                    Code = pdfWorkerError.Code
                });
            }
            catch (System.Exception e)
            {
                Sender?.Tell(new ErrorGeneratePdf
                {
                    Code = PdfGenerationError.Unknown,
                    Message = e.Message
                });
            }

            Sender?.SendSimpleMessage("Actor finished");

            return Task.CompletedTask;
        }
    }
}
