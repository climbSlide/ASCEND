# Project Overview
- Game Title: FPS Microgame
- High-Level Concept: A fast-paced first-person shooter where the player navigates levels to complete objectives and defeat bosses.
- Players: Single player
- Inspiration / Reference Games: Doom, Quake, classic FPS templates.
- Tone / Art Direction: Stylized, sci-fi/technological.
- Target Platform: PC / WebGL
- Screen Orientation / Resolution: Landscape 1920x1080
- Render Pipeline: URP

# Game Mechanics
## Core Gameplay Loop
The player fights through enemies to reach the boss area. The "ANCIENT_DRAGON_BOSS" serves as the final encounter in the volcano area. Upon defeat, it spawns a "Sacred Relic" required to complete the mission.
## Controls and Input Methods
- WASD for movement
- Mouse for aiming and shooting
- Event-driven objective system tracks the boss's health and death.

# UI
- The Boss Health Bar (already present in the scene) will be maintained but could be visually updated to match the "Cyber" aesthetic if needed.

# Key Asset & Context
- **New Mesh**: `CyberDragon_Mesh.fbx` - A high-tech, mechanical dragon model.
- **New Materials**: 
    - `CyberDragon_Body_Mat`: Metallic, dark grey/black base with paneling details.
    - `CyberDragon_Glow_Mat`: High-intensity emission (Neon Blue or Orange) for "cyber" circuits and eyes.
- **Animator Controller**: `CyberDragon_AnimatorController` - New animations for flying, idle, and attacking.
- **Prefab**: `ANCIENT_DRAGON_BOSS` (Modified) - Replacing the HoverBot visuals with the Cyber Dragon.

# Implementation Steps
## 1. Context Gathering & Asset Preparation
- **Description**: Verify the current boss setup and ensure all dependencies (Health, DragonBossController) are preserved.
- **Assigned role**: explorer
- **Dependencies**: None
- **Parallelizable**: Yes

## 2. Generate/Import Cyber Dragon Model
- **Description**: Create or source a "3D Cyber Dragon" model. This includes the mesh and rigging for mechanical parts (wings, head, tail).
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No

## 3. Create Cyber Materials
- **Description**: Develop URP materials for the cyber dragon. Use metallic workflows and strong emission for the "cyber" look.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: Yes

## 4. Set Up New Animator
- **Description**: Create a new Animator Controller for the dragon. If new animations are imported, wire them up for Idle, Move (Fly), and Attack (Laser/Breath).
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: Yes

## 5. Rebuild Boss Prefab
- **Description**: Replace the `BasicRobot` child object in the `ANCIENT_DRAGON_BOSS` prefab with the new Cyber Dragon mesh. Re-assign the `DragonBossController` references and ensure the `Health` component is correctly linked.
- **Assigned role**: developer
- **Dependencies**: Step 3, Step 4
- **Parallelizable**: No

## 6. Adjust Scaling and Collisions
- **Description**: The current boss is scaled by 5. Adjust the transform and the `BoxCollider`/`CapsuleCollider` on the `HitBox` child to fit the new dragon form.
- **Assigned role**: developer
- **Dependencies**: Step 5
- **Parallelizable**: Yes

# Verification & Testing
- **Visual Check**: Inspect the boss in the Scene view to ensure the "Cyber Dragon" looks as intended with correct textures and scaling.
- **Animation Check**: Enter Play Mode and verify that the dragon plays its idle and movement animations.
- **Gameplay Check**: Reduce the boss's health to zero and verify that the `OnBossDeath` event triggers, spawning the Relic correctly.
- **Console Check**: Ensure no NullReferenceExceptions occur during the transition to the new model.
