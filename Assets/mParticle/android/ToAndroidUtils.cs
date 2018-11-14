using System;
using UnityEngine;
using mParticle;
using System.Collections.Generic;
using System.Linq;

namespace mParticle.android
{
	internal class ToAndroidUtils
	{
		internal AndroidJavaClass identityTypeClass;
		internal AndroidJavaClass eventTypeClass;
		internal AndroidJavaClass installTypeClass;
		internal AndroidJavaClass environmentClass;
		internal AndroidJavaClass logLevelClass;
		internal AndroidJavaClass integerClass;
		internal AndroidJavaClass doubleClass;

		internal ToAndroidUtils()
		{
			eventTypeClass = new AndroidJavaClass("com.mparticle.MParticle$EventType");
			installTypeClass = new AndroidJavaClass("com.mparticle.MParticle$InstallType");
			environmentClass = new AndroidJavaClass("com.mparticle.MParticle$Environment");
			logLevelClass = new AndroidJavaClass("com.mparticle.MParticle$LogLevel");
			identityTypeClass = new AndroidJavaClass("com.mparticle.MParticle$IdentityType");
			integerClass = new AndroidJavaClass("java.lang.Integer");
			doubleClass = new AndroidJavaClass("java.lang.Double");
		}

		internal AndroidJavaObject ConvertToMpEventType(mParticle.EventType eventType)
		{
			return eventTypeClass.CallStatic<AndroidJavaObject>("valueOf", eventType.ToString());
		}

		internal AndroidJavaObject ConvertToMpUserIdentity(UserIdentity userIdentity)
		{
			return identityTypeClass.CallStatic<AndroidJavaObject>("valueOf", userIdentity.ToString());
		}

		internal AndroidJavaObject ConvertToMpEnvironment(mParticle.Environment environment)
		{
			return environmentClass.CallStatic<AndroidJavaObject>("valueOf", environment.ToString());
		}

		internal AndroidJavaObject ConvertToMpInstallType(InstallType installType)
		{
			return installTypeClass.CallStatic<AndroidJavaObject>("valueOf", installType.ToString());
		}

		internal AndroidJavaObject ConvertToMpLogLevel(LogLevel logLevel)
		{
			return logLevelClass.CallStatic<AndroidJavaObject>("valueOf", logLevel.ToString());
		}

		internal string ConvertToMpProductAction(ProductAction action)
		{
			switch (action)
			{
				case ProductAction.AddToCart:
					return "add_to_cart";
				case ProductAction.AddToWishlist:
					return "add_to_wishlist";
				case ProductAction.Checkout:
					return "checkout";
				case ProductAction.CheckoutOption:
					return "checkout_option";
				case ProductAction.Click:
					return "click";
				case ProductAction.Purchase:
					return "purchase";
				case ProductAction.Refund:
					return "refund";
				case ProductAction.RemoveFromWishlist:
					return "remove_from_wishlist";
				case ProductAction.RemoveFromCart:
					return "remove_from_cart";
				case ProductAction.ViewDetail:
					return "view_detail";
				default:
					return null;
			}
		}

		internal string ConvertToMpPromotionAction(PromotionAction action)
		{
			switch (action)
			{
				case PromotionAction.Click:
					return "click";
				case PromotionAction.View:
					return "view";
				default:
					return null;
			}
		}

		internal AndroidJavaObject ConvertToMpProduct(Product product)
		{
			AndroidJavaObject builder = new AndroidJavaObject("com.mparticle.commerce.Product$Builder", product.Name, product.Sku, product.Price);
			builder.Call<AndroidJavaObject>("customAttributes", ConvertDictToMap(product.customAttributes));
			builder.Call<AndroidJavaObject>("category", product.Category);
			builder.Call<AndroidJavaObject>("couponCode", product.CouponCode);
			if (product.Position.HasValue)
			{
				AndroidJavaObject javaInteger = integerClass.CallStatic<AndroidJavaObject>("valueOf", product.Position);
				builder.Call<AndroidJavaObject>("position", javaInteger);
			}

			builder.Call<AndroidJavaObject>("quantity", product.Quantity);
			builder.Call<AndroidJavaObject>("brand", product.Brand);
			builder.Call<AndroidJavaObject>("variant", product.Variant);
			return builder.Call<AndroidJavaObject>("build");
		}

		internal AndroidJavaObject ConvertToMpPromotion(Promotion promotion)
		{
			AndroidJavaObject javaPromotion = new AndroidJavaObject("com.mparticle.commerce.Promotion");
			javaPromotion.Call<AndroidJavaObject>("setCreative", promotion.Creative);
			javaPromotion.Call<AndroidJavaObject>("setId", promotion.Id);
			javaPromotion.Call<AndroidJavaObject>("setName", promotion.Name);
			javaPromotion.Call<AndroidJavaObject>("setPosition", promotion.Position.ToString());
			return javaPromotion;
		}

		internal AndroidJavaObject ConvertToMpImpression(Impression impression)
		{
			AndroidJavaObject javaImpression = new AndroidJavaObject("com.mparticle.commerce.Impression", impression.ImpressionListName, null);
			if (impression.Products != null)
			{

				foreach (Product product in impression.Products)
				{
					javaImpression.Call<AndroidJavaObject>(
						"addProduct", 
						new object[]{ ConvertToMpProduct(product) }
					);
				}

			}

			return javaImpression;
		}

