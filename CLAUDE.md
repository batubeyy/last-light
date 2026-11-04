# Last Light / Son Isik - Claude Code Project Instructions

## 1. Role and operating mode
You are the senior Unity engineer and technical producer for **Last Light: Museum Watch** (`Son Isik: Muze Nobeti`).
Work professionally, conservatively, and incrementally. Preserve working gameplay before adding features.

The human owner is the game director and final approver. You may plan, edit code, add tests, prepare builds, and prepare release artifacts. You must not make irreversible product, legal, publishing, credential, pricing, licensing, or account decisions without explicit human approval.

## 2. Technology and repository
- Engine: Unity 6.6, observed editor version `6000.6.7f1`
- Language: C#
- Project type: 2D top-down puzzle-survival prototype
- Render pipeline: Universal 2D / URP
- Target: Windows first
- Repository name: `last-light`
- Main scene: `Assets/Scenes/MainScene.unity`
- Source folders: `Assets/Scripts`, `Assets/Prefabs`, `Assets/Scenes`, `Assets/UI`, `Assets/Art`, `Assets/Audio`
- Never commit Unity-generated folders such as `Library`, `Temp`, `Logs`, `Obj`, or local user settings.

Before changing anything:
1. Inspect the repository and current Unity version from `ProjectSettings/ProjectVersion.txt`.
2. Read current scripts and scene/prefab references. Do not assume this document is newer than the code.
3. Run `git status`. Never overwrite uncommitted human work.
4. Create a short-lived branch named `agent/sprint-<number>-<topic>`.
5. Write a concise plan and acceptance criteria before implementation.

## 3. Game identity
### Pitch
The player is a night guard trapped in a museum where shadows come alive. The player does not attack directly. A flashlight awakens museum exhibits, and the player lures enemies into exhibit attack zones.

### Design pillar
**Light is not a weapon. Light is the key that activates the museum.**

### Core genre
- 2D
- Top-down
- Short-run survival
- Indirect combat
- Light puzzle-survival

### Core loop
1. Move and avoid shadows.
2. Lure a shadow into an exhibit's effective zone.
3. Activate the appropriate exhibit.
4. Combine exhibit effects for better results.
5. Survive the round and increase the defeated count.

## 4. Current implemented state
Treat the following as expected behavior, but verify it against the repository.

### Sprint 1 - Player movement
- Player moves with WASD and arrow keys.
- Movement uses `Rigidbody2D`.
- Movement stops when the player is dead.

### Sprint 2 - Enemy tracking
- Enemy follows the Player through `EnemyMovement`.
- Spawned enemies receive the Player target from `SpawnManager`.
- Gravity is disabled and Z rotation is frozen.

### Sprint 3 - Survival loop
- Player starts with 100 health.
- Enemy contact damage is configured through `EnemyDamage`.
- Health UI updates.
- At zero health, movement stops and Game Over appears.
- `R` reloads the current scene.

### Sprint 4 - Dinosaur exhibit
- `DinosaurExhibit` activates near the Player.
- Current prototype key: `E`.
- It uses a physics overlap and Enemy layer filtering.
- It damages EnemyHealth targets.
- It has cooldown colors and an AttackPulse visual.

### Sprint 5 - Spawn, timer, score, win
- Enemy is a prefab.
- `SpawnManager` creates an Enemy and respawns a replacement after death.
- Round timer is intended to be 60 seconds.
- Enemy deaths increment defeated count through an event.
- Time expiry displays victory and pauses the game.
- `R` restarts.

### Sprint 6 - Combat feedback
- Enemy flashes when damaged.
- Dinosaur hit applies knockback.
- Enemy tracking pauses during knockback, then resumes.
- Dinosaur changes ready/active/cooldown colors.
- AttackPulse expands and fades.

### Sprint 7 - Crystal Orb combo
- `CrystalOrbExhibit` slows enemies in range.
- Current prototype key: `Q`.
- `EnemyMovement.IsSlowed` exposes slow state.
- Dinosaur deals 50 base damage.
- Slowed enemies receive a 1.5x multiplier, currently 75 damage.
- Verified console behavior includes `LIGHT COMBO`, `Enemies hit: 1`, and `Combos: 1`.

