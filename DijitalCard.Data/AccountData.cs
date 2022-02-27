namespace DijitalCard.Data
{
    using Microsoft.Extensions.Options;
    using DijitalCard.Data.Infrastructure;
    using DijitalCard.Data.Infrastructure.Entities;

    public class AccountData : EntityBaseData<Model.Account>
    {
        public AccountData(IOptions<DatabaseSettings> dbOptions) 
            : base(new DataContext(dbOptions.Value.ConnectionString))
        {

        }
    }
}
