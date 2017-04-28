#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections.Generic;
namespace mParticle
{
	public sealed class MParticleAndroid : IMParticleSDK
	{
		private AndroidJavaObject mp;

		AndroidJavaClass identityTypeClass = new AndroidJavaClass ("com.mparticle.MParticle$IdentityType");
		AndroidJavaClass eventTypeClass = new AndroidJavaClass ("com.mparticle.MParticle$EventType");
		AndroidJavaClass environmentClass = new AndroidJavaClass ("com.mparticle.MParticle$Environment");
		AndroidJavaClass integerClass = new AndroidJavaClass ("java.lang.Integer");
		AndroidJavaClass doubleClass = new AndroidJavaClass ("java.lang.Double");

		public MParticleAndroid ()
		{

		}

		public void Initialize (string apiKey, string apiSecret)
		{
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
			new AndroidJavaClass ("com.mparticle.MParticle").CallStatic ("start", 
				new object[] { jc.GetStatic<AndroidJavaObject>("currentActivity"), apiKey, apiSecret });
			mp = new AndroidJavaClass ("com.mparticle.MParticle").
				CallStatic<AndroidJavaObject> ("getInstance");
		}

		public Environment GetEnvironment ()
		{
			return ConvertToCSharpEnvironment (mp.Call<AndroidJavaObject> (
				"getOptOut"
			));
		}

		public void SetEnvironment (Environment environment)
		{
			mp.Call (
				"setEnvironment", 
				new object[] { ConvertToMpEnvironment (environment) }
			);
		}

		public void SetOptOut (bool optOut)
		{
			mp.Call (
				"setOptOut",
				ConvertToJavaBoolean (optOut)
			);
		}

		public void LogCommerceEvent (CommerceEvent commerceEvent)
		{
			AndroidJavaObject builder;
			if (commerceEvent.ProductAction > 0 && commerceEvent.Products != null && commerceEvent.Products.Length > 0)
			{
				builder = new AndroidJavaObject("com.mparticle.commerce.CommerceEvent$Builder", ConvertToMpProductAction(commerceEvent.ProductAction), ConvertToMpProduct(commerceEvent.Products[0]));
			}
			else if (commerceEvent.Promotions != null && commerceEvent.Promotions.Length > 0)
			{
				builder = new AndroidJavaObject("com.mparticle.commerce.CommerceEvent$Builder", ConvertToMpPromotionAction(commerceEvent.PromotionAction), ConvertToMpPromotion(commerceEvent.Promotions[0]));
			}
			else 
			{
				builder = new AndroidJavaObject("com.mparticle.commerce.CommerceEvent$Builder", ConvertToMpImpression(commerceEvent.Impressions[0]));
			}
			builder.Call<AndroidJavaObject>("transactionAttributes", ConvertToMpTransactionAttributes(commerceEvent.TransactionAttributes));
			builder.Call<AndroidJavaObject>("screen", commerceEvent.ScreenName);
			builder.Call<AndroidJavaObject>("currency", commerceEvent.Currency);
			builder.Call<AndroidJavaObject>("customAttributes", ConvertDictToMap(commerceEvent.CustomAttributes));
			builder.Call<AndroidJavaObject>("checkoutOptions", commerceEvent.CheckoutOptions);
			if (commerceEvent.CheckoutStep != null) 
			{
				builder.Call<AndroidJavaObject>("checkoutStep", integerClass.CallStatic<AndroidJavaObject> ("valueOf", commerceEvent.CheckoutStep));
			}
			if ( commerceEvent.NonInteractive.HasValue) 
			{
				builder.Call<AndroidJavaObject>("nonInteraction", (bool)commerceEvent.NonInteractive);
			}

			if (commerceEvent.Products != null) 
			{
				AndroidJavaObject productList = new AndroidJavaObject ("java.util.ArrayList");

				foreach (Product product in commerceEvent.Products) {
					productList.Call<bool> (
						"add", 
						new object[]{ ConvertToMpProduct(product)}
					);
				}
				builder.Call<AndroidJavaObject>("products", productList);
			}

			if (commerceEvent.Promotions != null) 
			{
				AndroidJavaObject promotionList = new AndroidJavaObject ("java.util.ArrayList");

				foreach (Promotion promotion in commerceEvent.Promotions) {
					promotionList.Call<bool> (
						"add", 
						new object[]{ ConvertToMpPromotion(promotion)}
					);
				}
				builder.Call<AndroidJavaObject>("promotions", promotionList);
			}

			if (commerceEvent.Impressions != null) 
			{
				AndroidJavaObject impressionList = new AndroidJavaObject ("java.util.ArrayList");

				foreach (Impression impression in commerceEvent.Impressions) {
					impressionList.Call<bool> (
						"add", 
						new object[]{ ConvertToMpImpression(impression)}
					);
				}
				builder.Call<AndroidJavaObject>("impressions", impressionList);
			}
			AndroidJavaObject javaCommerceEvent = builder.Call<AndroidJavaObject>("build");
			mp.Call (
				"logEvent", 
				new object[] { javaCommerceEvent }
			);
		}

