# Racing Game #

## How to Start ##

To Start using Racing Game Unity project open `Preloader` scene and press `Run`.

Hint: You can find all useful scenes in the top menu `Racing -> Scenes`.

The Project using [Zenject](https://github.com/modesttree/Zenject) for dependency injections and all game architecture was built around it.

Music from [Ultimate Game Music Collection](https://assetstore.unity.com/packages/audio/music/orchestral/ultimate-game-music-collection-37351).

Tweener - [FastTweener](https://assetstore.unity.com/packages/tools/animation/fasttweener-142403).

JSON - [Json .NET fro Unity](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347).

Also was used a lot of free models assets and sounds.

The game consists of modules (player profile, preloader, main menu, etc). You can find each module in `Scripts/Modules` folder.


## Explanation ##

### Car ###

Car physics made with `Wheel Colliders`. Standard Unit physics made 50% of work. Car controller code you can find in `CarController.cs` class.

### Camera ###

Here are 3 types of camera you can choose in `Esc Menu`:
* Simple Camera - simple smooth fallowing camera made with code. 
* Cinemachine Camera - camera made with cinemachine almost without code.
* Following Camera - Simple camera, but it will always try to keep in screen two next checkpoints.

### Winning And Losing ###

* When you turn the car upside down - you lose.
* When your time is up - you lose.
* When you take all checkpoint and cross finish line - you ween.
* If you do it fast you will take more start.
* When your path the level, you can play the next one.


### Signals ###

The Game use signals to inform modules about some events without creating dependencies.

All signals are self-described:
* OnCheckpointAchievedSignal
* OnLoseCheckpointSignal
* OnRaceStartSignal
* OnStarFailedSignal

## How to make your level ##

* Create a new scene;
* Go to `Racing -> Settings`.
**Coming soon**
* Next, you need to create a button in the main menu and setup level num.
* Done!

## What should be redo ##

* Player Profile should be saved to Persistent Data instead of Player Prefs;
* Player Profile should be encrypted;
* Car physic can be better;
* Assets can be not optimal;
* BUG: Tires trails can change color sometimes;
* Almost all assets used Standart shader, it's very bad for performance;
* Post processing can be not optimal;
