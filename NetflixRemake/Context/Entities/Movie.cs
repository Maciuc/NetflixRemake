namespace Infrastructure.Entities
{
    public partial class Movie
    {
        public Movie()
        {
            MoviesOptions = new HashSet<MoviesOption>();
            Ratings = new HashSet<Rating>();
            UserMovies = new HashSet<UserMovie>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? VideoPath { get; set; }
        public int StartPrice { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<MoviesOption> MoviesOptions { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }
    }
}
