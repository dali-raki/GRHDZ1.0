namespace Implementation.Services.Post
{
    public interface IPostGeneratePDF
    {
        Task<byte[]> GeneratePDF(int equipeId, DateTime date);
    }
}