namespace DijitalCard.Data
{
    using Microsoft.Extensions.Options;
    using DijitalCard.Data.Infrastructure;
    using DijitalCard.Data.Infrastructure.Entities;

    public class PlatformData : EntityBaseData<Model.Platform>
    {
        public PlatformData(IOptions<DatabaseSettings> dbOptions) 
            : base(new DataContext(dbOptions.Value.ConnectionString))
        {

        }
    }
}
