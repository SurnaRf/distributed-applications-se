var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("Api", (serviceProvider, client) =>
{
	client.BaseAddress = new Uri("https://localhost:7233/");

	var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
	var session = httpContextAccessor.HttpContext?.Session;

	var token = session?.GetString("Token");

	if (!string.IsNullOrEmpty(token))
	{
		client.DefaultRequestHeaders.Authorization =
			new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
	}
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(60); 
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<PatternApiService>();
builder.Services.AddScoped<ProjectApiService>();
builder.Services.AddScoped<YarnApiService>();
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<UserApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
else
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}")
	.WithStaticAssets();


app.Run();
