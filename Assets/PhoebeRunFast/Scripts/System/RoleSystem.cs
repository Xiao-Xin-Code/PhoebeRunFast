using UnityEngine;
using QMVC;
using System.Collections.Generic;
using System;

public class RoleSystem : AbstractSystem
{
    Dictionary<string, RoleController> roles = new Dictionary<string, RoleController>();


    public RoleController GetRoleById(string id)
    {
        if (roles.ContainsKey(id))
        {
            return roles[id];
        }
        else
        {
            //TODO: 尝试加载资源
        }
        return null;
    }




    protected override void OnInit()
    {
        //读取RoleJson文件，解析为RoleData数组
        //RoleJson[] roleDatas = JsonUtility.FromJson<RoleJson>(Resources.Load<TextAsset>("Role").text);
    }
}

//////
/// {
///     rolejsonpath,
///     goodsjsonpath,
///     settingjsonpath,
/// }
/// 
/// 