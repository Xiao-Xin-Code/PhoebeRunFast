

using System.Collections.Generic;
using DG.Tweening;
using QMVC;
using UnityEngine;
using UnityEngine.EventSystems;

public class LotteryResultController : BaseController
{
	[SerializeField] LotteryResultView _view;


    MonoPool<PrizeController> prizePool;



    protected override void OnInit()
    {
        base.OnInit();

        _view.RegisterSkipPressed(OnSkipPressed);
        _view.RegisterPointerDownEvent(OnPointerDown);
        _view.RegisterPointerUpEvent(OnPointerUp);
        this.RegisterEvent<ToShowLotteryResultEvent>(OnShowLotteryResult);

        Transform poolParent = new GameObject("Pool").transform;
        PrizeController prize = Resources.Load<PrizeController>("PrizeCard");
        prizePool = new MonoPool<PrizeController>(prize, poolParent);

        gameObject.SetActive(false);
    }

    Sequence showSequence;

    HashSet<PrizeController> prizesCache = new HashSet<PrizeController>();


    public void OnShowLotteryResult(ToShowLotteryResultEvent e)
    {
        prizePool.RecycleAll(OnPrizeRecycle);
        _view.SetOverActive(false);
        _view.SetSkipActive(false);
        gameObject.SetActive(true);

        //TODO: 根据传递来的数据量，确定生成多少来展示结果

        //先进行卡片移动动画，结束后再开启可点击
        

        int count = 10;

        showSequence = DOTween.Sequence();

        for (int i = 0; i < count; i++)
        {
            PrizeController prize = prizePool.Get();
            prizesCache.Add(prize);
            prize.transform.SetParent(_view.ResultParent, false);
            prize.transform.rotation = Quaternion.Euler(0, 0, 0);
            prize.transform.position = _view.StartRect.position;
            showSequence.Append(prize.transform.DOMove(_view.Points[i].position, 0.5f));
        }
        showSequence.OnComplete(() =>
        {
            showSequence = null;
            //发牌完成 开启可点击
            _view.SetSkipActive(false);
            SetPrizesCacheCanOperation();
           
        });
        showSequence.Play();
    }

    private void OnPrizeRecycle(PrizeController prize)
    {
        prize.Init(false);
    }

    private void OnSkipPressed()
    {
        if(showSequence != null)
        {
            //如果正在播放发牌动画，直接跳过发牌，直接显示发完状态
            showSequence.Kill(true);
            showSequence = null;
        }
        else
        {
            //直接跳过翻牌，直接显示结果
            SkipPrizesCacheFlip();
            _view.SetOverActive(true);
        }
         _view.SetSkipActive(false);
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("_______OnPointerDown");
        //在发牌阶段，点击空白，开启跳过键
        //在翻牌阶段，点击空白，再次开启跳过键
        //在全部翻完阶段，点击空白，关闭抽奖结果界面
        if(showSequence != null)
        {
            //如果正在播放发牌动画，点击空白，关闭抽奖结果界面
            _view.SetSkipActive(true);
        }
        else
        {
            if(!_view.GetOverActive())
            {
                //如果发牌动画结束，点击空白，开启跳过键
                _view.SetSkipActive(true);
            }
            else
            {
                //如果全部翻完，点击空白，关闭抽奖结果界面
                Close();
            }
        }






    }

    private void OnPointerUp(PointerEventData eventData)
    {
        
    }


    /// <summary>
    /// 恢复奖品的可操作状态
    /// </summary>
    private void SetPrizesCacheCanOperation()
    {
        foreach(PrizeController prize in prizesCache)
        {
            prize.CanOperation = true;
        }
    }

    private void SkipPrizesCacheFlip()
    {
        foreach(PrizeController prize in prizesCache)
        {
            prize.SkipFlip();
        }
    }

    private void Close()
    {
        //清空缓存
        prizesCache.Clear();
        //关闭抽奖结果界面
        prizePool.RecycleAll(OnPrizeRecycle);
        _view.SetOverActive(false);
        _view.SetSkipActive(false);
        gameObject.SetActive(false);
    }





    protected override void OnDeInit()
    {
        base.OnDeInit();
        prizePool.RecycleAll();
        showSequence?.Kill(true);
        showSequence = null;
        _view.UnRegisterSkipPressed(OnSkipPressed);
        this.UnRegisterEvent<ToShowLotteryResultEvent>(OnShowLotteryResult);
    }
}