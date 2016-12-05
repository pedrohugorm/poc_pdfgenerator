using System;

namespace SharedKernel.Document
{
    public class FileGenerationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public string FileName { get; set; }
    }
}