#!/bin/bash

project_dir=${PWD}

/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -projectPath "${PWD}" \
    -exportPackage Assets/mParticle/MParticle.cs \
                   Assets/mParticle/MParticleAndroid.cs \
                   Assets/mParticle/MParticleiOS.cs \
                   Assets/Plugins/Android/mParticleAndroidCore.aar \
                   Assets/Plugins/iOS/MParticleUnity.h \
                   Assets/Plugins/iOS/MParticleUnity.m \
                   Assets/Editor/PostprocessBuildPlayer_mParticle \
    mParticle.unitypackage -batchmode -quit