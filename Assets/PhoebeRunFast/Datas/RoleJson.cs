using UnityEngine;
using System;

[Serializable]
public class RoleJson
{
    public IdentityJson identity;
    public LevelJson level;
    public PropertyJson baseProperty;
    public PropertyLevelUpgradeJson[] rolePropertyUpgrades;
    public StarUpgradeJson[] roleStarUpgrades;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static RoleJson FromJson(string json)
    {
        return JsonUtility.FromJson<RoleJson>(json);
    }
}

[Serializable]
public class IdentityJson
{
    public int roleId;
    public string roleName;
}

[Serializable]
public class PropertyJson
{
    public float health;
    public float energy;
    public float attack;
    public float defense;
    public float speed;
    public float cooldownReduction;
    public float skillCooldown;
    public float ultimateCooldown;
}

[Serializable]
public class PropertyLevelJson
{
    public int healthLevel;
    public int energyLevel;
    public int defenseLevel;
    public int cooldownReductionLevel;
}

[Serializable]
public class LevelJson
{
    public PropertyLevelJson propertyLevel;
    public int starLevel;
}

[Serializable]
public class PropertyLevelUpgradeJson
{
    public int healthPerLevel;
    public int energyPerLevel;
    public int defensePerLevel;
    public int cooldownReductionPerLevel;
}

[Serializable]
public class StarUpgradeJson
{
    public float healthPerStar;
    public float energyPerStar;
    public float attackPerStar;
    public float defensePerStar;
    public float speedPerStar;
    public float cooldownReductionPerStar;
    public float skillCooldownPerStar;
    public float ultimateCooldownPerStar;
}