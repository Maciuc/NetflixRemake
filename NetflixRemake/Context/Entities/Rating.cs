namespace Infrastructure.Entities
{
    public partial class Rating
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; } = null!;
        public int RatingValue { get; set; }
        public string? Comment { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Movie Movie { get; set; } = null!;
        public virtual AspNetUser User { get; set; } = null!;
    }
}
