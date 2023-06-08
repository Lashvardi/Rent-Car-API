using Microsoft.EntityFrameworkCore;
using RentCar.data;
using RentCar.SMTP;
using SMTP;
using Bogus;
using RentCar.models;
using System.Text;
using RentCar.Faker;
using RentCar.Repository.Abstract;
using RentCar.Repository.Implementation;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
        });
});

builder.Services.AddControllers();
// Connecting To Database
builder.Services.AddDbContext<DataContext>(options =>
        options.UseMySql("server=localhost;user=root;password=admin;database=stepapi;",
            ServerVersion.AutoDetect("server=localhost;user=root;password=admin;database=stepapi;")));


// Services
builder.Services.AddTransient<SendMessage>();
builder.Services.AddTransient<SendRentMail>();
builder.Services.AddTransient<GetRentMail>();
builder.Services.AddTransient<IFileService, FileService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Seed Data
// Faker
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    // Provide How Many Entities You Want To Seed
    // When Running Application Whith Uncommented Seeder It Will Seed Data.


    //DataSeeder.SeedCars(serviceProvider, 1000);
    //DataSeeder.SeedUsers(serviceProvider, 1000);
}


app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();
//Get Accesto Resources Folder
// Get Acces to Image
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
               Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});
app.MapControllers();

app.Run();
