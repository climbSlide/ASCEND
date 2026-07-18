This technical overview describes the architecture and systems of the Unity FPS Microgame, a template designed for high-performance first-person shooter experiences in Unity.

## 1. Project Description
This project is a high-octane First-Person Shooter (FPS) built on Unity's Universal Render Pipeline (URP). It serves as a foundational framework for creators to build mission-based shooters. The experience focuses on tight character movement, projectile-based combat, and objective-driven gameplay within stylized environments. Core pillars include responsive player control, a modular AI system, and a flexible event-driven architecture for gameplay logic.

## 2. Gameplay Flow / User Loop
The experience follows a standard mission loop:
1.  **Boot & Menu**: The game starts in `IntroMenu.unity`, providing options to start the game or adjust settings via `MenuNavigation`.
2.  **Mission Start**: Loading `MainScene.unity` initializes the `GameFlowManager`, `ObjectiveManager`, and `ActorsManager`.
3.  **Active Gameplay**: The player moves through the environment, engaging enemies and completing objectives (e.g., `ObjectiveKillEnemies`, `ObjectiveReachPoint`).
4.  **State Transitions**: 
    *   **Winning**: Triggered when all required `Objective` components report completion to the `ObjectiveManager`. The `GameFlowManager` then transitions the user to `WinScene.unity`.
    *   **Losing**: Triggered by `PlayerDeathEvent`. The `GameFlowManager` handles the screen fade and transition to `LoseScene.unity`.
5.  **Shutdown/Restart**: Users can restart the mission from the Win/Lose scenes or return to the main menu.

## 3. Architecture
The project utilizes a decoupled, event-driven architecture centered around a static `EventManager` to handle communication between disparate systems.

*   **Event System**: A `GameEvent` based system allows components to broadcast and listen for specific occurrences (e.g., `EnemyKillEvent`, `DamageEvent`) without direct references.
*   **Singleton/Manager Pattern**: Key gameplay-wide logic is handled by managers typically located in the `FPS/Scripts/Game/Managers` folder.
    *   `GameFlowManager`: Manages win/loss states and scene transitions.
    *   `EventManager`: Provides a centralized hub for cross-system communication.
    *   `ActorsManager`: Tracks all active actors (Player and Enemies) in the scene.
*   **Component-Based Design**: Functionality is heavily modularized into reusable scripts like `Health`, `Damageable`, and `WeaponController`.

Location: `Assets/FPS/Scripts/Game`

## 4. Game Systems & Domain Concepts

### Combat & Vitality System
A two-part system handling damage distribution and life tracking.
*   `Health`: Manages current HP, invincibility, and triggers `OnDie` actions.
*   `Damageable`: A proxy component that receives hits and forwards them to a `Health` component, applying multipliers (e.g., headshots).
*   `DamageArea`: Defines zones that inflict damage over time or on entry.

Location: `Assets/FPS/Scripts/Game/Shared`

### Weapon & Projectile System
Handles the firing logic, ammo management, and projectile physics.
*   `WeaponController`: Manages fire rates, ammo ratios, reloading, and muzzle flashes. Supports Manual, Automatic, and Charge fire types.
*   `ProjectileBase`: Abstract class for all projectiles.
*   `ProjectileStandard`: Implementation for physical projectiles with gravity and impact logic.

Location: `Assets/FPS/Scripts/Game/Shared` and `Assets/FPS/Scripts/Gameplay`

### AI System
A modular AI framework using Unity's NavMesh for pathfinding and a state-based approach for behavior.
*   `EnemyController`: The central hub for AI, managing weapons, detection, and health integration.
*   `DetectionModule`: Handles line-of-sight and proximity detection for targets.
*   `EnemyMobile`: Implements state-based behavior for moving bots (Patrol, Follow, Attack).
*   `EnemyTurret`: Implements a stationary defensive behavior.

Location: `Assets/FPS/Scripts/AI`

### Objective System
Tracks mission progress through discrete, trackable goals.
*   `Objective`: Base class for all mission goals.
*   `ObjectiveManager`: Aggregates all objectives and notifies `GameFlowManager` upon completion.
*   `ObjectiveKillEnemies`: Tracks enemy counts via `EnemyKillEvent`.

Location: `Assets/FPS/Scripts/Game/Shared` and `Assets/FPS/Scripts/Gameplay/Objectives`

## 5. Scene Overview
*   **IntroMenu**: Contains the main UI, background environment, and initial game configuration.
*   **MainScene**: The primary gameplay arena containing the `Player` prefab, `Enemy` placements, and `Objective` triggers.
*   **WinScene / LoseScene**: Minimalistic scenes used for endgame feedback and navigation back to the loop start.
*   **SecondaryScene**: An additional level for demonstrating scene transitions.

## 6. UI System
Built using standard Unity UI (uGUI), the system is divided into HUD elements and Menu screens.
*   **HUD**: Managed by individual controllers like `PlayerHealthBar`, `AmmoCounter`, and `Compass`.
*   **Objective UI**: `ObjectiveHUDManager` dynamically creates `ObjectiveToast` elements when new objectives are registered.
*   **Menus**: `InGameMenuManager` handles pausing, cursor locking, and settings adjustment (Sensitivity, Volume, Shadows).
*   **Data Binding**: UI elements primarily update by subscribing to `EventManager` broadcasts or polling singleton manager states.

Location: `Assets/FPS/Scripts/UI`

## 7. Asset & Data Model
*   **Prefabs**: All game entities (Player, Enemies, Weapons, Pickups) are prefabs stored in `Assets/FPS/Prefabs`.
*   **Input**: Uses the New Input System with `InputSystem_Actions.inputactions` for cross-platform mapping.
*   **Materials & Shaders**: Stylized look achieved via URP Lit shaders and custom VFX shaders in `Assets/FPS/Art/Shaders`.
*   **Audio**: Uses an `AudioMixer` for volume control and `AudioUtility` for spawning spatialized one-shots.
*   **ScriptableObjects**: Used for parameters like `MinMaxFloat` and specialized settings (though many parameters are currently serialized on MonoBehaviours).

## 8. Notes, Caveats & Gotchas
*   **Event Cleanup**: Always use `EventManager.RemoveListener` in `OnDestroy` to prevent memory leaks and null reference exceptions in the static event bus.
*   **Ground Detection**: The `PlayerCharacterController` uses a complex `CapsuleCast` for ground detection rather than just `isGrounded` to support smooth movement on slopes.
*   **Cursor Locking**: The `InGameMenuManager` and `GameFlowManager` both manipulate `Cursor.lockState`. Ensure logic for unlocking the cursor is maintained when adding new UI layers.
*   **Actor Registration**: Enemies must be registered with the `EnemyManager` and `ActorsManager` to be tracked by objectives and UI; this is handled automatically in the `EnemyController.Start()` method.