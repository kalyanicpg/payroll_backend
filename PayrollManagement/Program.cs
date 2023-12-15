using PayRollManagementSystemAPI.Contracts;
using PayRollManagementSystemAPI.Models;
using PayRollManagementSystemAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PayRollManagementSystemDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase"))
    );
    builder.Services.AddTransient<DbContext,PayRollManagementSystemDbContext>();
builder.Services.AddIdentity<AccountUser, IdentityRole>()
    .AddEntityFrameworkStores<PayRollManagementSystemDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddTransient<IUserRepository,UserRepository>();
builder.Services.AddTransient<IAllowanceRepository,AllowanceRepository>();
builder.Services.AddTransient<ILeaveRepository, LeaveRepository>();
builder.Services.AddTransient<ISalaryRepository, SalaryRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(policyName: "CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


