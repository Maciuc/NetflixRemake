namespace Context.Entities
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            Users = new HashSet<AspNetUser>();
        }

        public string Id { get; set; } = null!;
        public string? ConcurrencyStamp { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }

        public virtual ICollection<AspNetUser> Users { get; set; }
    }
}
