# AmazingGraceJester
Lethal Company mod that replaces the jester's wind up sound with a creepy old recording of Amazing Grace

## Audio 
The audio of Amazing Grace was inspired by version in the Mandela Catalogue OST. However, the version in the OST was too short for this version. So I ended up re-creating a full version of the OST, based off of [this](https://www.youtube.com/watch?v=0OcsVolWYNI) recording of Amazing Grace sung by the 'Original Sacred Harp Choir', in 1922.

You can find this full version of the edited Amazing Grace track [here](https://github.com/ShimmyMySherbet/AmazingGraceJester/raw/master/AmazingGraceJester/Assets/AmazingGraceEffect.ogg).

It's just a version of Amazing Grace from 1922, but with some compression, a low pass filter, tone shift, slow, and a couple other minor effects that add to it's crepyness. The goal was to re-create the version from the Mandela Catalogue OST.

## How it works
The recording of Amazing Grace used by this mod is embedded in the mod itself, and on load is copied to the BepInEx cache folder.

From there, it can be loaded into an AudioClip using Unity's UnityWebRequest methods. This avoids having to use a third-party library such as NLayer to load the audio clip from memory.

Then, whenever a jester spawns, it replaces the AudioClip for it's wind-up with the AmazingGrace audio.
