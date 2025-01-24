using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

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

app.MapGet("/insecure", () => "Hello World!");
app.MapGet("/secure", () => "This page is secure!").RequireCors().RequireAuthorization();
app.MapGet("/pushimage", async () =>
  {
    // var fileName = Path.GetTempFileName();
    // try
    // {
    //   using(FileStream fs = File.OpenWrite(fileName))
    //   {
    //     await stream.CopyToAsync(fs);
    //   }
    //
    //   var imageParts = "ttutko/testimage:v1".Split(':');
    //   var tag = imageParts[1];
    //   var name = $"registry/{imageParts[0]}";
    //
    //   var args = new List<string>() {
    //     "cp",
    //     "--from-oci-layout",
    //     $"{fileName}:{tag}",
    //     $"{name}:{tag}"
    //   };
    //
    //   var start = new ProcessStartInfo {
    //     FileName = "/oras/oras",
    //     Arguments = string.Join(" ", args),
    //     UseShellExecute = false,
    //     RedirectStandardOutput = true
    //   };
    //
    //   var outputFile = string.Empty;
    //   using(var process = System.Diagnostics.Process.Start(start))
    //   using(var reader = process.StandardOutput)
    //   {
    //     outputFile = await reader.ReadToEndAsync();
    //     outputFile = outputFile.Replace("\n", "");
    //   }
    // }
    // finally
    // {
    //   File.Delete(fileName);
    // }
  }).RequireCors().RequireAuthorization();
app.MapFallbackToFile("index.html");

app.Run();
