using UnityEngine;
using QMVC;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class RoleSystem : AbstractSystem
{
	/// <summary>
	/// 角色数据
	/// </summary>
    Dictionary<string, RoleJson> roleJsons = new Dictionary<string, RoleJson>();

	/// <summary>
	/// 角色信息
	/// </summary>
	Dictionary<string, InfoJson> infoJsons = new Dictionary<string, InfoJson>();
	/// <summary>
	/// 角色资产
	/// </summary>
	Dictionary<string, AssetJson> assetJsons = new Dictionary<string, AssetJson>();
	/// <summary>
	/// 角色解锁信息
	/// </summary>
	Dictionary<string, LockJson> lockJsons = new Dictionary<string, LockJson>();
	/// <summary>
	/// 角色基础属性
	/// </summary>
	Dictionary<string, PropertyJson> propertyJsons = new Dictionary<string, PropertyJson>();
	/// <summary>
	/// 角色共鸣链
	/// </summary>
	Dictionary<string, ChainLevelJson> chainLevelJsons = new Dictionary<string, ChainLevelJson>();
	/// <summary>
	/// 角色属性等级
	/// </summary>
	Dictionary<string, PropertyLevelJson> propertyLevelJsons = new Dictionary<string, PropertyLevelJson>();
	/// <summary>
	/// 角色共鸣链等级升级成本
	/// </summary>
	Dictionary<string, ChainLevelCostJson> chainLevelCostJsons = new Dictionary<string, ChainLevelCostJson>();
	/// <summary>
	/// 角色属性等级升级成本
	/// </summary>
	Dictionary<string, PropertyLevelCostJson> propertyLevelCostJsons = new Dictionary<string, PropertyLevelCostJson>();
	/// <summary>
	/// 角色共鸣链等级升级带来的属性提升
	/// </summary>
	Dictionary<string, ChainUpgradeJson> chainUpgradeJsons = new Dictionary<string, ChainUpgradeJson>();
	/// <summary>
	/// 角色属性等级升级带来的属性提升
	/// </summary>
	Dictionary<string, PropertyUpgradeJson> propertyUpgradeJsons = new Dictionary<string, PropertyUpgradeJson>();
	

	Dictionary<string, RoleController> roles = new Dictionary<string, RoleController>();



	Transform rolesParent;

	public void SetRoleJsons(RoleJson[] jsons)
	{
		roleJsons = new Dictionary<string, RoleJson>();
		foreach (var item in jsons)
		{
			roleJsons.Add(item.roleId, item);
		}
	}

	public void RecycleRole(RoleController roleController)
	{
		roleController.gameObject.SetActive(false);
		roleController.transform.SetParent(rolesParent);
	}

	public void RecycleAllRole()
	{
		foreach (var item in roles) 
		{
			item.Value.gameObject.SetActive(false);
			item.Value.transform.SetParent(rolesParent);
		}
	}

	public async Task<RoleController> GetRole(string roleId)
    {
        if (roles.ContainsKey(roleId))
        {
			roles[roleId].gameObject.SetActive(true);
            return roles[roleId];
        }
        else
        {
			AssetJson data = await GetAsset(roleId);
			if (data == null) return null;
			TaskCompletionSource<RoleController> task = new TaskCompletionSource<RoleController>();
			ResourceRequest request = Resources.LoadAsync<RoleController>(data.roleModel);
			request.completed += e =>
			{
				task.SetResult(request.asset as RoleController);
			};
			RoleController controller = await task.Task;
			if (controller != null)
			{
				RoleController result = GameObject.Instantiate(controller, rolesParent);
				roles.Add(roleId, result);
				result.gameObject.SetActive(true);
				return result;
			}
			return null;
        }
    }

    public async Task<InfoJson> GetInfo(string roleId)
    {
		if (infoJsons.ContainsKey(roleId)) return infoJsons[roleId];
        if(roleJsons.TryGetValue(roleId,out RoleJson json))
        {
            string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleInfo);
			if (File.Exists(path))
			{
				string content = await File.ReadAllTextAsync(path);
				InfoJson info = JsonConvert.DeserializeObject<InfoJson>(content);
				if (info != null)
				{
					infoJsons.Add(roleId, info);
				}
				return info;
			}
        }
        return null;
    }

	public async Task<AssetJson> GetAsset(string roleId)
	{
		if (assetJsons.ContainsKey(roleId)) return assetJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleAsset);
			if (File.Exists(path))
			{
				string content = await File.ReadAllTextAsync(path);
				AssetJson asset = JsonConvert.DeserializeObject<AssetJson>(content);
				if (asset != null) assetJsons.Add(roleId, asset);
				return asset;
			}
		}
		return null;
	}

	public async Task<LockJson> GetLock(string roleId)
	{
		if (lockJsons.ContainsKey(roleId)) return lockJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleLock);
			if (File.Exists(path))
			{
				string content = await File.ReadAllTextAsync(path);
				LockJson lockJson = JsonConvert.DeserializeObject<LockJson>(content);
				if (lockJson != null) lockJsons.Add(roleId, lockJson);
				return lockJson;
			}
		}
		return null;
	}

	public async Task<PropertyJson> GetProperty(string roleId)
	{
		if (propertyJsons.ContainsKey(roleId)) return propertyJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleProperty);
			if (File.Exists(path))
			{
				string content = await File.ReadAllTextAsync(path);
				PropertyJson property = JsonConvert.DeserializeObject<PropertyJson>(content);
				if (property != null) propertyJsons.Add(roleId, property);
				return property;
			}
		}
		return null;
	}

	public async Task<ChainLevelJson> GetChainLevel(string roleId)
	{
		if (chainLevelJsons.ContainsKey(roleId)) return chainLevelJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleChainLevel);
			if (File.Exists(path))
			{
				string content = await File.ReadAllTextAsync(path);
				ChainLevelJson chainLevel = JsonConvert.DeserializeObject<ChainLevelJson>(content);
				if (chainLevel != null) chainLevelJsons.Add(roleId, chainLevel);
				return chainLevel;
			}
		}
		return null;
	}

	public async Task<PropertyLevelJson> GetPropertyLevel(string roleId)
	{
		if (propertyLevelJsons.ContainsKey(roleId)) return propertyLevelJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.rolePropertyLevel);
			if (File.Exists(path))
			{
				string content = await File.ReadAllTextAsync(path);
				PropertyLevelJson propertyLevel = JsonConvert.DeserializeObject<PropertyLevelJson>(content);
				if (propertyLevel != null) propertyLevelJsons.Add(roleId, propertyLevel);
				return propertyLevel;
			}
		}
		return null;
	}

	public async Task<PropertyLevelCostJson> GetPropertyLevelCost(string roleId)
	{
		if (propertyLevelCostJsons.ContainsKey(roleId)) return propertyLevelCostJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			if (!string.IsNullOrEmpty(json.rolePropertyLevelCost))
			{
				string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.rolePropertyLevelCost);
				if (File.Exists(path))
				{
					string content = await File.ReadAllTextAsync(path);
					PropertyLevelCostJson propertyLevelCost = JsonConvert.DeserializeObject<PropertyLevelCostJson>(content);
					if (propertyLevelCost != null) propertyLevelCostJsons.Add(roleId, propertyLevelCost);
					return propertyLevelCost;
				}
			}
		}
		return null;
	}

	public async Task<ChainLevelCostJson> GetChainLevelCost(string roleId)
	{
		if (chainLevelCostJsons.ContainsKey(roleId)) return chainLevelCostJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			if (!string.IsNullOrEmpty(json.roleChainLevelCost))
			{
				string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleChainLevelCost);
				if (File.Exists(path))
				{
					string content = await File.ReadAllTextAsync(path);
					ChainLevelCostJson chainLevelCost = JsonConvert.DeserializeObject<ChainLevelCostJson>(content);
					if (chainLevelCost != null) chainLevelCostJsons.Add(roleId, chainLevelCost);
					return chainLevelCost;
				}
			}
		}
		return null;
	}

	public async Task<PropertyUpgradeJson> GetPropertyUpgrade(string roleId)
	{
		if (propertyUpgradeJsons.ContainsKey(roleId)) return propertyUpgradeJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			if (!string.IsNullOrEmpty(json.rolePropertyUpgrade))
			{
				string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.rolePropertyUpgrade);
				if (File.Exists(path))
				{
					string content = await File.ReadAllTextAsync(path);
					PropertyUpgradeJson propertyUpgrade = JsonConvert.DeserializeObject<PropertyUpgradeJson>(content);
					if (propertyUpgrade != null) propertyUpgradeJsons.Add(roleId, propertyUpgrade);
					return propertyUpgrade;
				}
			}
		}
		return null;
	}

	public async Task<ChainUpgradeJson> GetChainUpgrade(string roleId)
	{
		if (chainUpgradeJsons.ContainsKey(roleId)) return chainUpgradeJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			if (!string.IsNullOrEmpty(json.roleChainUpgrade))
			{
				string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleChainUpgrade);
				if (File.Exists(path))
				{
					string content = await File.ReadAllTextAsync(path);
					ChainUpgradeJson chainUpgrade = JsonConvert.DeserializeObject<ChainUpgradeJson>(content);
					if (chainUpgrade != null) chainUpgradeJsons.Add(roleId, chainUpgrade);
					return chainUpgrade;
				}
			}
		}
		return null;
	}


	public async Task UpdateChainLevel(string roleId)
	{
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleChainLevel);
		    ChainLevelJson data = await GetChainLevel(roleId);
			string content = JsonConvert.SerializeObject(data);
			await File.WriteAllTextAsync(path, content);
		}
	}

	public async Task UpdatePropertyLevel(string roleId)
	{
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.rolePropertyLevel);
			PropertyLevelJson data = await GetPropertyLevel(roleId);
			string content = JsonConvert.SerializeObject(data);
			await File.WriteAllTextAsync(path, content);
		}
	}



	protected override void OnInit()
    {
		rolesParent = new GameObject("Roles").transform;
    }
}