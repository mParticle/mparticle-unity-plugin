﻿using System;
using UnityEngine;
using mParticle;
using mParticleAndroid;

namespace mParticle.android
{
	#if UNITY_ANDROID || UNITY_EDITOR
	class AndroidUserAliasHandler : AndroidJavaProxy
	{
		OnUserAlias userAliasHandler;

		public AndroidUserAliasHandler(OnUserAlias handler)
			: base("com.mparticle.identity.UserAliasHandler")
		{
			this.userAliasHandler = handler;
		}

		public void onUserAlias(AndroidJavaObject previousUser, AndroidJavaObject newUser)
		{
			userAliasHandler.Invoke(new MParticleUserImpl(previousUser), new MParticleUserImpl(newUser));
		}
	}
	#endif
}