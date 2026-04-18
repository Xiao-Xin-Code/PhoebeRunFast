using System.Collections;
using QMVC;
using UnityEngine;

/// <summary>
/// 技能控制器
/// </summary>
public class SkillsController : BaseController
{
    [SerializeField] SkillsView _view;

    SkillsEntity _entity;


    /// <summary>
    /// 初始化方法
    /// </summary>
    protected override void OnInit()
    {
        base.OnInit();
        _entity = new SkillsEntity();

        _view.RegisterSkillPressed(OnSkillPressed);
        _view.RegisterUltimatePressed(OnUltimatePressed);
    }

    private void OnSkillPressed()
    {
        

        //启动状态检测

    }

    private void OnUltimatePressed()
    {
        
    }

    IEnumerator OnSkillCoolDown()
    {
        float cooldown = _entity.skillCooldown;
        _view.UpdateSkillCooldown(1);
        while (cooldown > 0)
        {
            float parent = cooldown/_entity.skillCooldown;
            _view.UpdateSkillCooldown(parent);
            yield return null;
        }
    }

    IEnumerator OnUltimateCoolDown()
    {
        float cooldown = _entity.ultimateCooldown;
        _view.UpdateUltimateCooldown(1);
        while (cooldown > 0)
        {
            float parent = cooldown/_entity.ultimateCooldown;
            _view.UpdateUltimateCooldown(parent);
            yield return null;
        }
    }


    IEnumerator OnNormalSkillState()
    {
        while (true)
        {
            //获取精力

            yield return null;
        }
    }



    /// <summary>
    /// 反初始化方法
    /// </summary>
    protected override void OnDeInit()
    {
        base.OnDeInit();

        // 注销事件
        _view.UnRegisterSkillPressed(OnSkillPressed);
        _view.UnRegisterUltimatePressed(OnUltimatePressed);
    }
}
