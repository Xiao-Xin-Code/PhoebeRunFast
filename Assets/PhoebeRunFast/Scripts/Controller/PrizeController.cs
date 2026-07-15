using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrizeController : BaseController
{
    [SerializeField] PrizeView _view;

    PrizeEntity _entity;

    public bool CanOperation {get=>_entity.canOperation;set=>_entity.canOperation = value;}

    protected override void OnInit()
    {
        base.OnInit();
        _entity = new PrizeEntity();
        _view.RegisterPointerDown(OnPointerDown);
        _view.RegisterPointerUp(OnPointerUp);

    }



    private void OnPointerDown(PointerEventData eventData)
    {
        if(!_entity.canOperation) return;

        //还未进行翻转
        if (!_entity.isTriggerFlip)
        {
            _entity.canOperation = false;
            _entity.isTriggerFlip = true;
            ShakeAndFlip();
        }

        if(_entity.isFlipped)
        {
           //进入详细介绍 
        }
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        
    }

    Sequence mainSequence;

    private void ShakeAndFlip()
    {
        mainSequence = DOTween.Sequence();
        Sequence shakeSequence = _view.ShakeSequence();
        Sequence flipSequence = _view.FlipSequence();
        mainSequence.Append(shakeSequence);
        mainSequence.Append(flipSequence);
        mainSequence.OnComplete(() =>
        {
            _entity.isFlipped = true;
            _entity.canOperation = true;
        });
        mainSequence.Play();
    }

    /// <summary>
    /// 恢复最初标记状态
    /// </summary>
    public void Init(bool canOperation = true,bool isFlipped = false)
    {
        _entity.canOperation = canOperation;
        _entity.isTriggerFlip = isFlipped;
        _entity.isFlipped = isFlipped;
        _view.SetState(isFlipped);
    }


    /// <summary>
    /// 跳过翻牌，直接显示结果状态
    /// </summary>
    public void SkipFlip()
    {
        if(_entity.isFlipped) return;
        if(_entity.isTriggerFlip)
        {
            mainSequence.Kill(true);
            mainSequence = null;
        }
        else
        {
            _entity.canOperation = true;
            _entity.isTriggerFlip = true;
            _entity.isFlipped = true;
            _view.SetState(true);
        }
    }


    protected override void OnDeInit()
    {
        base.OnDeInit();
        _view.UnRegisterPointerDown(OnPointerDown);
        _view.UnRegisterPointerUp(OnPointerUp);
    }

}