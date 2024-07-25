
using TutorialApp.Contexts;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Repositories;
using TutorialApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Azure.Storage.Blobs;

namespace Tutorial_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews().AddNewtonsoftJson(
    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddLogging(l => l.AddLog4Net());


            #region Swagger
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            #endregion

            #region JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"]))
                    };

                });

            #endregion

            #region AzureBlob
            builder.Services.AddSingleton(x =>
            {
                var configuration = x.GetRequiredService<IConfiguration>();
                return new BlobServiceClient(configuration.GetConnectionString("azureBlobStorage"));
            });

            #endregion


            #region Contexts
            builder.Services.AddDbContext<TutorialAppContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"))
                );
            #endregion

            #region Repositories
            builder.Services.AddScoped<IRepository<int, Module>, ModuleRepository>();
            builder.Services.AddScoped<IRepository<int, Question>, QuestionRepository>();
            builder.Services.AddScoped<IRepository<int, Quiz>, QuizRepository>();
            builder.Services.AddScoped<IRepository<int, Wishlist>, WishlistRepository>();
            builder.Services.AddScoped<IRepository<String, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int, Cart>, CartRepository>();
            builder.Services.AddScoped<IRepository<int, Category>, CategoryRepository>();
            builder.Services.AddScoped<IRepository<int, Course>, CourseRepository>();
            builder.Services.AddScoped<IRepository<int, Enrollment>, EnrollmentRepository>();
            builder.Services.AddScoped<IRepository<string, UserCredential>, UserCredentialRepository>();

            #endregion

            #region Services
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAzureBlobService, AzureBlobService>();
            builder.Services.AddScoped<IQuizService, QuizService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IWishlistService, WishlistService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<ICartService, CartService>();



            #endregion

            #region CORS

            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("AllowAll", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}