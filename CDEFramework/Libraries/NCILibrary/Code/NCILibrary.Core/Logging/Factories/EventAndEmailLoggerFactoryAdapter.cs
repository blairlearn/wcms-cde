using System;
using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.Simple;

namespace NCI.Logging.Factories
{
    class EventAndEmailLoggerFactoryAdapter : AbstractSimpleLoggerFactoryAdapter
    {
        public LogLevel EventLevel { get; private set; }
        public LogLevel EmailLevel { get; private set; }
        
        public string LogSource { get; private set; }

        /// <summary>
        /// The sender email address
        /// </summary>
        public string EmailAddressFrom { get; private set; }

        /// <summary>
        /// The Destination email addresses
        /// </summary>
        public string EmailAddressesTo { get; private set; }

        public EventAndEmailLoggerFactoryAdapter(NameValueCollection properties)
            : base(properties)
        {
            // keep the overall configured level as the event level
            EventLevel = Level;

            string emailLevelString, logsource, addressFrom, addressesTo;
            LogLevel emailLevel = Level;
            if (properties.TryGetValue("emailLevel", out emailLevelString) &&
                Enum.TryParse<LogLevel>(emailLevelString, true, out emailLevel))
            {
                EmailLevel = emailLevel;
            }
            if (properties.TryGetValue("logSource", out logsource))
            {
                LogSource = logsource;
            }
            if (properties.TryGetValue("emailAddressFrom", out addressFrom))
            {
                EmailAddressFrom = addressFrom;
            }

            if (properties.TryGetValue("emailAddressesTo", out addressesTo))
            {
                EmailAddressesTo = addressesTo;
            }

            // set the overall level to the lower of email and event
            Level = EventLevel < EmailLevel ? EventLevel : EmailLevel;
        }

        protected override ILog CreateLogger(string name, LogLevel level, 
            bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
        {
            return new EventAndEmailLogger(name, level, showLevel, showDateTime, showLogName, dateTimeFormat, 
                EventLevel, EmailLevel, LogSource, EmailAddressFrom, EmailAddressesTo);
        }
    }
}
