using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class RoleDataEditor : EditorWindow
{
    private string rootPath = "";
    private List<RoleJsonData> roles = new List<RoleJsonData>();
    private Vector2 scrollPosition;
    private Vector2 dataScrollPosition;
    private int selectedRoleIndex = -1;

    // 折叠状态
    private bool infoFoldout = true;
    private bool assetFoldout = true;
    private bool lockFoldout = true;
    private bool propertyFoldout = true;
    private bool starLevelFoldout = true;
    private bool propertyLevelFoldout = true;

    [MenuItem("Tools/Role Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<RoleDataEditor>("Role Data Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Role Data Editor", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // 选择根目录
        GUILayout.BeginHorizontal();
        GUILayout.Label("Root Path:", GUILayout.Width(70));
        string newPath = EditorGUILayout.TextField(rootPath);
        if (newPath != rootPath)
        {
            rootPath = newPath;
            // 当路径改变时，尝试加载数据
            LoadDataFromPath();
        }
        if (GUILayout.Button("Browse", GUILayout.Width(70)))
        {
            string path = EditorUtility.OpenFolderPanel("Select Root Folder", rootPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                rootPath = path;
                // 当选择新路径时，尝试加载数据
                LoadDataFromPath();
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        // 角色列表
        GUILayout.Label("Roles:", EditorStyles.boldLabel);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

        for (int i = 0; i < roles.Count; i++)
        {
            GUILayout.BeginHorizontal();
            bool isSelected = i == selectedRoleIndex;
            if (GUILayout.Button(roles[i].roleId, isSelected ? EditorStyles.toolbarButton : EditorStyles.label))
            {
                selectedRoleIndex = i;
                // 重置折叠状态
                ResetFoldoutStates();
            }
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                roles.RemoveAt(i);
                if (selectedRoleIndex >= i)
                    selectedRoleIndex--;
                break;
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();

        GUILayout.Space(10);

        // 添加角色
        if (GUILayout.Button("Add Role"))
        {
            RoleJsonData newRole = new RoleJsonData();
            newRole.roleId = "new_role" + (roles.Count + 1);
            roles.Add(newRole);
            selectedRoleIndex = roles.Count - 1;
            // 重置折叠状态
            ResetFoldoutStates();
        }

        GUILayout.Space(20);

        // 编辑选中角色
        if (selectedRoleIndex >= 0 && selectedRoleIndex < roles.Count)
        {
            RoleJsonData role = roles[selectedRoleIndex];
            GUILayout.Label("Role Details:", EditorStyles.boldLabel);

            // 只显示 Role ID，其他路径字段自动生成
            role.roleId = EditorGUILayout.TextField("Role ID", role.roleId);

            GUILayout.Space(10);

            // 数据编辑区域，添加滚动功能
            dataScrollPosition = GUILayout.BeginScrollView(dataScrollPosition, GUILayout.Height(400));

            // 角色信息编辑
            infoFoldout = EditorGUILayout.Foldout(infoFoldout, "Info Data");
            if (infoFoldout)
            {
                EditorGUI.indentLevel++;
                role.InfoData.roleName = EditorGUILayout.TextField("Role Name", role.InfoData.roleName);
                role.InfoData.roleDesc = EditorGUILayout.TextField("Description", role.InfoData.roleDesc);
                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            // 角色资产编辑
            assetFoldout = EditorGUILayout.Foldout(assetFoldout, "Asset Data");
            if (assetFoldout)
            {
                EditorGUI.indentLevel++;
                role.AssetData.roleImg = EditorGUILayout.TextField("Role Image", role.AssetData.roleImg);
                role.AssetData.roleModel = EditorGUILayout.TextField("Role Model", role.AssetData.roleModel);
                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            // 角色锁定编辑
            lockFoldout = EditorGUILayout.Foldout(lockFoldout, "Lock Data");
            if (lockFoldout)
            {
                EditorGUI.indentLevel++;
                role.LockData.isUnlock = EditorGUILayout.Toggle("Is Unlocked", role.LockData.isUnlock);
                
                // 解锁条件编辑
                GUILayout.Label("Unlock Conditions:");
                if (role.LockData.unlockCondition == null || role.LockData.unlockCondition.Length == 0)
                {
                    role.LockData.unlockCondition = new string[1];
                }
                
                for (int i = 0; i < role.LockData.unlockCondition.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    role.LockData.unlockCondition[i] = EditorGUILayout.TextField("Condition " + (i + 1), role.LockData.unlockCondition[i]);
                    if (GUILayout.Button("+", GUILayout.Width(20)))
                    {
                        string[] newConditions = new string[role.LockData.unlockCondition.Length + 1];
                        System.Array.Copy(role.LockData.unlockCondition, newConditions, role.LockData.unlockCondition.Length);
                        newConditions[newConditions.Length - 1] = "";
                        role.LockData.unlockCondition = newConditions;
                    }
                    if (role.LockData.unlockCondition.Length > 1 && GUILayout.Button("-", GUILayout.Width(20)))
                    {
                        string[] newConditions = new string[role.LockData.unlockCondition.Length - 1];
                        for (int j = 0, k = 0; j < role.LockData.unlockCondition.Length; j++)
                        {
                            if (j != i)
                            {
                                newConditions[k] = role.LockData.unlockCondition[j];
                                k++;
                            }
                        }
                        role.LockData.unlockCondition = newConditions;
                        break;
                    }
                    GUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            // 角色属性编辑
            propertyFoldout = EditorGUILayout.Foldout(propertyFoldout, "Property Data");
            if (propertyFoldout)
            {
                EditorGUI.indentLevel++;
                role.PropertyData.health = EditorGUILayout.FloatField("Health", role.PropertyData.health);
                role.PropertyData.energy = EditorGUILayout.FloatField("Energy", role.PropertyData.energy);
                role.PropertyData.attack = EditorGUILayout.FloatField("Attack", role.PropertyData.attack);
                role.PropertyData.defense = EditorGUILayout.FloatField("Defense", role.PropertyData.defense);
                role.PropertyData.speed = EditorGUILayout.FloatField("Speed", role.PropertyData.speed);
                role.PropertyData.cooldownReduction = EditorGUILayout.FloatField("Cooldown Reduction", role.PropertyData.cooldownReduction);
                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            // 角色星级等级编辑
            starLevelFoldout = EditorGUILayout.Foldout(starLevelFoldout, "Star Level Data");
            if (starLevelFoldout)
            {
                EditorGUI.indentLevel++;
                role.StarLevelData.starLevel.baseLevel = EditorGUILayout.IntField("Base Level", role.StarLevelData.starLevel.baseLevel);
                role.StarLevelData.starLevel.currentLevel = EditorGUILayout.IntField("Current Level", role.StarLevelData.starLevel.currentLevel);
                role.StarLevelData.starLevel.maxLevel = EditorGUILayout.IntField("Max Level", role.StarLevelData.starLevel.maxLevel);
                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);

            // 角色属性等级编辑
            propertyLevelFoldout = EditorGUILayout.Foldout(propertyLevelFoldout, "Property Level Data");
            if (propertyLevelFoldout)
            {
                EditorGUI.indentLevel++;
                
                GUILayout.Label("Health Level:");
                role.PropertyLevelData.healthLevel.baseLevel = EditorGUILayout.IntField("Base Level", role.PropertyLevelData.healthLevel.baseLevel);
                role.PropertyLevelData.healthLevel.currentLevel = EditorGUILayout.IntField("Current Level", role.PropertyLevelData.healthLevel.currentLevel);
                role.PropertyLevelData.healthLevel.maxLevel = EditorGUILayout.IntField("Max Level", role.PropertyLevelData.healthLevel.maxLevel);
                
                GUILayout.Label("Energy Level:");
                role.PropertyLevelData.energyLevel.baseLevel = EditorGUILayout.IntField("Base Level", role.PropertyLevelData.energyLevel.baseLevel);
                role.PropertyLevelData.energyLevel.currentLevel = EditorGUILayout.IntField("Current Level", role.PropertyLevelData.energyLevel.currentLevel);
                role.PropertyLevelData.energyLevel.maxLevel = EditorGUILayout.IntField("Max Level", role.PropertyLevelData.energyLevel.maxLevel);
                
                GUILayout.Label("Defense Level:");
                role.PropertyLevelData.defenseLevel.baseLevel = EditorGUILayout.IntField("Base Level", role.PropertyLevelData.defenseLevel.baseLevel);
                role.PropertyLevelData.defenseLevel.currentLevel = EditorGUILayout.IntField("Current Level", role.PropertyLevelData.defenseLevel.currentLevel);
                role.PropertyLevelData.defenseLevel.maxLevel = EditorGUILayout.IntField("Max Level", role.PropertyLevelData.defenseLevel.maxLevel);
                
                GUILayout.Label("Cooldown Reduction Level:");
                role.PropertyLevelData.cooldownReductionLevel.baseLevel = EditorGUILayout.IntField("Base Level", role.PropertyLevelData.cooldownReductionLevel.baseLevel);
                role.PropertyLevelData.cooldownReductionLevel.currentLevel = EditorGUILayout.IntField("Current Level", role.PropertyLevelData.cooldownReductionLevel.currentLevel);
                role.PropertyLevelData.cooldownReductionLevel.maxLevel = EditorGUILayout.IntField("Max Level", role.PropertyLevelData.cooldownReductionLevel.maxLevel);
                EditorGUI.indentLevel--;
            }

            GUILayout.EndScrollView();
        }

        GUILayout.Space(20);

        // 生成文件
        if (GUILayout.Button("Generate Files"))
        {
            GenerateFiles();
        }
    }

    private void ResetFoldoutStates()
    {
        infoFoldout = true;
        assetFoldout = true;
        lockFoldout = true;
        propertyFoldout = true;
        starLevelFoldout = true;
        propertyLevelFoldout = true;
    }

    private void LoadDataFromPath()
    {
        if (string.IsNullOrEmpty(rootPath))
        {
            return;
        }

        string tableFilePath = Path.Combine(rootPath, "RoleTable.json");
        if (!File.Exists(tableFilePath))
        {
            // 如果没有 RoleTable.json 文件，清空数据
            roles.Clear();
            selectedRoleIndex = -1;
            return;
        }

        // 读取 RoleTable.json
        string tableJson = File.ReadAllText(tableFilePath);
        RoleTable table = JsonUtility.FromJson<RoleTable>(tableJson);

        // 清空现有数据
        roles.Clear();

        // 加载每个角色的数据
        foreach (RoleJson roleJson in table.roles)
        {
            RoleJsonData roleData = new RoleJsonData();
            roleData.roleId = roleJson.roleId;

            // 加载 info 数据
            string infoFilePath = Path.Combine(rootPath, roleJson.roleInfo);
            if (File.Exists(infoFilePath))
            {
                string infoJson = File.ReadAllText(infoFilePath);
                roleData.InfoData = JsonUtility.FromJson<RoleJsonData.InfoJson>(infoJson);
            }

            // 加载 asset 数据
            string assetFilePath = Path.Combine(rootPath, roleJson.roleAsset);
            if (File.Exists(assetFilePath))
            {
                string assetJson = File.ReadAllText(assetFilePath);
                roleData.AssetData = JsonUtility.FromJson<RoleJsonData.AssetJson>(assetJson);
            }

            // 加载 lock 数据
            string lockFilePath = Path.Combine(rootPath, roleJson.roleLock);
            if (File.Exists(lockFilePath))
            {
                string lockJson = File.ReadAllText(lockFilePath);
                roleData.LockData = JsonUtility.FromJson<RoleJsonData.LockJson>(lockJson);
            }

            // 加载 property 数据
            string propertyFilePath = Path.Combine(rootPath, roleJson.roleProperty);
            if (File.Exists(propertyFilePath))
            {
                string propertyJson = File.ReadAllText(propertyFilePath);
                roleData.PropertyData = JsonUtility.FromJson<RoleJsonData.PropertyJson>(propertyJson);
            }

            // 加载 starLevel 数据
            string starLevelFilePath = Path.Combine(rootPath, roleJson.roleStarLevel);
            if (File.Exists(starLevelFilePath))
            {
                string starLevelJson = File.ReadAllText(starLevelFilePath);
                roleData.StarLevelData = JsonUtility.FromJson<RoleJsonData.StarLevelJson>(starLevelJson);
            }

            // 加载 propertyLevel 数据
            string propertyLevelFilePath = Path.Combine(rootPath, roleJson.rolePropertyLevel);
            if (File.Exists(propertyLevelFilePath))
            {
                string propertyLevelJson = File.ReadAllText(propertyLevelFilePath);
                roleData.PropertyLevelData = JsonUtility.FromJson<RoleJsonData.PropertyLevelJson>(propertyLevelJson);
            }

            roles.Add(roleData);
        }

        // 选择第一个角色
        if (roles.Count > 0)
        {
            selectedRoleIndex = 0;
            ResetFoldoutStates();
        }
    }

    private void GenerateFiles()
    {
        if (string.IsNullOrEmpty(rootPath))
        {
            EditorUtility.DisplayDialog("Error", "Please select a root folder first.", "OK");
            return;
        }

        // 确保根目录存在
        if (!Directory.Exists(rootPath))
        {
            Directory.CreateDirectory(rootPath);
        }

        // 创建文件夹结构
        string infosPath = Path.Combine(rootPath, "Infos");
        string assetsPath = Path.Combine(rootPath, "Assets");
        string locksPath = Path.Combine(rootPath, "Locks");
        string propertiesPath = Path.Combine(rootPath, "Properties");
        string starLevelsPath = Path.Combine(rootPath, "StarLevels");
        string propertyLevelsPath = Path.Combine(rootPath, "PropertyLevels");

        if (!Directory.Exists(infosPath)) Directory.CreateDirectory(infosPath);
        if (!Directory.Exists(assetsPath)) Directory.CreateDirectory(assetsPath);
        if (!Directory.Exists(locksPath)) Directory.CreateDirectory(locksPath);
        if (!Directory.Exists(propertiesPath)) Directory.CreateDirectory(propertiesPath);
        if (!Directory.Exists(starLevelsPath)) Directory.CreateDirectory(starLevelsPath);
        if (!Directory.Exists(propertyLevelsPath)) Directory.CreateDirectory(propertyLevelsPath);

        // 生成总表
        RoleTable table = new RoleTable();
        table.roles = new RoleJson[roles.Count];

        for (int i = 0; i < roles.Count; i++)
        {
            RoleJsonData roleData = roles[i];
            RoleJson role = new RoleJson();
            table.roles[i] = role;

            role.roleId = roleData.roleId;

            // 生成 info 文件
            string infoFileName = $"{roleData.roleId}_info.json";
            string infoFilePath = Path.Combine(infosPath, infoFileName);
            string infoJson = JsonUtility.ToJson(roleData.InfoData, true);
            File.WriteAllText(infoFilePath, infoJson);
            role.roleInfo = Path.Combine("Infos", infoFileName);

            // 生成 asset 文件
            string assetFileName = $"{roleData.roleId}_asset.json";
            string assetFilePath = Path.Combine(assetsPath, assetFileName);
            string assetJson = JsonUtility.ToJson(roleData.AssetData, true);
            File.WriteAllText(assetFilePath, assetJson);
            role.roleAsset = Path.Combine("Assets", assetFileName);

            // 生成 lock 文件
            string lockFileName = $"{roleData.roleId}_lock.json";
            string lockFilePath = Path.Combine(locksPath, lockFileName);
            string lockJson = JsonUtility.ToJson(roleData.LockData, true);
            File.WriteAllText(lockFilePath, lockJson);
            role.roleLock = Path.Combine("Locks", lockFileName);

            // 生成 property 文件
            string propertyFileName = $"{roleData.roleId}_property.json";
            string propertyFilePath = Path.Combine(propertiesPath, propertyFileName);
            string propertyJson = JsonUtility.ToJson(roleData.PropertyData, true);
            File.WriteAllText(propertyFilePath, propertyJson);
            role.roleProperty = Path.Combine("Properties", propertyFileName);

            // 生成 starLevel 文件
            string starLevelFileName = $"{roleData.roleId}_starLevel.json";
            string starLevelFilePath = Path.Combine(starLevelsPath, starLevelFileName);
            string starLevelJson = JsonUtility.ToJson(roleData.StarLevelData, true);
            File.WriteAllText(starLevelFilePath, starLevelJson);
            role.roleStarLevel = Path.Combine("StarLevels", starLevelFileName);

            // 生成 propertyLevel 文件
            string propertyLevelFileName = $"{roleData.roleId}_propertyLevel.json";
            string propertyLevelFilePath = Path.Combine(propertyLevelsPath, propertyLevelFileName);
            string propertyLevelJson = JsonUtility.ToJson(roleData.PropertyLevelData, true);
            File.WriteAllText(propertyLevelFilePath, propertyLevelJson);
            role.rolePropertyLevel = Path.Combine("PropertyLevels", propertyLevelFileName);
        }

        // 保存总表
        string tableFilePath = Path.Combine(rootPath, "RoleTable.json");
        string tableJson = JsonUtility.ToJson(table, true);
        File.WriteAllText(tableFilePath, tableJson);

        EditorUtility.DisplayDialog("Success", "Role data files generated successfully!\n\nRoot Path: " + rootPath, "OK");
    }
}

[System.Serializable]
public class RoleTable
{
    public RoleJson[] roles;
}

// 与 RoleJson.cs 保持一致的结构
[System.Serializable]
public class RoleJson
{
    public string roleId;
    public string roleInfo;
    public string roleAsset;
    public string roleLock;
    public string roleProperty;
    public string roleStarLevel;
    public string rolePropertyLevel;
}

// 编辑器内部使用的数据结构
[System.Serializable]
public class RoleJsonData
{
    public string roleId;

    [System.Serializable]
    public class InfoJson
    {
        public string roleName = "New Role";
        public string roleDesc = "Role description";
    }

    [System.Serializable]
    public class AssetJson
    {
        public string roleImg = "Images/Default";
        public string roleModel = "Models/Default";
    }

    [System.Serializable]
    public class LockJson
    {
        public bool isUnlock = false;
        public string[] unlockCondition = new string[0];
    }

    [System.Serializable]
    public class PropertyJson
    {
        public float health = 100f;
        public float energy = 50f;
        public float attack = 20f;
        public float defense = 10f;
        public float speed = 5f;
        public float cooldownReduction = 0f;
    }

    [System.Serializable]
    public class LevelJson
    {
        public int baseLevel = 0;
        public int currentLevel = 0;
        public int maxLevel = 10;
    }

    [System.Serializable]
    public class StarLevelJson
    {
        public LevelJson starLevel = new LevelJson();
    }

    [System.Serializable]
    public class PropertyLevelJson
    {
        public LevelJson healthLevel = new LevelJson();
        public LevelJson energyLevel = new LevelJson();
        public LevelJson defenseLevel = new LevelJson();
        public LevelJson cooldownReductionLevel = new LevelJson();
    }

    public InfoJson InfoData = new InfoJson();
    public AssetJson AssetData = new AssetJson();
    public LockJson LockData = new LockJson();
    public PropertyJson PropertyData = new PropertyJson();
    public StarLevelJson StarLevelData = new StarLevelJson();
    public PropertyLevelJson PropertyLevelData = new PropertyLevelJson();
}