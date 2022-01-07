using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
var confBuilder = new ConfigurationBuilder();

confBuilder.AddInMemoryCollection(new Dictionary<string, string> {
    {"MyKey", "MyValue"},
});

confBuilder.AddJsonFile("appsettings.json");

IConfigurationRoot Configuration = confBuilder.Build();

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.Audience = Configuration["AAD:ResourceId"];
    opt.Authority = $"{Configuration["AAD:Instance"]}{Configuration["AAD:TenantID"]}";
});
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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
