
public class StarSound : GoodsBase
{
    public Goods Goods => Goods.Resource;
    public int Amount => amount;
    public int MaxAmount => maxAmount;

    public void SetAmount(int newAmount)
    {
        amount = newAmount;
    }
}