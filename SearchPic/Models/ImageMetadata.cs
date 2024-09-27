namespace SearchPic.Models
{
    public class ImageMetadata
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public List<string> Keywords { get; set; }
    }
}
