namespace NetflixRemake.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? VideoPath { get; set; }
        public int StartPrice { get; set; }
        public int Views { get; set; }
    }


    public class CreateMovieViewModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? VideoPath { get; set; }
        public int StartPrice { get; set; }
    }

    public class UpdateMovieViewModel : CreateMovieViewModel
    {
        public int Id { get; set; }
    }

    public class UpdateViewsValueViewModel
    {
        public int MovieId { get; set; }
        public float Value { get; set; }
        public int ViewsThreshold { get; set; }
    }
}