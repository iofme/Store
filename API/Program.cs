using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using API.Benchmark;
using API.Data;
using API.Extensions;
using API.Filtros;
using API.Interface;
using API.Logging;
using API.Models;
using API.Repository;
using API.Services;
using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => 
    {
     opt.Filters.Add(typeof(ApiExceptionFilter));
    }
    )
        .AddJsonOptions(opt => 
        opt.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Removida a linha duplicada: builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

var secretKey = builder.Configuration["JWT:SecretKey"]
									?? throw new ArgumentException("Invalid secret key!!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
                                             Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("Admin").RequireClaim("id", "macoratti"));
    opt.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    opt.AddPolicy("ExclusiveOnly", policy => policy.RequireAssertion(context => context.User.HasClaim(claim => claim.Type == "id" && claim.Value == "macoratti") || context.User.IsInRole("SuperAdmin")));
});

builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

if (args.Contains("--benchmark"))
{
    // Executar benchmarks
    BenchmarkRunner.Run<ProdutosBenchmark>();
    return; // Encerra a aplicação após os benchmarks
}

builder.Services.AddRateLimiter(opt =>
{
	opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
	opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpcontext =>
											RateLimitPartition.GetFixedWindowLimiter(
																					partitionKey: httpcontext.User.Identity?.Name ??
																												httpcontext.Request.Headers.Host.ToString(),
											factory: partition => new FixedWindowRateLimiterOptions
											{
												AutoReplenishment = true,
												PermitLimit = 5,
												QueueLimit = 0,
												Window = TimeSpan.FromSeconds(10)
											}));
});

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfig 
{
    LogLevel = LogLevel.Information,
}));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var OrigensComAcessoPermitido = "_origensComAcessoPermitido";

builder.Services.AddCors(options =>
    options.AddPolicy(name: OrigensComAcessoPermitido,
    policy =>
    {
        policy.WithOrigins("http://www.apirequest.io");
    }
    ));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
            options.SwaggerEndpoint("/openapi/v1.json", "weather api"));
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();

app.UseCors(OrigensComAcessoPermitido);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();