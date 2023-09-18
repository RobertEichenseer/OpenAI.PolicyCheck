using PCheck.Models;
using PCheck.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
AddServiceInstances(); 

var app = builder.Build();

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



void AddServiceInstances() {

    builder.Services.AddTransient<IOpenAI>(service => new OpenAI(
        apiKey: GetRequiredConfig("PCheck_AOAI_APIKEY"),
        endpoint: GetRequiredConfig("PCheck_AOAI_ENDPOINT"),
        embeddingModelDeploymentName: GetRequiredConfig("PCheck_AOAI_EMBEDDINGDEPLOYMENTNAME"))
    );

    builder.Services.AddSingleton<IRepository<Policy>>(service => new PolicyRepository(
        dataFolder: "../../preloaded_policies/")
    );

}

string GetRequiredConfig(string key)
{

    string value= ConfigurationBinder.GetValue<string>(builder.Configuration, key) ?? string.Empty;
    if (value == string.Empty) {
        throw new Exception($"Missing configuration \"{key}\"");
    }

    return value;
}
