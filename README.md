# quake-character-controller

Quake-like Character Controller for Unity

![Demo](demo.gif)

An attempt to recreate the feel of Quake 1 with the original logic for acceleration, friction, etc.

## Features
- Input handling
- Smooth movement, rotation and jumps
- Camera rotation system
- Custom ground detection
- High customizability
- Minimalistic and lightweight

## Installation

### Option 1: Unity Package (Recommended)
Download the latest .unitypackage which includes scripts, player prefab and a demo scene.

### Option 2: Manual Installation
1. Copy all files from the `Scripts/` folder
2. Paste into your Unity project's `Assets/Scripts/` folder
3. Attach `QuakeCharacterController.cs` and `CameraRotation.cs` to your player GameObject with Unity's 'CharacterController' component already on.

## Requirements
- Legacy Input Manager (or adapt for New Input System yourself)

## License
MIT License - see LICENSE file