		public void SetUserAttributeArray (string key, string[] values)
		{
			AndroidJavaObject valueList = new AndroidJavaObject ("java.util.ArrayList");

			foreach (string value in values) {
				valueList.Call<bool> (
					"add", 
					new object[]{ value }
				);
			}
			mp.Call<bool> (
				"setUserAttributeList", 
				new object[] { key, valueList }
			);
		}

		public void RemoveUserAttribute (string key)
		{
			mp.Call<bool> (
				"removeUserAttribute", 
				new object[] { key }
			);
		}

		public int IncrementUserAttribute (string key, int value)
		{
			mp.Call<bool> (
				"incrementUserAttribute", 
				new object[] { key, value }
			);
			return 0;
		}

		public void Logout ()
		{
			mp.Call (
				"logout"
			);
		}


		public void LeaveBreadcrumb (string breadcrumb, Dictionary<string,string> eventData)
		{
			mp.Call (
				"leaveBreadcrumb", 
				new object[] { breadcrumb }
			);
		}

		public void SetUserAttribute (string key, string value)
		{
			mp.Call<Boolean> (
				"setUserAttribute", 
				new object[] { key, (object)value }
			);
		}

		public void SetUserIdentity (string id, UserIdentity type)
		{
			mp.Call (
				"setUserIdentity", 
				new object[] { id, ConvertToMpUserIdentity (type) }
			);
		}

		public void SetUserTag (string tag)
		{
			mp.Call<Boolean> (
				"setUserTag", 
				tag
			);
		}

		public void LogEvent (string name, EventType eventType, Dictionary<string, string> eventData, long eventLength)
		{
			mp.Call (
				"logEvent", 
				new object[] { name, ConvertToMpEventType (eventType), ConvertDictToMap (eventData), eventLength }
			);
		}

		public void LogEvent (string name, EventType eventType, Dictionary<string, string> eventData)
		{
			mp.Call (
				"logEvent", 
				new object[] { name, ConvertToMpEventType (eventType), ConvertDictToMap (eventData) }
			);
		}

		public void LogEvent (string name, EventType eventType, long eventLength)
		{
			mp.Call (
				"logEvent", 
				new object[] { name, ConvertToMpEventType (eventType), eventLength }
			);
		}

		public void LogEvent (string name, EventType eventType, Dictionary<string, string> eventData, long eventLength, string category)
		{
			mp.Call (
				"logEvent", 
				new object[] { name, ConvertToMpEventType (eventType), ConvertDictToMap (eventData), eventLength, category }
			);
		}

		public void LogScreen (string screenName, Dictionary<string, string> eventData)
		{
			mp.Call (
				"logScreen", 
				new object[] { screenName, ConvertDictToMap (eventData) }
			);
		}

		//Utility methods
		private AndroidJavaObject ConvertDictToMap (Dictionary<string, string> data)
		{
			AndroidJavaObject map = new AndroidJavaObject ("java.util.HashMap");
			if (data != null) {
				foreach (KeyValuePair<string, string> entry in data) {
					map.Call<string> (
						"put", 
						new string[]{ entry.Key, entry.Value }
					);
				}
			}
			return map;
		}

		private AndroidJavaObject ConvertToMpEventType (EventType eventType)
		{
			return eventTypeClass.CallStatic<AndroidJavaObject> ("valueOf", eventType.ToString ());
		}

