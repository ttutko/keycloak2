using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors(options =>
    {
      options.AddDefaultPolicy(builder =>
      {
        builder.WithOrigins("http://localhost:5000", "http://127.0.0.1:5000", "https://web.dev.smooth.tnt").AllowAnyHeader().AllowCredentials(); //.WithMethods("GET").AllowCredentials();
      });
    })
  // .AddCors(options =>
  // {
  //   options.AddDefaultPolicy(builder =>
  //   {
  //     builder.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
  //   });
  // })
  .AddAuthentication(options =>
    {
      //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    })
  // .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
  //     {
  //       options.Cookie.Name = "oidc";
  //       options.Cookie.SameSite = SameSiteMode.None;
  //       options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
  //       options.Cookie.IsEssential = true;
  //
  //     })
  .AddJwtBearer(o =>
      {
        o.Authority = "https://keycloak.dev.smooth.tnt/realms/DevRealm";
        // o.MetadataAddress = "https://keycloak.dev.smooth.tnt/realms/DevRealm";
        o.Audience = "account";
        o.IncludeErrorDetails = true;
        // o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
          ValidIssuer = "https://keycloak.dev.smooth.tnt/realms/DevRealm",
          ValidAudience = "account"          
        };

      });

builder.Services.AddAuthorization();
//builder.Services.AddAntiforgery();
// .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
//     {
//       options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
//       options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
//       options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//       options.GetClaimsFromUserInfoEndpoint = true;
//
//       options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
//
//       options.ResponseMode = OpenIdConnectResponseMode.FormPost;
//
//
//       options.Authority = "http://localhost:8888/realms/master";
//       options.ClientId = "aurelia-client-id";
//       options.ClientSecret = "WbqxYKBiTq6wikml2kPjjH6vcxXb7c5A";
//       options.ResponseType = OpenIdConnectResponseType.Code;
//       options.UsePkce = true;
//
//       options.SaveTokens = true;
//       options.GetClaimsFromUserInfoEndpoint = true;
//       options.Scope.Add("openid");
//       options.Scope.Add("email");
//       options.Scope.Add("phone");
//       options.Scope.Add("profile");
//
//       options.RequireHttpsMetadata = false;
//
//       options.Events = new OpenIdConnectEvents
//       {
//         OnRedirectToIdentityProviderForSignOut = context =>
//         {
//           context.Response.Redirect("http://localhost:8080");
//           context.HandleResponse();
//
//           return Task.CompletedTask;
//         },
//
//         OnRemoteFailure = context =>
//         {
//           Console.WriteLine(context.Failure);
//           context.Response.Redirect("/error");
//           context.HandleResponse();
//           return Task.FromResult(0);
//         }
//       };
//     }
//
//   );

var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseStaticFiles();
app.UseAuthorization();
//app.UseAntiforgery();

app.MapGet("/insecure", () => "Hello World!");
app.MapGet("/secure", () => "This page is secure!").RequireCors().RequireAuthorization();
app.MapPost("/upload", FileHandler.Upload).RequireCors().DisableAntiforgery();
app.MapFallbackToFile("index.html");

app.Run();
