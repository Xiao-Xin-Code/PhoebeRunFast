
public abstract class GoodsBase
{
    /// <summary>
    /// 物品ID
    /// </summary>
    protected string goodsId;
    /// <summary>
    /// 物品稀有度
    /// </summary>
    protected Rarity rarity;
    /// <summary>
    /// 物品类型
    /// </summary>
    protected GoodsType type;
    /// <summary>
    /// 物品名称
    /// </summary>
    protected string goodsName;
    /// <summary>
    /// 物品描述
    /// </summary>
    protected string goodsDesc;

    /// <summary>
    /// 物品数量
    /// </summary>
	protected int amount;
    /// <summary>
    /// 物品最大数量
    /// </summary>
    protected int maxAmount = int.MaxValue;
}