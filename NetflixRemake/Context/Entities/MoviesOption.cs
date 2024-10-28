namespace Infrastructure.Entities
{
    public partial class MoviesOption
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int OptionId { get; set; }

        public virtual Movie Movie { get; set; } = null!;
        public virtual Option Option { get; set; } = null!;
    }
}
