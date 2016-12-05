using SharedKernel.Message;

namespace SharedKernel.Actor.Exception
{
    public class PdfWorkerError : System.Exception
    {
        public PdfGenerationError Code { get; set; }
        public GeneratePdf ActorMessage { get; set; }

        public PdfWorkerError(PdfGenerationError code, GeneratePdf actorMessage)
            : base($"Error CODE = {code} when generating PDF")
        {
            Code = code;
            ActorMessage = actorMessage;
        }
    }
}