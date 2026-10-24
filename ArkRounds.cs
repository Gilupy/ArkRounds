using System.Reflection;
using BepInEx;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace ArkRounds
{
    // ------------------------------------------------------------------
    //  PLUGIN
    // ------------------------------------------------------------------
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class ArkRoundsPlugin : BaseUnityPlugin
    {
        public const string ModId = "com.gilupy.rounds.arkrounds";
        public const string ModName = "ArkRounds";
        public const string Version = "1.1.0";
        public const string ModInitials = "ARK";

        private void Awake()
        {
            // Nothing to patch yet - kept for future Harmony patches.
        }

        private void Start()
        {
            // Registers each card with UnboundLib so it shows up in the card pool.

            // --- Original set ---
            CustomCard.BuildCard<SilverAshTruesilverSlash>();
            CustomCard.BuildCard<ExusiaiApplePie>();
            CustomCard.BuildCard<SariaCalcification>();
            CustomCard.BuildCard<SurtrTwilight>();
            CustomCard.BuildCard<MyrtleGoldenApples>();

            // --- New cards ---
            CustomCard.BuildCard<ChenChiXiao>();
            CustomCard.BuildCard<EyjafjallaVolcanicSparks>();
            CustomCard.BuildCard<HoshigumaOnisResolve>();
            CustomCard.BuildCard<SkadiAbyssalHunt>();
            CustomCard.BuildCard<TexasSicilianRush>();
            CustomCard.BuildCard<WChaoticBounce>();
            CustomCard.BuildCard<SpecterUnchainedReturn>();
            CustomCard.BuildCard<NearlRadiantKnight>();
        }
    }

    // ------------------------------------------------------------------
    //  SHARED BASE CLASS (inherits CustomCard)
    //  Provides default no-op OnAddCard / OnRemoveCard, no card art,
    //  and the mod name shown on the card.
    // ------------------------------------------------------------------
    public abstract class ArknightsCard : CustomCard
    {
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data,
                                       HealthHandler health, Gravity gravity, Block block,
                                       CharacterStatModifiers characterStats) { }

        public override void OnRemoveCard() { }

        protected override GameObject GetCardArt() => null;

        public override string GetModName() => ArkRoundsPlugin.ModInitials;
    }

    // ------------------------------------------------------------------
    //  1. SILVERASH - TRUESILVER SLASH  (Rare)
    //  +100% damage, +100% bullet speed, +50% block cooldown, -30% move speed
    // ------------------------------------------------------------------
    public class SilverAshTruesilverSlash : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            gun.damage = 2.0f;               // +100% damage
            gun.projectileSpeed = 2.0f;      // +100% bullet travel speed
            block.cdMultiplier = 1.5f;       // +50% block cooldown (longer = worse)
            statModifiers.movementSpeed = 0.7f; // -30% movement speed
        }

        protected override string GetTitle() => "SilverAsh - Truesilver Slash";
        protected override string GetDescription() =>
            "The Lord of Karlan's blade reaches far and cuts deep, but demands your full commitment.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.TechWhite;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Damage",          amount = "+100%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Bullet Speed",    amount = "+100%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Block Cooldown",  amount = "+50%",  simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Movement Speed",  amount = "-30%",  simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ------------------------------------------------------------------
    //  2. EXUSIAI - APPLE PIE!  (Uncommon)
    //  +150% attack speed, +4 max ammo, -40% damage
    // ------------------------------------------------------------------
    public class ExusiaiApplePie : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            // gun.attackSpeed is the delay between shots, so +150% fire rate = 1 / 2.5
            gun.attackSpeed = 1f / 2.5f;
            gun.ammo = 4;                    // +4 max ammo
            gun.damage = 0.6f;               // -40% damage
        }

        protected override string GetTitle() => "Exusiai - Apple Pie!";
        protected override string GetDescription() =>
            "Fire first, deliver later. A relentless stream of small-caliber rounds.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.FirepowerYellow;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Attack Speed", amount = "+150%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Max Ammo",     amount = "+4",    simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Damage",       amount = "-40%",  simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ------------------------------------------------------------------
    //  3. SARIA - CALCIFICATION  (Rare)
    //  +80% max health, +1 block charge, -25% move speed
    // ------------------------------------------------------------------
    public class SariaCalcification : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.health = 1.8f;          // +80% max health
            block.additionalBlocks = 1;           // +1 block charge
            statModifiers.movementSpeed = 0.75f;  // -25% movement speed
        }

        protected override string GetTitle() => "Saria - Calcification";
        protected override string GetDescription() =>
            "Stand like a wall. Nothing gets through, and nothing gets past quickly.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.DefensiveBlue;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Health",         amount = "+80%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Block Charges",  amount = "+1",   simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Movement Speed", amount = "-25%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ------------------------------------------------------------------
    //  4. SURTR - TWILIGHT  (Rare)
    //  +200% damage, +30% bullet size, drains 5% max health every second.
    // ------------------------------------------------------------------
    public class SurtrTwilight : ArknightsCard
    {
        private TwilightDrain drain;

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            gun.damage = 3.0f;       // +200% damage
            gun.size = 0.3f;         // +30% bullet size (additive in ROUNDS)
            cardInfo.allowMultiple = false; // the drain component isn't stackable
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data,
                                       HealthHandler health, Gravity gravity, Block block,
                                       CharacterStatModifiers characterStats)
        {
            drain = player.gameObject.GetComponent<TwilightDrain>();
            if (drain == null)
            {
                drain = player.gameObject.AddComponent<TwilightDrain>();
            }
        }

        public override void OnRemoveCard()
        {
            if (drain != null)
            {
                Destroy(drain);
                drain = null;
            }
        }

        protected override string GetTitle() => "Surtr - Twilight";
        protected override string GetDescription() =>
            "Burn everything, including yourself. The flame consumes its wielder.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.DestructiveRed;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Damage",      amount = "+200%",  simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Bullet Size",  amount = "+30%",   simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Health Drain", amount = "5%/s",   simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    /// <summary>
    /// Attached to the player by Surtr's card. Once per second, deals damage equal
    /// to 5% of the player's max health. Only ticks while the player is alive and
    /// in an active round.
    /// </summary>
    public class TwilightDrain : MonoBehaviour
    {
        public const float DrainFraction = 0.05f; // 5% of max health
        public const float Interval = 1f;         // every second
        public const bool CanKillOwner = false;   // true = the drain can finish you off

        private CharacterData data;
        private HealthHandler health;
        private float timer;

        // CharacterData's max-health member isn't named the same in every build of ROUNDS,
        // so it's looked up by reflection. If it can't be found, the highest health seen
        // this session is used as the max instead.
        private FieldInfo maxHealthField;
        private PropertyInfo maxHealthProp;
        private float peakHealth;

        private void Start()
        {
            data = GetComponent<CharacterData>();
            health = GetComponent<HealthHandler>();
            FindMaxHealthMember();
        }

        private void FindMaxHealthMember()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (string name in new[] { "maxHealth", "MaxHealth" })
            {
                maxHealthField = typeof(CharacterData).GetField(name, flags);
                if (maxHealthField != null) return;
                maxHealthProp = typeof(CharacterData).GetProperty(name, flags);
                if (maxHealthProp != null) return;
            }
            Debug.LogWarning("[ArkRounds] Couldn't find CharacterData max health; Twilight will use peak health instead.");
        }

        private float GetMaxHealth()
        {
            try
            {
                if (maxHealthField != null) return System.Convert.ToSingle(maxHealthField.GetValue(data));
                if (maxHealthProp != null) return System.Convert.ToSingle(maxHealthProp.GetValue(data, null));
            }
            catch { /* fall through to the fallback below */ }

            peakHealth = Mathf.Max(peakHealth, data.health);
            return peakHealth;
        }

        private void Update()
        {
            if (data == null || health == null) return;

            // Don't drain while dead or between points / during card picks.
            if (data.dead || !data.isPlaying)
            {
                timer = 0f;
                return;
            }

            timer += Time.deltaTime;
            if (timer < Interval) return;
            timer -= Interval;

            float amount = GetMaxHealth() * DrainFraction;

            if (!CanKillOwner)
            {
                // Leave the player at 1 HP at minimum.
                amount = Mathf.Min(amount, data.health - 1f);
            }
            if (amount <= 0f) return;

            // healthRemoval = true so blocking / shields don't negate the self-damage.
            health.TakeDamage(Vector2.up * amount, (Vector2)transform.position,
                              Color.red, null, null, true);
        }
    }

    // ------------------------------------------------------------------
    //  5. MYRTLE - GOLDEN APPLES  (Common)
    //  -40% block cooldown, +15% move speed, -20% damage
    // ------------------------------------------------------------------
    public class MyrtleGoldenApples : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            block.cdMultiplier = 0.6f;            // -40% block cooldown
            statModifiers.movementSpeed = 1.15f;  // +15% movement speed
            gun.damage = 0.8f;                    // -20% damage
        }

        protected override string GetTitle() => "Myrtle - Golden Apples";
        protected override string GetDescription() =>
            "Quick on your feet and quicker to recover. Not built for a straight fight.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Common;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.NatureBrown;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Block Cooldown",  amount = "-40%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Movement Speed",  amount = "+15%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Damage",          amount = "-20%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ==================================================================
    //  NEW CARDS
    // ==================================================================

    // ------------------------------------------------------------------
    //  6. CH'EN - CHI XIAO  (Rare)
    //  +40% attack speed, +25% damage, +25% reload time
    // ------------------------------------------------------------------
    public class ChenChiXiao : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            gun.attackSpeed = 1f / 1.4f;   // +40% fire rate (attackSpeed is a delay)
            gun.damage = 1.25f;            // +25% damage
            gun.reloadTime = 1.25f;        // +25% reload time (longer = worse)
        }

        protected override string GetTitle() => "Ch'en - Chi Xiao";
        protected override string GetDescription() =>
            "A flurry of precise cuts, then a moment to steady the blade.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.EvilPurple;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Attack Speed", amount = "+40%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Damage",       amount = "+25%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Reload Time",  amount = "+25%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ------------------------------------------------------------------
    //  7. EYJAFJALLA - VOLCANIC SPARKS  (Uncommon)
    //  +1 bullet, -30% damage, wider spread
    // ------------------------------------------------------------------
    public class EyjafjallaVolcanicSparks : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            gun.numberOfProjectiles = 1;   // +1 bullet per shot (additive)
            gun.spread = 0.1f;             // wider spread (additive)
            gun.damage = 0.7f;             // -30% damage per bullet
        }

        protected override string GetTitle() => "Eyjafjalla - Volcanic Sparks";
        protected override string GetDescription() =>
            "Sheep-shaped sparks scatter in every direction. Some of them will hit.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.MagicPink;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Bullets", amount = "+1",       simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Damage",  amount = "-30%",     simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Spread",  amount = "Increased", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ------------------------------------------------------------------
    //  8. HOSHIGUMA - ONI'S RESOLVE  (Rare)
    //  +50% max health, +50% knockback, -15% move speed
    // ------------------------------------------------------------------
    public class HoshigumaOnisResolve : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.health = 1.5f;          // +50% max health
            gun.knockback = 1.5f;                 // +50% bullet knockback
            statModifiers.movementSpeed = 0.85f;  // -15% movement speed
        }

        protected override string GetTitle() => "Hoshiguma - Oni's Resolve";
        protected override string GetDescription() =>
            "An immovable shield and a heavy hand. Anything that touches you gets pushed back.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.DefensiveBlue;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Health",         amount = "+50%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Knockback",      amount = "+50%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Movement Speed", amount = "-15%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ------------------------------------------------------------------
    //  9. SKADI - ABYSSAL HUNT  (Rare)
    //  +60% damage, +20% lifesteal, +30% reload time
    // ------------------------------------------------------------------
    public class SkadiAbyssalHunt : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            gun.damage = 1.6f;                 // +60% damage
            statModifiers.lifeSteal = 0.2f;    // +20% lifesteal (additive)
            gun.reloadTime = 1.3f;             // +30% reload time (longer = worse)
        }

        protected override string GetTitle() => "Skadi - Abyssal Hunt";
        protected override string GetDescription() =>
            "The sea takes, and the sea gives back. Every wound you deal feeds the hunt.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.ColdBlue;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Damage",      amount = "+60%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Lifesteal",   amount = "+20%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Reload Time", amount = "+30%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ------------------------------------------------------------------
    //  10. TEXAS - SICILIAN RUSH  (Uncommon)
    //  +25% move speed, +1 jump, -15% max health
    // ------------------------------------------------------------------
    public class TexasSicilianRush : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.movementSpeed = 1.25f;  // +25% movement speed
            statModifiers.numberOfJumps = 1;      // +1 extra jump (additive)
            statModifiers.health = 0.85f;         // -15% max health
        }

        protected override string GetTitle() => "Texas - Sicilian Rush";
        protected override string GetDescription() =>
            "In and out before they can blink. Just don't get caught.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.TechWhite;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Movement Speed", amount = "+25%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Jumps",          amount = "+1",   simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Health",         amount = "-15%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ------------------------------------------------------------------
    //  11. W - CHAOTIC BOUNCE  (Uncommon)
    //  +2 bounces, +20% bullet speed, -15% damage
    // ------------------------------------------------------------------
    public class WChaoticBounce : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            gun.reflects = 2;              // +2 wall bounces (additive)
            gun.projectileSpeed = 1.2f;    // +20% bullet speed
            gun.damage = 0.85f;            // -15% damage
        }

        protected override string GetTitle() => "W - Chaotic Bounce";
        protected override string GetDescription() =>
            "Why aim when the walls will do it for you? Hope you like surprises.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Uncommon;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.DestructiveRed;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Bounces",      amount = "+2",   simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Bullet Speed", amount = "+20%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Damage",       amount = "-15%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ------------------------------------------------------------------
    //  12. SPECTER - UNCHAINED RETURN  (Rare)
    //  +1 respawn, -25% max health, -10% damage
    // ------------------------------------------------------------------
    public class SpecterUnchainedReturn : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.respawns = 1;       // +1 respawn (additive)
            statModifiers.health = 0.75f;     // -25% max health
            gun.damage = 0.9f;                // -10% damage
        }

        protected override string GetTitle() => "Specter - Unchained Return";
        protected override string GetDescription() =>
            "The Abyssal Hunter doesn't stay down. A frailer body, but a second chance.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Rare;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.EvilPurple;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Respawns", amount = "+1",   simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Health",   amount = "-25%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Damage",   amount = "-10%", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }

    // ------------------------------------------------------------------
    //  13. NEARL - RADIANT KNIGHT  (Common)
    //  +15% max health, +2 HP/s regeneration, -10% damage
    //  NOTE: regen is a flat additive value - tune it in-game if it feels off.
    // ------------------------------------------------------------------
    public class NearlRadiantKnight : ArknightsCard
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats,
                                       CharacterStatModifiers statModifiers, Block block)
        {
            statModifiers.health = 1.15f;     // +15% max health
            statModifiers.regen = 2f;         // passive regeneration (additive)
            gun.damage = 0.9f;                // -10% damage
        }

        protected override string GetTitle() => "Nearl - Radiant Knight";
        protected override string GetDescription() =>
            "A knight's light mends wounds as fast as they come. Not much bite, though.";
        protected override CardInfo.Rarity GetRarity() => CardInfo.Rarity.Common;
        protected override CardThemeColor.CardThemeColorType GetTheme() =>
            CardThemeColor.CardThemeColorType.TechWhite;

        protected override CardInfoStat[] GetStats() => new[]
        {
            new CardInfoStat { positive = true,  stat = "Health",       amount = "+15%",   simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = true,  stat = "Regeneration", amount = "+2 HP/s", simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
            new CardInfoStat { positive = false, stat = "Damage",       amount = "-10%",   simepleAmount = CardInfoStat.SimpleAmount.notAssigned },
        };
    }
}
