
public class Goods : GoodsBase
{
    public string Id => goodsId;
    public Rarity Rarity => rarity;
    public GoodsType Type => type;
    public int Amount => amount;
    public int MaxAmount => maxAmount;

    public string Name => goodsName;
    public string Description => goodsDesc;


    public void SetId(string id)
    {
        goodsId = id;
    }
    public void SetRarity(Rarity rarity)
    {
        this.rarity = rarity;
    }
    public void SetType(GoodsType type)
    {
        this.type = type;
    }

    //设置名称
    public void SetName(string name)
    {
        goodsName = name;
    }
    //设置描述
    public void SetDescription(string desc)
    {
        goodsDesc = desc;
    }


    /// <summary>
    /// 消耗
    /// </summary>
    /// <param name="amount"></param>
    public void Consume(int amount)
    {
        this.amount -= amount;
    }

    /// <summary>
    /// 增加
    /// </summary>
    /// <param name="amount"></param>
    public void Add(int amount)
    {
        this.amount += amount;
    }

}
