namespace Infrastructure.Entities
{
    public partial class ViewsValue
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int Value { get; set; }
        public int ViewsThreshold { get; set; }

        public virtual Movie Movie { get; set; } = null!;
    }
}
