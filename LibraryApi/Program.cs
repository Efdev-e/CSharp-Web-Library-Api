using LibraryApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("LibraryApi", new OpenApiInfo()
    {
        Title = "Library Api",
        Description = "Açıklama",
        Contact = new OpenApiContact() { Email = "efeok.dev@gmail.com"},
        Version = "v1",
        
    });
});
builder.Services.AddDbContext<LibraryDBContext>(options =>
    options.UseInMemoryDatabase("LibraryDB")
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryDBContext>();
    context.Database.EnsureCreated(); // This triggers OnModelCreating and seeds the data
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/LibraryApi/swagger.json", "Library Api");
            options.RoutePrefix = "swagger";

        }
    );
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
