using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v2",
        Title = "API Title",
        Description = "API Description",
        Contact = new OpenApiContact {
            Name = "Your Name Here",
            Email = "Your Email Here"
        },
        License = new OpenApiLicense {
            Name = "Organization Name",
            Url = new Uri("https://www.yourwebsite.com/")
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.EnableAnnotations();
});


builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        builder => {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Create an instance of IConfiguration and bind appsettings.json to it.
var configuration = builder.Configuration;


var app = builder.Build();
app.UseStaticFiles();
app.UseCors("AllowAll");

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseDeveloperExceptionPage();

app.UseSwagger(option => {
    option.RouteTemplate = "docs/{documentName}/swagger.json";
    option.SerializeAsV2 = true;
});

app.UseSwaggerUI(option => {
    option.DocumentTitle = "Open API";
    option.RoutePrefix = "";
    option.SwaggerEndpoint("docs/v1/swagger.json", "Open API");

    option.InjectStylesheet("swagger-ui/custom.css");
    option.InjectJavascript("swagger-ui/custom.js");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
