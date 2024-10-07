using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors(options =>
    {
      options.AddDefaultPolicy(builder =>
      {
        builder.WithOrigins("http://localhost:8080", "http://127.0.0.1:8080").AllowAnyHeader().AllowCredentials(); //.WithMethods("GET").AllowCredentials();
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
        o.Authority = "http://localhost:8888/realms/MyRealm";
        o.Audience = "account";
        o.IncludeErrorDetails = true;
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
          ValidIssuer = "http://localhost:8888/realm/MyRealm",
          ValidAudience = "account"          
        };
        
      });

  builder.Services.AddAuthorization();
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
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapGet("/secure", () => "This page is secure!").RequireCors().RequireAuthorization();

app.Run();
