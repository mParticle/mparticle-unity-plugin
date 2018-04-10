using System;
using mParticle;
using System.Collections.Generic;
using System.Linq;

namespace mParticle.ios
{
	internal class IdentityApiRequestDto : Dictionary<string, object>
	{
		private const string TaskUUID = "TaskUUID";
		private const string UserAliasUUID = "UserAliasUUID";
		private const string UserIdentities = "UserIdentities";

		internal IdentityApiRequestDto(IdentityApiRequest request, string taskUUID = null, string userAliasUUID = null)
		{
			if (userAliasUUID != null)
			{
				Add(UserAliasUUID, userAliasUUID);
			}
			if (request != null)
			{
				Dictionary<string, string> userIdentities = new Dictionary<string, string>();
				request.UserIdentities.ToList().ForEach(pair => userIdentities.Add(((int)pair.Key).ToString(), pair.Value));
				Add(UserIdentities, userIdentities);
			}
			if (taskUUID != null)
			{
				Add(TaskUUID, taskUUID);
			}
		}
	}
}

