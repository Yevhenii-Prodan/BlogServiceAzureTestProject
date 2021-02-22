namespace BlogService.SaveFunction.Models
{
    public class BlogPostModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsModerated { get; set; } = false;
        public bool ModerationSucceed { get; set; } = false;
    }
}