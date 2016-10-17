using System;
using System.Collections.Generic;
using System.Net.Http;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE.Util
{
    class ReCaptchaResponse
    {
        public string Success { get; set; }
        public List<String> ErrorCodes { get; set; }
        public string Hostname { get; set; }
        public string challenge_ts { get; set; }
    }

    public class ReCaptchaValidator
    {
        #region Public Accessors
        public bool Success { get; set; }
        public List<string> ErrorCodes { get; set; }
        public string Hostname { get; set; }
        public string ChallengeTS { get; set; }
        #endregion

        #region Private Static Strings
        private static string reCaptchaApiString =
            "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}";
        #endregion

        public bool Validate(string encodedResponse, string userIPAddress)
        {
            // reset success state and check that response and private key are available.
            Success = false;
            ReCaptchaResponse reCaptcha = null;
            if (string.IsNullOrEmpty(encodedResponse)) return false;
            if (string.IsNullOrEmpty(ReCaptchaConfig.PrivateKey)) return false;

            // create the new client and retrieve the response
            var client = new HttpClient();

            HttpResponseMessage googleResponse = client.GetAsync(string.Format(reCaptchaApiString,
                ReCaptchaConfig.PrivateKey, encodedResponse, userIPAddress)).Result;

            if (googleResponse.IsSuccessStatusCode)
            {
                // read the recaptcha response object from JSON and save results
                reCaptcha = googleResponse.Content.ReadAsAsync<ReCaptchaResponse>().Result;

                ErrorCodes = reCaptcha.ErrorCodes;
                Success = reCaptcha.Success.Equals("true", StringComparison.InvariantCultureIgnoreCase);
                Hostname = reCaptcha.Hostname;
                ChallengeTS = reCaptcha.challenge_ts;
            }
            else
            {
                Success = false;
            }

            return Success;
        }
    }
}
