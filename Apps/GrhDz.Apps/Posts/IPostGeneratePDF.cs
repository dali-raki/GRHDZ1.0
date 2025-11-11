namespace GrhDz.Apps.Posts
{
    public interface IPostGeneratePDF
    {
        Task<byte[]> GeneratePDF(int equipeId, DateTime date);
    }
}