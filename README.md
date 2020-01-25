<p align="center">
  <img src="https://i.imgur.com/23N5qOC.png" alt="Undermod">
</p>

#### **[Download on Nexus Mods](https://www.nexusmods.com/undermine/mods/1) | [Join us on Discord](https://discord.gg/adCeFQK) | [Support us on Patreon](https://www.patreon.com/join/bwdy)**

***

#### UnderMod is a mod loader and API for UnderMod. This first version provides a mechanism to load your mod DLLs into the game, enables Harmony patching, provides a console window for the game to assist with debugging, and a minimal API to interface with the game cleanly.

#### Note for Mod Developers: You'll want to add a reference to UnderMod.dll (after installing the patch, this will be located in /UnderMine_Data/Managed/UnderMod.dll). You might also want a reference to UnderMine.dll to access the Thor namespace. Browse the UnderModAPI namespace for features. You'll need to extend UnderModAPI.Mod class, and override the OnEntry method to kick things off. More functionality, documentation, and samples coming soon.


# Features

* **Version 1.0.0.0 of UnderMod supports UnderMine 0.4.1.5 (Win64), Steam version. Other platforms coming soon.**
* Patches the game to allow mod loading and Harmony support, via the UnderMod Launcher.
* You can still play without mods by running the game directly from Steam (use the launcher, included, to run the game with mods.)
* Make your own mods using the API (documentation in progress).

***

Code is licensed as MIT unless otherwise indicated. Non-code content and add-on assets are considered the property of their creators or copyright holders, and are not included under the MIT license. You can read the MIT license text here: 
[LICENSE.MD](LICENSE.MD)