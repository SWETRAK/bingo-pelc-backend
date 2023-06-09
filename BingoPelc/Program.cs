using BingoPelc;
using BingoPelc.Authentication;
using BingoPelc.Authorization;
using BingoPelc.Middlewares;
using BingoPelc.Repositories;
using BingoPelc.Services;
using BingoPelc.Validators;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Load dbContext;
builder.Services.AddDbContext<DomainContextDb>();

// Load repositories
builder.Services.AddRepositories();

// Load services
builder.Services.AddServices();

// Load Middlewares
builder.Services.AddMiddlewares();

// Load authentication
builder.Services.AddAuthenticationCustom(builder);

// Load authorization
builder.Services.AddAuthorizationCustom();

// Load validators
builder.Services.AddValidators();

// Load controllers
builder.Services.AddControllers();

// Load automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    // .WithOrigins("https://localhost:4200")); // Allow only this origin can also have multiple origins separated with comma
    .AllowCredentials()); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddlewares();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();