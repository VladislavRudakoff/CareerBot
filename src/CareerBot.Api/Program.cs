WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(t => t.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(t =>
    t.SwaggerDoc("v1", new() { Title = "CareerBot.Api", Version = "v1" }));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CheckAvailabilityDelivery v1"));
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapGet("/", async (string query) => await VacancyAggregator.GetAllVacancies(query));

await app.RunAsync();
