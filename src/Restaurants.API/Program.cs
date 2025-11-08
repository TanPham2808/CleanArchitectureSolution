
using Restaurants.API.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;
using Restaurants.API.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Sử dụng presentation layer
    builder.AddPresentation();

    // Sử dụng infrastructure layer
    builder.Services.AddEndpointsApiExplorer(); // cho Minimal APIs 
    builder.Services.AddInfrastructure(builder.Configuration);

    // Sử dụng application layer
    builder.Services.AddApplication();

    var app = builder.Build();

    // Seed dữ liệu ban đầu
    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
    await seeder.Seed();

    // Thêm middleware Serilog để log các request HTTP
    app.UseSerilogRequestLogging();

    // Thêm middleware xử lý lỗi
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeLoggingMiddleware>(); // Thêm middleware log thời gian xử lý request

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();


    // Tạo sẵn endpoint đăng ký/đăng nhập, refresh token, xác thực email, quên/đổi mật khẩu, 2FA…
    // dùng Bearer token cho SPA/mobile (không phải cookie UI truyền thống).
    app.MapGroup("api/identity")
        .WithTags("Identity") // Thêm thẻ "Identity" cho nhóm endpoint này trong Swagger
        .MapIdentityApi<User>();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

