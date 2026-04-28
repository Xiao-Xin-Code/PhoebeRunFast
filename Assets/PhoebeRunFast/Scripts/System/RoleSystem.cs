using UnityEngine;
using QMVC;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using System.Threading.Tasks;

public class RoleSystem : AbstractSystem
{
    Dictionary<string, RoleJson> roleJsons = new Dictionary<string, RoleJson>();

	Dictionary<string, InfoJson> infoJsons = new Dictionary<string, InfoJson>();
	Dictionary<string, AssetJson> assetJsons = new Dictionary<string, AssetJson>();
	Dictionary<string, LockJson> lockJsons = new Dictionary<string, LockJson>();
	Dictionary<string, PropertyJson> propertyJsons = new Dictionary<string, PropertyJson>();
	Dictionary<string, StarLevelJson> starLevelJsons = new Dictionary<string, StarLevelJson>();
	Dictionary<string, PropertyLevelJson> propertyLevelJsons = new Dictionary<string, PropertyLevelJson>();
	Dictionary<string, PropertyLevelCostJson> propertyLevelCostJsons = new Dictionary<string, PropertyLevelCostJson>();
	Dictionary<string, StarLevelCostJson> starLevelCostJsons = new Dictionary<string, StarLevelCostJson>();
	Dictionary<string, PropertyUpgradeJson> propertyUpgradeJsons = new Dictionary<string, PropertyUpgradeJson>();
	Dictionary<string, StarUpgradeJson> starUpgradeJsons = new Dictionary<string, StarUpgradeJson>();

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

	public async Task<StarLevelJson> GetStarLevel(string roleId)
	{
		if (starLevelJsons.ContainsKey(roleId)) return starLevelJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleStarLevel);
			if (File.Exists(path))
			{
				string content = await File.ReadAllTextAsync(path);
				StarLevelJson starLevel = JsonConvert.DeserializeObject<StarLevelJson>(content);
				if (starLevel != null) starLevelJsons.Add(roleId, starLevel);
				return starLevel;
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

	public async Task<StarLevelCostJson> GetStarLevelCost(string roleId)
	{
		if (starLevelCostJsons.ContainsKey(roleId)) return starLevelCostJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			if (!string.IsNullOrEmpty(json.roleStarLevelCost))
			{
				string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleStarLevelCost);
				if (File.Exists(path))
				{
					string content = await File.ReadAllTextAsync(path);
					StarLevelCostJson starLevelCost = JsonConvert.DeserializeObject<StarLevelCostJson>(content);
					if (starLevelCost != null) starLevelCostJsons.Add(roleId, starLevelCost);
					return starLevelCost;
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

	public async Task<StarUpgradeJson> GetStarUpgrade(string roleId)
	{
		if (starUpgradeJsons.ContainsKey(roleId)) return starUpgradeJsons[roleId];
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			if (!string.IsNullOrEmpty(json.roleStarUpgrade))
			{
				string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleStarUpgrade);
				if (File.Exists(path))
				{
					string content = await File.ReadAllTextAsync(path);
					StarUpgradeJson starUpgrade = JsonConvert.DeserializeObject<StarUpgradeJson>(content);
					if (starUpgrade != null) starUpgradeJsons.Add(roleId, starUpgrade);
					return starUpgrade;
				}
			}
		}
		return null;
	}


	public async Task UpdateStarLevel(string roleId)
	{
		if (roleJsons.TryGetValue(roleId, out RoleJson json))
		{
			string path = Path.Combine(Application.streamingAssetsPath, "RoleTable", json.roleStarLevel);
		    StarLevelJson data = await GetStarLevel(roleId);
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