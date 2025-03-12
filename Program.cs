using PearlsonTestBackend.Interfaces;
using PearlsonTestBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var mongoSettings = builder.Configuration.GetSection("MongoDB");
string mongoConnectionString = mongoSettings["ConnectionString"];
string mongoDatabaseName = mongoSettings["DatabaseName"];
string mongoCollectionName = mongoSettings["CollectionName"];

builder.Services.AddSingleton<IBookService, BookService>(serviceProvider =>
    new BookService(
        mongoConnectionString,
        mongoDatabaseName,
        mongoCollectionName));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:3000") 
              .AllowAnyHeader()
              .AllowAnyMethod();

        policy.WithOrigins("https://26.190.181.121:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowSpecificOrigin");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
