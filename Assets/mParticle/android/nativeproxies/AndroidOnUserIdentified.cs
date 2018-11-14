using System;
using UnityEngine;
using mParticle;
using mParticleAndroid;

namespace mParticle.android
{
	class AndroidOnUserIdentified : AndroidJavaProxy
	{
		OnUserIdentified onUserIdentifiedHandler;

		public AndroidOnUserIdentified(OnUserIdentified onUserIdentifiedHandler)
			: base("com.mparticle.identity.IdentityStateListener")
		{
			this.onUserIdentifiedHandler = onUserIdentifiedHandler;
		}

		void onUserIdentified(AndroidJavaObject user)
		{
			onUserIdentifiedHandler.Invoke(new MParticleUserImpl(user));
		}
	}
}