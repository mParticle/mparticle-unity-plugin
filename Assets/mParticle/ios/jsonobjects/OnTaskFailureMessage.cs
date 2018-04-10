using System;

namespace mParticle.ios
{
    [Serializable]
    public class OnTaskFailureMessage
    {
        public string CallbackUuid;
        public string ErrorCode;
        public string Domain;
        public string UserInfo;
    }
}

