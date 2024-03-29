using InsuranceCorp.API.Middlewares;
using InsuranceCorp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); //slou�� pro vygenerov�n� open API
builder.Services.AddSwaggerGen();           //slou�� pro vygenerov�n� open API

builder.Services.AddDbContext<InsCorpDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseLogRequestMiddleware(); //p�id�n n� middelware

app.UseAuthorization();

app.MapControllers();

app.Run();
