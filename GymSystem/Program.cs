using GymSystem.Repository;
using GymSystem.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ×¢²á´æ´¢¿âºÍ·þÎñ
builder.Services.AddScoped<AvailableTimeRepository>();
builder.Services.AddScoped<AdminRepository>();
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<CoachRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<ExerciseDataRepository>();
builder.Services.AddScoped<ExerciseHistoryRepository>();
builder.Services.AddScoped<BookCoachService>();
builder.Services.AddScoped<LoginService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