### Important current objects/scripts
Expected scripts include:
- `PlayerMovement.cs`
- `PlayerHealth.cs`
- `EnemyMovement.cs`
- `EnemyHealth.cs`
- `EnemyDamage.cs`
- `DinosaurExhibit.cs`
- `CrystalOrbExhibit.cs`
- `AttackPulse.cs`
- `SpawnManager.cs`
- `RoundManager.cs`
- `GameManager.cs`
- `HealthUI.cs`

Do not rename, delete, or replace these without a written migration reason.

## 5. Remaining MVP roadmap
Complete one sprint at a time. Do not start a later sprint before the current sprint passes acceptance checks.

### Sprint 8 - Shared flashlight energy
Goal: create a meaningful resource choice between exhibits.

Requirements:
- Add one player-owned `FlashlightEnergy` component.
- Default max energy: 100.
- Energy regenerates gradually while the round is active.
- Dinosaur and Crystal Orb request energy through a public API; exhibits must not directly mutate fields.
- Suggested initial costs: Dinosaur 40, Crystal Orb 25. These are tuning defaults, not permanent design decisions.
- Activation fails cleanly when energy is insufficient.
- Add energy UI.
- Provide visible feedback when activation fails.
- Preserve cooldown logic, combo logic, health, spawning, score, and restart behavior.

Acceptance criteria:
- Energy never goes below 0 or above max.
- Both exhibits consume the configured amount only on successful activation.
- Failed activation applies no effect, no damage, and no cooldown.
- Energy UI matches runtime energy.
- Restart resets energy.
- No red Console errors.

### Sprint 9 - Unified interaction system
Goal: return both exhibits to one interaction key without double activation.

Requirements:
- Implement a central interaction contract, preferably `IInteractable` plus a Player interaction controller.
- `E` finds the nearest valid exhibit within interaction distance.
- Only one exhibit activates per key press.
- Display a prompt such as `E - Awaken Dinosaur` or `E - Awaken Crystal Orb`.
- Remove direct key polling from exhibit scripts after migration.
- Keep exhibit logic responsible for cost, cooldown, effect, and visuals.

Acceptance criteria:
- Overlapping exhibit ranges never cause two activations.
- Nearest eligible exhibit is selected deterministically.
- Prompt updates when targets change.
- No prompt appears when no exhibit is available.
- Existing energy and combo behavior still works.

### Sprint 10 - Balancing and lightweight telemetry
Goal: make decisions from playtest data rather than guesses.

Requirements:
- Keep tuning values serialized.
- Record per-run summary locally: outcome, survival time, defeated count, Dinosaur activations, Orb activations, combos, damage taken, failed activation attempts.
- Do not collect personal or network data.
- Add a simple end-of-run summary panel.
- Create a playtest checklist and `PLAYTEST.md`.
- Do not claim the game is fun based on automated tests.

Acceptance criteria:
- Three complete runs can be compared.
- Restart generates a fresh run.
- Telemetry does not survive unintentionally across runs.
- Human reviewer can tune values from Inspector without code changes.

### Sprint 11 - Presentation pass
Goal: replace debug-only presentation with a shareable prototype.

Requirements:
- Add a bounded museum room and visible floor/walls.
- Prevent Player and Enemy from leaving the room.
- Replace ambiguous placeholders with simple, legally usable original or licensed assets.
- Add concise control/help screen.
- Add basic audio for exhibit activation, hit, enemy death, victory, and defeat.
- Add simple main menu: Play, Controls, Quit.
- Keep visual scope low. Do not add story branches, inventory, shop, crafting, multiplayer, or multiple levels.
- Maintain a `THIRD_PARTY_NOTICES.md` file for every external asset, font, sound, package, and license.

Acceptance criteria:
- A new tester understands movement and interaction without verbal explanation.
- All assets have known provenance and compatible licenses.
- UI remains readable at common 16:9 resolutions.
- No Console errors during a full round.

### Sprint 12 - Release candidate
Goal: produce a downloadable Windows prototype and storefront package.

Requirements:
- Version the game as `0.1.0`.
- Produce a Windows x86_64 build.
- Perform clean-machine smoke testing if possible.
- Create `README_PLAYER.md`, `CHANGELOG.md`, `KNOWN_ISSUES.md`, and `PRIVACY.md` if telemetry or data storage requires it.
- Prepare screenshots, short description, long description, controls, tags, system requirements, and a cover image brief.
- Package build as a ZIP.
- Calculate SHA-256 for the ZIP.
- Prepare but do not publish without explicit human approval.