		internal AndroidJavaObject ConvertToMpTransactionAttributes(TransactionAttributes attributes)
		{
			AndroidJavaObject javaAttributes = new AndroidJavaObject("com.mparticle.commerce.TransactionAttributes", attributes.TransactionId);
			javaAttributes.Call<AndroidJavaObject>("setCouponCode", attributes.CouponCode);
			if (attributes.Tax.HasValue)
			{
				javaAttributes.Call<AndroidJavaObject>("setTax", doubleClass.CallStatic<AndroidJavaObject>("valueOf", attributes.Tax));
			}

			if (attributes.Shipping.HasValue)
			{
				javaAttributes.Call<AndroidJavaObject>("setShipping", doubleClass.CallStatic<AndroidJavaObject>("valueOf", attributes.Shipping));
			}

			if (attributes.Revenue.HasValue)
			{
				javaAttributes.Call<AndroidJavaObject>("setRevenue", doubleClass.CallStatic<AndroidJavaObject>("valueOf", attributes.Revenue));
			}

			return javaAttributes.Call<AndroidJavaObject>("setAffiliation", attributes.Affiliation);
		}

		internal AndroidJavaObject ConvertToJavaBoolean(bool value)
		{
			return new AndroidJavaObject("java.lang.Boolean", value.ToString());
		}

		internal AndroidJavaObject ConvertToMpIdentifyRequest(IdentityApiRequest request)
		{
			AndroidJavaObject builder = new AndroidJavaClass("com.mparticle.identity.IdentityApiRequest").CallStatic<AndroidJavaObject>("withEmptyUser");
            if (request != null)
            {
                request.UserIdentities.ToList().ForEach(pair => builder.Call<AndroidJavaObject>("userIdentity", new object[] { ConvertToMpUserIdentity(pair.Key), pair.Value }));
                if (request.UserAliasHandler != null)
                {
                    builder.Call<AndroidJavaObject>("userAliasHandler", new object[] { new AndroidUserAliasHandler(request.UserAliasHandler) });
                }
            }
			return builder.Call<AndroidJavaObject>("build");
		}

		internal AndroidJavaObject ConvertDictToMap(IDictionary<string, string> data)
		{
			AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
			if (data != null)
			{
				foreach (KeyValuePair<string, string> entry in data)
				{
					map.Call<string>(
						"put", 
						new string[]{ entry.Key, entry.Value }
					);
				}
			}
			return map;
		}

		internal AndroidJavaObject ConvertToMpOptions(MParticleOptions options, object context)
		{
			var nativeMpOptions = new AndroidJavaClass("com.mparticle.MParticleOptions").CallStatic<AndroidJavaObject>("builder", new object[]{ context });
			nativeMpOptions.Call<AndroidJavaObject>("credentials", new object[] { options.ApiKey, options.ApiSecret });
			if (options.InstallType.HasValue)
			{
				nativeMpOptions.Call<AndroidJavaObject>("installType", new object[]{ ConvertToMpInstallType(options.InstallType.Value) });	
			}
			if (options.Environment.HasValue)
			{
				nativeMpOptions.Call<AndroidJavaObject>("environment", new object[] { ConvertToMpEnvironment(options.Environment.Value) });
			}
			if (options.IdentifyRequest != null)
			{
				nativeMpOptions.Call<AndroidJavaObject>("identify", new object[]{ ConvertToMpIdentifyRequest(options.IdentifyRequest) });
			}
			if (options.DevicePerformanceMetricsDisabled.HasValue)
			{
				nativeMpOptions.Call<AndroidJavaObject>("devicePerformanceMetricsDisabled", new object[]{ options.DevicePerformanceMetricsDisabled });
			}
			if (options.UploadInterval.HasValue)
			{
				nativeMpOptions.Call<AndroidJavaObject>("uploadInterval", new object[]{ options.UploadInterval });
			}
			if (options.SessionTimeout.HasValue)
			{
				nativeMpOptions.Call<AndroidJavaObject>("sessionTimeout", new object[]{ options.SessionTimeout });
			}
			if (options.UnCaughtExceptionLogging.HasValue)
			{
				nativeMpOptions.Call<AndroidJavaObject>("enableUncaughtExceptionLogging", new object[] { options.UnCaughtExceptionLogging });
			}
			if (options.LogLevel.HasValue)
			{
				nativeMpOptions.Call<AndroidJavaObject>("logLevel", new object[]{ ConvertToMpLogLevel(options.LogLevel.Value) });
			}
			if (options.LocationTracking != null)
			{
				if (options.LocationTracking.Enabled)
				{
					nativeMpOptions.Call<AndroidJavaObject>("locationTrackingEnabled", new object[]
						{
							options.LocationTracking.Provider,
							options.LocationTracking.MinTime,
							options.LocationTracking.MinDistance
						});
				}
				else
				{
					nativeMpOptions.Call<AndroidJavaObject>("locationTrackingDisabled", new object[]{ });
				}
			}
			if (options.PushRegistration != null && options.PushRegistration.AndroidInstanceId != null && options.PushRegistration.AndroidSenderId != null)
			{
				nativeMpOptions.Call<AndroidJavaObject>("pushRegistration", new object[]{ options.PushRegistration.AndroidInstanceId, options.PushRegistration.AndroidSenderId });
			}
			return nativeMpOptions.Call<AndroidJavaObject>("build");
		}
	}
}