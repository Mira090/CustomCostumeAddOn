# CustomCostumeAddOn

[한국어](README_ko-KR.md)<br>
[日本語](README_ja-JP.md)

## Overview

This mod allows you to add costumes to TEAM HORAY's <a href="https://store.steampowered.com/app/2436940/_/">Sephiria</a>.

**Mod loaders like MelonLoader or BepInEx are not needed**; this mod uses Sephiria's built-in mod loading functionality.

## Installation
1. Download the latest `CustomCostumeAddOn-1.X.X.zip` from Releases and extract it.
2. Create an `AddOns` folder inside the `Program Files (x86)\Steam\steamapps\common\Sephiria` folder.
3. Copy the `CustomCostume` folder into the `Program Files (x86)\Steam\steamapps\common\Sephiria\AddOns` folder.

## Creating a costume
- Costumes placed in the `Costume` folder within the `StreamingAssets` folder will be loaded.
- A costume consists of a folder containing `Metadata.json` and image files.
- `Metadata.json` contains costume information. You need to specify the name of the image file to be used in `animationData`.
- `costumeName` and `costumeFlavorText` can be used to write the name and description of the costume. If translation is planned, you should write the translation key here and add translation files for each language in which the translated text will be written.
- `stats` specifies the status effects of a costume. Write the status ID to the left of the slash and the value to the right.
- `startingItems` allows you to specify the initial items for a costume. Enter the item ID (a numerical value).

<a href="https://github.com/Mira090/CustomCostume">CustomCostume Mod</a> includes an example costume in its release.

## Notes
- This repository and its contributors maintain no affiliation with Sephiria, TEAM HORAY, or any associated entities.
- Do not use this mod with <a href="https://github.com/Mira090/CustomCostume">CustomCostume Mod</a>.