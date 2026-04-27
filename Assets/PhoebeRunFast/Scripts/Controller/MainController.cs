using System.Collections;
using System.IO;
using System.Threading.Tasks;
using QMVC;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// 主控制器
/// </summary>
public class MainController : BaseController
{

	GlobalSystem _globalSystem;
	RoleSystem _roleSystem;

	/// <summary>
	/// 初始化方法
	/// </summary>
	protected override void OnInit()
	{
		base.OnInit();

		_globalSystem = this.GetSystem<GlobalSystem>();
		_globalSystem.SetMainSingleton(this);
		_roleSystem = this.GetSystem<RoleSystem>();

		// 注册系统事件
		this.RegisterEvent<LoadMainEvent>(OnLoadMain);
		this.RegisterEvent<UnLoadMainEvent>(OnUnLoadMain);
		this.RegisterEvent<MainInitByTransitionOverEvent>(OnMainInitByTransitionOver);
	}


	//TODO: 加载Roles文件



	/// <summary>
	/// 主界面初始化完成事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnMainInitByTransitionOver(MainInitByTransitionOverEvent evt)
	{
		
	}

	/// <summary>
	/// 加载主界面事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnLoadMain(LoadMainEvent evt)
	{
		evt.enumerator = MainAssetLoad();
	}

	/// <summary>
	/// 卸载主界面事件
	/// </summary>
	/// <param name="evt">事件参数</param>
	private void OnUnLoadMain(UnLoadMainEvent evt)
	{
		evt.enumerator = MainAssetUnLoad();
	}

	/// <summary>
	/// 加载主界面资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator MainAssetLoad()
	{
		string rolesPath = Path.Combine(Application.streamingAssetsPath, "RoleTable/RoleTable.json");
		string goodsPath = Path.Combine(Application.streamingAssetsPath, "GoodsTable/GoodsTable.json");
		Task<string> rolesContent = File.ReadAllTextAsync(rolesPath);
		Task<string> goodsContent = File.ReadAllTextAsync(goodsPath);
		Task total = Task.WhenAll(rolesContent, goodsContent);
		yield return new WaitUntil(() => total.IsCompleted);
		RoleJson[] roles = JsonConvert.DeserializeObject<RoleJson[]>(rolesContent.Result);
		GoodsJson[] goods = JsonConvert.DeserializeObject<GoodsJson[]>(goodsContent.Result);
		_globalSystem.GlobalModel.SetRoleJsons(roles);
		_globalSystem.GlobalModel.SetGoodsJsons(goods);
		_roleSystem.SetRoleJsons(roles);
		yield return new WaitForSeconds(1f);
		this.SendCommand(new InitCharacterCommand(_globalSystem.GlobalModel.OutRoleTableId.Value));
	}

	/// <summary>
	/// 卸载主界面资源
	/// </summary>
	/// <returns>协程</returns>
	IEnumerator MainAssetUnLoad()
	{
		yield return new WaitForSeconds(1f);
	}

	/// <summary>
	/// 反初始化方法
	/// </summary>
	protected override void OnDeInit()
	{
		base.OnDeInit();
		_globalSystem.SetMainSingleton(null);

		// 注销事件
		this.UnRegisterEvent<LoadMainEvent>(OnLoadMain);
		this.UnRegisterEvent<UnLoadMainEvent>(OnUnLoadMain);
		this.UnRegisterEvent<MainInitByTransitionOverEvent>(OnMainInitByTransitionOver);
	}

}
