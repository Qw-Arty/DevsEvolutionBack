using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


//JSON Serializer
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
    = new DefaultContractResolver());

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();

var app = builder.Build();

if (app.Request.HttpMethod == "OPTIONS")
{
    app.Response.StatusCode = 200;
    app.Response.AddHeader("Access-Control-Allow-Headers", "content-type,accept,authorization");
    app.Response.AddHeader("Access-Control-Allow-Origin", "*");
    app.Response.AddHeader("Access-Control-Allow-Credentials", "true");
    app.Response.AddHeader("Access-Control-Allow-Methods", "POST,GET,OPTIONS,PUT,DELETE");
    app.Response.AddHeader("Content-Type", "application/json");
    app.Response.AddHeader("Accept", "application/json");
    app.Response.End();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");



app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

