using UnityEngine;

[System.Serializable]
public class RoleData
{
    public RoleIdentity identity;
    public RoleLevel level;
    public RoleProperty baseProperty;
    public RolePropertyLevelUpgrade[] rolePropertyUpgrades;
    public RoleStarUpgrade[] roleStarUpgrades;
}

[System.Serializable]
public class RoleIdentity
{
    public int roleId;
    public int roleName;
}

[System.Serializable]
public class RoleProperty
{
    public float health;//生命值
    public float energy;//能量值
    public float attack;//攻击值
    public float defense;//防御值
    public float speed;//速度
    public float cooldownReduction;//冷却缩减
    public float skillCooldown;//技能冷却时间
    public float ultimateCooldown;//最终技能冷却时间
}

[System.Serializable]
public class RolePropertyLevel
{
    public int healthLevel;//生命等级
    public int energyLevel;//能量等级
    public int defenseLevel;//防御等级
    public int cooldownReductionLevel;//冷却缩减
}

[System.Serializable]
public class RoleLevel
{
    public RolePropertyLevel propertyLevel;
    public int starLevel;
}

[System.Serializable]
public class RolePropertyLevelUpgrade
{
    public int healthPerLevel;//生命等级升级
    public int energyPerLevel;//能量等级升级
    public int defensePerLevel;//防御等级升级
    public int cooldownReductionPerLevel;//冷却缩减等级升级
}

[System.Serializable]
public class RoleStarUpgrade
{
    public float healthPerStar;//生命值星级升级
    public float energyPerStar;//能量星级升级
    public float attackPerStar;//攻击值星级升级
    public float defensePerStar;//防御星级升级  
    public float speedPerStar;//速度星级升级
    public float cooldownReductionPerStar;//冷却缩减星级升级
    public float skillCooldownPerStar;//技能冷却时间星级升级
    public float ultimateCooldownPerStar;//最终技能冷却时间星级升级
}