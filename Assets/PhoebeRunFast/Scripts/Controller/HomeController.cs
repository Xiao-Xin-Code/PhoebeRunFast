using System.Collections;
using System.IO;
using System.Threading.Tasks;
using QMVC;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// 主页控制器
/// </summary>
public class HomeController : BaseController
{
    [SerializeField] HomeView _view;

	GlobalSystem _globalSystem;
	AccountSystem _accountSystem;

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();

		_globalSystem = this.GetSystem<GlobalSystem>();
		_accountSystem = this.GetSystem<AccountSystem>();
		_globalSystem.SetHomeSingleton(this);



		// 注册系统事件
		this.RegisterEvent<LoadHomeEvent>(OnLoadHome);
		this.RegisterEvent<UnLoadHomeEvent>(OnUnLoadHome);
		this.RegisterEvent<HomeInitByTransitionOverEvent>(OnHomeInitByTransitionOver);

	}

	//TODO: 加载管道与Bird资源
	//TODO: 加载界面资源
	

	/// <summary>
	/// 主页初始化完成事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnHomeInitByTransitionOver(HomeInitByTransitionOverEvent evt)
	{
		
	}

	/// <summary>
	/// 加载主页事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnLoadHome(LoadHomeEvent evt)
	{
		evt.enumerator = HomeAssetLoad();
	}

	/// <summary>
	/// 卸载主页事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnUnLoadHome(UnLoadHomeEvent evt)
	{
		evt.enumerator = HomeAssetUnLoad();
	}

	/// <summary>
	/// 加载主页资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator HomeAssetLoad()
	{
		_globalSystem.BootSingleton.SetMaskVisible(false);
		string accountPath = Path.Combine(Application.streamingAssetsPath, "AccountTable/AccountTable.json");
		string userPath = Path.Combine(Application.streamingAssetsPath, "AccountTable/UserCache.json");

		if (!File.Exists(accountPath))
		{
			Debug.LogError("账户信息丢失");
			yield break;
		}
		if (File.Exists(userPath))
		{
			Task<string> userContent = File.ReadAllTextAsync(userPath);
			yield return new WaitUntil(() => userContent.IsCompleted);
			if (userContent.Result != null)
			{
				UserJson userJson = JsonConvert.DeserializeObject<UserJson>(userContent.Result);
				_globalSystem.GlobalModel.SetUserJson(userJson);
			}
		}


		//直接加载完善用户数据
		Task<string> accountContent = File.ReadAllTextAsync(accountPath);
		yield return new WaitUntil(() => accountContent.IsCompleted);

		if(accountContent.Result == null)
		{
			Debug.LogError("账户信息丢失");
			yield break;
		}

		if (accountContent.Result != null)
		{
			//加载全部的info
			AccountJson[] accountJsons = JsonConvert.DeserializeObject<AccountJson[]>(accountContent.Result);
			if (accountJsons != null && accountJsons.Length > 0)
			{
				_globalSystem.GlobalModel.SetAccountJsons(accountJsons);
				_accountSystem.SetAccountJsons(accountJsons);

				for (int i = 0; i < accountJsons.Length; i++)
				{
					Task<AccountInfo> accountInfoTask = _accountSystem.GetInfo(accountJsons[i].accountId);
					yield return new WaitUntil(() => accountInfoTask.IsCompleted);
					AccountInfo accountInfo = accountInfoTask.Result;
				}
			}

			
		}
	}

	/// <summary>
	/// 卸载主页资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator HomeAssetUnLoad()
	{
		yield return new WaitForSeconds(1f);
	}

	/// <summary>
	/// 反初始化方法
	/// </summary>
	protected override void OnDeInit()
	{
		base.OnDeInit();
		_globalSystem.SetHomeSingleton(null);

		// 注销事件
		this.UnRegisterEvent<LoadHomeEvent>(OnLoadHome);
		this.UnRegisterEvent<UnLoadHomeEvent>(OnUnLoadHome);
		this.UnRegisterEvent<HomeInitByTransitionOverEvent>(OnHomeInitByTransitionOver);
	}

}
