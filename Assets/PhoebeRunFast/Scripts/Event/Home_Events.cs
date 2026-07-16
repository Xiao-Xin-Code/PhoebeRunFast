
#region 小鸟事件

/// <summary>
/// 小鸟上升事件
/// </summary>
public class BirdUpEvent
{

}

#endregion



public class SignLoginActiveEvent
{
    public bool isActive;

    public SignLoginActiveEvent(bool isActive)
    {
        this.isActive = isActive;
    }
}