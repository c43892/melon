# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview
This is a Unity game project called "melon" - a card-based battle game. The project uses Unity 2023.x with Universal Render Pipeline (URP) and includes 2D tools, Input System, and Visual Scripting packages.

## Development Commands
- **Build**: Use Unity Editor to build the project (File → Build Settings)
- **Play/Test**: Use Unity Editor's Play button to test gameplay
- **C# Solution**: Open `melon.sln` in Visual Studio or other IDE for code editing

## Architecture Overview

### Core Framework (`Assets/Scripts/Framework/`)
- **GameCore**: Main game system that inherits from ModuleContainer
- **ModuleContainer**: Dependency injection system for managing game modules
- **Module interfaces**: IModule, IInitializable, IFarmeDriven, IOnModuleAdded
- **Fixed-point math**: Fixed64 with lookup tables for trigonometric functions

### Game Systems (`Assets/Scripts/Gameplay/`)
- **Battle system**: Turn-based combat with rule-based action processing
- **Card system**: Playing cards with suit and value enums
- **Character system**: Battle characters with equipment
- **Rules/Equipment**: Modular rule system for game mechanics

### Key Classes
- `GameDriver`: MonoBehaviour that initializes and updates GameCore
- `Battle`: Core battle logic with rule runner interface  
- `BattleContext`: Context object passed between battle actions
- `Card`: Represents playing cards with suit/value
- `Rule`: Base class for game rules that modify battle actions
- `Equip`: Equipment system for character modifications

### UI (`Assets/Scripts/UI/`)
- `UIBattle`: Main battle interface
- `UICard`/`UICardsInHand`: Card display and hand management
- `UIEquip`/`UIEquipsInBattle`: Equipment UI

### Scene Management (`Assets/Scripts/Scene/`)
- Scene-specific MonoBehaviour classes (SBattleScene, SBattleChar)

## Code Conventions
- Uses C# namespaces: `Framework`, `Melon.Gameplay`
- Interface naming: Prefix with 'I' (IModule, IInitializable)
- Rule classes: Prefix with 'R' (RBasic, RSpadePlus)
- Equipment classes: Prefix with 'E' (ESpadePlus, ESpadeTimes)
- UI classes: Prefix with 'UI'
- Scene classes: Prefix with 'S'
- Enums use PascalCase values
- Public properties use PascalCase, fields may use camelCase

## Important Notes
- This is a card-based battle game with modular rule systems
- Uses Unity's new Input System package
- Framework supports module dependency injection and initialization
- Battle system uses rule-based action modification pattern