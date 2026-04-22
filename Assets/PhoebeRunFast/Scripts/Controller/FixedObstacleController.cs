
public class FixedObstacleController : ObstacleController
{
    [SerializeField] FixedObstacleView _view;

    public override void Init()
    {
        base.Init();

        _view.ColliderArea.RegisterEnterEvent(TakeDamage);
    }


    public void OnEnter()
    {
        TakeDamage();
        SlowDown();
    }

    private void TakeDamage()
    {
        
    }

    private void SlowDown()
    {
        
    }




}