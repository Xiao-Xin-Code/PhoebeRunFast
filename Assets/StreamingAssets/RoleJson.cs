
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


public class InfoJson
{
    public string roleName;
    public string roleDesc;
}

public class AssetJson
{
    public string roleImg;
    public string roleModel;
}

public class LockJson
{
    public bool isUnlock;
    public string[] unlockCondition;
}


public class PropertyJson
{
    public float health;
    public float energy;
    public float attack;
    public float defense;
    public float speed;
    public float cooldownReduction;
}


public class LevelJson
{
    public int baseLevel;
    public int currentLevel;
    public int maxLevel;
}

public class PropertyLevelJson
{
    public LevelJson healthLevel;
    public LevelJson energyLevel;
    public LevelJson defenseLevel;
    public LevelJson cooldownReductionLevel;
}

public class StarLevelJson
{
    public LevelJson starLevel;
}

