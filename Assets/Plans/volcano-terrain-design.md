# Project Overview
- **Game Title**: ASCEND
- **High-Level Concept**: An epic, vertical FPS game where players scale a massive active volcano, clear enemy fortifications in the lower levels, survive an inventory-stripping hazard in the mid-levels, defeat an ancient dragon boss at the crater summit, retrieve a relic, and escape down a high-speed sliding slide dodging hazards.
- **Players**: Single-player
- **Inspiration / Reference Games**: Doom (verticality & fast combat), Temple Run / Tribes (sliding/skiing physics), Journey (scaling a massive landmark)
- **Tone / Art Direction**: Intense, atmospheric, stylized volcanic environment (low-poly aesthetics matching the existing FPS Microgame style).
- **Target Platform**: WebGL
- **Screen Orientation / Resolution**: Landscape 1920x1080
- **Render Pipeline**: Built-in / Default Render Pipeline (using the active `Default_PipelineAsset` settings)

---

# Game Mechanics

## Core Gameplay Loop
1. **Ascend & Fight (Zone A - 0m to 200m)**: Standard FPS combat. Players navigate a winding spiral pathway up the volcano's base, neutralizing enemy turrets and hoverbots while avoiding lava rivers.
2. **Scavenge & Survive (Zone B - 200m to 400m)**: Stealth and resourcefulness. Entering Zone B triggers a volcanic electromagnetic pulse (EMP) that strips the player of all weapons. Players must sneak past patrol routes in ruined forts and scavenge for newly placed weapons to survive.
3. **Boss Encounter (Zone C - 400m to 600m)**: Climactic showdown. Defeat the massive Dragon Boss guarding the crater rim, retrieve the sacred Relic, and trigger an immediate eruption escape phase.
4. **Slide & Escape (Downward Slide - 600m to 0m)**: High-speed sliding phase. Bypassing normal walking, players slide down a steep volcanic slide, steering around obstacles and lava pits to reach the escape point.

## Controls and Input Methods
- **Walking / Exploration (Zones A & B)**: Standard keyboard and mouse controls (WASD to move, Mouse to aim/shoot, Space to jump, C to crouch).
- **Stealth and Scavenging (Zone B)**: Slower, crouched movement (C) to avoid detection zones, interaction with pickups automatically arms weapons.
- **Sliding Phase (Post-Boss Slide)**:
  - **Automatic Forward Velocity**: Player accelerates downhill along the slide path.
  - **Steering**: A / D or Mouse Horizontal to drift left and right to dodge obstacles.
  - **Jumping**: Space to leap over lava gaps.

---

# UI
- **Zone Entry Banners**: Large, stylized text overlays appearing on screen when entering new zones (e.g., "ZONE A: GET OVER IT", "ZONE B: LOST IT ALL!", "ZONE C: SURVIVE & RECOVER", "ERUPTION! ESCAPE THE VOLCANO!").
- **Objective Tracker / Compass**: The standard HUD compass pointing players to the next major objective waypoint (e.g., Mountain Pass, Ruin Cache, Dragon Arena, Escape Chute).
- **Inventory/Weapon HUD**: Updates dynamically to show stripped inventory in Zone B and newly scavenged weapons.

---

# Key Asset & Context

## 1. Terrain Design (2000m x 2000m)
- **Base Dimensions**: 2000m width, 2000m length, 600m max height.
- **Conical Volcano Structure**: Centered on the map with a large hollow crater at the top (600m).
- **Ascending Spiral Paths**:
  - **Main Path**: A winding dirt/rock road wrapping around the volcano 1.5 times.
  - **Slide Path**: A dedicated, smooth, concave "slide chute" carved down the opposite side of the mountain from the peak back to the base.
- **Lava Hazards**: `DamageArea` triggers with red emissive materials filling the base canyons and the summit crater lake.

## 2. Key Code Scripts
- **`ZoneDisarmTrigger.cs`**:
  - Attached to a checkpoint volume at the Zone B boundary.
  - Strips all weapon assets from `PlayerWeaponsManager` and triggers an alarm notification.
- **`PlayerSlideController.cs`**:
  - Handles the sliding movement state. Disables default WASD forward walking, applies constant forward downhill force, and allows mouse/keyboard steering.
- **`DragonBossController.cs`**:
  - Attached to a scaled-up, modified HoverBot template. Handles boss phases, increased health bar, and rapid fire-projectile patterns.
- **`LavaHazard.cs`**:
  - Deals periodic damage to the player if they stand in lava pools or slide into lava flows.

## 3. Key Prefabs & Checkpoints
- `ObjectivePickupItem_Relic`: The sacred relic held in the boss arena.
- `Pickup_Blaster`, `Pickup_Shotgun`, `Pickup_Launcher`: Placed as scavenge points in Zone B.
- `Enemy_HoverBot`, `Enemy_Turret`: Set up as outposts in Zone A and patrol routes in Zone B.
- `SlideObstacle_Spikes`, `SlideObstacle_Log`: Large colliders placed on the escape slide.

