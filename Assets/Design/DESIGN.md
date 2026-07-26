# Game Design

This document specifies the design for various features in the project.

---

## Sword of Arthur (Excalibur)

This section specifies the design for the "Sword of Arthur" (Excalibur) weapon addition.

### UI Design (Sword)

- **Color system:** 
    - **Accent (Arthurian):** #FFD700 (Gold) and #00F2FF (Cyan Glow).
    - **Surface hierarchy:** Consistent with the existing FPS Microgame HUD (translucent dark panels).
- **Weapon HUD:**
    - **Weapon Icon (core):** A stylized, low-poly silhouette of a sword with a prominent cross-guard.
    - **Crosshair (core):** A custom circular crosshair with a vertical line through the center, suggesting a blade edge. The color should transition from white to Cyan (#00F2FF) when targeting an enemy.
- **Layout:**
    - Weapon slot icon should be assigned to a new slot (typically slot 3 or 4).
    - Switching via number keys (1-9) is already supported by the `PlayerInputHandler`.

### Asset Design (Sword)

- **Visual identity:** High-fantasy, regal, stylized low-poly.
- **Sword of Arthur (core):**
    - **Blade:** Silver/Steel material with a strong emissive cyan glow along the center ridge.
    - **Hilt:** Ornate gold material with a central blue gemstone.
    - **Style:** Match the existing low-poly, clean-shaded aesthetic of the FPS Microgame blaster and shotgun.
- **VFX (core):**
    - **Blade Trail:** A translucent cyan ribbon that follows the blade's path during a swing.
    - **Hit Flash:** A bright white/cyan burst at the point of impact.

### Game Feedback (Sword)

- **Genre profile:** High-Energy (Arcade FPS).
- **Interaction map:**

| Interaction | Tier | Importance | Camera | Time | Transform | Visual | Audio | Input | Rationale |
|------------|------|-----------|--------|------|-----------|--------|-------|-------|-----------|
| Sword Swing | core | Medium | Subtle roll | — | Squash/Stretch | Blade trail ribbon | Whoosh SFX | — | Conveys effort and reach |
| Sword Impact | core | Heavy | Directional shake | 0.05s Hitstop | — | Cyan hit sparks | Metallic 'Clink' | — | High impact feel for melee |
| Weapon Switch | core | Light | — | — | — | Bob/Lift | — | Metallic draw SFX | — | Confirmation of switch |

- **Sequences:**
    - **Melee Hit:** Impact (0ms) → Hitstop 0.05s → Camera Shake (50ms) → Particle burst (50ms)

### Melee Mechanics (Sword)

- **Swing Logic (core):**
    - **Type:** Manual (requires click per swing).
    - **Delay Between Shots:** 0.6s (swing speed).
    - **Damage:** 60.
    - **Projectile Strategy:** Fires a short-range `ProjectileStandard`.
        - **Speed:** 100 (near-instant).
        - **MaxLifeTime:** 0.05s.
        - **Radius:** 1.5m (to simulate a broad swing).
        - **Gravity:** 0.
- **Weapon Switching (core):**
    - Integration into `PlayerWeaponsManager`.
    - Mapping to keyboard digit keys (handled by `PlayerInputHandler.GetSelectWeaponInput`).

---

## Pixelated Zombies

This section specifies the design for the "Pixelated Zombies" enemy type, introducing a "Retro Incursion" aesthetic into the world.

### Asset Design (Zombies)

- **Visual Identity:** "Neo-Retro". Contrast with the clean, stylized world using low-fidelity assets.
- **Style Details:**
    - **Models (core):** Low-poly humanoid meshes (~500-1000 triangles) with jittery animations.
    - **Textures (core):** Low-resolution (64x64) textures with visible pixels and limited color palettes.
    - **Palette:** 
        - **Flesh:** Desaturated olive greens (#556B2F) and muddy browns (#4B3621).
        - **Clothing:** Torn, dark grey rags (#2F2F2F).
        - **Contrast:** Bright glowing red eyes (#FF0000) for distance readability.
- **Composition:** Distinctive "zombie shuffle" silhouette — hunched shoulders, dragging feet.

### Game Feedback (Zombies)

- **Genre Profile:** High-Energy / Arcade. Immediate and readable feedback.

#### Interaction Map

| Interaction | Tier | Importance | Camera | Time | Transform | Visual | Audio | Rationale |
|------------|------|-----------|--------|------|-----------|--------|-------|-----------|
| **Zombie Groan** | core | Minor | — | — | — | — | Random pitch groans | Ambient alerting. |
| **Zombie Lunge** | core | Medium | — | — | Quick forward snap | Swipe trail | Whoosh | Telegraphs melee attack. |
| **Zombie Damage** | core | Medium | — | — | Stagger / Scale pop | Pixelated blood splat | Meat thud | Hit confirmation. |
| **Zombie Death** | core | Heavy | — | 0.05s freeze | Collapse animation | Dissolve to pixels | Deep moan | Kill satisfaction. |

### Mechanics & Spawning (core)

- **Movement:** Slow shuffling speed (1.5 - 2.0 m/s). NavMesh-based pathfinding.
- **Attack:** Melee strike. Range: 1.5m. Damage: 30.
- **Random Spawning:**
    - **Strategy:** Random distribution across the terrain on valid NavMesh surfaces.
    - **Exclusion:** Minimum 20m distance from the player's current position.
    - **Density:** 1 zombie per 100m² in designated spawn zones.
    - **Occlusion:** Prefer spawning behind the player or behind large environmental props to avoid pop-in.

### Assets Needed

| Asset | Tier | Type | Reference | Palette | Usage |
|-------|------|------|-----------|---------|-------|
| **Zombie_Mesh** | core | Mesh | Low-poly humanoid | Green/Brown | Main enemy model. |
| **Zombie_Pixel_Tex**| core | Texture | 64x64 Pixel Art | Green/Red | "Pixelated" visual identity. |
| **Blood_Pixel_VFX** | optional | Particles | Red square sprites | Bright Red | Feedback for hits. |
| **Zombie_Sounds** | core | Audio | Distorted groans | — | SFX for behavior states. |

    - **Surface hierarchy:** Consistent with the existing FPS Microgame HUD (translucent dark panels).
- **Weapon HUD:**
    - **Weapon Icon (core):** A stylized, low-poly silhouette of a sword with a prominent cross-guard.
    - **Crosshair (core):** A custom circular crosshair with a vertical line through the center, suggesting a blade edge. The color should transition from white to Cyan (#00F2FF) when targeting an enemy.
- **Layout:**
    - Weapon slot icon should be assigned to a new slot (typically slot 3 or 4).
    - Switching via number keys (1-9) is already supported by the `PlayerInputHandler`.

## Asset Design

- **Visual identity:** High-fantasy, regal, stylized low-poly.
- **Sword of Arthur (core):**
    - **Blade:** Silver/Steel material with a strong emissive cyan glow along the center ridge.
    - **Hilt:** Ornate gold material with a central blue gemstone.
    - **Style:** Match the existing low-poly, clean-shaded aesthetic of the FPS Microgame blaster and shotgun.
- **VFX (core):**
    - **Blade Trail:** A translucent cyan ribbon that follows the blade's path during a swing.
    - **Hit Flash:** A bright white/cyan burst at the point of impact.

## Game Feedback

- **Genre profile:** High-Energy (Arcade FPS).
- **Interaction map:**

| Interaction | Tier | Importance | Camera | Time | Transform | Visual | Audio | Input | Rationale |
|------------|------|-----------|--------|------|-----------|--------|-------|-------|-----------|
| Sword Swing | core | Medium | Subtle roll | — | Squash/Stretch | Blade trail ribbon | Whoosh SFX | — | Conveys effort and reach |
| Sword Impact | core | Heavy | Directional shake | 0.05s Hitstop | — | Cyan hit sparks | Metallic 'Clink' | — | High impact feel for melee |
| Weapon Switch | core | Light | — | — | Bob/Lift | — | Metallic draw SFX | — | Confirmation of switch |

- **Sequences:**
    - **Melee Hit:** Impact (0ms) → Hitstop 0.05s → Camera Shake (50ms) → Particle burst (50ms)

## Melee Mechanics

- **Swing Logic (core):**
    - **Type:** Manual (requires click per swing).
    - **Delay Between Shots:** 0.6s (swing speed).
    - **Damage:** 60.
    - **Projectile Strategy:** Fires a short-range `ProjectileStandard`.
        - **Speed:** 100 (near-instant).
        - **MaxLifeTime:** 0.05s.
        - **Radius:** 1.5m (to simulate a broad swing).
        - **Gravity:** 0.
- **Weapon Switching (core):**
    - Integration into `PlayerWeaponsManager`.
    - Mapping to keyboard digit keys (handled by `PlayerInputHandler.GetSelectWeaponInput`).
