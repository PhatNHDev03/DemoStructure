namespace Api.Middlewares
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));
                options.AddPolicy("AdminAndTeacher", policy =>
                  policy.RequireRole("Admin", "Teacher"));
            });
            return services;
        }
    }
}
