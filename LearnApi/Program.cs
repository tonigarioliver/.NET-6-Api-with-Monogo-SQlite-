using LearnApi.Config;
using LearnApi.COR;
using LearnApi.Data;
using LearnApi.Entity;
using LearnApi.ExceptionHandler;
using LearnApi.Helper;
using LearnApi.Services;
using LearnApi.Servies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JWT"));
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.Configure<MongoDBConfig>(builder.Configuration.GetSection("MongoDatabase"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(ICustomerService), typeof(CustomerService));
builder.Services.AddScoped<IUserService, UserServcie>();
builder.Services.AddScoped<JwtHelper, JwtHelper>();
builder.Services.AddScoped<MongoDBConnection, MongoDBConnection>();
//builder.Services.AddCors(p => p.AddPolicy("corspolicy", policy => policy
//                                                            .WithOrigins("*")
//                                                            .AllowAnyHeader()
//                                                            .AllowAnyMethod()
//));
builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
{
    options.Window = TimeSpan.FromSeconds(10);
    options.PermitLimit = 1;
    options.QueueLimit = 1;
    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
}).RejectionStatusCode = StatusCodes.Status401Unauthorized);

builder.Services.AddCors(p => p.AddDefaultPolicy(policy => policy
                                                            .WithOrigins("*")
                                                            .AllowAnyHeader()
                                                            .AllowAnyMethod()
));
var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(builder.Configuration.GetSection("Logging:LogPath").Value,
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new HandleCustomExceptionAttribute());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAuthentication("BasicAuthentication")
//   .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value);
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("JWT:ValidAudience").Value,
        ValidIssuer = builder.Configuration.GetSection("JWT:ValidIssuer").Value,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/minimalapi", () => "Nihira Techiees");

app.MapGet("/getchannel", (string channelname) => "Welcome to " + channelname).WithOpenApi(opt =>
{
    var parameter = opt.Parameters[0];
    parameter.Description = "Enter Channel Name";
    return opt;
});

app.MapGet("/getcustomer", async (IUnitOfWork unitOfWork) => {
    return await unitOfWork.CustomerRepository.GetAllAsync();
});
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
