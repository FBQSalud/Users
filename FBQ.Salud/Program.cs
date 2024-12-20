using FBQ.Salud_AccessData.Commands;
using FBQ.Salud_AccessData.Data;
using FBQ.Salud_AccessData.Queries;
using FBQ.Salud_Application.Services;
using FBQ.Salud_Application.Validations;
using FBQ.Salud_Domain.Commands;
using FBQ.Salud_Domain.Queries;
using FBQ.Salud_Domain.Utilities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services
        .AddControllers()
        .AddFluentValidation(c =>
        c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddValidatorsFromAssemblyContaining<UserValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UserPutValidation>();

builder.Services.AddControllers().AddJsonOptions(x =>
                                x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<FbqSaludDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Software Lion", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Jwt Authorization",
            Name = "Authorization",
            In=ParameterLocation.Header,
            Type=SecuritySchemeType.ApiKey,
            Scheme="Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference= new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id= "Bearer"
                    }
                },
                new string[]{}
            }
        });

    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IUserValidatorExist, UserValidatorExist>();
builder.Services.AddTransient<IUserCommand, UsersCommand>();
builder.Services.AddTransient<IUserQuery, UserQuery>();
builder.Services.AddTransient<IUserServices, UsersServices>();
builder.Services.AddTransient<IRolService, RolServices>();
builder.Services.AddTransient<IRolQuery, RolQuery>();
builder.Services.AddTransient<IAdminQuery, AdminQuery>(); 
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options
                                               .AllowAnyOrigin()
                                                .AllowAnyMethod()
                                                .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyMethod();
    options.AllowAnyHeader();
    options.AllowAnyOrigin();
});
app.UseHttpsRedirection();

app.UseAuthentication();            

app.UseAuthorization();

app.MapControllers();

app.Run();
