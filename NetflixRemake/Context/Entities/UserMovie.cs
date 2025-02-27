﻿namespace Infrastructure.Entities
{
    public partial class UserMovie
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public virtual Movie Movie { get; set; } = null!;
        public virtual AspNetUser User { get; set; } = null!;
    }
}
