using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    class ReCaptchaKeyElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (string)base["value"]; }
            set { base["value"] = value; }
        }
    }

    class ReCaptchaConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("PublicKey", IsRequired = true)]
        public ReCaptchaKeyElement PublicKey
        {
            get
            {
                return (ReCaptchaKeyElement)base["PublicKey"];
            }
        }

        [ConfigurationProperty("PrivateKey", IsRequired = true)]
        public ReCaptchaKeyElement PrivateKey
        {
            get
            {
                return (ReCaptchaKeyElement)base["PrivateKey"];
            }
        }
    }

    public static class ReCaptchaConfig
    {
        private static string _privateKey = "";
        private static string _publicKey = "";

        public static string PublicKey
        {
            get { return _publicKey; }
        }

        public static string PrivateKey
        {
            get { return _privateKey; }
        }

        static ReCaptchaConfig()
        {
            ReCaptchaConfigSection section = (ReCaptchaConfigSection)ConfigurationManager.GetSection("nci/web/recaptchaConfig");

            _privateKey = section.PrivateKey.Value;
            _publicKey = section.PublicKey.Value;
        }
    }
}
