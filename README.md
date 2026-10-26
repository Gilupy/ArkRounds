# ArkRounds

An unofficial, fan-made [ROUNDS](https://store.steampowered.com/app/1557740/ROUNDS/) card pack themed around *Arknights* operators, built on BepInEx and UnboundLib.

> Not affiliated with or endorsed by Hypergryph, Yostar, or the ROUNDS developers. Arknights is the property of its respective owners.

> **Vibe-coded:** this mod was written entirely with AI assistance (Claude) and hasn't been through a traditional code review. Card balance is guesswork, and bugs are likely. Issues and pull requests are welcome.

## Install (Windows)

You don't need to build anything. Download the prebuilt DLL from the [Releases page](https://github.com/<your-username>/ArkRounds/releases) and drop it into the game.

1. In Steam, right-click **ROUNDS** → **Properties** → **Betas**, and select **old build for mods**. The mod won't work on the current build.
2. Install [r2modman](https://thunderstore.io/package/ebkr/r2modman/), choose **ROUNDS**, and create a profile.
3. In r2modman, search for and install **UnboundLib**. BepInEx is installed automatically as a dependency.
4. Download `ArkRounds.dll` from the latest release.
5. In r2modman, go to **Settings** → **Browse profile folder**, open `BepInEx/plugins`, and put `ArkRounds.dll` in there.
6. Click **Start modded**. The cards will show up in the card pool.

Everyone in a lobby needs the same mods and versions, or the game will desync or refuse to connect.

<details>
<summary>Installing without a mod manager</summary>

1. Download **BepInExPack_ROUNDS** and **UnboundLib** from [Thunderstore](https://thunderstore.io/c/rounds/).
2. Extract the contents of the BepInEx pack into your ROUNDS folder (the one containing `Rounds.exe`).
3. Put UnboundLib in `ROUNDS/BepInEx/plugins/`.
4. Put `ArkRounds.dll` in `ROUNDS/BepInEx/plugins/` too.
5. Launch ROUNDS from Steam as normal.

</details>

## Cards

| Card | Rarity | Effect |
|---|---|---|
| SilverAsh - Truesilver Slash | Rare | +100% damage, +100% bullet speed, +50% block cooldown, -30% move speed |
| Exusiai - Apple Pie! | Uncommon | +150% attack speed, +4 max ammo, -40% damage |
| Saria - Calcification | Rare | +80% health, +1 block charge, -25% move speed |
| Surtr - Twilight | Rare | +200% damage, +30% bullet size, drains 5% max health per second |
| Myrtle - Golden Apples | Common | -40% block cooldown, +15% move speed, -20% damage |
| Ch'en - Chi Xiao | Rare | +40% attack speed, +25% damage, +25% reload time |
| Eyjafjalla - Volcanic Sparks | Uncommon | +1 bullet, -30% damage, wider spread |
| Hoshiguma - Oni's Resolve | Rare | +50% health, +50% knockback, -15% move speed |
| Skadi - Abyssal Hunt | Rare | +60% damage, +20% lifesteal, +30% reload time |
| Texas - Sicilian Rush | Uncommon | +25% move speed, +1 jump, -15% health |
| W - Chaotic Bounce | Uncommon | +2 bounces, +20% bullet speed, -15% damage |
| Specter - Unchained Return | Rare | +1 respawn, -25% health, -10% damage |
| Nearl - Radiant Knight | Common | +15% health, +2 HP/s regen, -10% damage |
| Lappland - Insatiable Wolf | Uncommon | +8% damage per shot (max +200%), resets when you hit an opponent |
| Mudrock - Rockbreaker | Rare | +70% health, +30% damage, -30% move speed |
| Blaze - Chainsaw Charge | Uncommon | +50% damage, +20% health, -20% attack speed |
| Ptilopsis - Serenity | Common | -20% reload time, -20% block cooldown, -15% damage |
| Amiya - Arcane Echo | Common | +25% bullet speed, +10% attack speed, -10% health |
| Rosmontis - Memory Storm | Rare | +3 max ammo, +20% bullet size, -15% health |
| Mostima - Time Lock | Rare | -50% block cooldown, +1 block charge, -25% damage |
| Kal'tsit - Medical Protocol | Rare | +25% health, +3 HP/s regen, -20% attack speed |
| Hellagur - Duelist's Crescendo | Rare | +7% damage per shot with no cap, no reset on hit, clears at the end of each point |
| Ulpianus - Natural Holdfast | Rare | +50% health, +2 HP/s regen, +15% damage, -25% move speed |
| Skadi the Corrupting Heart - Gnawing Tide | Rare | +30% lifesteal, +25% attack speed, -30% health |
| Gladiia - Undertow | Uncommon | +25% bullet size, -25% reload time, -15% damage |

## Requirements

- ROUNDS (use the **old build for mods** beta branch in Steam)
- [BepInEx](https://thunderstore.io/c/rounds/p/BepInEx/BepInExPack_ROUNDS/)
- [UnboundLib](https://thunderstore.io/c/rounds/p/willis81808/UnboundLib/)

## Building (Arch Linux, Steam + Proton)

```bash
git clone https://github.com/<your-username>/ArkRounds.git
cd ArkRounds
bash setup_arkrounds.sh
```

The script installs the build tools, BepInEx and UnboundLib if missing, then compiles the plugin. The DLL is written to `~/Documents/ArkRounds/ArkRounds.dll`.

Useful options:

```bash
GAME_DIR=/path/to/ROUNDS bash setup_arkrounds.sh   # non-default game location
INSTALL=1 bash setup_arkrounds.sh                  # also copy the DLL into BepInEx/plugins
SKIP_MODS=1 bash setup_arkrounds.sh                # build only, don't touch BepInEx/UnboundLib
```

To run under Proton, set this Steam launch option:

```
WINEDLLOVERRIDES="winhttp=n,b" %command%
```

## Installing a prebuilt DLL (Linux)

Put `ArkRounds.dll` from the [Releases page](https://github.com/<your-username>/ArkRounds/releases) in `ROUNDS/BepInEx/plugins/` (or your mod manager profile's `plugins` folder), and set the Proton launch option shown above. Windows users: see [Install (Windows)](#install-windows).
