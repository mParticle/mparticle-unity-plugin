#!/bin/bash

# Build logs are outputed to '~/Library/Logs/Unity/Editor.log'

project_dir=${PWD}

/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -projectPath "${PWD}" \
    -logFile \
    -exportPackage Assets/mParticle/MParticle.cs \
                   Assets/mParticle/MParticleAndroid.cs \
                   Assets/mParticle/MParticleiOS.cs \
                   Assets/mParticle/android/ToAndroidUtils.cs \
                   Assets/mParticle/android/ToCSUtils.cs \
                   Assets/mParticle/android/nativeproxies/AndroidOnFailureListener.cs \
                   Assets/mParticle/android/nativeproxies/AndroidOnSuccessListener.cs \
                   Assets/mParticle/android/nativeproxies/AndroidOnUserIdentified.cs \
                   Assets/mParticle/android/nativeproxies/AndroidUserAliasHandler.cs \
                   Assets/mParticle/ios/ToCSUtils.cs \
                   Assets/mParticle/ios/ToiOSUtils.cs \
                   Assets/mParticle/ios/jsonobjects/IdentityApiRequestDto.cs \
                   Assets/mParticle/ios/jsonobjects/MParticleOptionsDto.cs \
                   Assets/mParticle/ios/jsonobjects/OnTaskFailureMessage.cs \
                   Assets/mParticle/ios/jsonobjects/OnTaskSuccessMessage.cs \
                   Assets/mParticle/ios/jsonobjects/OnUserAliasMessage.cs \
                   Assets/mParticle/ios/jsonobjects/MpidDto.cs \
                   Assets/mParticle/ios/jsonobjects/MpeventDto.cs \
                   Assets/mParticle/ios/jsonobjects/PushRegistrationDto.cs \
                   Assets/mParticle/ios/jsonobjects/LocationTrackingDto.cs \
                   Assets/Plugins/Android/mparticle-core.jar \
                   Assets/Plugins/iOS/MParticleUnity.h \
                   Assets/Plugins/iOS/MParticleUnity.m \
                   Assets/Plugins/iOS/mParticle_Apple_SDK.framework \
                   Assets/Editor/PostprocessBuildPlayer_mParticle \
    mParticle.unitypackage -nographics -batchmode -quit;