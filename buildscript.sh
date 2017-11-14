#!/bin/bash

project_dir=${PWD}

/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -projectPath "${PWD}" \
    -exportPackage Assets/mParticle/MParticle.cs \
                   Assets/mParticle/MParticleAndroid.cs \
                   Assets/mParticle/MParticleiOS.cs \
                   Assets/Plugins/Android/mparticle-android-core.aar \
                   Assets/Plugins/iOS/MParticleUnity.h \
                   Assets/Plugins/iOS/MParticleUnity.m \
                   Assets/Plugins/iOS/mParticle_Apple_SDK.framework \
                   Assets/Editor/PostprocessBuildPlayer_mParticle \
    mParticle.unitypackage -nographics -batchmode -quit
