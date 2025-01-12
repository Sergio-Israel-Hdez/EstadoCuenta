using AutoMapper;
using EstadoCuenta_Backend.Handlers;
using EstadoCuenta_Backend.Services.Validators;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:5141");
builder.Services.AddHealthChecks();
// Add services to the container.
builder.Services.AddAutoMapper(typeof(EstadoCuenta_Backend.Models.DTO.MappingProfile));

builder.Services.AddSingleton<EstadoCuentaQueryHandler>(service =>
{
    return new EstadoCuentaQueryHandler(builder.Configuration.GetConnectionString("DefaultConnection"), service.GetRequiredService<IMapper>());
});
builder.Services.AddSingleton<InsertarCompraCommandHandler>(service =>
{
    return new InsertarCompraCommandHandler(builder.Configuration.GetConnectionString("DefaultConnection"), service.GetRequiredService<IMapper>());
});
builder.Services.AddSingleton<InsertarPagoCommandHandler>(service =>
{
    return new InsertarPagoCommandHandler(builder.Configuration.GetConnectionString("DefaultConnection"), service.GetRequiredService<IMapper>());
});
builder.Services.AddSingleton<TransaccionesMensualesQueryHandler>(service =>
{
    return new TransaccionesMensualesQueryHandler(builder.Configuration.GetConnectionString("DefaultConnection"), service.GetRequiredService<IMapper>());
});
builder.Services.AddSingleton<ComprasQueryHandler>(service =>
{
    return new ComprasQueryHandler(builder.Configuration.GetConnectionString("DefaultConnection"), service.GetRequiredService<IMapper>());
});
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<InsertarCompraCommandDTOValidator>();
        config.RegisterValidatorsFromAssemblyContaining<InsertarPagoCommandDTOValidator>();
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

var app = builder.Build();
app.MapHealthChecks("/health");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors();

app.Run();
