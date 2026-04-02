# CustomCostumeAddOn

[English](README.md)<br>
[日本語](README_ja-JP.md)

## 개요

이 모드를 사용하면 TEAM HORAY의 <a href="https://store.steampowered.com/app/2436940/_/">Sephiria</a>에 의상을 추가할 수 있습니다.

**MelonLoader나 BepInEx 같은 모드 로더는 필요하지 않습니다**; 이 모드는 Sephiria의 내장 모드 로딩 기능을 사용합니다.

## 설치
1. 릴리스 페이지에서 최신 `CustomCostumeAddOn-1.X.X.zip` 파일을 다운로드하여 압축을 풉니다.
2. `Program Files (x86)\Steam\steamapps\common\Sephiria` 폴더 안에 `AddOns` 폴더를 생성합니다.
3. `CustomCostume` 폴더를 `Program Files (x86)\Steam\steamapps\common\Sephiria\AddOns` 폴더로 복사합니다.

## 의상 만들기
- `StreamingAssets` 폴더 내의 `Costume` 폴더에 위치한 의상들이 로드됩니다.
- 의상은 `Metadata.json` 파일과 이미지 파일이 포함된 폴더로 구성됩니다.
- `Metadata.json`에는 의상 정보가 포함되어 있습니다. `animationData`에 사용할 이미지 파일의 이름을 지정해야 합니다.
- `costumeName`과 `costumeFlavorText`를 사용하여 의상의 이름과 설명을 작성할 수 있습니다. 번역을 계획하고 있다면, 여기에 번역 키를 작성하고 번역된 텍스트가 포함될 각 언어에 대한 번역 파일을 추가해야 합니다.
- `stats`는 의상의 상태 효과를 지정합니다. 슬래시 왼쪽에 상태 ID를, 오른쪽에 값을 입력합니다.
- `startingItems`를 사용하면 의상의 초기 아이템을 지정할 수 있습니다. 아이템 ID(숫자 값)를 입력합니다.

<a href="https://github.com/Mira090/CustomCostume">CustomCostume Mod</a>에는 릴리스에 예시 코스튬이 포함되어 있습니다.

## 참고 사항
- 이 저장소 및 기여자들은 Sephiria, TEAM HORAY 또는 관련 단체와 어떠한 관계도 없습니다.
- 이 모드를 <a href="https://github.com/Mira090/CustomCostume">CustomCostume Mod</a>와 함께 사용하지 마십시오.