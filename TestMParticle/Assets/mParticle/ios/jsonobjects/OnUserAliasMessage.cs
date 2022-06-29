using System;

namespace mParticle.ios
{
    [Serializable]
    public class OnUserAliasMessage
    {
        public string CallbackUuid;
        public string PreviousMpid;
        public string NewMpid;
    }
}

