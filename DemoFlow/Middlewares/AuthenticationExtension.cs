using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Api.Middlewares
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection service, IConfiguration configuration)
        {

            var key = configuration.GetValue<string>("JWT:Secret");
            service.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero //bỏ đi cơ chế cộng thêm 5 phút
                };
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = context => // CHẶN LẠI KHI MÀ CHƯA LOGIN
                    {
                        context.HandleResponse(); // Ngăn default 401 response
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new { message = "Please log in to access this resource" });
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context => // CHẶN LẠI KHI KO ĐỦ QUYỀN
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new { message = "You do not have permission to access this resource" });
                        return context.Response.WriteAsync(result);
                    }
                };
            });
            return service;
        }
    }
}
