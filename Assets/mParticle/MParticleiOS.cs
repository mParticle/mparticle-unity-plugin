using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using mParticle;
using mParticle.ios;
using System.Collections;
using System.Linq;

namespace mParticleiOs
{
	public class MParticleiOS : IMParticleSDK
	{
		/*
		 * MParticle interface methods
		 */
		[DllImport("__Internal")]
		private static extern void _Initialize(string mparticleOptionsJSON);

		[DllImport("__Internal")]
		private static extern void _LogEvent(string mpEvent);

		[DllImport("__Internal")]
		private static extern void _LogCommerceEvent(string commerceEventJSON);

		[DllImport("__Internal")]
		private static extern void _LogScreen(string screenName);

		[DllImport("__Internal")]
		private static extern void _SetATTStatus(ATTAuthStatus status, double timestamp);

		[DllImport("__Internal")]
		private static extern void _LeaveBreadcrumb(string breadcrumbName);

		[DllImport("__Internal")]
		private static extern int _GetEnvironment();

		[DllImport("__Internal")]
		private static extern void _SetOptOut(bool optOut);

		[DllImport("__Internal")]
		internal static extern void _Upload();

		/*
		 * Identity interface methods 
		 */

		[DllImport("__Internal")]
		internal static extern void _Identity_Identify(string identityApiRequestJSON);

		[DllImport("__Internal")]
		internal static extern void _Identity_Login(string identityApiRequestJSON);

		[DllImport("__Internal")]
		internal static extern void _Identity_Logout(string identityApiRequestJSON);

		[DllImport("__Internal")]
		internal static extern void _Identity_Modify(string identityApiRequestJSON);

		[DllImport("__Internal")]
		internal static extern void _Identity_AddIdentityStateListener();

		[DllImport("__Internal")]
		internal static extern void _Identity_RemoveIdentityStateListener();

		[DllImport("__Internal")]
		internal static extern string _Identity_GetCurrentUser();


		/*
		 * MParticleUser interface methods
		 */

		[DllImport("__Internal")] 
		internal static extern string _User_SetUserAttribute(string mpid, string key, string value);

		[DllImport("__Internal")] 
		internal static extern string _User_SetUserAttributes(string mpid, string attributesJSON);

		[DllImport("__Internal")]
		internal static extern string _User_SetUserTag(string mpid, string tag);

		[DllImport("__Internal")] 
		internal static extern string _User_RemoveUserAttribute(string mpid, string key);

		[DllImport("__Internal")] 
		internal static extern string _User_GetUserAttributes(string mpid);

		[DllImport("__Internal")] 
		internal static extern string _User_GetUserIdentities(string mpid);

		[DllImport("__Internal")] 
		internal static extern string _User_GetCurrentUserMpid(string mpid);

		/*
	     Private variables
	     */
		private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private ToiOSUtils toUtils = new ToiOSUtils();

		/*
	     Private methods
	     */
		public static double DateTimeToSeconds(DateTime sourceDateTime)
		{
			DateTime referenceDateTime = (sourceDateTime.Kind == DateTimeKind.Unspecified) ? DateTime.SpecifyKind(sourceDateTime, DateTimeKind.Utc) : sourceDateTime.ToUniversalTime();
			return (double)((referenceDateTime - epoch).TotalSeconds);
		}

