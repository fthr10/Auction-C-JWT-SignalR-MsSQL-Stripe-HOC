using Auction.Hubs.ConnectionManagment;
using Auction_Bussines.Abstraction;
using Auction_Bussines.Concrete;
using Auction_Core.MailHelper;
using Auction_Core.Models;

namespace Auction.Extensions
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            #region services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IBidService, BidService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IPaymentHistoryService, PaymentHistoryService>();
            services.AddScoped<IConnectionManager, ConnectionManager>();
            services.AddScoped(typeof(ApiResponse));
            #endregion
            return services;
        }
    }
}
