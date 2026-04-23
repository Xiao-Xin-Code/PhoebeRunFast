
using UnityEngine;

public class FixedObstacleController : ObstacleController
{
    [SerializeField] FixedObstacleView _view;


    protected override void OnInit()
    {
        base.OnInit();

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