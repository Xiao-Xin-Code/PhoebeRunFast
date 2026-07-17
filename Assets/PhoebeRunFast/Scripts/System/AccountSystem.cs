using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using QMVC;
using Newtonsoft.Json;
using UnityEngine;
using System;
using System.Linq;

public class AccountSystem : AbstractSystem
{

    GlobalSystem _globalSystem;

    Dictionary<string, AccountJson> accountJsons = new Dictionary<string, AccountJson>();
    Dictionary<string, AccountInfo> accountInfos = new Dictionary<string, AccountInfo>();
    Dictionary<string, Dictionary<string, AccountRole>> accountRoles = new Dictionary<string, Dictionary<string, AccountRole>>();
    Dictionary<string, Dictionary<string, AccountGoods>> accountGoods = new Dictionary<string, Dictionary<string, AccountGoods>>();


    public void SetAccountJsons(AccountJson[] jsons)
    {
        accountJsons.Clear();
        foreach (AccountJson accountJson in jsons)
        {
            accountJsons.Add(accountJson.accountId, accountJson);
        }
    }


    public async Task<AccountInfo> GetInfo(string accountId)
    {
        if (accountInfos.ContainsKey(accountId)) return accountInfos[accountId];
        if(accountJsons.TryGetValue(accountId, out AccountJson accountJson))
        {
            string path = Path.Combine(Application.streamingAssetsPath, accountJson.accountInfo);
            if(File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                AccountInfo info = JsonConvert.DeserializeObject<AccountInfo>(content);
                if (info != null)
                {
                    accountInfos.Add(accountId, info);
                }
                return info;
            }
        }
        return null;
    }
    
    public async Task<AccountGoods> GetGoods(string accountId,string goodsId)
    {
        if (accountGoods.ContainsKey(accountId))
        {
            accountGoods[accountId].TryGetValue(goodsId, out AccountGoods goods);
            return goods;
        }
        return null;
    }

    public async Task<AccountRole> GetRole(string accountId, string accountRoleId)
    {
        if (accountRoles.ContainsKey(accountId))
        {
            accountRoles[accountId].TryGetValue(accountRoleId, out AccountRole value);
            return value;
        }
        return null;
    }

    //提前获取好已有的角色列表
    public async Task<AccountRole[]> GetRoles(string accountId)
    {
        if (accountRoles.ContainsKey(accountId)) return accountRoles[accountId].Values.ToArray();
        if (accountJsons.TryGetValue(accountId, out AccountJson accountJson))
        {
            string path = Path.Combine(Application.streamingAssetsPath, accountJson.accountRoles);
            if (File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                AccountRole[] roles = JsonConvert.DeserializeObject<AccountRole[]>(content);
                if (roles != null && roles.Length > 0)
                {
                    accountRoles[accountId] = new Dictionary<string, AccountRole>();
                    foreach (var item in roles)
                    {
                        accountRoles[accountId].Add(item.roleId, item);
                    }
                    return roles;
                }
            }
        }
        return null;
    }

    //提前获取好已有的物品列表
    public async Task<AccountGoods[]> GetGoods(string accountId)
    {
        if (accountGoods.ContainsKey(accountId)) return accountGoods[accountId].Values.ToArray();
        if (accountJsons.TryGetValue(accountId, out AccountJson accountJson))
        {
            string path = Path.Combine(Application.streamingAssetsPath, accountJson.accountGoods);
            if (File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                AccountGoods[] goods = JsonConvert.DeserializeObject<AccountGoods[]>(content);
                if (goods != null && goods.Length > 0)
                {
                    accountGoods[accountId] = new Dictionary<string, AccountGoods>();
                    foreach (var item in goods)
                    {
                        accountGoods[accountId].Add(item.goodsId, item);
                    }
                    return goods;
                }
            }
        }
        return null;
    }


    #region 注册登陆    

    public bool CheckHasAccount(string accountId)
    {
        return accountJsons.ContainsKey(accountId);
    }


    //账户检测
    public void CheckAccount(string username, string password)
    {
        //先检测输入是否是ID
        //GetInfo


        foreach (var item in accountInfos)
        {
            if (item.Value.accountName == username && item.Value.accountPassword == password)
            {
                //账户存在
                return;
            }
        }



    }


    public async Task<bool> SignAccount(string username, string password)
    {
        //随机分配一个不重复的ID
        string id = $"ACC_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
        if (accountJsons.ContainsKey(id))
        {
            id = $"ACC_{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
        }
        
        AccountJson accountJson = new AccountJson();
        accountJson.accountId = id;
        accountJson.accountInfo = $"AccountTable/Infos/{id}_info.json";
        accountJson.accountGoods = $"AccountTable/Goods/{id}_goods.json";
        accountJson.accountRoles = $"AccountTable/Roles/{id}_role.json";
        accountJsons.Add(id, accountJson);

        //创建用户信息
        AccountInfo info = new AccountInfo();
        info.accountName = username;
        info.accountPassword = password;
        accountInfos.Add(id, info);

        //转换为JSON字符串
        string json = JsonConvert.SerializeObject(info);
        //写入文件
        string path = Path.Combine(Application.streamingAssetsPath, accountJson.accountInfo);

        await File.WriteAllTextAsync(path, json);

        //创建用户角色信息
        AccountRole role = new AccountRole();
        role.roleId = "000000001";
        role.rolePropertyLevel = new RolePropertyLevel();
        accountRoles.Add(id, new Dictionary<string, AccountRole>());
        accountRoles[id].Add(role.roleId, role);
        
        //转换为JSON字符串
        json = JsonConvert.SerializeObject(new AccountRole[] { role });
        //写入文件
        path = Path.Combine(Application.streamingAssetsPath, accountJson.accountRoles);
        await File.WriteAllTextAsync(path, json);

        //创建用户物品信息
        //默认初始没有物品
        accountGoods.Add(id, new Dictionary<string, AccountGoods>());
        json = JsonConvert.SerializeObject(new AccountGoods[] { });
        //写入文件
        path = Path.Combine(Application.streamingAssetsPath, accountJson.accountGoods);
        await File.WriteAllTextAsync(path, json);

        await UpdateAccountJsons();
        await UpdateUserCache(id, role.roleId);
        return true;
    }


    #endregion


    public async Task UpdateAccountJsons()
    {
        AccountJson[] accountJsons = this.accountJsons.Values.ToArray();
        _globalSystem.GlobalModel.SetAccountJsons(accountJsons);
        
        //转换json为字符串
        string json = JsonConvert.SerializeObject(accountJsons);
        //写入文件
        string path = Path.Combine(Application.streamingAssetsPath, "AccountTable/AccountTable.json");
        await File.WriteAllTextAsync(path, json);
    }


    public async Task UpdateUserCache(string accountId, string accountRoleId)
    {
        UserJson userJson = new UserJson();
        userJson.userId = accountId;
        userJson.outRoleId = accountRoleId;
        _globalSystem.GlobalModel.SetUserJson(userJson);

        //转换json为字符串
        string json = JsonConvert.SerializeObject(userJson);
        //写入文件
        string path = Path.Combine(Application.streamingAssetsPath, "AccountTable/UserCache.json");
        await File.WriteAllTextAsync(path, json);
    }


    protected override void OnInit()
    {
        _globalSystem = this.GetSystem<GlobalSystem>();
    }
}