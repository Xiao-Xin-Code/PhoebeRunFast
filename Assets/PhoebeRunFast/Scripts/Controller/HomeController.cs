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

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();

		_globalSystem = this.GetSystem<GlobalSystem>();
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
		string userPath = Path.Combine(Application.streamingAssetsPath, "User/UserJson.json");

		if (!File.Exists(userPath))
		{
			//无用户信息， 自动跳转到登陆
		}
		else
		{
			Task<string> userContent = File.ReadAllTextAsync(userPath);
			yield return new WaitUntil(() => userContent.IsCompleted);
			UserJson userJson = JsonConvert.DeserializeObject<UserJson>(userContent.Result);
			//userjson的用户ID数据合法

			Task<string> accountContent = File.ReadAllTextAsync(accountPath);
			yield return new WaitUntil(() => accountContent.IsCompleted);
			AccountJson accountJson = JsonConvert.DeserializeObject<AccountJson>(accountContent.Result);
			//accounttable的账户数据数据合法

			//判断是否用户数据在账户数据中
			//存在即可使用可直接进入游戏，如果不存在仍需要登陆


		}

		
		

		yield return new WaitForSeconds(1f);
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
