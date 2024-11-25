using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DevNet_BusinessLayer.Interfaces;
using DevNet_BusinessLayer.Services;
using DevNet_DataAccessLayer.Data;
using DevNet_DataAccessLayer.Interfaces;
using DevNet_DataAccessLayer.Repositories;
using DevNet_WebAPI.Hubs;
using DevNet_WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR(); // Este servicio no debe duplicarse.

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFollowerService, FollowerService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UserAccountService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<FollowerService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();

// Repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped(typeof(Repository<>));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ChatRepository>();
builder.Services.AddScoped<CommentRepository>();
builder.Services.AddScoped<FollowerRepository>();
builder.Services.AddScoped<PostRepository>();

// Servicios adicionales
builder.Services.AddScoped<FileHandlingService>(provider =>
{
    var uploadDirectory = builder.Configuration.GetValue<string>("FileSettings:UploadDirectory");
    return new FileHandlingService(uploadDirectory);
});

// DbContext
builder.Services.AddDbContext<DevnetDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuraci�n de JWT
var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "default-secret-key";
var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes)
    };
});

builder.Services.AddAuthorization();

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Ajusta el origen seg�n tu frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo { Title = "DevNet API", Version = "v1" });
    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your bearer token in this format - Bearer {your token} to access this API"
    });
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(origin => true) // Permitir todas las solicitudes de origen
    .AllowCredentials());

app.UseHttpsRedirection();

app.UseCors("AllowLocalhost4200"); // Aplicar la pol�tica de CORS

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapHub<MessageHub>("/MessageHub");

app.Run();
