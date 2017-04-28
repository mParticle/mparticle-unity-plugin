#!/bin/bash

# Copyright 2014-2015. mParticle, Inc.  All Rights Reserved.
#
# This script updates the source files in a Unity working project
#
# ./update_project.sh ~/Documents/Unity/Particles

project_path="$1"

if [[ -z $project_path ]]; then
    echo -e "\nThe project path is missing. Aborting the script."
    echo -e "Usage example: ./update_project.sh ~/Documents/Unity/Particles\n"
    exit 1
fi

plugins_dir="${project_path}/Assets/Plugins"

if [[ ! -d ${plugins_dir} ]]; then
    echo -e "Plugins directory: ${plugins_dir} is not available. Aborting the script."
    exit 1
fi

ios_dir="${plugins_dir}/iOS"
android_dir="${plugins_dir}/Android"

cp -f -v "buildscript.sh" "${plugins_dir}/"
cp -f -v "MParticle.cs" "${plugins_dir}/"
cp -f -v "MPEvent.cs" "${plugins_dir}/"
cp -f -v "MPProduct.cs" "${plugins_dir}/"

cp -f -v "iOS/MParticleiOS.cs" "${ios_dir}/"
cp -f -v "iOS/mParticleUnity.h" "${ios_dir}/"
cp -f -v "iOS/mParticleUnity.m" "${ios_dir}/"
cp -f -v "iOS/MParticleUnityException.h" "${ios_dir}/"
cp -f -v "iOS/MParticleUnityException.m" "${ios_dir}/"

cp -f -v "Android/MParticleAndroid.cs" "${android_dir}/"
cp -f -v "Android/AndroidManifest.xml" "${android_dir}/"
cp -f -v "Android/res/values/mparticle.xml" "${android_dir}/res/values/"

echo -e "\n### Ended updating the source files for project: ${project_path} ###\n"
