using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using QMVC;
using Newtonsoft.Json;
using UnityEngine;

public class AccountSystem : AbstractSystem
{

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
        if(accountJsons.TryGetValue(accountId,out AccountJson accountJson))
        {
            string path = Path.Combine(Application.streamingAssetsPath, accountJson.accountGoods);
            if(File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                AccountGoods[] goods = JsonConvert.DeserializeObject<AccountGoods[]>(content);
                if(goods != null && goods.Length > 0)
                {
                    accountGoods[accountId] = new Dictionary<string, AccountGoods>();
                    foreach (AccountGoods item in goods)
                    {
                        accountGoods[accountId].Add(item.goodsId, item);
                    }
                    accountGoods[accountId].TryGetValue(goodsId, out AccountGoods _goods);
                    return _goods;
                }
            }
        }
        return null;
    }

    public async Task<AccountRole> GetRole(string accountId,string accountRoleId)
    {
        if (accountRoles.ContainsKey(accountId)) 
        {
            accountRoles[accountId].TryGetValue(accountRoleId, out AccountRole value);
            return value;
        } 
        if(accountJsons.TryGetValue(accountId,out AccountJson accountJson))
        {
            string path = Path.Combine(Application.streamingAssetsPath, accountJson.accountRoles);
            if(File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                AccountRole[] roles = JsonConvert.DeserializeObject<AccountRole[]>(content);
                if(roles != null && roles.Length > 0)
                {
                    accountRoles[accountId] = new Dictionary<string, AccountRole>();
                    foreach (var item in roles)
                    {
                        accountRoles[accountId].Add(item.roleId, item);
                    }

                    accountRoles[accountId].TryGetValue(accountRoleId, out AccountRole role);
                    return role;
                }
            }
        }
        return null;
    }


    protected override void OnInit()
    {

    }
}