using System;
using UnityEngine;
using mParticle;
using mParticleAndroid;

namespace mParticle.android
{
	public class AndroidOnSuccessListener : AndroidJavaProxy
	{
		OnSuccess listener;


		internal AndroidOnSuccessListener(OnSuccess listener)
			: base("com.mparticle.identity.TaskSuccessListener")
		{
			this.listener = listener;
		}

		void onSuccess(AndroidJavaObject result)
		{
			if (result == null && result.Call<AndroidJavaObject>("getUser") != null)
			{
				listener.Invoke(null);
			}
			else
			{
				listener.Invoke(new IdentityApiResult()
					{
						User = new MParticleUserImpl(result.Call<AndroidJavaObject>("getUser"))
					});
			}
		}

	}
}