---

# Implementation Steps

### Step 1: Terrain Creation and Scene Reset
- **Description**: 
  - Clear unnecessary geometry from `MainScene` while retaining managers, the player controller, and UI systems.
  - Create a new 2000m x 2000m Unity Terrain. Sculpt a majestic volcano with a hollow crater at its summit.
  - Carve out the winding ascending path on the front side and the steep sliding slide on the back side.
  - Set up lighting, skybox, and basic lava materials (using emissive red shaders).
  - Generate/Bake a static NavMesh for the entire mountain.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: No

### Step 2: Zone A ("Get Over It") Setup
- **Description**: 
  - Divide the path from altitude 0m to 200m into 3 fortified sectors.
  - Place barricades, outposts, and multiple groups of `Enemy_Turret` and `Enemy_HoverBot` units.
  - Configure the first objective: "Climb the Volcano and Reach the Outpost Ruins".
  - Add lava rivers along the canyon edges using `DamageArea` components.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

### Step 3: Zone B ("Lost It All") Stripped Mechanics & Ruined Fortress
- **Description**: 
  - Build a ruin fortress at altitude 200m to 400m with patrol routes.
  - Implement the `ZoneDisarmTrigger.cs` script. Place a wide box trigger spanning the mountain path at 200m altitude. Upon trigger, strip weapons from `PlayerWeaponsManager` and display a custom "EMP Volcanic Pulse: Weapons Destroyed!" message.
  - Place weapon pickups (`Pickup_Blaster`, `Pickup_Shotgun`) in hidden alcoves of the ruins.
  - Configure stealth patrols of HoverBots with visible detection cones/indicators so the unarmed player must sneak or search quickly.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

### Step 4: Zone C ("Survive & Recover") Dragon Boss Summit
- **Description**: 
  - Design the volcano's crater summit (400m to 600m altitude) with a glowing lava lake and a large central platform.
  - Set up the Dragon Boss using a scaled-up `Enemy_HoverBot` prefab with custom high health, custom red projectiles, and a dedicated boss UI health bar.
  - Place the Relic prefab inside the crater, configured as an `ObjectivePickupItem` which unlocks once the Dragon is destroyed.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

### Step 5: Sliding System & Escape Sequence
- **Description**: 
  - Implement `PlayerSlideController.cs` to override standard walking controls when the relic is collected.
  - Program automatic forward acceleration along the slide slide, supporting left/right keyboard/mouse drifting.
  - Place obstacles (falling rocks, tree trunks, lava spikes) along the slide path.
  - Configure collision logic: hitting obstacles deals significant damage to the player.
- **Assigned role**: developer
- **Dependencies**: Step 1, Step 4
- **Parallelizable**: No

### Step 6: Objectives, UI Flow, and Polish
- **Description**: 
  - Configure sequential objectives using `ObjectiveReachPoint` and `ObjectivePickupItem`.
  - Add zone notifications on-screen when entering each zone.
  - Set up the "Escape the Eruption!" UI warning and countdown timer during the sliding sequence.
  - Place the final "Rescue Helicraft" escape point at the bottom of the slide to trigger the game-win sequence via `GameFlowManager`.
- **Assigned role**: developer
- **Dependencies**: Steps 2, 3, 4, 5
- **Parallelizable**: No

### Step 7: Tuning & Balancing
- **Description**: 
  - Playtest the full 15-minute loop.
  - Balance enemy counts and placements to ensure smooth progression.
  - Tune slide physics (sliding speed, drift responsiveness, jump height over gaps) to make the escape sequence feel fast and responsive.
- **Assigned role**: developer
- **Dependencies**: Step 6
- **Parallelizable**: No

---

# Verification & Testing

## 1. Automated & Console Logging Checks
- Verify no compilation errors or assembly mismatches.
- Ensure that entering the Zone B trigger logs: `"Player disarmed successfully. Weapons inventory cleared."`
- Verify that pickup events for scavenged weapons correctly re-initialize the Weapon HUD.

## 2. Manual Playtest Verification Cases
- **Zone A Check**: Ensure enemies navigate the terrain paths correctly and don't fall off the mountain.
- **Zone B Check**: Verify the player loses weapons *only* upon crossing the threshold, and that scavenged weapons can be fired normally. Check that if the player dies in Zone B, they restart with correct weapon states.
- **Boss Arena Check**: Verify the boss cannot move outside the platform and fires fireballs within acceptable ranges.
- **Slide Check**: Verify that when the Relic is grabbed:
  - Movement switches to automatic slide mode.
  - Left/right keys steer the player.
  - Collision with obstacles reduces health.
  - Reaching the bottom triggers the victory screen seamlessly.