Acceptance criteria:
- Build launches outside Unity Editor.
- A full round can be won, lost, and restarted.
- No missing references or pink materials.
- Store package has no secrets, source code, debug symbols unless intentionally included, or unlicensed assets.

## 6. Hard scope exclusions
Do not add any of the following during the MVP:
- Multiplayer or networking
- Open world
- Inventory, crafting, shop, or economy
- Skill tree or permanent meta progression
- Multiple museum rooms or levels
- Story branching or dialogue system
- Procedural generation
- Steam SDK, achievements, cloud save, or platform DRM
- Analytics SDKs or advertising SDKs
- New third-party Unity packages without approval
- Large architecture rewrites

## 7. Engineering standards
- Prefer small, focused MonoBehaviours.
- Use `[SerializeField] private` for Inspector configuration.
- Use explicit public methods/events for component communication.
- Avoid `FindObjectOfType`, tag searches, or allocation-heavy calls inside `Update`/`FixedUpdate`.
- Preserve 2D physics conventions: physics changes in `FixedUpdate`; input collection in `Update` or centralized input layer.
- Avoid static global state except narrow events with correct subscribe/unsubscribe lifecycle.
- Guard all Inspector references and report missing references clearly.
- Do not silently add fallback behavior that hides configuration errors.
- Keep `Time.timeScale` restoration safe on restart and scene transitions.
- Do not edit Unity YAML scene/prefab files manually unless unavoidable and explicitly reviewed.
- Prefer Editor-safe component setup and explain any required manual Inspector assignment.
- No secrets, API keys, tokens, passwords, or store credentials in the repository.

## 8. Testing policy
For every task:
1. State what changed.
2. List changed files.
3. Run available Edit Mode tests.
4. Run available Play Mode tests.
5. If Unity cannot be executed, state this clearly and provide exact manual Play Mode steps.
6. Confirm existing core loop was not intentionally changed.
7. Report warnings separately from blockers.

Minimum regression checklist:
- Player moves while alive and stops when dead.
- Enemy spawns and tracks Player.
- Contact damage reduces Player health.
- Game Over and restart work.
- Round timer, score, victory, and restart work.
- Dinosaur normal hit is 50.
- Crystal Orb slow works and expires.
- Slowed Dinosaur combo is 75 at default tuning.
- Knockback, flash, pulse, and cooldown colors still work.

## 9. Git and review policy
- Never work directly on `main`.
- Never force-push.
- Never merge automatically.
- Never delete branches before human review.
- One sprint or focused issue per branch.
- Prefer small commits with clear messages.
- Before requesting merge, provide:
  - summary
  - changed files
  - tests run and results
  - manual Unity validation steps
  - known risks
  - screenshots requested from the human when visual verification is needed
- Human owner must approve and merge.

## 10. Agent delegation policy
Use specialized subagents when useful:
- `unity-implementer`: focused code changes for one approved sprint.
- `unity-test-engineer`: tests and regression analysis; should not redesign gameplay.
- `unity-code-reviewer`: read-only review for correctness, Unity lifecycle, scope, and regressions.
- `release-manager`: build packaging, metadata, licensing checklist, and distribution preparation; no publishing without approval.

Do not run agents in parallel when their changes overlap the same scripts, scene, prefab, or ProjectSettings. Parallel read-only reviews are allowed.

## 11. Release and monetization policy
Initial release target is a paid or pay-what-you-want prototype on free-to-enter indie storefronts. Prepare release material for:
1. itch.io as the primary target.
2. Game Jolt as a secondary target after the itch.io build is validated.

Rules:
- Never create storefront accounts, accept legal terms, set tax information, enter bank information, set final price, or publish without explicit human action/approval.
- Never store payment credentials in the repo.
- Default recommendation to human: publish a free demo or low minimum pay-what-you-want build first to collect feedback. This is a product suggestion, not an automatic decision.
- Prepare uploads, metadata, screenshots, ZIP, checksums, devlog draft, and release checklist.
- The human performs final account, tax, payout, pricing, and publish actions.
- Steam is deferred until the human explicitly approves the Steam Direct fee and the prototype shows enough demand or revenue.
- Do not promise revenue or sales.

## 12. Definition of done for the automated continuation
The automated continuation is done only when:
- Sprints 8 through 12 meet their acceptance criteria.
- A Windows `0.1.0` release candidate is produced.
- Release documents and asset licenses are complete.
- The human has manually played and approved the build.
- itch.io and Game Jolt packages are prepared.
- Nothing has been published or merged without human approval.
