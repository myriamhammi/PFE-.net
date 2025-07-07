using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TEST_PFE.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Liste des chemins publics qui ne nécessitent pas d'authentification
            var publicPaths = new[]
            {
                "/auth/login",
                "/css",
                "/js",
                "/images",
                "/lib",
                "/favicon.ico"
            };

            // Vérifier si le chemin actuel est public
            bool isPublicPath = false;
            foreach (var publicPath in publicPaths)
            {
                if (path.StartsWith(publicPath))
                {
                    isPublicPath = true;
                    break;
                }
            }

            // Si le chemin n'est pas public et l'utilisateur n'est pas authentifié
            if (!isPublicPath && context.Session.GetString("IsAuthenticated") != "true")
            {
                // Rediriger vers la page de connexion
                context.Response.Redirect("/Auth/Login");
                return;
            }

            await _next(context);
        }
    }

    // Extension method pour ajouter le middleware dans Program.cs
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}