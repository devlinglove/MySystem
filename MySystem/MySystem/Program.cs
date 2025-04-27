using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySystem.Domain;
using MySystem.Infrastructure;
using System.Text;
using MySystem;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MySystemConnectionString"));
});

builder.Services.AddOptions<AuthConfiguration>().BindConfiguration(nameof(AuthConfiguration));
builder.Services.AddOptions<EmailConfiguration>().BindConfiguration(nameof(EmailConfiguration));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
		options.SignIn.RequireConfirmedEmail = true;
	})
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["AuthConfiguration:Issuer"],
		ValidAudience = builder.Configuration["AuthConfiguration:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(builder.Configuration["AuthConfiguration:Key"]!))
	};
});

// Cross-Origin 
builder.Services
	.AddCors(options =>
	{
		options.AddPolicy("AllowOrigin", builder => builder
			.WithOrigins("http://localhost:4200")
			.AllowAnyHeader()
			.AllowAnyMethod()

		);
	});

builder.Services.MySystemExtensionsMethods();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
