<img src="http://static.mparticle.com/sdk/logo.svg" width="280">

## Unity Plugin

This is the mParticle Unity plugin - use it to send your data to the [mParticle platform](https://www.mparticle.com/) and off to 100+ app services. The plugin exposes a native C# interface for direct use from Unity scripts, and is bundled with mParticle's native SDKs for iOS and Android. With the mParticle Unity SDK, developers can leverage mParticle's wide range of supported integrations that are otherwise unsupported by Unity.

## Plugin Setup

Download and import the plugin package to get started:

1.  Navigate to the [releases page](https://github.com/mParticle/mparticle-unity-plugin/releases), download `mParticle.unitypackage`
2.  Open an existing Unity project or create a new project
3.  Open the package directly, or import it to your project by selecting Assets -> Import Package -> Custom Package...  

### iOS Setup

`mParticle.unitypackage` contains the mParticle Apple SDK as a static library and the required headers which will be automatically imported into your project.

#### Automated Xcode configuration

`mParticle.unitypackage` includes `PostprocessBuildPlayer_mParticle` that automates several Xcode project configuration steps that are required to successfully build your application for the iOS platform. During the build process, Unity will locate and execute this script.

> Starting with Unity 5.3, post-process scripts are not run automatically. In those cases, the script will be run by `mParticleBuildPostprocessor.cs` instead.

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
          MParticle.Instance.Initialize("REPLACE ME", "REPLACE ME");
         #elif UNITY_IPHONE
          MParticle.Instance.Initialize("REPLACE ME", "REPLACE ME");
         #endif
        }
    }
}
```


### Events

Events are central to many of mParticle's integrations; analytics integrations typically require events, and you can create mParticle Audiences based on the recency and frequency of different events.

#### App Events

App Events represent specific actions that a user has taken in your app. At minimum they require a name and a type, but can also be associated with a free-form dictionary of key/value pairs.

```cs
MParticle.Instance.LogEvent (
        "Hello world", 
        EventType.Navigation, 
        new Dictionary<string, string>{{ "foo", "bar" }}
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
products[0] = new Product("foo name", "foo sku", 42, 2);
products[0].Brand = "foo brand";
products[0].Category = "foo category";
products[0].CouponCode = "foo coupon";

products[1] = new Product("foo name 2", "foo sku 2", 100, 3);
products[1].Brand = "foo brand 2";
products[1].Category = "foo category 2";
products[1].CouponCode = "foo coupon 2";

TransactionAttributes transactionAttributes = new TransactionAttributes("foo transaction id");
transactionAttributes.Revenue = 180;
transactionAttributes.Shipping = 10;
transactionAttributes.Tax = 15;
transactionAttributes.Affiliation = "foo affiliation";
transactionAttributes.CouponCode = "foo coupon code";
CommerceEvent eCommEvent = new CommerceEvent (
    ProductAction.Purchase, 
    products, 
    transactionAttributes
);
MParticle.Instance.LogCommerceEvent(eCommEvent);       
```
#### Screen events

```cs
MParticle.Instance.LogScreenEvent
(
    "Test screen", 
    new Dictionary<string, string>{{ "Test key 1", "Test value 1" }}
);
```

#### User Identities

User identities allow you to associate specific identifiers with the current user: 

```cs
MParticle.Instance.SetUserIdentity("example@example.com", UserIdentity.Email);
```

In addition to this, the underlying iOS and Android SDKs will automatically collect device IDs.

#### User Attributes

User attributes allow for free form description of a user for segmentation and analytics:

```cs
MParticle.Instance.SetUserAttribute ("foo attribute", "bar value");
```

```cs
MParticle.Instance.SetUserTag("foo tag");
```

```cs
MParticle.Instance.RemoveUserAttribute("foo attribute/tag");
```

### License

[Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0)
