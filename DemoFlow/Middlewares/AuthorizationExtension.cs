namespace Api.Middlewares
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {// Có thể thêm policy nếu muốn
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));
                options.AddPolicy("AdminAndTeacher", policy =>
                  policy.RequireRole("Admin", "Teacher"));
            });
            return services;
        }
    }
}
