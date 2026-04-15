using System.Collections;
using System.Collections.Generic;
using Frame;
using QMVC;
using UnityEngine;

public class FlyBirdController : BaseController
{
	MonoPool<PipeController> pipePool;

	[SerializeField] PipeController pipe;

	[SerializeField] FlyBirdView _view;

	FlyBirdEntity _entity;


	[SerializeField] BirdController bird;


	public MonoPool<PipeController> PipePool => pipePool;

	public RectTransform PipeRecyclePoint => _view.PipeRecyclePoint;

	public FlyBirdState FlyBirdState { get => _entity.FlyBirdState.Value; set => _entity.FlyBirdState.Value = value; }


	Coroutine coroutine;
	

	protected override void OnInit()
	{
		base.OnInit();
		pipe.SetFlyBird(this);
		Transform poolParent = new GameObject("Pool").transform;
		pipePool = new MonoPool<PipeController>(pipe, poolParent);

		_entity = new FlyBirdEntity();

		_entity.FlyBirdState.Register(OnFlyBirdStateChanged);



		this.RegisterEvent<BirdInputDownEvent>(OnStartInput);
		

	}


	IEnumerator PipeSpawn()
	{
		while (true)
		{
			Debug.Log("创建");
			PipeController pipe = pipePool.Get();
			pipe.transform.SetParent(_view.PipeParent);
			pipe.transform.position = _view.PipPoint.position;
			MonoService.Instance.AddUpdateListener(pipe.OnPipeMove);
			yield return new WaitForSeconds(1f);
		}
	}



	private void OnFlyBirdStateChanged(FlyBirdState newState)
	{

		switch (newState)
		{
			case FlyBirdState.Ready:
				//等待开始
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
				_entity.FlyBirdState.Value = FlyBirdState.Ready;


				break;
			default:
				break;
		}
	}



	private void OnStartInput(BirdInputDownEvent evt)
	{
		if(_entity.FlyBirdState.Value == FlyBirdState.Ready)
		{
			_entity.FlyBirdState.Value = FlyBirdState.Run;
		}
	}



	private void OnPipeRectcle(PipeController pipe)
	{
		MonoService.Instance.RemoveUpdateListener(pipe.OnPipeMove);
	}


	protected override void OnDeInit()
	{
		base.OnDeInit();
		MonoService.Instance.StopCoroutine(coroutine);
	}
}
