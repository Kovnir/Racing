# Racing Game #

## How to Start ##

To Start using Racing Game Unity project open `Preloader` scene and press `Run`.

Hint: You can find all useful scenes in the top menu `Racing -> Scenes`.

Unity Version 2019.1.0f2

The Project using [Zenject](https://github.com/modesttree/Zenject) for dependency injections and all game architecture was built around it.

Music from [Ultimate Game Music Collection](https://assetstore.unity.com/packages/audio/music/orchestral/ultimate-game-music-collection-37351).

Tweener - [FastTweener](https://assetstore.unity.com/packages/tools/animation/fasttweener-142403).

JSON - [Json .NET fro Unity](https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347).

Also was used a lot of free models assets and sounds. Two shaders were taken from my other project.

The game consists of modules (player profile, preloader, main menu, etc). You can find each module in `Scripts/Modules` folder.

<img src="/Documentation/race.gif" width="600px">

## Explanation ##

### Car ###

Car physics made with `Wheel Colliders`. Standard Unit physics made 50% of work. Car controller code you can find in `CarController.cs` class.

You can control the vehicle using `WASD` or `screen buttons` for mobile. The car goes into a skid, so don't push boost to match during turning. Car physics can be changed if tune `Wheel Colliders`, `Rigidbody`, and `Physic Materials` parameters.


### Camera ###

Here are 3 types of camera you can choose in `Esc Menu`:
* Simple Camera - simple smooth fallowing camera made with code. 
* Cinemachine Camera - camera made with cinemachine almost without code.
* Following Camera - Simple camera, but it will always try to keep in screen two next checkpoints.

<img src="/Documentation/esc-menu.gif" width="600px">

### Signals ###

The Game use signals to inform modules about some events without creating dependencies.

All signals are self-described:
* OnCameraModeChangedSignal
* OnCheckpointAchievedSignal
* OnFinishAchievedSignal
* OnLevelFailedSignal
* OnLevelFinishedSignal
* OnLoseCheckpointSignal
* OnPostProcessingSettingsChangedSignal
* OnRaceStartSignal
* OnStarFailedSignal
* OnTakeCheckpointSignal

### Mobile Input ###

Mobile input was made with Unity's standard `Cross Platform Input`.

## Game Mechanics ##

* When you turn the car upside down - you `lose`.
* When your time is up - you `lose`.
* When you take all checkpoint and cross finish line - you `win`.
* If you do it fast you will take `more stars`.
* When your path the level, you can play the next one.

### Ghost-Rider ###

When you pass the level with more start than early, your ghost will be recorded. Then, when you open the level, the ghost will ride with you!

Game take position and rotation value every 0.5 seconds. It's enough and looks great even without interpolation!

<img src="/Documentation/road.gif" width="600px">

### Surfaces ###

There is a 3 types of surfaces:

| Type  | Acceleration | Turns | Slowdown | Drift |
| ------------- | ------------- | ----- | ----- | ----- |
| `Asphalt`  | hight  | hight | low | meduim |
| `Grass`  | medium  | low | medium | low |
| `Ice` | low  | low | low | hight |

When the car rides through the grass it generate particles and tire tracks.

### Levels ###

#### Level 1 ####

You should collect 6 checkpoint and rich the finish. Ease!

#### Level 2 ####

You should collect 6 checkpoint and rich the finish, but there is a lot of garbage on the road!

#### Level 3 ####

You should collect 6 checkpoint and rich the finish, but a big part of the road contains from ice!



## How to make your level ##

* Create a new scene;
* Go to `Racing -> Settings`.
* Press `Add LevelContext Prefab to Current Scene` button;
* Next, You need to just create all environment with colliders. No addition knowledge needed!
* For ice and grass you can use prefabs `Ice` and `Grass` from `Prefabs` folder;
* For road use items from `Prefabs/Road` folder;
* You need to place start point and finish point. You can find it in `Prefabs` folder;
* You need to set `Checkpoint`. You can find it in `Prefabs` folder. Copy it to your scene and place wherever you want. When you have done, set `Index` values for each checkpoint;
* Go to `Racing -> Settings`, select `Levels` tab and push `Create New Level` button;
* Setup time values for each star and set level name;
* Next, you need to create a button in the main menu and setup level num.
* Done!

<img src="/Documentation/editor.gif" width="600px">


## What should be redo ##

* Player Profile should be saved to Persistent Data instead of Player Prefs;
* Player Profile should be encrypted;
* Car physic can be better;
* Hud Manager should be divided into Hud Manager and Timer/Start Manager. This class has do too lot of responsibilities;
* Assets can be not optimal;
* BUG: Tires trails have visual artifacts;
* Almost all assets used Standart shader, it's very bad for performance;
* Post processing can be not optimal;
* Loader module should show loading screen and work asynchronously;
* Hide mobile Input when it not needed;
* It was tested only on MAC Pro 2017.
