
public class RoleJson
{
    public string roleId;
    public string roleInfo;
    public string roleAsset;
    public string roleLock;
    public string roleProperty;
    public string roleStarLevel;
    public string rolePropertyLevel;
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
public class LevelCost
{
    public Goods[] types;
    public int[] amounts;
}