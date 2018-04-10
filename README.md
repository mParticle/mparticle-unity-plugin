<img src="http://static.mparticle.com/sdk/logo.svg" width="280">

## Unity Plugin

This is the mParticle Unity plugin - use it to send your data to the [mParticle platform](https://www.mparticle.com/) and off to 100+ app services. The plugin exposes a native C# interface for direct use from Unity scripts, and is bundled with mParticle's native SDKs for iOS and Android. With the mParticle Unity SDK, developers can leverage mParticle's wide range of supported integrations that are otherwise unsupported by Unity.

## Plugin Setup

Download and import the plugin package to get started:

1.  Navigate to the [releases page](https://github.com/mParticle/mparticle-unity-plugin/releases), download `mParticle.unitypackage`
2.  Open an existing Unity project or create a new project
3.  Open the package directly, or import it to your project by selecting Assets -> Import Package -> Custom Package...  

### iOS Setup

`mParticle.unitypackage` contains the mParticle Apple SDK as a dynamic framework library and the required headers which will be automatically imported into your project.

You may need to add the mParticle Apple SDK as an embedded library in your xcode project build settings (under General -> Embedded Binaries). You may also need to build the "Device SDK" project from Unity (under Player Settings -> Target Device) first and then switch to "Simulator SDK" if you wish to run on the simulator.

#### Automated Xcode configuration

`mParticle.unitypackage` includes `PostprocessBuildPlayer_mParticle` that automates several Xcode project configuration steps that are required to successfully build your application for the iOS platform. During the build process, Unity will locate and execute this script.

> Starting with Unity 5.3, post-process scripts are not run automatically. In those cases, the script will be run by `mParticleBuildPostprocessor.cs` instead.

Ensure your Xcode project is in a folder called `iOS` and that the folder is in the root of your unity project folder.

#### Manual Xcode configuration

You can also configure Xcode manually by adding the required frameworks specified in the [Apple SDK's manual installation guide](https://github.com/mParticle/mparticle-apple-sdk/wiki/Manual-installation-instructions#manual-installation).

### Android Setup

`mParticle.unitypackage` contains the mParticle core AAR artifact which will be automatically imported into your project.

## Usage

### mParticle Singleton

The mParticle Unity package contains a class file named `MParticle.cs`, which is a Unity `Monobehavior` that exposes the mParticle API via `MParticle.Instance`.  The package also contains the classes `MParticleiOS` and `MParticleAndroid`. These classes are used by the mParticle singleton to interact with the native iOS and Android libraries. You should never access those classes directly from your code.

### Initialization

The plugin must be initialized with your mParticle workspace key and secret prior to use, for example by placing the following into you main script's `Awake` method:

```cs
using UnityEngine;
using mParticle;
namespace MyProject
{
    public class Example : MonoBehaviour
    {
        void Awake ()
        {
         //use the correct workspace API key and secret for iOS and Android
         #if UNITY_ANDROID
          		apiKey = "REPLACE ME ANDROID KEY";
				apiSecret = "REPLACE ME ANDROID SECRET";
         #elif UNITY_IPHONE
          		apiKey = "REPLACE ME IOS KEY";
				apiSecret = "REPLACE ME IOS SECRET";   
         #endif
        }
        
        new MParticleSDK().Initialize(new MParticleOptions()
        {
        	ApiKey = apiKey,
        	ApiSecret = apiSecret
        });
    }
}
```

#### MParticleOptions

It is now required to initialize the SDK with an MParticleOptions object. MParticleOptions' only required fields are ApiKey and ApiSecret:

```cs
	MParticle.Instance.Initialize (new MParticleOptions () {
			ApiKey = key,
			ApiSecret = secret,
			InstallType = InstallType.KnownUpgrade,
			Environment = mParticle.Environment.Development,
			DevicePerformanceMetricsDisabled = false,
			IdDisabled = false,
			UploadInterval = 650,
			SessionTimeout = 50,
			UnCaughtExceptionLogging = false,
			LogLevel = LogLevel.VERBOSE,
			LocationTracking = new LocationTracking ("GPS", 100, 350, 22),
			PushRegistration = new PushRegistration () {
				AndroidSenderId = "12345-abcdefg",
				AndroidInstanceId = "andriod-secret-instance-id",
				IOSToken = "09876654321qwerty"
			},
		});
```

##### Initial Identify Request & IdentityStateListeners
The identity API is an integral part of the mParticle platform. The SDK operates is such a way that requires a current `MParticleUser` to be present. It is explained more in the Android & iOS documentation, but the way we go about ensuring this is to make a call to the `IdentityApi.Identify()` endpoint upon initialization. By default, we use the user identities associated with the current, stored `MParticleUser`, or an empty request if an application is running for the first time. 

Using `MParticleOptions`, you have an option to override this default behavior, by registering a custom `IdentityApiRequest`.

Additionally, you may set a global OnUserIdentified delegate in `MParticleOptions`, which will be the most effective way to listen for any current `MParticleUser` changes that may take place as a result of the initial Identify request:

```cs
	new MParticleSDK().Initialize(new MParticleOptions()
        {
        	ApiKey = apiKey,
        	ApiSecret = apiSecret,
        	IdentifyRequest = new IdentityApiRequest () {
				UserIdentities = new Dictionary<UserIdentity, string> () {
					{ UserIdentity.CustomerId, "Customer ID 1" }
				},
				UserAliasHandler = ((previousUser, newUser) => newUser.SetUserAttribute("key", "value"))
			},
			IdentityStateListener = newUser => Console.WriteLine("New MParticleUser found, Mpid = " + newUser.Mpid);
        });
        
```


### Events

Events are central to many of mParticle's integrations; analytics integrations typically require events, and you can create mParticle Audiences based on the recency and frequency of different events.

#### App Events

App Events represent specific actions that a user has taken in your app. At minimum they require a name and a type, but can also be associated with a free-form dictionary of key/value pairs:

```cs
MParticle.Instance.LogEvent (new MPEvent("Hello world", EventType.Navigation) {
		Duration = 1000,
		StartTime = 123456789,
		EndTime = 1234557789,
    	Info = new Dictionary<string, string>{{ "foo", "bar" }},
    	CustomFlags = new Dictionary<string, List<string>> {
			{ "custom flag 1", new List<string> () { "one", "two", "five" } },
			{ "custom flag 2", new List<string> () { } },
			{ "custom flag 3", new List<string> () { "singleVal" } }
		}
    }
);
```
#### Commerce Events

The `CommerceEvent` is central to mParticle's eCommerce measurement. `CommerceEvents` can contain many data points but it's important to understand that there are 3 core variations:

- Product-based: Used to measure datapoints associated with one or more products
- Promotion-based: Used to measure datapoints associated with internal promotions or campaigns
- Impression-based: Used to measure interactions with impressions of products and product-listings

Here's an example of a Product-based purchase event:

```cs
Product[] products = new Product[2];
products[0] = new Product("foo name", "foo sku", 42, 2) {
	Brand = "foo brand",
	Category = "foo category",
	CouponCode = "foo coupon",
	CustomAttributes = new Dictionary<string, string>() {
		{
			"Key1",
			"Value1"
		},
		{
			"Key2",
			"Value2"
		}
}

products[1] = new Product("foo name 2", "foo sku 2", 100, 3) {
	Brand = "foo brand 2";
	Category = "foo category 2";
	CouponCode = "foo coupon 2";
}
	
TransactionAttributes transactionAttributes = new TransactionAttributes("foo transaction id") {
	Revenue = 180,
	Shipping = 10,
	Tax = 15,
	Affiliation = "foo affiliation",
	CouponCode = "foo coupon code"
};
CommerceEvent eCommEvent = new CommerceEvent (
    ProductAction.Purchase, 
    products, 
    transactionAttributes
) {
	ScreenName = "HomeScreen",
	Currency = "USD",
	CustomAttributes = new Dictionary<string, string>() {
		{"key1", "value1"}
	}
}
MParticle.Instance.LogEvent(eCommEvent);       
```
#### Screen events

```cs
MParticle.Instance.LogScreen("Test screen");
```

### Identity

Version 2 of the MParticle Unity SDK supports the full range of IDSync features. For more in depth information about MParticle's Identity features, checkout either the [Android docs](http://docs.mparticle.com/developers/sdk/android/identity/) or the [iOS docs](http://docs.mparticle.com/developers/sdk/ios/identity) on the topic


#### Accessing IdentityApi

To get a reference to the `IdentityApi`:

```cs
var identityApi = MParticle.Instance.Identity;
```

#### Updating Identities 


User identities allow you to associate specific identifiers with the current user: 

```cs
IdentityApiRequest request = new IdentityApiRequest(identityApi);
		request.UserIdentities.Add(UserIdentity.CustomerId, "customerId");
		request.UserIdentities.Remove(UserIdentity.Email);
identityApi.Modify(request);
```

or

```cs
identityApi.Modify(new IdentityApiRequest() 
                {
                    UserIdentities = new Dictionary<UserIdentity, string>()
                    {
                        { UserIdentity.CustomerId, "" }
                    }
                });
```

In addition to this, the underlying iOS and Android SDKs will automatically collect device IDs.

#### User Attributes

User attributes allow for free form description of a user for segmentation and analytics:

```cs
identityApi.CurrentUser.SetUserAttribute("foo attribute", "bar value");
```

```cs
identityApi.CurrentUser.SetUserTag("foo tag");
```

```cs
identityApi.CurrentUser.RemoveUserAttribute("foo attribute");
```

#### Identity Change Callbacks

Since the identity API calls, `Identify()`, `Login()`, `Logout()`, and `Modify()` are asynchronous, you can register a callback for the results of the request:

```cs
 identityApi.Logout()
         .AddSuccessListener(success => Console.WriteLine(success.User.UserAttributes))
         .AddFailureListener(failure => Console.WriteLine("HttpCode = " + failure.HttpCode + "/nErrors = " + failure.Errors));
```

Additionally, the identity API allows you to register a global `MParticleUser state change listener. Any time the currentUser value changes, the callback will be invoked:

```cs
OnUserIdentified onUserIdentified = newUser => Console.WriteLine("New MParticleUser found, Mpid = " + newUser.Mpid);
identityApi.Identity.AddIdentityStateListener(onUserIdentified);
```

These delegates are tracked by references, so it is wise to maintain a reference to the listener if you plan on later removing it:

```cs
identityApi.RemoveIdentityStateListener(onUserIdentified);
```

#### User Aliasing

When a new user is returned through an identity API request, you may want to transition the SDK and data from the previous user to the new user:

```cs
identityApi.Login(new IdentityApiRequest()
            {
                UserAliasHandler = (previousUser, newUser) => 
                {
                    // do some stuff, but for example:
                    var persistentAttribute = "persistent user attribute";
                    if (previousUser.UserAttributes.ContainsKey(persistentAttribute))
                    {
                        newUser.UserAttributes.Add(persistentAttribute, previousUser.UserAttributes.GetValueOrDefault(persistentAttribute));
                    }
                }
            });
```

####Identity Error Handling
The SDK will not retry Identity requests automatically. Identity Requests will return an `IdentityHttpResponse` object containing information on the error. On rare occurances you may receive and retry throttling errors:

```cs
identityApi.Login()
	.AddFailureListener(failure => {
			if (failure.HttpCode == IdentityApi.ThrottleError) {
				//retry request here
			}
			Console.WriteLine("Http Code = " + failure.HttpCode + "/nErrors = " + failure.Errors.Aggregate ("", (composit, nextError) => composit += nextError.Message + ", "));
		});
```

### License

[Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0)
