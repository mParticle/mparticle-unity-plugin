using System;
using System.Collections.Generic;

namespace mParticle.ios
{
    public class PushRegistrationDto : Dictionary<string, string>
    {
        private const string AndroidSenderId = "AndroidSenderId";
        private const string AndroidInstanceId = "AndroidInstanceId";
        private const string IOSToken = "IOSToken";

        public PushRegistrationDto(PushRegistration pushRegistration)
        {
            if (!String.IsNullOrEmpty(pushRegistration.AndroidSenderId))
            {
                Add(AndroidSenderId, pushRegistration.AndroidSenderId);
            }
            if (!String.IsNullOrEmpty(pushRegistration.AndroidInstanceId))
            {
                Add(AndroidInstanceId, pushRegistration.AndroidInstanceId);
            }
            if (!String.IsNullOrEmpty(pushRegistration.IOSToken))
            {
                Add(IOSToken, pushRegistration.IOSToken);
            }
        }
    }
}

