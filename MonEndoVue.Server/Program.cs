using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using MonEndoVue.Server.Data;
using MonEndoVue.Server.Services;


namespace MonEndoVue.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
            
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_prod")));
            }

            
            builder.Services.AddAuthorization();
            
            builder.Services.AddScoped<CarnetSanteService>();
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policyBuilder =>
                {
                    policyBuilder.WithOrigins("https://localhost:7206/")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Access-Control-Allow-Origin")
                        .AllowCredentials();
                } );
            });
            
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.MigrateAsync();
    
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var rootUser = new ApplicationUser { UserName = "coralie.owczaruk@yahoo.fr", Email = "coralie.owczaruk@yahoo.fr", EmailConfirmed = true };

                if (await userManager.FindByNameAsync(rootUser.UserName) == null)
                {
                    await userManager.CreateAsync(rootUser, "Password123$");
                }
            }

            if (app.Environment.IsProduction())
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await dbContext.Database.MigrateAsync();
    
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var rootUser = new ApplicationUser { UserName = Environment.GetEnvironmentVariable("ROOT_USERNAME"), Email = Environment.GetEnvironmentVariable("ROOT_EMAIL"), EmailConfirmed = true };

                if (await userManager.FindByNameAsync(rootUser.UserName) == null)
                {
                    await userManager.CreateAsync(rootUser, Environment.GetEnvironmentVariable("ROOT_PASSWORD"));
                }
            }
            
            app.MapIdentityApi<ApplicationUser>();
            
            app.UseCors(options =>
            {
                options.WithOrigins("https://localhost:5173") 
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
            
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");
            await app.RunAsync();
        }
    }
}
