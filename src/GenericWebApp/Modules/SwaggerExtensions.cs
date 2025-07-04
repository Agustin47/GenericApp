using Microsoft.OpenApi.Models;

namespace GenericWebApp.Modules;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
             // Created the Swagger document
             c.SwaggerDoc("v1", new OpenApiInfo
             {
                 Version = "Version .Net 9.0.x",
                 Title = "Swagger UI Personalized .Net 9",
                 Description = " Thanks for sharing.",
                 TermsOfService = new Uri("https://cvmendozacr.com/"),
                 Contact = new OpenApiContact
                 {
                     Name = "Ing. Javier Mendoza Blandón and Ing. Edson Martinez Zuniga",
                     Url = new Uri("https://sigemapro.com/autores.html")
                 },

                 License = new OpenApiLicense
                 {
                     Name = "For capacitation and reference use.",
                     Url = new Uri("https://example.com/license")
                 }
             });

             // form 2 to generate the swagger documentation
             foreach (var name in Directory.GetFiles(AppContext.BaseDirectory, "*.XML", SearchOption.TopDirectoryOnly))
             {
                 c.IncludeXmlComments(filePath: name);
             }

             // second form to give Authorization without write the world Bearer
             var securityScheme = new OpenApiSecurityScheme()
             {
                 Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                 Name = "Authorization",
                 In = ParameterLocation.Header,
                 Type = SecuritySchemeType.Http,
                 Scheme = "bearer",
                 BearerFormat = "JWT" // Optional
             };

             var securityRequirement = new OpenApiSecurityRequirement
               {
                     {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id = "bearerAuth"
                             }
                         },
                         new string[] {}
                     }
                 };
             c.AddSecurityDefinition("bearerAuth", securityScheme);
             c.AddSecurityRequirement(securityRequirement);
        });

     return services;
    } // end method AddSwagger
}