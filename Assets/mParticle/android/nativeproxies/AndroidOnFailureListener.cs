using System;
using UnityEngine;
using mParticle;
using System.Collections.Generic;

namespace mParticle.android
{
    internal class AndroidOnFailureListener : AndroidJavaProxy
    {
        OnFailure listener;

        internal AndroidOnFailureListener(OnFailure listener)
            : base("com.mparticle.identity.TaskFailureListener")
        {
            this.listener = listener;
        }

        void onFailure(AndroidJavaObject response)
        {
            if (response == null)
            {
                listener.Invoke(null);
            }
            else
            {
                var errors = new List<Error>();
                foreach (AndroidJavaObject errorObject in response.Call<AndroidJavaObject>("getErrors").Call<AndroidJavaObject[]>("toArray"))
                {
                    if (errorObject != null)
                    {
                        errors.Add(new Error()
                            {
                                Code = errorObject.Get<string>("code"),
                                Message = errorObject.Get<string>("message")
                            });
                    }
                }
                listener.Invoke(new IdentityHttpResponse()
                    {
                        HttpCode = response.Call<int>("getHttpCode"),
                        IsSuccessful = response.Call<bool>("isSuccessful"),
                        Errors = errors
                    });
            }
        }

    }
}