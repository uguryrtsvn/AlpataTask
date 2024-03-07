using Microsoft.Extensions.Configuration;

namespace AlpataUI.AppSettings
{
    public class AppSettingsHelper
    {
        private readonly IConfiguration _configuration;

        public AppSettingsHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        } 
        private string ApiUrl { get => _configuration?["ApiUrl"]; }

        public static string GetApiUrl(IConfiguration configuration)
        {
            var helper = new AppSettingsHelper(configuration);
            return helper.ApiUrl;
        }
    }
}
