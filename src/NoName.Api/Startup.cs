using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NoName.Core.NetFoundation;
using NoName.Core.Repository;
using NoName.Core.Service;
using NoName.Data;
using NoName.Data.Context;
using NoName.Data.DbInitializer;
using NoName.Model.Account;
using NoName.Repository;
using NoName.Service;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenAuthenticationConfigSection>(Configuration.GetSection(nameof(TokenAuthenticationConfigSection)));
            var databaseConnection = Configuration.GetConnectionString(Common.Foundation.Constants.Common.DefaultConnection);

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }); ;

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(databaseConnection));
            services.AddTransient<DbContext, ApplicationDbContext>();

            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddTransient<IAccountService, AccountService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = Configuration[$"{nameof(TokenAuthenticationConfigSection)}:{nameof(TokenAuthenticationConfigSection.Issuer)}"],
                    ValidAudience = Configuration[$"{nameof(TokenAuthenticationConfigSection)}:{nameof(TokenAuthenticationConfigSection.Issuer)}"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration[$"{nameof(TokenAuthenticationConfigSection)}:{nameof(TokenAuthenticationConfigSection.SecretKey)}"]))
                };

                options.SecurityTokenValidators.Clear();
                options.SecurityTokenValidators.Add(new CustomJwtSecurityTokenHandler());

                var serviceProvider = services.BuildServiceProvider();
                
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        await TokenValidatedEvent(context);
                    }
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                    if (context.AllMigrationsApplied())
                    {
                        DbInitializer.Initialize(context);
                    }
                }
            }

            app.UseAuthentication();
            app.UseMvc();
        }

        private async Task TokenValidatedEvent(TokenValidatedContext context)
        {
            var userId = Convert.ToInt64(context.Principal.Claims.First(f => f.Type == Common.Foundation.Constants.Claim.UserId).Value);


        }
    }
}