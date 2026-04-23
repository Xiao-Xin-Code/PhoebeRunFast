using QMVC;
using UnityEngine;
using System.Collections.Generic;

public class AssetSystem : AbstractSystem
{
    readonly static string rolesTablePath = Application.streamingAssetsPath + "/RoleJson.json";

    Dictionary<string,RoleJson> rolesTable = new Dictionary<string,RoleJson>();


    protected override void OnInit()
    {
        

    }
}