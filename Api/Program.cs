using Api.Middlewares;
using Application.IEventBus;
using Infastructure.Persistence;
using Infrastructure.Messages;

var builder = WebApplication.CreateBuilder(args);

//CORS
builder.Services.AddCustomCors();
// Dependence injection
builder.Services.AddDatabaseAndConfiguration(builder.Configuration);
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices(builder.Configuration);
//JWT custom
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();
//kafka
var kafkaBootstrapServers = builder.Configuration.GetValue<string>("Kafka:BootstrapServers")
                          ?? "localhost:9092"; // fallback cho development

builder.Services.AddSingleton<IMessagePublisher>(sp =>
    new KafkaMessagePublisher(kafkaBootstrapServers));
builder.Services.AddSingleton<IMessageConsumer>(sp =>
    new KafkaMessageConsumer(kafkaBootstrapServers));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseExceptionMiddleware();
app.UseStaticFiles();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
