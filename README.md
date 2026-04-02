# CustomCostumeAddOn

## 概要

TEAM HORAY の <a href="https://store.steampowered.com/app/2436940/_/">Sephiria</a> にコスチュームを追加できるようにする Mod です。

MelonLoader や BepInEx などの **Mod ローダーは不要**で、Sephiria の Mod 読み込み機能を使用します。

## インストール 
1. Releases から最新の `CustomCostumeAddOn-1.X.X.zip` をダウンロードし、解凍してください。
2. `Program Files (x86)\Steam\steamapps\common\Sephiria` フォルダ内に `AddOns` フォルダを作成してください。
3. `Program Files (x86)\Steam\steamapps\common\Sephiria\AddOns` フォルダ内に `CustomCostume` フォルダをコピーしてください。

## コスチューム作成
- `StreamingAssets` フォルダの中の `Costume` フォルダ内に置いたコスチュームが読み込まれます。
- コスチュームは `Metadata.json` と画像ファイルを含むフォルダで構成されます。
- `Metadata.json` にはコスチュームの情報があります。 `animationData` で使用する画像ファイルの名前を指定する必要があります。
- `costumeName` と `costumeFlavorText` はコスチュームの名前と説明文を書くことができます。翻訳を考慮する場合、ここに翻訳キーを書き、翻訳後の文章を書いた各言語の翻訳ファイルを追加する必要があります。
- `stats` はコスチュームのステータス効果を指定できます。スラッシュ左側にステータスのID、右側に値を書きます。
- `startingItems` はコスチュームの初期アイテムを指定できます。アイテムのID（数値）を書きます。

<a href="https://github.com/Mira090/CustomCostume">Custom Costume Mod</a> の Release にコスチュームの例があります。

## 注意事項
- このリポジトリおよびその貢献者は、Sephiria、TEAM HORAY、または関連団体とは一切関係がありません 
- <a href="https://github.com/Mira090/CustomCostume">Custom Costume Mod</a> とは同時に使用しないでください。