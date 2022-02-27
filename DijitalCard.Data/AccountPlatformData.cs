namespace DijitalCard.Data
{
    using Microsoft.Extensions.Options;
    using DijitalCard.Data.Infrastructure;
    using DijitalCard.Data.Infrastructure.Entities;

    public class AccountPlatformData : EntityBaseData<Model.AccountPlatform>
    {
        public AccountPlatformData(IOptions<DatabaseSettings> dbOptions) 
            : base(new DataContext(dbOptions.Value.ConnectionString))
        {

        }
    }
}