		private AndroidJavaObject ConvertToMpUserIdentity (UserIdentity userIdentity)
		{
			return identityTypeClass.CallStatic<AndroidJavaObject> ("valueOf", userIdentity.ToString ());
		}

		private AndroidJavaObject ConvertToMpEnvironment (Environment environment)
		{
			return environmentClass.CallStatic<AndroidJavaObject> ("valueOf", environment.ToString ());
		}

		private string ConvertToMpProductAction (ProductAction action)
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
			default:
				return null;
			}
		}

		private string ConvertToMpPromotionAction (PromotionAction action)
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

		private AndroidJavaObject ConvertToMpProduct (Product product)
		{
			AndroidJavaObject builder = new AndroidJavaObject ("com.mparticle.commerce.Product$Builder", product.Name, product.Sku, product.Price);
			builder.Call<AndroidJavaObject>("customAttributes", ConvertDictToMap(product.customAttributes));
			builder.Call<AndroidJavaObject>("category", product.Category);
			builder.Call<AndroidJavaObject>("couponCode", product.CouponCode);
			if (product.Position.HasValue) 
			{
				AndroidJavaObject javaInteger = integerClass.CallStatic<AndroidJavaObject> ("valueOf", product.Position);
				builder.Call<AndroidJavaObject>("position", javaInteger);
			}

			builder.Call<AndroidJavaObject>("quantity", product.Quantity);
			builder.Call<AndroidJavaObject>("brand", product.Brand);
			builder.Call<AndroidJavaObject>("variant", product.Variant);
			return builder.Call<AndroidJavaObject>("build");
		}

		private AndroidJavaObject ConvertToMpPromotion (Promotion promotion)
		{
			AndroidJavaObject javaPromotion = new AndroidJavaObject ("com.mparticle.commerce.Promotion");
			javaPromotion.Call<AndroidJavaObject>("setCreative", promotion.Creative);
			javaPromotion.Call<AndroidJavaObject>("setId", promotion.Id);
			javaPromotion.Call<AndroidJavaObject>("setName", promotion.Name);
			javaPromotion.Call<AndroidJavaObject>("setPosition", promotion.Position);
			return javaPromotion;
		}

		private AndroidJavaObject ConvertToMpImpression(Impression impression)
		{
			AndroidJavaObject javaImpression = new AndroidJavaObject ("com.mparticle.commerce.Impression", impression.ImpressionListName, null);
			if (impression.Products != null) 
			{

				foreach (Product product in impression.Products) {
					javaImpression.Call<bool> (
						"addProduct", 
						new object[]{ ConvertToMpProduct(product)}
					);
				}

			}

			return javaImpression;
		}

		private AndroidJavaObject ConvertToMpTransactionAttributes(TransactionAttributes attributes)
		{
			AndroidJavaObject javaAttributes = new AndroidJavaObject ("com.mparticle.commerce.TransactionAttributes", attributes.TransactionId);
			javaAttributes.Call<AndroidJavaObject>("setCouponCode", attributes.CouponCode);
			if (attributes.Tax.HasValue)
			{
				javaAttributes.Call<AndroidJavaObject>("setTax", doubleClass.CallStatic<AndroidJavaObject> ("valueOf", attributes.Tax));
			}

			if (attributes.Shipping.HasValue)
			{
				javaAttributes.Call<AndroidJavaObject>("setShipping", doubleClass.CallStatic<AndroidJavaObject> ("valueOf", attributes.Shipping));
			}

			if (attributes.Revenue.HasValue)
			{
				javaAttributes.Call<AndroidJavaObject>("setRevenue", doubleClass.CallStatic<AndroidJavaObject> ("valueOf", attributes.Revenue));
			}

			return javaAttributes.Call<AndroidJavaObject>("setAffiliation", attributes.Affiliation);
		}

		private AndroidJavaObject ConvertToJavaBoolean (bool value)
		{
			return new AndroidJavaObject ("java.lang.Boolean", value.ToString ());
		}

		private bool ConvertToCSharpBoolean (AndroidJavaObject value)
		{
			return Boolean.Parse (value.Call<string> ("toString"));
		}

		private Environment ConvertToCSharpEnvironment (AndroidJavaObject value)
		{
			return (Environment)Enum.Parse (typeof(Environment), value.Call<string> ("name"));
		}
	}
}
#endif