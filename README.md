<img src="http://static.mparticle.com/sdk/logo.svg" width="280">

## Unity SDK

This is the mParticle Unity client SDK - use it to send your data to the [mParticle platform](https://www.mparticle.com/) and off to 100+ app services.

## Requirements

Unity 5 or later

<a class="btn-download" href="http://static.mparticle.com/sdk/unity/mParticle_Unity_2.11.3.zip">
    DOWNLOAD THE UNITY SDK<span class="downloadimage" alt="Download SDK" /> 
</a>
{: .right }

mParticle supports Unity-based applications for iOS and Android by providing an easy-to-use C# library class bundled with our native SDKs for iOS and Android. With the mParticle Unity SDK, developers can leverage mParticle's wide range of supported integrations that are otherwise unsupported by Unity. Additionally, with mParticle's server-based architecture, your Unity game will perform better with our single lightweight SDK than it would incorporating a series of other SDKs.

We've bundled the C# source code, required libraries, and other helper files as a single Unity package file to allow for easy import into an existing or new Unity project. Our library uses Unity's platform dependent compilation to determine which SDK it should incorporate into your application. Depending on if your application supports iOS, Android, or both platforms, you can get started with the following steps:

1.  Download and extract the `mParticleUnity.zip` file
2.  Open an existing Unity project or create a new project
3.  Import mParticle.unitypackage to your project by selecting Assets -> Import Package -> Custom Package...  

#### mParticle Singleton
The mParticle Unity package contains a class file named MParticle.cs, which is a Unity `Monobehavior` and implements the singleton pattern.  This is the class you will use throughout your application to interact with the mParticle API.  The package also contains the classes `MParticleiOS` and `MParticleAndroid`. These classes are used by the mParticle singleton to interact with the native iOS and Android libraries. You should never access those classes directly from your code.

#### Unity on iOS

During the build process, Unity looks for a script named `PostprocessBuildPlayer` in your Project's `Assets/Editor` folder.  If Unity finds this script, it is invoked when the Unity Player finishes building.  We've built a `PostprocessBuildPlayer` that automates several Xcode project configuration steps that are required to successfully build your application for the iOS platform. You can find the script in the directory where you expanded `mParticleUnity.zip`.  

If you choose to use our `PostprocessBuildPlayer` script, just copy it to the `Assets/Editor` folder in your Unity project and after building your app from Unity go to step **#5** of the manual instructions below. The script will copy `mParticle.framework` to your Xcode project root directory.

If you choose to configure Xcode manually, here are the configuration instructions:  

1. Copy `mParticle.framework` from *Assets/Plugins/iOS* to your Xcode project directory
2. Add `Accounts.framework`, `CoreTelephony.framework`, `Social.framework`, and `AdSupport.framework` to your Xcode project (`AdSupport.framework` is optional and only for projects targeting iOS 6 and above)
3. Add `libsqlite3.dylib` and `libz.dylib` to your Xcode project  
4. Add the *-ObjC* linker flag in **Other Linker Flags** (Your Xcode Project > Build Settings > Linking)
5. Add `mParticle.framework` to your Xcode project
6. Import the mParticle header in **UnityAppController.mm**
7. See the Initialize the SDK section for how to start the iOS SDK, which applies equally to Unity and native iOS apps.

~~~objc
    #import <mParticle/mParticle.h>
~~~


#### Unity on Android
After the mParticle Unity Package has been imported into your project, there are only a few more steps to get going with Android!

##### 1. Setting the mParticle API Credentials

~~~html
<?xml version="1.0" encoding="utf-8" ?>
<resources>
    <string name="mp_key">APP KEY</string>
    <string name="mp_secret">APP SECRET</string>
</resources>
~~~

The mParticle SDK communicates securely over HTTPS with the mParticle platform and requires an application key and secret for authentication. The mParticle Unity Package includes an mparticle.xml configuration file that you can use to fill in these credentials. After setting up your Unity Android application in the Unity console, copy your app key and app secret into mparticle.xml, which will be located in your Unity project's `Plugins/Android/res/values` directory.


##### 2. AndroidManifest.xml


Every Android application, Unity or not, requires an AndroidManifest.xml file. Many optional features of the mParticle SDK require special permissions, additions, and modification of the AndroidManifest.xml file. *If you are already using a customized AndroidManifest.xml file, you can skip the following:* 

When Unity compiles and packages your application it places a number of temporary files, including your AndroidManifest.xml in the directory `PROJECT_DIR/Temp/StagingArea`. Build your current project and then copy this file from the StagingArea directory to your project's `Assets/Plugins/Android` directory. You now have an AndroidManifest.xml that you can safely customize moving forward.

##### 3. Adding required permissions

See the Getting Started with Android section above to add required and optional permissions to your AndroidManifest.xml file.

##### 4. Using the `MPUnityNativeActivity`

The `com.mparticle.activity.MPUnityNativeActivity` class provides a default implementation that simply starts the SDK and does the minimum to ensure that session management and screen view tracking is working correctly. There are two ways to leverage this class:  

1. If you have already customized the Activitys that your Unity application uses, simply have all of your Activitys *extend* `com.mparticle.activity.MPUnityNativeActivity`. 
2. With default Unity Project settings, the launch Activity of your application will be set to the class `com.unity3d.player.UnityPlayerNativeActivity`. Change this to `com.mparticle.activity.MPUnityNativeActivity` in the AndroidManifest.xml that you copied into `Assets/Plugins/Android`.

##### Manual Initialization

In cases where extending/subclassing `MPUnityNativeActivity` or customizing the AndroidManifest.xml file is not an option, you may also manually instrument the SDK. See the Initialize the SDK section meant native Android apps for an extended description.

<aside>From within an Activity you have full access to all the mParticle native Android SDK's Java APIs.</aside>


### Installation

```sh
```

Import the package:

```cs
```

### Usage

**Logging** events:

```cs
MParticle.Instance.LogEvent ("Test event", MParticle.EventType.Other, new Dictionary<string, string>{{ "Test key 1", "Test value 1" }});
```

**Logging** commerce events:

```cs
```

```cs
```

```cs
```

**Logging** screen events:

```cs
MParticle.Instance.LogScreenEvent("Test screen", new Dictionary<string, string>{{ "Test key 1", "Test value 1" }});
```

## User Attributes

**Setting** user attributes and tags:

```cs
MParticle.Instance.SetUserAttribute ("Test key", "Test value");
```

```cs
MParticle.Instance.SetUserAttribute(MParticle.UserAttributeType.FirstName, "Test first name");
```

```cs
```

```cs
MParticle.Instance.SetUserTag("Test key");
```

```cs
MParticle.Instance.RemoveUserAttribute("Test key");
```

## User Identities

**Setting** user identities:

```cs
MParticle.Instance.setUserIdentity("example@example.com", MParticle.UserIdentityType.Email);
```

### License

[Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0)