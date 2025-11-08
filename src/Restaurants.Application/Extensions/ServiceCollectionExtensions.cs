using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantsService, RestaurantsService>();

            // Đăng ký tất cả các Profile trong assembly này
            services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

            // AddValidatorsFromAssembly is an extension method from FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddFluentValidationAutoValidation();

            // Đăng ký MediatR và quét assembly hiện tại để tìm các handler, request, v.v.
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            });

            // Đăng ký UserContext
            services.AddScoped<IUserContext,UserContext>();

            // Đăng ký IHttpContextAccessor để UserContext có thể sử dụng
            services.AddHttpContextAccessor();
        }
    }
}
