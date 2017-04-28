#!/bin/bash

# Copyright 2014-2015. mParticle, Inc.  All Rights Reserved.
#
# This script must be copied to and run from the Assets/Plugins of your Unity project.
#
# ./buildscript.sh
# or
# ./buildscript.sh <iOS SDK version> (e.g. ./buildscript.sh 2.8.0)

sdk_version="$1"
if [[ -z $sdk_version ]]; then
    echo "Please enter the iOS SDK version: "
    read sdk_version
fi

unity_sdk_repository="${HOME}/Documents/Unity/Unity-SDK"
unity_editor_repository="${HOME}/Documents/Unity/Unity-Editor"
unity_sdk_dir="${HOME}/Documents/mParticle/UnitySDK/mParticleSDK_Unity_${sdk_version}"
project_dir=${PWD%/*/*}
plugins_dir="${project_dir}/Assets/Plugins"

echo "Removing existing framework and jar"
rm -rf "${plugins_dir}/iOS/mParticle.framework"
rm -rf "${plugins_dir}"/Android/*jar

echo "Copying the lastest files from the repository to the project directories"
cp -pR "${unity_sdk_repository}/Android" "${plugins_dir}"
cp -pR "${unity_sdk_repository}/iOS" "${plugins_dir}"
cp -pR "${unity_sdk_repository}/MParticle.cs" "${plugins_dir}/"
cp -pR "${unity_sdk_repository}/MPProduct.cs" "${plugins_dir}/"

android_jar=""
for android_jar in "${plugins_dir}"/Android/*jar; do
    android_jar=`echo -e "$android_jar" | awk '{ print substr($0, index($0, "mParticle-android-unity")) }'`
done

echo "Building the Unity Package"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -projectPath "${project_dir}" \
    -exportPackage Assets/Plugins/MParticle.cs \
                   Assets/Plugins/MPProduct.cs \
                   Assets/Plugins/Android/MParticleAndroid.cs \
                   Assets/Plugins/Android/res/values/mparticle.xml \
                   Assets/Plugins/Android/${android_jar} \
                   Assets/Plugins/iOS/MParticleiOS.cs \
                   Assets/Plugins/iOS/MParticleUnity.h \
                   Assets/Plugins/iOS/MParticleUnity.m \
                   Assets/Plugins/iOS/MParticleUnityException.m \
                   Assets/Plugins/iOS/MParticleUnityException.h  \
                   Assets/Plugins/iOS/mParticle.framework \
    mParticle.unitypackage -batchmode -quit

echo -e "Copying files to their destination"
if [[ ! -d "${unity_sdk_dir}" ]]; then
    echo -e "Unity SDK directory does not exist. Package will not be moved to destination."
    exit 1
fi

cp -pR "${project_dir}/mParticle.unitypackage" "${unity_sdk_dir}/"
cp -pR "${unity_editor_repository}/PostprocessBuildPlayer" "${unity_sdk_dir}/"

echo -n "Files are ready at: "
echo -e "${unity_sdk_dir}"

echo -e "\n### End of the Unity build script ###\n"
