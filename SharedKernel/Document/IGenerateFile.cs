namespace SharedKernel.Document
{
    public interface IGenerateFile<in T> where T : IFileGenerationParameter
    {
        FileGenerationResponse Create(T parameters);
    }
}
