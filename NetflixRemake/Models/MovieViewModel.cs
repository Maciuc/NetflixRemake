namespace NetflixRemake.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }


    public class CreateMovieViewModel
    {
        public string Name { get; set; } = null!;
    }
}