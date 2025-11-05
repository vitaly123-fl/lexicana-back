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

        //TODO це має бути в середині метода адд AddSingleton
        if (FirebaseApp.DefaultInstance == null)
        {
            var credential = GoogleCredential.FromFile(credsPath);

            FirebaseApp.Create(new AppOptions
            {
                Credential = credential,
                ProjectId = projectId
            });
        }

        //TODO чому ти вибрав саме AddSingleton
        services.AddSingleton(FirebaseAuth.DefaultInstance);
        return services;
    }
}