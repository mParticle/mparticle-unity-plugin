using System;
using UnityEngine;

namespace mParticle.android
{
	internal class ToCSUtils
	{
		public ToCSUtils()
		{
		}

		internal static bool ConvertToCSharpBoolean(AndroidJavaObject value)
		{
			return Boolean.Parse(value.Call<string>("toString"));
		}

		internal static mParticle.Environment ConvertToCSharpEnvironment(AndroidJavaObject value)
		{
			return (mParticle.Environment)Enum.Parse(typeof(Environment), value.Call<string>("name"));
		}
	}
}