using MA.Data;
using MA.Data.Entities;
using MA.Service;
using MA.Service.Absract;
using MA.Service.MovieService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IGenericService<Movie>, GenericService<Movie>>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<MADBContext>(options=>options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

using (var dbContext = builder.Services.BuildServiceProvider().GetService<MADBContext>())
{
    dbContext.Database.Migrate();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors("*");
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthorization();
app.Run();

