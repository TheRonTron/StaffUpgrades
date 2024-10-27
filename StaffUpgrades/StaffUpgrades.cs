using BepInEx;
using Jotunn.Managers;
using BepInEx.Configuration;

namespace StaffUpgrades
{
    [BepInPlugin("TheRonTron.StaffUpgrades", "StaffUpgrades", "4.2.0")]
    [BepInDependency(Jotunn.Main.ModGuid)]
    internal class StaffUpgrades : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.StaffUpgrades";
        public ConfigEntry<int> Balls;
        public ConfigEntry<int> Shards;
        public void Awake()
        {
            GenerateConfig();
            Jotunn.Logger.ShowDate = true;
            PrefabManager.OnPrefabsRegistered += () =>
            {
                var FireballStaff = PrefabManager.Instance.GetPrefab("StaffFireball").GetComponent<ItemDrop>().m_itemData.m_shared;
                var SackDrop = FireballStaff.m_secondaryAttack;

                FireballStaff.m_attack.m_attackStartNoise = 10;
                FireballStaff.m_aiAttackRange = 15;
                FireballStaff.m_aiAttackRangeMin = 1;
                FireballStaff.m_aiAttackInterval = 6;
                FireballStaff.m_aiAttackMaxAngle = 15;
                
                SackDrop.m_attackType = Attack.AttackType.Projectile;
                SackDrop.m_attackAnimation = "staff_fireball";
                SackDrop.m_attackChainLevels = 2;
                SackDrop.m_attackStamina = 0;
                SackDrop.m_attackEitr = 35*Balls.Value;
                SackDrop.m_speedFactor = .3f;
                SackDrop.m_speedFactorRotation = .3f;
                SackDrop.m_attackHitNoise = 0;
                SackDrop.m_attackRange = 2;
                SackDrop.m_attackHeight = 1.2f;
                SackDrop.m_attackOffset = .2f;
                SackDrop.m_attackProjectile = FireballStaff.m_attack.m_attackProjectile;
                SackDrop.m_projectileVel = 20;
                SackDrop.m_projectileAccuracy = 4;
                SackDrop.m_projectileAccuracyMin = 2;
                SackDrop.m_launchAngle = -5;
                SackDrop.m_projectiles = Balls.Value;

                var IceShardsStaff = PrefabManager.Instance.GetPrefab("StaffIceShards").GetComponent<ItemDrop>().m_itemData.m_shared;
                var Blizzard = IceShardsStaff.m_secondaryAttack;

                IceShardsStaff.m_aiAttackRange = 15;
                IceShardsStaff.m_aiAttackRangeMin = 1;
                IceShardsStaff.m_aiAttackInterval = 6;
                IceShardsStaff.m_aiAttackMaxAngle = 15;

                Blizzard.m_attackType = Attack.AttackType.Projectile;
                Blizzard.m_attackAnimation = "staff_rapidfire";
                Blizzard.m_loopingAttack = true;
                Blizzard.m_attackStamina = 0;
                Blizzard.m_attackEitr = 5;
                Blizzard.m_speedFactor = .5f;
                Blizzard.m_speedFactorRotation = 1f;
                Blizzard.m_attackHitNoise = 0;
                Blizzard.m_damageMultiplier = (float)1/Shards.Value;
                Blizzard.m_attackRange = 2;
                Blizzard.m_attackHeight = 1.2f;
                Blizzard.m_attackOffset = .2f;
                Blizzard.m_raiseSkillAmount = .2f;
                Blizzard.m_attackProjectile = IceShardsStaff.m_attack.m_attackProjectile;
                Blizzard.m_projectileVel = 30;
                Blizzard.m_projectileVelMin = 30;
                Blizzard.m_projectileAccuracy = 8;
                Blizzard.m_projectileAccuracyMin = 4;
                Blizzard.m_launchAngle = -4;
                Blizzard.m_projectiles = Shards.Value;
                Blizzard.m_projectileBursts = 999;
                Blizzard.m_burstInterval = .2f;
                Blizzard.m_perBurstResourceUsage = true;

                Blizzard.m_startEffect = IceShardsStaff.m_attack.m_startEffect;
                Blizzard.m_burstEffect = IceShardsStaff.m_attack.m_burstEffect;
            };
        }
        public void GenerateConfig() //dragoonnnnns
        {
            Config.SaveOnConfigSet = true;

            Balls = Config.Bind("Server config", "Fireball Projectiles", 2,
            new ConfigDescription("Number of Fireballs", null,
            new AcceptableValueRange<int>(1, 100),
            new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Shards = Config.Bind("Server config", "Blizzard Projectiles", 8,
            new ConfigDescription("Number of Blizzard Shards", null,
            new AcceptableValueRange<int>(1, 100),
            new ConfigurationManagerAttributes { IsAdminOnly = true }));

            SynchronizationManager.OnConfigurationSynchronized += (obj, attr) =>  
            {
                if (attr.InitialSynchronization)
                {
                    Jotunn.Logger.LogMessage("Initial Config sync event received");
                }
                else
                {
                    Jotunn.Logger.LogMessage("Config sync event received");
                }
            };
        }
    }
}
