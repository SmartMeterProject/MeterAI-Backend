using System.Diagnostics.Metrics;
using System.Text;
using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Business.Extensions;
using Business.Security.Authorize;
using Business.Security.JWT;
using Business.Utilities;
using Counter.Provider;
using Counter.Services.BackgorundServices;
using Counter.Services.MailService;
using DataAccess.Abstract;
using DataAccess.Abstract.Models;
using DataAccess.Concrete;
using Entity.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Bcpg;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
builder.Services.AddHttpContextAccessor();


//builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddScoped<IUserOneSignalRepository, UserOneSignalRepository>();
builder.Services.AddScoped<IUserOneSignalService, UserOneSignalManager>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserManager>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityManager>();

builder.Services.AddScoped<ICompanyAddressSpecialRepository, CompanyAddressSpecialRepository>();
builder.Services.AddScoped<ICompanyAddressSpecialService, CompanyAddressSpecialManager>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyManager>();

builder.Services.AddScoped<ICompanySpecialPriceRepository, CompanySpecialPriceRepository>();
builder.Services.AddScoped<ICompanySpecialPriceService, CompanySpecialPriceManager>();

builder.Services.AddScoped<ICountyRepository, CountyRepository>();
builder.Services.AddScoped<ICountyService, CountyManager>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceManager>();

builder.Services.AddScoped<IPredictionService, PredictionManager>();

builder.Services.AddScoped<IMeterRepository, MeterRepository>();
builder.Services.AddScoped<IMeterService, MeterManager>();


builder.Services.AddScoped<IMeterTypeRepository, MeterTypeRepository>();
builder.Services.AddScoped<IMeterTypeService, MeterTypeManager>();

builder.Services.AddScoped<INewRepository, NewRepository>();
builder.Services.AddScoped<INewService, NewManager>();

builder.Services.AddScoped<IRecordRepository, RecordRepository>();
builder.Services.AddScoped<IRecordService, RecordManager>();

builder.Services.AddScoped<IStateTaxRepository, StateTaxRepository>();
builder.Services.AddScoped<IStateTaxService, StateTaxManager>();

builder.Services.AddScoped<IUserHouseRepository, UserHouseRepository>();
builder.Services.AddScoped<IUserHouseService, UserHouseManager>();

//------------------------------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserManager>();

builder.Services.AddScoped<IUserProvider, UserProvider>();

builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<ISecuredOperation, SecuredOperation>();
builder.Services.AddSingleton<IEmailSender, AuthMessageSender>();

builder.Services.AddDependencyResolvers(new ICoreModule[]
{
    new CoreModule(),
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = "http://localhost",
        ValidAudience = "http://localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecretkeymysecretkeymysecretkey")),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CounterDbContext>(options =>
    options.UseSqlServer(connectionString));
//builder.Services.AddDbContext<CounterDbContext>(_ => _.UseSqlServer("Server = localhost; Database = CounterDb; Trusted_Connection = True;"));
//Data Source=SQL5105.site4now.net;Initial Catalog=db_a99454_counterdb;User Id=db_a99454_counterdb_admin;Password=bilal1795
//builder.Services.AddHostedService<MailBackgroundService>();
//workstation id = CounterDb.mssql.somee.com; packet size = 4096; user id = halilyildiz_SQLLogin_1; pwd = qpjwzl5wa8; data source = CounterDb.mssql.somee.com; persist security info=False; TrustServerCertificate = True; initial catalog = CounterDb
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
