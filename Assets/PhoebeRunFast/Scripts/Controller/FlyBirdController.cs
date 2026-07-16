using System.Collections;
using System.Collections.Generic;
using Frame;
using QMVC;
using UnityEngine;

/// <summary>
/// 飞行小鸟控制器
/// </summary>
public class FlyBirdController : BaseController
{
	/// <summary>
	/// 管道对象池
	/// </summary>
	MonoPool<PipeController> pipePool;

	[SerializeField] PipeController pipe;

	[SerializeField] FlyBirdView _view;

	FlyBirdEntity _entity;

	[SerializeField] BirdController bird;

	/// <summary>
	/// 获取管道对象池
	/// </summary>
	public MonoPool<PipeController> PipePool => pipePool;

	/// <summary>
	/// 获取管道回收点
	/// </summary>
	public RectTransform PipeRecyclePoint => _view.PipeRecyclePoint;

	/// <summary>
	/// 飞行小鸟状态
	/// </summary>
	public FlyBirdState FlyBirdState { get => _entity.FlyBirdState.Value; set => _entity.FlyBirdState.Value = value; }

	Coroutine coroutine;
	
	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();
		pipe.SetFlyBird(this);
		Transform poolParent = new GameObject("Pool").transform;
		pipePool = new MonoPool<PipeController>(pipe, poolParent);

		_entity = new FlyBirdEntity();

		// 注册状态变化事件
		_entity.FlyBirdState.Register(OnFlyBirdStateChanged);

		// 注册输入事件
		this.RegisterEvent<BirdInputDownEvent>(OnStartInput);
	}

	public void AddCurScore()
	{
		_entity.currentScore++;
		Debug.Log(_entity.currentScore);
		_view.SetScore(_entity.currentScore);
	}

	public void SetMaxScore(int maxScore)
	{
		_entity.maxScore = maxScore;
		_view.SetMaxScore(maxScore);
	}



	/// <summary>
	/// 生成管道
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator PipeSpawn()
	{
		while (true)
		{
			Debug.Log("创建");
			PipeController pipe = pipePool.Get();
			pipe.transform.SetParent(_view.PipeParent);
			//随机管道高度位置
			float y = Random.Range(-200, 200);
			Vector3 anchoredPosition = _view.PipPoint.anchoredPosition;
			anchoredPosition.y = y;
			pipe.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;	
			pipe.SetPipeGap(Random.Range(300, 800));
			MonoService.Instance.AddUpdateListener(pipe.OnPipeMove);
			yield return new WaitForSeconds(2f);
		}
	}

	/// <summary>
	/// 飞行小鸟状态变化事件
	/// </summary>
	/// <param name="newState">新状态</param>
	private void OnFlyBirdStateChanged(FlyBirdState newState)
	{
		switch (newState)
		{
			case FlyBirdState.Ready:
				//等待开始
				_entity.currentScore = 0;
				break;
			case FlyBirdState.Run:
				bird.SetIsKinematic(false);
				this.SendCommand<BirdInputDownCommand>();
				if (coroutine != null)
				{
					MonoService.Instance.StopCoroutine(coroutine);
					coroutine = null;
				}
				//开始生成等方法
				coroutine = MonoService.Instance.StartCoroutine(PipeSpawn());
				break;
			case FlyBirdState.Over:
				//更新最高分
				//回收所有pool数据
				//移除绑定方法
				//恢复到初始状态
				//转到Ready
				bird.SetIsKinematic(true);
				bird.StateInit();
				MonoService.Instance.StopCoroutine(coroutine);
				pipePool.RecycleAll(OnPipeRectcle);
				if(_entity.currentScore > _entity.maxScore)
				{
					_entity.maxScore = _entity.currentScore;
					SetMaxScore(_entity.maxScore);
				}
				_entity.FlyBirdState.Value = FlyBirdState.Ready;
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// 开始输入事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnStartInput(BirdInputDownEvent evt)
	{
		if(_entity.FlyBirdState.Value == FlyBirdState.Ready)
		{
			_entity.FlyBirdState.Value = FlyBirdState.Run;
		}
	}

	/// <summary>
	/// 管道回收
	/// </summary>
	/// <param name="pipe">管道控制器</param>
	private void OnPipeRectcle(PipeController pipe)
	{
		MonoService.Instance.RemoveUpdateListener(pipe.OnPipeMove);
	}

	/// <summary>
	/// 反初始化方法
	/// </summary>
	protected override void OnDeInit()
	{
		base.OnDeInit();
		MonoService.Instance.StopCoroutine(coroutine);
	}
}
