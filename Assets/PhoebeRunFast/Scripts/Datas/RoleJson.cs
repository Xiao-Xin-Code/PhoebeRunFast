
public class RoleJson
{
    public string roleId;
    public string roleInfo;
    public string roleAsset;
    public string roleLock;
    public string roleProperty;
    public string roleStarLevel;
    public string rolePropertyLevel;
    public string rolePropertyLevelCost;
    public string roleStarLevelCost;
    public string rolePropertyUpgrade;
    public string roleStarUpgrade;
}

/// <summary>
/// 角色信息
/// </summary>
public class InfoJson
{
    public string roleName;
    public string roleDesc;
}

/// <summary>
/// 角色资产
/// </summary>
public class AssetJson
{
    public string roleImg;
    public string roleModel;
}

/// <summary>
/// 角色锁定
/// </summary>
public class LockJson
{
    public bool isUnlock;
    public string[] unlockCondition;
}

/// <summary>
/// 角色属性
/// </summary>
public class PropertyJson
{
    public float health;
    public float energy;
    public float attack;
    public float defense;
    public float speed;
    public float cooldownReduction;
}

/// <summary>
/// 等级
/// </summary>
public class LevelJson
{
    public int baseLevel;
    public int currentLevel;
    public int maxLevel;
}

/// <summary>
/// 属性等级
/// </summary>
public class PropertyLevelJson
{
    public LevelJson healthLevel;
    public LevelJson energyLevel;
    public LevelJson defenseLevel;
    public LevelJson cooldownReductionLevel;
}

/// <summary>
/// 星级等级
/// </summary>
public class StarLevelJson
{
    public LevelJson starLevel;
}


/// <summary>
/// 等级升级成本
/// </summary>
public class LevelCostJson
{
    public string[] goodsIds;
    public int[] amounts;
}


/// <summary>
/// 属性提升代价
/// </summary>
public class PropertyLevelCostJson
{
    public LevelCostJson[] healthLevelCosts;
    public LevelCostJson[] energyLevelCosts;
    public LevelCostJson[] defenseLevelCosts;
    public LevelCostJson[] cooldownReductionLevelCosts;
}

/// <summary>
/// 星级提升代价
/// </summary>
public class StarLevelCostJson
{
    public LevelCostJson[] starLevelCosts;
}

//提升
public class UpgradeJson
{
    public float healthUpgrade;
    public float energyUpgrade;
    public float attackUpgrade;
    public float defenseUpgrade;
    public float speedUpgrade;
    public float cooldownReductionUpgrade;
}

public class PropertyUpgradeJson
{
    public float[] healthUpgrade;
    public float[] energyUpgrade;
    public float[] defenseUpgrade;
    public float[] cooldownReductionUpgrade;
}

public class StarUpgradeJson
{
    public UpgradeJson[] upgradeJsons;
}


