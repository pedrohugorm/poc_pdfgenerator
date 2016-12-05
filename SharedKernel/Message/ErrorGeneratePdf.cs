namespace SharedKernel.Message
{
    public enum PdfGenerationError
    {
        Unknown = -1,
        NoSenderIdentified = 1,
        //TODO implement known errors
    }
    public class ErrorGeneratePdf
    {
        public PdfGenerationError Code { get; set; }
        public string Message { get; set; }
    }
}