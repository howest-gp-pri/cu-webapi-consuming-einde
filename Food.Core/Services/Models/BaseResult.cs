namespace Food.Core.Services.Models
{
    public abstract class BaseResult
    {
        public bool Success => !Errors.Any();
        public List<string> Errors { get; set; } = new List<string>();
    }
}
