namespace Infrastructure.Entities
{
    public partial class Option
    {
        public Option()
        {
            MoviesOptions = new HashSet<MoviesOption>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ValueInfluence { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<MoviesOption> MoviesOptions { get; set; }
    }
}
