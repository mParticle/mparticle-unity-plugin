using System;
using mParticle;
using System.Collections.Generic;
using UnityEngine;

namespace mParticle.ios
{
	internal class MParticleOptionsDto : Dictionary<string, object>
	{
		private const string InstallType = "InstallType";
		private const string Environment = "Environment";
		private const string ApiKey = "ApiKey";
		private const string ApiSecret = "ApiSecret";
		private const string IdentifyRequest = "IdentifyRequest";
		private const string DevicePerformanceMetricsDisabled = "DevicePerformanceMetricsDisabled";
		private const string IdDisabled = "IdDisabled";
		private const string UploadInterval = "UploadInterval";
		private const string SessionTimeout = "SessionTimeout";
		private const string UnCaughtExceptionLogging = "UnCaughtExceptionLogging";
		private const string LogLevel = "LogLevel";
		private const string LocationTracking = "LocationTracking";
		private const string PushRegistration = "PushRegistration";

		internal MParticleOptionsDto(MParticleOptions options, String userAliasUuid = null)
		{
			Add(ApiKey, options.ApiKey);
			Add(ApiSecret, options.ApiSecret);
			if (options.InstallType.HasValue)
			{
				Add(InstallType, ((int)options.InstallType.Value).ToString());
			}
			if (options.Environment.HasValue)
			{
				Add(Environment, ((int)options.Environment.Value).ToString());
			}
			if (options.IdentifyRequest != null)
			{
				Add(IdentifyRequest, new IdentityApiRequestDto(options.IdentifyRequest, null, userAliasUuid));
			}
			if (options.DevicePerformanceMetricsDisabled.HasValue)
			{
				Add(DevicePerformanceMetricsDisabled, options.DevicePerformanceMetricsDisabled.Value.ToString());
			}
			if (options.IdDisabled.HasValue)
			{
				Add(IdDisabled, options.IdDisabled.Value.ToString());
			}
			if (options.UploadInterval.HasValue)
			{
				Add(UploadInterval, options.UploadInterval.Value.ToString());
			}
			if (options.SessionTimeout.HasValue)
			{
				Add(SessionTimeout, options.SessionTimeout.Value.ToString());
			}
			if (options.UnCaughtExceptionLogging.HasValue)
			{
				Add(UnCaughtExceptionLogging, options.UnCaughtExceptionLogging.Value.ToString());
			}
			if (options.LogLevel.HasValue)
			{
				Add(LogLevel, ((int)options.LogLevel.Value).ToString());
			}
			if (options.LocationTracking != null)
			{
				Add(LocationTracking, new LocationTrackingDto(options.LocationTracking));
			}
			if (options.PushRegistration != null)
			{
				Add(PushRegistration, new PushRegistrationDto(options.PushRegistration));
			}
		}
	}
}

