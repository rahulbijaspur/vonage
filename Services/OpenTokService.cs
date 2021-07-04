using System;
using api.Helper;
using Microsoft.Extensions.Options;
using OpenTokSDK;

namespace api.Services
{
    public class OpenTokService
    {
        public Session Session { get; protected set; }
        public OpenTok OpenTok { get; protected set; }
        public IOptions<TokboxSettings> Config { get; }


        public OpenTokService(IOptions<TokboxSettings> config)
        {
            Config = config;
            int apiKey = 0;
            string apiSecret = null;

            try
            {
                string apiKeyString = config.Value.API_KEY;
                apiSecret = config.Value.API_SECRET;
                apiKey = Convert.ToInt32(apiKeyString);
            }

            catch (Exception ex)
            {
               Console.Write(ex);

            }

            finally
            {
                if (apiKey == 0 || apiSecret == null)
                {
                    Console.WriteLine(
                        "The OpenTok API Key and API Secret were not set in the application configuration. " +
                        "Set the values in App.config and try again. (apiKey = {0}, apiSecret = {1})", apiKey, apiSecret);
                    Console.ReadLine();
                    Environment.Exit(-1);
                }
            }

            this.OpenTok = new OpenTok(apiKey, apiSecret);
        }


        public string CreateSession()
        {
             this.Session = this.OpenTok.CreateSession();
             return Session.Id;
        }
        public string GenerateToken(string sessionId,Role role, double expireTime, string data)
        {
            return this.OpenTok.GenerateToken(sessionId,role);
        }
    }
}