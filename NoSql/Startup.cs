using Microsoft.Owin;
using MongoDB.Bson.Serialization;
using NoSql.Models.DbModels;
using Owin;

[assembly: OwinStartupAttribute(typeof(NoSql.Startup))]
namespace NoSql
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