		public void Initialize(MParticleOptions options)
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				return;
			}
			if (options == null)
			{
				options = new MParticleOptions();
			}
			if (options.IdentityStateListener != null)
			{
				Identity.AddIdentityStateListener(options.IdentityStateListener);
			}
			String optionsJson;
			if (options.IdentifyRequest != null && options.IdentifyRequest.UserAliasHandler != null)
			{
				optionsJson = toUtils.SerializeDictionary(new MParticleOptionsDto(options, (Identity as IdentityApiImpl).addUserAliasHandler(options.IdentifyRequest.UserAliasHandler)));
			}
			else
			{
				optionsJson = toUtils.SerializeDictionary(new MParticleOptionsDto(options));
			}
			_Initialize(optionsJson);
		}

		private IIdentityApi _identityApi;

		public IIdentityApi Identity
		{
			get
			{
				return _identityApi ?? (_identityApi = new IdentityApiImpl());
			}
		}

		public void LogEvent(MPEvent mpEvent)
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				return;
			}

			string eventJSON = toUtils.SerializeDictionary(new MPEventDto(mpEvent));
			_LogEvent(eventJSON);
		}

		public void LogEvent(CommerceEvent commerceEvent)
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				return;
			}
	            
			string commerceEventJSON = JsonUtility.ToJson(commerceEvent);
			_LogCommerceEvent(commerceEventJSON);
		}

		public void LogScreen(string screenName)
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				return;
			}
				
			_LogScreen(screenName);
		}

		public void SetATTStatus(ATTAuthStatus status, double timestamp)
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				return;
			}
				
			_SetATTStatus(status, timestamp);
		}

		public void LeaveBreadcrumb(string breadcrumbName)
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				return;
			}
				
			_LeaveBreadcrumb(breadcrumbName);
		}

		public mParticle.Environment Environment
		{
			get
			{
				if (Application.platform == RuntimePlatform.OSXEditor)
				{
					return mParticle.Environment.Development;
				}

				return (mParticle.Environment)Enum.Parse(typeof(mParticle.Environment), _GetEnvironment().ToString());
			}
		}

		public void SetOptOut(bool optOut)
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				return;
			}

			_SetOptOut(optOut);        
		}

		public void Upload()
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				return;
			}

			_Upload();
		}
	}

	class IdentityApiImpl : IIdentityApi, IdentityCallbacks
	{
		const string CallbackIdKey = "CallbackId";

		const string MpidKey = "MPID";
		const string PreviousMpidKey = "PreviousMPID";
		const string NewMpidKey = "NewMPID";

		private static IDictionary<string, BaseTask> _taskCallbacks = new Dictionary<string, BaseTask>();

		List<OnUserIdentified> _identityStateHandlers = new List<OnUserIdentified>();
		ToiOSUtils toUtils = new ToiOSUtils();

		public MParticleUser GetUser(long mpid)
		{
			return MParticleUserImpl.GetUserInstance(mpid);
		}

		public MParticleUser CurrentUser
		{
			get
			{
				string mpidString = MParticleiOS._Identity_GetCurrentUser();
				return MParticleUserImpl.GetUserInstance(mpidString);
			}
		}

		public void AddIdentityStateListener(OnUserIdentified listener)
		{
			if (!_identityStateHandlers.Contains(listener))
			{
				_identityStateHandlers.Add(listener);
				MParticleiOS._Identity_AddIdentityStateListener();
			}
		}

		public void RemoveIdentityStateListener(OnUserIdentified listener)
		{
			if (_identityStateHandlers.Remove(listener))
			{
				MParticleiOS._Identity_RemoveIdentityStateListener();
			}
		}

		public IMParticleTask<IdentityApiResult> Identify(IdentityApiRequest request)
		{
			string taskUuid = getRandomKey();
			BaseTask baseTask = new BaseTask();
			_taskCallbacks.Add(taskUuid, baseTask);
			string userAliasUuid = null;
			if (request != null)
			{
				userAliasUuid = addUserAliasHandler(request.UserAliasHandler);
			}		
			string requestJSON = toUtils.SerializeDictionary(new IdentityApiRequestDto(request, taskUuid, userAliasUuid));
			MParticleiOS._Identity_Identify(requestJSON);
			return baseTask;
		}

		public IMParticleTask<IdentityApiResult> Login(IdentityApiRequest request = null)
		{
			string taskUuid = getRandomKey();
			BaseTask baseTask = new BaseTask();
			_taskCallbacks.Add(taskUuid, baseTask);
			string userAliasUuid = null;
			if (request != null)
			{
				userAliasUuid = addUserAliasHandler(request.UserAliasHandler);
			}
			string requestJSON = toUtils.SerializeDictionary(new IdentityApiRequestDto(request, taskUuid, userAliasUuid));
			MParticleiOS._Identity_Login(requestJSON);
			return baseTask;
		}

		public IMParticleTask<IdentityApiResult> Logout(IdentityApiRequest request = null)
		{
			string taskUuid = getRandomKey();
			BaseTask baseTask = new BaseTask();
			_taskCallbacks.Add(taskUuid, baseTask);
			string userAliasUuid = null;
			if (request != null)
			{
				userAliasUuid = addUserAliasHandler(request.UserAliasHandler);
			}
			string requestJSON = toUtils.SerializeDictionary(new IdentityApiRequestDto(request, taskUuid, userAliasUuid));
			MParticleiOS._Identity_Logout(requestJSON);
			return baseTask;
		}

		public IMParticleTask<IdentityApiResult> Modify(IdentityApiRequest request)
		{
			string userAliasUuid = request.UserAliasHandler != null ? getRandomKey() : null;
			string taskUuid = getRandomKey();
			string requestJSON = toUtils.SerializeDictionary(new IdentityApiRequestDto(request, taskUuid, userAliasUuid));

			MParticleiOS._Identity_Modify(requestJSON);
			BaseTask baseTask = new BaseTask();
			_taskCallbacks.Add(taskUuid, baseTask);
			return baseTask;
		}

		internal String addUserAliasHandler(OnUserAlias onUserAlias)
		{
			string userAliasUuid = null;
			if (onUserAlias != null)
			{
				userAliasUuid = getRandomKey();
				_onUserAliasCallbacks.Add(userAliasUuid, onUserAlias);
			}
			return userAliasUuid;
		}

		public void OnUserIdentified(string userIdentified)
		{
			MpidDto body = JsonUtility.FromJson<MpidDto>(userIdentified);
			string mpidString = body.Mpid;
			long mpid = toUtils.ToLong(mpidString, 0);
			if (mpid != 0)
			{
				MParticleUser user = MParticleUserImpl.GetUserInstance(mpid);
				List<OnUserIdentified> identityStateHandlersCopy = new List<OnUserIdentified>(_identityStateHandlers);
				foreach (OnUserIdentified handler in identityStateHandlersCopy)
				{
					if (handler != null)
					{
						handler.Invoke(user);
					}
				}
			}
		}

		private IDictionary<String, OnUserAlias> _onUserAliasCallbacks = new Dictionary<String, OnUserAlias>();

		public void OnUserAlias(string usersMessage)
		{
			var message = JsonUtility.FromJson<OnUserAliasMessage>(usersMessage);

			if (message.CallbackUuid == null)
			{
				Debug.Log("CallbackID not provided, aborting OnUserAlias callback");
				return;
			}
			if (message.PreviousMpid == null || message.NewMpid == null)
			{
				Debug.Log("PreviousMpid and NewMpid not provided, aborting OnUserAlias callback");
				return;
			}
			OnUserAlias userAliasDelegate;
			if (!_onUserAliasCallbacks.TryGetValue(message.CallbackUuid, out userAliasDelegate))
			{
				Debug.Log("OnUserAlias with CallbackId not found, aborting OnUserAlias callback");
				return;
			}
			userAliasDelegate.Invoke(MParticleUserImpl.GetUserInstance(message.PreviousMpid), MParticleUserImpl.GetUserInstance(message.NewMpid));
		}

		public void OnTaskSuccess(string successMessage)
		{
			var message = JsonUtility.FromJson<OnTaskSuccessMessage>(successMessage);

			if (String.IsNullOrEmpty(message.CallbackUuid))
			{
				Debug.Log("CallbackID not provided, aborting OnTaskSuccess callback");
				return;
			}
				
			if (String.IsNullOrEmpty(message.Mpid))
			{
				Debug.Log("Success MPID not found, aborting OnTaskSuccess callback");
				return;
			}
			BaseTask task;
			if (!_taskCallbacks.TryGetValue(message.CallbackUuid, out task))
			{
				Debug.Log("Task with CallbackID not found, aborting OnTaskSuccess callback");
				return;
			}
			task.setResult(new IdentityApiResult()
				{
					User = MParticleUserImpl.GetUserInstance(message.Mpid)
				});

		}

		public void OnTaskFailure(string failureMessage)
		{
			//parse the IdentityHttpResponse
			Error error = new Error();
			var message = JsonUtility.FromJson<OnTaskFailureMessage>(failureMessage);
			if (String.IsNullOrEmpty(message.CallbackUuid))
			{
				
				Debug.Log("CallbackID not provided, aborting OnTaskFailure callback:\n" + JsonUtility.ToJson(message));
				return;
			}
			if (!String.IsNullOrEmpty(message.ErrorCode))
			{
				error.Code = message.ErrorCode;
			}
			String errorMessage = "";
			if (!String.IsNullOrEmpty(message.Domain))
			{
				errorMessage += "Domain: " + message.Domain;
			}
			if (!String.IsNullOrEmpty(message.UserInfo))
			{
				if (!String.IsNullOrEmpty(message.Domain))
				{
					errorMessage += "\n";
				}
				errorMessage += "User Info: " + message.UserInfo;
			}
			IdentityHttpResponse identityHttpResponse = new IdentityHttpResponse()
			{
				Errors = new List<Error>{ error },
				IsSuccessful = false
			};
			int httpCode;
			if (!String.IsNullOrEmpty(message.ErrorCode) && Int32.TryParse(message.ErrorCode, out httpCode))
			{
				identityHttpResponse.HttpCode = httpCode;
			}

			//Invoke the delegate with the specified callback Id
			BaseTask foundTask;
			if (_taskCallbacks.TryGetValue(message.CallbackUuid, out foundTask))
			{
				if (foundTask != null)
				{
					foundTask.setFailed(identityHttpResponse);
				}
			}
		}

		private string getRandomKey()
		{
			return System.Guid.NewGuid().ToString();
		}
	}

	class MParticleUserImpl : MParticleUser
	{
		ToiOSUtils toUtils = new ToiOSUtils();

		private MParticleUserImpl(long mpid)
		{
			_mpid = mpid;
		}

		internal static MParticleUserImpl GetUserInstance(string mpid)
		{
			return GetUserInstance(new ToiOSUtils().ToLong(mpid, 0));
		}

		internal static MParticleUserImpl GetUserInstance(long mpid)
		{
			if (mpid != 0)
			{
				return new MParticleUserImpl(mpid);
			}
			else
			{
				return null;
			}
		}

		private long _mpid;

		public override long Mpid
		{
			get
			{
				return _mpid;
			}
		}

		public override bool SetUserTag(string tag)
		{
			string result = MParticleiOS._User_SetUserTag(_mpid.ToString(), tag);
			bool resultBool = false;
			Boolean.TryParse(result, out resultBool);
			return resultBool;
		}

		public override Dictionary<UserIdentity, string> GetUserIdentities()
		{
			string userIdentitiesJson = MParticleiOS._User_GetUserIdentities(_mpid.ToString());
			return ToCSUtils.toUserIdentityDictionary(userIdentitiesJson);
		}

		public override Dictionary<string, string> GetUserAttributes()
		{
			string result = MParticleiOS._User_GetUserAttributes(_mpid.ToString());
			return ToCSUtils.toStringDictionary(result);
		}

		public override bool SetUserAttributes(Dictionary<string, string> userAttributes)
		{
			string userAttributeJson = toUtils.SerializeDictionary(userAttributes);
			string result = MParticleiOS._User_SetUserAttributes(_mpid.ToString(), userAttributeJson);
			return toUtils.ParseBoolean(result, false);
		}

		public override bool SetUserAttribute(string key, string val)
		{
			string result = MParticleiOS._User_SetUserAttribute(_mpid.ToString(), key, val);
			return toUtils.ParseBoolean(result, false);
		}

		public override bool RemoveUserAttribute(string key)
		{
			string result = MParticleiOS._User_RemoveUserAttribute(_mpid.ToString(), key);
			return toUtils.ParseBoolean(result, false);
		}
	}

	interface IdentityCallbacks
	{
		void OnUserIdentified(string userIdentified);

		void OnUserAlias(string usersMessage);

		void OnTaskSuccess(string successMessage);

		void OnTaskFailure(string failureMessage);
	}

	class BaseTask : IMParticleTask<IdentityApiResult>
	{


		private bool _isComplete;
		private bool _isSuccessful;
		private IdentityApiResult _result;
		private IdentityHttpResponse _failure;

		List<OnSuccess> _successListeners = new List<OnSuccess>();
		List<OnFailure> _onFailureListeners = new List<OnFailure>();

		public bool IsComplete()
		{
			return _isComplete;
		}

		internal void setComplete(bool isComplete)
		{
			_isComplete = isComplete;
		}

		public bool IsSuccessful()
		{
			return _isSuccessful;
		}

		internal void setIsSuccessful(bool isSuccessful)
		{
			_isSuccessful = isSuccessful;
		}

		public IdentityApiResult GetResult()
		{
			return _result;
		}

		public IMParticleTask<IdentityApiResult> AddSuccessListener(OnSuccess listener)
		{
			if (!_successListeners.Contains(listener))
			{
				if (_result != null)
				{
					listener.Invoke(_result);
				}
				_successListeners.Add(listener);
			}
			return this;
		}

		internal void setResult(IdentityApiResult onSuccess)
		{
			_result = onSuccess;
			foreach (OnSuccess listener in _successListeners)
			{
				listener.Invoke(_result);
			}
		}

		public IMParticleTask<IdentityApiResult> AddFailureListener(OnFailure listener)
		{
			if (!_onFailureListeners.Contains(listener))
			{
				if (_failure != null)
				{
					listener.Invoke(_failure);
				}
				_onFailureListeners.Add(listener);
			}
			return this;
		}

		internal void setFailed(IdentityHttpResponse onFailure)
		{
			_failure = onFailure;
			foreach (OnFailure listener in _onFailureListeners)
			{
				listener.Invoke(_failure);
			}
		}

	}
}