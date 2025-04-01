using TaskManagementSystem.API.Configurations;
using TaskManagementSystem.API.Contracts;
using TaskManagementSystem.API.Data;
using TaskManagementSystem.API.MIddleware;
using TaskManagementSystem.API.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.API.Consumer;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(busConfiguration =>
{
    busConfiguration.AddConsumer<TaskCreatedConsumer>(c => 
        c.UseMessageRetry(r => r.Interval(2, 1000)));
    busConfiguration.AddConsumer<TaskUpdatedConsumer>(c => 
        c.UseMessageRetry(r => r.Interval(2, 1000)));
    busConfiguration.SetKebabCaseEndpointNameFormatter();
    busConfiguration.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]!);
            h.Username(builder.Configuration["MessageBroker:Password"]!);
        });
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddDbContext<TaskManagementSystemDbContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITaskRepository, TasksRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
