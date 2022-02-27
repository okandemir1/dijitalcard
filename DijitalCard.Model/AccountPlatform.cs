namespace DijitalCard.Model
{
    using System;

    public class AccountPlatform : Core.ModelBase
    {
        public int AccountId { get; set; }
        public int PlatformId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
