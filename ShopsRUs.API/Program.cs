using Microsoft.EntityFrameworkCore;
using ShopsRUs.Core;
using ShopsRUs.Core.Configurations;
using ShopsRUs.Data;
using ShopsRUs.Services;
using ShopsRUs.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;

builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddMvc();

var appSettings = new AppSettings();
configuration.Bind(appSettings);
builder.Services.Configure<AppSettings>(configuration);



var connectionStringsSection = configuration.GetSection("ConnectionStrings");
builder.Services.Configure<ConnectionStrings>(connectionStringsSection);

builder.Services.AddScoped<DbContext, ShopsRUsContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//app.MapGet("/", () => "Hello World!");

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.MapControllers();

app.Run();