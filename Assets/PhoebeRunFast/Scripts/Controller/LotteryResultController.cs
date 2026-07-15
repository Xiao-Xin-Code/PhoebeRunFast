

using DG.Tweening;
using QMVC;
using UnityEngine;
using UnityEngine.EventSystems;

public class LotteryResultController : BaseController,
    IPointerDownHandler,IPointerUpHandler
{
	[SerializeField] LotteryResultView _view;


    MonoPool<PrizeController> prizePool;



    protected override void OnInit()
    {
        base.OnInit();

        _view.RegisterSkipPressed(OnSkipPressed);
        this.RegisterEvent<ToShowLotteryResultEvent>(OnShowLotteryResult);

        Transform poolParent = new GameObject("Pool").transform;
        PrizeController prize = Resources.Load<PrizeController>("PrizeCard");
        prizePool = new MonoPool<PrizeController>(prize, poolParent);

        gameObject.SetActive(false);
    }

    Sequence showSequence;

    public void OnShowLotteryResult(ToShowLotteryResultEvent e)
    {
        gameObject.SetActive(true);

        //TODO: 根据传递来的数据量，确定生成多少来展示结果

        //先进行卡片移动动画，结束后再开启可点击

        prizePool.RecycleAll();

        int count = 10;

        showSequence = DOTween.Sequence();


        for (int i = 0; i < count; i++)
        {
            PrizeController prize = prizePool.Get();
            prize.transform.SetParent(_view.ResultParent, false);
            prize.transform.rotation = Quaternion.Euler(0, 0, 0);
            prize.transform.position = _view.StartRect.position;
            showSequence.Append(prize.transform.DOMove(_view.Points[i].position, 0.5f));
        }

        showSequence.Play();
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
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //在发牌阶段，点击空白，开启跳过键
        //在翻牌阶段，点击空白，再次开启跳过键
        //在全部翻完阶段，点击空白，关闭抽奖结果界面


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
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