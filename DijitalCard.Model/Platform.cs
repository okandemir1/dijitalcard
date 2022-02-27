namespace DijitalCard.Model
{
    using System;

    public class Platform : Core.ModelBase
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
