using Auction_Data_Access.Context;
using Auction_Data_Access.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auction.Extensions
{
    public static class PersistenceExtensionLayer
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {

            #region Context
            services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); });
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            #endregion
            return services;
        }
    }
}
