//账户数据

public class AccountJson
{
    public string accountId;

    /// <summary>
    /// 用户信息路径
    /// </summary>
    public string accountInfo;

    /// <summary>
    /// 用户资源数据路径
    /// </summary>
    public string accountGoods;

    /// <summary>
    /// 角色数据路径
    /// </summary>
    public string accountRoles;
}


public class AccountInfo
{
    public string accountName;
    public string accountPassword;
}


public class AccountRole
{
    public string roleId;
    public int chainLevel;
    public RolePropertyLevel rolePropertyLevel;
}

public class RolePropertyLevel
{
    /// <summary>
    /// 生命值
    /// </summary>
    public int health;
    /// <summary>
    /// 能量值
    /// </summary>
    public int energy;
    /// <summary>
    /// 攻击力
    /// </summary>  
    public int attack;
    /// <summary>
    /// 防御力
    /// </summary>
    public int defense;
    /// <summary>
    /// 移动速度
    /// </summary>
    public int speed;
    /// <summary>
    /// 冷却缩减
    /// </summary>
    public int cooldownReduction;
    /// <summary>
    /// 能量恢复速率
    /// </summary>
    public int energyRecoveryRate;
}


public class AccountGoods
{
    public string goodsId;
    public int count;
}