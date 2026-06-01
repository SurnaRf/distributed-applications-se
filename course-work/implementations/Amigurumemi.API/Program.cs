using Amigurumemi.Api.Middlewares;
using Amigurumemi.API.Extensions;
using Amigurumemi.ApplicationServices.Implementations;
using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Data;
using Amigurumemi.Repositories.Implementations;
using Amigurumemi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Amigurumemi API",
		Version = "v1"
	});
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	c.IncludeXmlComments(xmlPath);
});

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
		ValidIssuer = "Amigurumemi", 

		ValidateAudience = true, 
		ValidAudience = "AmigurumemiClient", 

		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,


		IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes("SUPER_SECRET_KEY_CHANGE_ME_123456"))
	};
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IPatternService, PatternService>();
builder.Services.AddScoped<IYarnService, YarnService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<AmigurumemiDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDbContext>(provider =>
	provider.GetRequiredService<AmigurumemiDbContext>());

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();  
						 

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Services.SeedAdminUser();

app.Run();