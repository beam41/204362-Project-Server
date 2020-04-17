namespace MheanMaa.Settings
{
    public class ImageSettings : IImageSettings
    {
        public int MaxWidth { get; set; }
    }

    public interface IImageSettings
    {
        public int MaxWidth { get; set; }
    }
}
