using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace lexicana.Configurations;

public static class FirebaseConfig
{
    public static IServiceCollection AddFirebaseConfig(this IServiceCollection services, ConfigurationManager configuration)
    {
        var credsPath = configuration["Firebase:CredentialsPath"];
        var projectId = configuration["Firebase:ProjectId"];

        if (FirebaseApp.DefaultInstance == null)
        {
            var credential = GoogleCredential.FromFile(credsPath);

            FirebaseApp.Create(new AppOptions
            {
                Credential = credential,
                ProjectId = projectId
            });
        }

        services.AddSingleton(FirebaseAuth.DefaultInstance);
        return services;
    }
}