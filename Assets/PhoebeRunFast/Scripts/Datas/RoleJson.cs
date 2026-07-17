

#region Base

/// <summary>
/// 特殊条件
/// </summary>
public class SpecialConditionJson
{
    /// <summary>
    /// 条件类型
    /// </summary>
    public string condition;
    /// <summary>
    /// 条件参数
    /// </summary>
    public string conditionParams;
}

/// <summary>
/// 消耗代价 主要是物品消耗
/// </summary>
public class CostJson
{
    public string goodsId;
    public int amount;
}

/// <summary>
/// 等级
/// </summary>
public class LevelJson
{
    public int baseLevel;
    public int maxLevel;
}


/// <summary>
/// 等级升级成本
/// </summary>
public class LevelCostJson
{
    public CostJson[] costJsons;
}


#endregion



public class RoleJson
{
    /// <summary>
    /// 角色ID
    /// </summary>
    public string roleId;
    /// <summary>
    /// 角色信息资源路径
    /// </summary>
    public string roleInfo;
    /// <summary>
    /// 角色资产资源路径
    /// </summary>
    public string roleAsset;
    /// <summary>
    /// 角色锁定资源路径
    /// </summary>
    public string roleLock;
    /// <summary>
    /// 角色属性资源路径
    /// </summary>
    public string roleProperty;
    /// <summary>
    /// 角色共鸣链等级资源路径
    /// </summary>
    public string roleChainLevel;
    /// <summary>
    /// 角色属性等级资源路径
    /// </summary>
    public string rolePropertyLevel;
    /// <summary>
    /// 角色共鸣链等级代价资源路径
    /// </summary>
    public string roleChainLevelCost;
    /// <summary>
    /// 角色属性等级代价资源路径
    /// </summary>
    public string rolePropertyLevelCost;
    /// <summary>
    /// 角色共鸣链等级提升资源路径
    /// </summary>
    public string roleChainUpgrade;
    /// <summary>
    /// 角色属性等级提升资源路径
    /// </summary>
    public string rolePropertyUpgrade;
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
    /// <summary>
    /// 解锁代价
    /// </summary>
    public CostJson[] unlockCosts;
    /// <summary>
    /// 解锁条件
    /// </summary>
    public SpecialConditionJson[] unlockConditionsJsons;
}

/// <summary>
/// 角色属性
/// </summary>
public class PropertyJson
{
    /// <summary>
    /// 生命值
    /// </summary>
    public float health;
    /// <summary>
    /// 能量值
    /// </summary>
    public float energy;
    /// <summary>
    /// 攻击力
    /// </summary>  
    public float attack;
    /// <summary>
    /// 防御力
    /// </summary>
    public float defense;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 冷却缩减
    /// </summary>
    public float cooldownReduction;
    /// <summary>
    /// 能量恢复速率
    /// </summary>
    public float energyRecoveryRate;
}


/// <summary>
/// 共鸣链等级
/// </summary>
public class ChainLevelJson
{
    public LevelJson chainLevel;
}

/// <summary>
/// 属性等级
/// </summary>
public class PropertyLevelJson
{
    public LevelJson healthLevel;
    public LevelJson energyLevel;
    public LevelJson attackLevel;
    public LevelJson speedLevel;
    public LevelJson defenseLevel;
    public LevelJson cooldownReductionLevel;
    public LevelJson energyRecoveryRateLevel;
}




/// <summary>
/// 共鸣链等级升级成本
/// </summary>
public class ChainLevelCostJson
{
    public LevelCostJson[] chainLevelCosts;
}

/// <summary>
/// 属性等级升级成本
/// </summary>
public class PropertyLevelCostJson
{
    public LevelCostJson[] healthLevelCosts;
    public LevelCostJson[] energyLevelCosts;
    public LevelCostJson[] attackLevelCosts;
    public LevelCostJson[] speedLevelCosts;
    public LevelCostJson[] defenseLevelCosts;
    public LevelCostJson[] cooldownReductionLevelCosts;
    public LevelCostJson[] energyRecoveryRateLevelCosts;
}



/// <summary>
/// 共鸣链等级提升带来的属性提升
/// </summary>
public class ChainUpgradeJson
{
    public PropertyJson[] upgradeJsons;
}

/// <summary>
/// 属性等级提升带来的属性提升
/// </summary>
public class PropertyUpgradeJson
{
    public float[] healthUpgrade;
    public float[] energyUpgrade;
    public float[] attackUpgrade;
    public float[] speedUpgrade;
    public float[] defenseUpgrade;
    public float[] cooldownReductionUpgrade;
    public float[] energyRecoveryRateUpgrade;
}






