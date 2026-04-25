using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

public class RoleDataEditor : EditorWindow
{
    private string rootPath = "";
    private List<RoleJson> roles = new List<RoleJson>();
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
    private bool propertyLevelCostFoldout = true;
    private bool starLevelCostFoldout = true;
    private bool propertyUpgradeFoldout = true;
    private bool starUpgradeFoldout = true;

    // 临时数据存储
    private Dictionary<string, InfoJson> infoData = new Dictionary<string, InfoJson>();
    private Dictionary<string, AssetJson> assetData = new Dictionary<string, AssetJson>();
    private Dictionary<string, LockJson> lockData = new Dictionary<string, LockJson>();
    private Dictionary<string, PropertyJson> propertyData = new Dictionary<string, PropertyJson>();
    private Dictionary<string, StarLevelJson> starLevelData = new Dictionary<string, StarLevelJson>();
    private Dictionary<string, PropertyLevelJson> propertyLevelData = new Dictionary<string, PropertyLevelJson>();
    private Dictionary<string, PropertyLevelCostJson> propertyLevelCostData = new Dictionary<string, PropertyLevelCostJson>();
    private Dictionary<string, StarLevelCostJson> starLevelCostData = new Dictionary<string, StarLevelCostJson>();
    private Dictionary<string, PropertyUpgradeJson> propertyUpgradeData = new Dictionary<string, PropertyUpgradeJson>();
    private Dictionary<string, StarUpgradeJson> starUpgradeData = new Dictionary<string, StarUpgradeJson>();

    [MenuItem("Tools/Role Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<RoleDataEditor>("Role Data Editor");
    }

    private void OnGUI()
    {
        // 捕获异常，防止界面崩溃
        try
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
                RoleJson newRole = new RoleJson();
                newRole.roleId = "new_role" + (roles.Count + 1);
                roles.Add(newRole);
                selectedRoleIndex = roles.Count - 1;
                // 重置折叠状态
                ResetFoldoutStates();
                // 初始化临时数据
                InitializeRoleData(newRole.roleId);
            }

            GUILayout.Space(20);

            // 编辑选中角色
            if (selectedRoleIndex >= 0 && selectedRoleIndex < roles.Count)
            {
                RoleJson role = roles[selectedRoleIndex];
                GUILayout.Label("Role Details:", EditorStyles.boldLabel);

                // 只显示 Role ID，其他路径字段自动生成
                role.roleId = EditorGUILayout.TextField("Role ID", role.roleId);

                GUILayout.Space(10);

                // 数据编辑区域，添加滚动功能
                dataScrollPosition = GUILayout.BeginScrollView(dataScrollPosition, GUILayout.Height(400));

                // 确保临时数据存在
                InitializeRoleData(role.roleId);

                // 角色信息编辑
                infoFoldout = EditorGUILayout.Foldout(infoFoldout, "Info Data");
                if (infoFoldout)
                {
                    EditorGUI.indentLevel++;
                    if (infoData.TryGetValue(role.roleId, out InfoJson info))
                    {
                        info.roleName = EditorGUILayout.TextField("Role Name", info.roleName);
                        info.roleDesc = EditorGUILayout.TextField("Description", info.roleDesc);
                        infoData[role.roleId] = info;
                    }
                    EditorGUI.indentLevel--;
                }

                GUILayout.Space(5);

                // 角色资产编辑
                assetFoldout = EditorGUILayout.Foldout(assetFoldout, "Asset Data");
                if (assetFoldout)
                {
                    EditorGUI.indentLevel++;
                    if (assetData.TryGetValue(role.roleId, out AssetJson asset))
                    {
                        asset.roleImg = EditorGUILayout.TextField("Role Image", asset.roleImg);
                        asset.roleModel = EditorGUILayout.TextField("Role Model", asset.roleModel);
                        assetData[role.roleId] = asset;
                    }
                    EditorGUI.indentLevel--;
                }

                GUILayout.Space(5);

                // 角色锁定编辑
                lockFoldout = EditorGUILayout.Foldout(lockFoldout, "Lock Data");
                if (lockFoldout)
                {
                    EditorGUI.indentLevel++;
                    if (lockData.TryGetValue(role.roleId, out LockJson lockJson))
                    {
                        lockJson.isUnlock = EditorGUILayout.Toggle("Is Unlocked", lockJson.isUnlock);
                        
                        // 解锁条件编辑
                        GUILayout.Label("Unlock Conditions:");
                        if (lockJson.unlockCondition == null || lockJson.unlockCondition.Length == 0)
                        {
                            lockJson.unlockCondition = new string[1];
                        }
                        
                        for (int i = 0; i < lockJson.unlockCondition.Length; i++)
                        {
                            GUILayout.BeginHorizontal();
                            lockJson.unlockCondition[i] = EditorGUILayout.TextField("Condition " + (i + 1), lockJson.unlockCondition[i]);
                            if (GUILayout.Button("+", GUILayout.Width(20)))
                            {
                                string[] newConditions = new string[lockJson.unlockCondition.Length + 1];
                                System.Array.Copy(lockJson.unlockCondition, newConditions, lockJson.unlockCondition.Length);
                                newConditions[newConditions.Length - 1] = "";
                                lockJson.unlockCondition = newConditions;
                            }
                            if (lockJson.unlockCondition.Length > 1 && GUILayout.Button("-", GUILayout.Width(20)))
                            {
                                string[] newConditions = new string[lockJson.unlockCondition.Length - 1];
                                for (int j = 0, k = 0; j < lockJson.unlockCondition.Length; j++)
                                {
                                    if (j != i)
                                    {
                                        newConditions[k] = lockJson.unlockCondition[j];
                                        k++;
                                    }
                                }
                                lockJson.unlockCondition = newConditions;
                                break;
                            }
                            GUILayout.EndHorizontal();
                        }
                        lockData[role.roleId] = lockJson;
                    }
                    EditorGUI.indentLevel--;
                }

                GUILayout.Space(5);

                // 角色属性编辑
                propertyFoldout = EditorGUILayout.Foldout(propertyFoldout, "Property Data");
                if (propertyFoldout)
                {
                    EditorGUI.indentLevel++;
                    if (propertyData.TryGetValue(role.roleId, out PropertyJson property))
                    {
                        property.health = EditorGUILayout.FloatField("Health", property.health);
                        property.energy = EditorGUILayout.FloatField("Energy", property.energy);
                        property.attack = EditorGUILayout.FloatField("Attack", property.attack);
                        property.defense = EditorGUILayout.FloatField("Defense", property.defense);
                        property.speed = EditorGUILayout.FloatField("Speed", property.speed);
                        property.cooldownReduction = EditorGUILayout.FloatField("Cooldown Reduction", property.cooldownReduction);
                        propertyData[role.roleId] = property;
                    }
                    EditorGUI.indentLevel--;
                }

                GUILayout.Space(5);

                // 角色星级等级编辑
                starLevelFoldout = EditorGUILayout.Foldout(starLevelFoldout, "Star Level Data");
                if (starLevelFoldout)
                {
                    EditorGUI.indentLevel++;
                    if (starLevelData.TryGetValue(role.roleId, out StarLevelJson starLevel))
                    {
                        starLevel.starLevel.baseLevel = EditorGUILayout.IntField("Base Level", starLevel.starLevel.baseLevel);
                        starLevel.starLevel.currentLevel = EditorGUILayout.IntField("Current Level", starLevel.starLevel.currentLevel);
                        starLevel.starLevel.maxLevel = EditorGUILayout.IntField("Max Level", starLevel.starLevel.maxLevel);
                        starLevelData[role.roleId] = starLevel;
                    }
                    EditorGUI.indentLevel--;
                }

                GUILayout.Space(5);

                // 角色属性等级编辑
                propertyLevelFoldout = EditorGUILayout.Foldout(propertyLevelFoldout, "Property Level Data");
                if (propertyLevelFoldout)
                {
                    EditorGUI.indentLevel++;
                    if (propertyLevelData.TryGetValue(role.roleId, out PropertyLevelJson propertyLevel))
                    {
                        GUILayout.Label("Health Level:");
                        propertyLevel.healthLevel.baseLevel = EditorGUILayout.IntField("Base Level", propertyLevel.healthLevel.baseLevel);
                        propertyLevel.healthLevel.currentLevel = EditorGUILayout.IntField("Current Level", propertyLevel.healthLevel.currentLevel);
                        propertyLevel.healthLevel.maxLevel = EditorGUILayout.IntField("Max Level", propertyLevel.healthLevel.maxLevel);
                        
                        GUILayout.Label("Energy Level:");
                        propertyLevel.energyLevel.baseLevel = EditorGUILayout.IntField("Base Level", propertyLevel.energyLevel.baseLevel);
                        propertyLevel.energyLevel.currentLevel = EditorGUILayout.IntField("Current Level", propertyLevel.energyLevel.currentLevel);
                        propertyLevel.energyLevel.maxLevel = EditorGUILayout.IntField("Max Level", propertyLevel.energyLevel.maxLevel);
                        
                        GUILayout.Label("Defense Level:");
                        propertyLevel.defenseLevel.baseLevel = EditorGUILayout.IntField("Base Level", propertyLevel.defenseLevel.baseLevel);
                        propertyLevel.defenseLevel.currentLevel = EditorGUILayout.IntField("Current Level", propertyLevel.defenseLevel.currentLevel);
                        propertyLevel.defenseLevel.maxLevel = EditorGUILayout.IntField("Max Level", propertyLevel.defenseLevel.maxLevel);
                        
                        GUILayout.Label("Cooldown Reduction Level:");
                        propertyLevel.cooldownReductionLevel.baseLevel = EditorGUILayout.IntField("Base Level", propertyLevel.cooldownReductionLevel.baseLevel);
                        propertyLevel.cooldownReductionLevel.currentLevel = EditorGUILayout.IntField("Current Level", propertyLevel.cooldownReductionLevel.currentLevel);
                        propertyLevel.cooldownReductionLevel.maxLevel = EditorGUILayout.IntField("Max Level", propertyLevel.cooldownReductionLevel.maxLevel);
                        
                        propertyLevelData[role.roleId] = propertyLevel;
                    }
                    EditorGUI.indentLevel--;
                }

                GUILayout.Space(5);

                // 角色属性等级成本编辑
                propertyLevelCostFoldout = EditorGUILayout.Foldout(propertyLevelCostFoldout, "Property Level Cost Data");
                if (propertyLevelCostFoldout)
                {
                    EditorGUI.indentLevel++;
                    if (propertyLevelCostData.TryGetValue(role.roleId, out PropertyLevelCostJson propertyLevelCost))
                    {
                        // Health Level Costs
                        GUILayout.Label("Health Level Costs:");
                        propertyLevelCost.healthLevelCosts = EditLevelCosts(propertyLevelCost.healthLevelCosts, "Health");
                        
                        // Energy Level Costs
                        GUILayout.Label("Energy Level Costs:");
                        propertyLevelCost.energyLevelCosts = EditLevelCosts(propertyLevelCost.energyLevelCosts, "Energy");
                        
                        // Defense Level Costs
                        GUILayout.Label("Defense Level Costs:");
                        propertyLevelCost.defenseLevelCosts = EditLevelCosts(propertyLevelCost.defenseLevelCosts, "Defense");
                        
                        // Cooldown Reduction Level Costs
                        GUILayout.Label("Cooldown Reduction Level Costs:");
                        propertyLevelCost.cooldownReductionLevelCosts = EditLevelCosts(propertyLevelCost.cooldownReductionLevelCosts, "Cooldown Reduction");
                        
                        propertyLevelCostData[role.roleId] = propertyLevelCost;
                    }
                    EditorGUI.indentLevel--;
                }

                GUILayout.Space(5);

                // 角色星级等级成本编辑
                starLevelCostFoldout = EditorGUILayout.Foldout(starLevelCostFoldout, "Star Level Cost Data");
                if (starLevelCostFoldout)
                {
                    EditorGUI.indentLevel++;
                    if (starLevelCostData.TryGetValue(role.roleId, out StarLevelCostJson starLevelCost))
                    {
                        // Star Level Costs
                        GUILayout.Label("Star Level Costs:");
                        starLevelCost.starLevelCosts = EditLevelCosts(starLevelCost.starLevelCosts, "Star");
                        
                        starLevelCostData[role.roleId] = starLevelCost;
                    }
                    EditorGUI.indentLevel--;
                }

                GUILayout.Space(5);

                // 角色属性升级编辑
                propertyUpgradeFoldout = EditorGUILayout.Foldout(propertyUpgradeFoldout, "Property Upgrade Data");
                if (propertyUpgradeFoldout)
                {
                    EditorGUI.indentLevel++;
                    if (propertyUpgradeData.TryGetValue(role.roleId, out PropertyUpgradeJson propertyUpgrade))
                    {
                        // Health Upgrade
                        GUILayout.Label("Health Upgrade:");
                        propertyUpgrade.healthUpgrade = EditFloatArray(propertyUpgrade.healthUpgrade, "Health");
                        
                        // Energy Upgrade
                        GUILayout.Label("Energy Upgrade:");
                        propertyUpgrade.energyUpgrade = EditFloatArray(propertyUpgrade.energyUpgrade, "Energy");
                        
                        // Defense Upgrade
                        GUILayout.Label("Defense Upgrade:");
                        propertyUpgrade.defenseUpgrade = EditFloatArray(propertyUpgrade.defenseUpgrade, "Defense");
                        
                        // Cooldown Reduction Upgrade
                        GUILayout.Label("Cooldown Reduction Upgrade:");
                        propertyUpgrade.cooldownReductionUpgrade = EditFloatArray(propertyUpgrade.cooldownReductionUpgrade, "Cooldown Reduction");
                        
                        propertyUpgradeData[role.roleId] = propertyUpgrade;
                    }
                    EditorGUI.indentLevel--;
                }

                GUILayout.Space(5);

                // 角色星级升级编辑
                starUpgradeFoldout = EditorGUILayout.Foldout(starUpgradeFoldout, "Star Upgrade Data");
                if (starUpgradeFoldout)
                {
                    EditorGUI.indentLevel++;
                    if (starUpgradeData.TryGetValue(role.roleId, out StarUpgradeJson starUpgrade))
                    {
                        // Star Upgrades
                        GUILayout.Label("Star Upgrades:");
                        starUpgrade.upgradeJsons = EditUpgradeJsons(starUpgrade.upgradeJsons);
                        
                        starUpgradeData[role.roleId] = starUpgrade;
                    }
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
        catch (System.Exception e)
        {
            Debug.LogError("Error in RoleDataEditor: " + e.Message);
            GUILayout.Label("Error: " + e.Message);
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
        propertyLevelCostFoldout = true;
        starLevelCostFoldout = true;
        propertyUpgradeFoldout = true;
        starUpgradeFoldout = true;
    }

    private void InitializeRoleData(string roleId)
    {
        if (!infoData.ContainsKey(roleId))
            infoData[roleId] = new InfoJson { roleName = "New Role", roleDesc = "Role description" };
        if (!assetData.ContainsKey(roleId))
            assetData[roleId] = new AssetJson { roleImg = "Images/Default", roleModel = "Models/Default" };
        if (!lockData.ContainsKey(roleId))
            lockData[roleId] = new LockJson { isUnlock = false, unlockCondition = new string[0] };
        if (!propertyData.ContainsKey(roleId))
            propertyData[roleId] = new PropertyJson { health = 100f, energy = 50f, attack = 20f, defense = 10f, speed = 5f, cooldownReduction = 0f };
        if (!starLevelData.ContainsKey(roleId))
            starLevelData[roleId] = new StarLevelJson { starLevel = new LevelJson { baseLevel = 0, currentLevel = 0, maxLevel = 10 } };
        if (!propertyLevelData.ContainsKey(roleId))
            propertyLevelData[roleId] = new PropertyLevelJson {
                healthLevel = new LevelJson { baseLevel = 0, currentLevel = 0, maxLevel = 10 },
                energyLevel = new LevelJson { baseLevel = 0, currentLevel = 0, maxLevel = 10 },
                defenseLevel = new LevelJson { baseLevel = 0, currentLevel = 0, maxLevel = 10 },
                cooldownReductionLevel = new LevelJson { baseLevel = 0, currentLevel = 0, maxLevel = 10 }
            };
        if (!propertyLevelCostData.ContainsKey(roleId))
            propertyLevelCostData[roleId] = new PropertyLevelCostJson {
                healthLevelCosts = new LevelCostJson[0],
                energyLevelCosts = new LevelCostJson[0],
                defenseLevelCosts = new LevelCostJson[0],
                cooldownReductionLevelCosts = new LevelCostJson[0]
            };
        if (!starLevelCostData.ContainsKey(roleId))
            starLevelCostData[roleId] = new StarLevelCostJson { starLevelCosts = new LevelCostJson[0] };
        if (!propertyUpgradeData.ContainsKey(roleId))
            propertyUpgradeData[roleId] = new PropertyUpgradeJson {
                healthUpgrade = new float[0],
                energyUpgrade = new float[0],
                defenseUpgrade = new float[0],
                cooldownReductionUpgrade = new float[0]
            };
        if (!starUpgradeData.ContainsKey(roleId))
            starUpgradeData[roleId] = new StarUpgradeJson { upgradeJsons = new UpgradeJson[0] };
    }

    private LevelCostJson[] EditLevelCosts(LevelCostJson[] costs, string label)
    {
        if (costs == null)
            costs = new LevelCostJson[0];
        
        for (int i = 0; i < costs.Length; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Level {i + 1}:");
            
            // 编辑 Goods 数组
            if (costs[i].goodsIds == null)
                costs[i].goodsIds = new string[0];
            if (costs[i].amounts == null)
                costs[i].amounts = new int[0];
            
            // 确保 types 和 amounts 数组长度一致
            if (costs[i].goodsIds.Length != costs[i].amounts.Length)
            {
                int minLength = Mathf.Min(costs[i].goodsIds.Length, costs[i].amounts.Length);
                string[] newTypes = new string[minLength];
                int[] newAmounts = new int[minLength];
                System.Array.Copy(costs[i].goodsIds, newTypes, minLength);
                System.Array.Copy(costs[i].amounts, newAmounts, minLength);
                costs[i].goodsIds = newTypes;
                costs[i].amounts = newAmounts;
            }
            
            for (int j = 0; j < costs[i].goodsIds.Length; j++)
            {
                GUILayout.BeginVertical();
                costs[i].goodsIds[j] = EditorGUILayout.TextField("Goods ID", costs[i].goodsIds[j]);
                costs[i].amounts[j] = EditorGUILayout.IntField("Amount", costs[i].amounts[j]);
                GUILayout.EndVertical();
            }
            
            if (GUILayout.Button("Add Good", GUILayout.Width(100)))
            {
                string[] newTypes = new string[costs[i].goodsIds.Length + 1];
                int[] newAmounts = new int[costs[i].amounts.Length + 1];
                System.Array.Copy(costs[i].goodsIds, newTypes, costs[i].goodsIds.Length);
                System.Array.Copy(costs[i].amounts, newAmounts, costs[i].amounts.Length);
                newTypes[newTypes.Length - 1] = null;
                newAmounts[newAmounts.Length - 1] = 0;
                costs[i].goodsIds = newTypes;
                costs[i].amounts = newAmounts;
            }
            
            if (costs[i].goodsIds.Length > 0 && GUILayout.Button("Remove Good", GUILayout.Width(100)))
            {
                string[] newTypes = new string[costs[i].goodsIds.Length - 1];
                int[] newAmounts = new int[costs[i].amounts.Length - 1];
                System.Array.Copy(costs[i].goodsIds, 0, newTypes, 0, newTypes.Length);
                System.Array.Copy(costs[i].amounts, 0, newAmounts, 0, newAmounts.Length);
                costs[i].goodsIds = newTypes;
                costs[i].amounts = newAmounts;
            }
            
            GUILayout.EndHorizontal();
        }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button($"Add {label} Level Cost"))
        {
            LevelCostJson[] newCosts = new LevelCostJson[costs.Length + 1];
            System.Array.Copy(costs, newCosts, costs.Length);
            newCosts[newCosts.Length - 1] = new LevelCostJson { goodsIds = new string[0], amounts = new int[0] };
            costs = newCosts;
        }
        
        if (costs.Length > 0 && GUILayout.Button($"Remove {label} Level Cost"))
        {
            LevelCostJson[] newCosts = new LevelCostJson[costs.Length - 1];
            System.Array.Copy(costs, 0, newCosts, 0, newCosts.Length);
            costs = newCosts;
        }
        GUILayout.EndHorizontal();
        
        return costs;
    }

    private float[] EditFloatArray(float[] array, string label)
    {
        if (array == null)
            array = new float[0];
        
        for (int i = 0; i < array.Length; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Level {i + 1}:", GUILayout.Width(80));
            array[i] = EditorGUILayout.FloatField(array[i]);
            GUILayout.EndHorizontal();
        }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button($"Add {label} Upgrade"))
        {
            float[] newArray = new float[array.Length + 1];
            System.Array.Copy(array, newArray, array.Length);
            newArray[newArray.Length - 1] = 0f;
            array = newArray;
        }
        
        if (array.Length > 0 && GUILayout.Button($"Remove {label} Upgrade"))
        {
            float[] newArray = new float[array.Length - 1];
            System.Array.Copy(array, 0, newArray, 0, newArray.Length);
            array = newArray;
        }
        GUILayout.EndHorizontal();
        
        return array;
    }

    private UpgradeJson[] EditUpgradeJsons(UpgradeJson[] upgrades)
    {
        if (upgrades == null)
            upgrades = new UpgradeJson[0];
        
        for (int i = 0; i < upgrades.Length; i++)
        {
            GUILayout.BeginVertical();
            GUILayout.Label($"Star Level {i + 1} Upgrade:");
            
            upgrades[i].healthUpgrade = EditorGUILayout.FloatField("Health Upgrade", upgrades[i].healthUpgrade);
            upgrades[i].energyUpgrade = EditorGUILayout.FloatField("Energy Upgrade", upgrades[i].energyUpgrade);
            upgrades[i].attackUpgrade = EditorGUILayout.FloatField("Attack Upgrade", upgrades[i].attackUpgrade);
            upgrades[i].defenseUpgrade = EditorGUILayout.FloatField("Defense Upgrade", upgrades[i].defenseUpgrade);
            upgrades[i].speedUpgrade = EditorGUILayout.FloatField("Speed Upgrade", upgrades[i].speedUpgrade);
            upgrades[i].cooldownReductionUpgrade = EditorGUILayout.FloatField("Cooldown Reduction Upgrade", upgrades[i].cooldownReductionUpgrade);
            
            GUILayout.EndVertical();
        }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Star Upgrade"))
        {
            UpgradeJson[] newUpgrades = new UpgradeJson[upgrades.Length + 1];
            System.Array.Copy(upgrades, newUpgrades, upgrades.Length);
            newUpgrades[newUpgrades.Length - 1] = new UpgradeJson {
                healthUpgrade = 10f,
                energyUpgrade = 5f,
                attackUpgrade = 2f,
                defenseUpgrade = 1f,
                speedUpgrade = 0.5f,
                cooldownReductionUpgrade = 0.01f
            };
            upgrades = newUpgrades;
        }
        
        if (upgrades.Length > 0 && GUILayout.Button("Remove Star Upgrade"))
        {
            UpgradeJson[] newUpgrades = new UpgradeJson[upgrades.Length - 1];
            System.Array.Copy(upgrades, 0, newUpgrades, 0, newUpgrades.Length);
            upgrades = newUpgrades;
        }
        GUILayout.EndHorizontal();
        
        return upgrades;
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
            infoData.Clear();
            assetData.Clear();
            lockData.Clear();
            propertyData.Clear();
            starLevelData.Clear();
            propertyLevelData.Clear();
            propertyLevelCostData.Clear();
            starLevelCostData.Clear();
            propertyUpgradeData.Clear();
            starUpgradeData.Clear();
            selectedRoleIndex = -1;
            return;
        }

        // 读取 RoleTable.json
        string tableJson = File.ReadAllText(tableFilePath);
        RoleJson[] roleJsons = JsonConvert.DeserializeObject<RoleJson[]>(tableJson);

        // 清空现有数据
        roles.Clear();
        infoData.Clear();
        assetData.Clear();
        lockData.Clear();
        propertyData.Clear();
        starLevelData.Clear();
        propertyLevelData.Clear();
        propertyLevelCostData.Clear();
        starLevelCostData.Clear();
        propertyUpgradeData.Clear();
        starUpgradeData.Clear();

        // 加载每个角色的数据
        foreach (RoleJson roleJson in roleJsons)
        {
            roles.Add(roleJson);

            // 确保所有数据结构都初始化
            InitializeRoleData(roleJson.roleId);

            // 加载 info 数据
            string infoFilePath = Path.Combine(rootPath, roleJson.roleInfo);
            if (File.Exists(infoFilePath))
            {
                string infoJson = File.ReadAllText(infoFilePath);
                InfoJson info = JsonConvert.DeserializeObject<InfoJson>(infoJson);
                infoData[roleJson.roleId] = info;
            }

            // 加载 asset 数据
            string assetFilePath = Path.Combine(rootPath, roleJson.roleAsset);
            if (File.Exists(assetFilePath))
            {
                string assetJson = File.ReadAllText(assetFilePath);
                AssetJson asset = JsonConvert.DeserializeObject<AssetJson>(assetJson);
                assetData[roleJson.roleId] = asset;
            }

            // 加载 lock 数据
            string lockFilePath = Path.Combine(rootPath, roleJson.roleLock);
            if (File.Exists(lockFilePath))
            {
                string lockJson = File.ReadAllText(lockFilePath);
                LockJson lockObj = JsonConvert.DeserializeObject<LockJson>(lockJson);
                lockData[roleJson.roleId] = lockObj;
            }

            // 加载 property 数据
            string propertyFilePath = Path.Combine(rootPath, roleJson.roleProperty);
            if (File.Exists(propertyFilePath))
            {
                string propertyJson = File.ReadAllText(propertyFilePath);
                PropertyJson property = JsonConvert.DeserializeObject<PropertyJson>(propertyJson);
                propertyData[roleJson.roleId] = property;
            }

            // 加载 starLevel 数据
            string starLevelFilePath = Path.Combine(rootPath, roleJson.roleStarLevel);
            if (File.Exists(starLevelFilePath))
            {
                string starLevelJson = File.ReadAllText(starLevelFilePath);
                StarLevelJson starLevel = JsonConvert.DeserializeObject<StarLevelJson>(starLevelJson);
                starLevelData[roleJson.roleId] = starLevel;
            }

            // 加载 propertyLevel 数据
            string propertyLevelFilePath = Path.Combine(rootPath, roleJson.rolePropertyLevel);
            if (File.Exists(propertyLevelFilePath))
            {
                string propertyLevelJson = File.ReadAllText(propertyLevelFilePath);
                PropertyLevelJson propertyLevel = JsonConvert.DeserializeObject<PropertyLevelJson>(propertyLevelJson);
                propertyLevelData[roleJson.roleId] = propertyLevel;
            }

            // 加载 propertyLevelCost 数据
            if (!string.IsNullOrEmpty(roleJson.rolePropertyLevelCost))
            {
                string propertyLevelCostFilePath = Path.Combine(rootPath, roleJson.rolePropertyLevelCost);
                if (File.Exists(propertyLevelCostFilePath))
                {
                    string propertyLevelCostJson = File.ReadAllText(propertyLevelCostFilePath);
                    PropertyLevelCostJson propertyLevelCost = JsonConvert.DeserializeObject<PropertyLevelCostJson>(propertyLevelCostJson);
                    propertyLevelCostData[roleJson.roleId] = propertyLevelCost;
                }
            }

            // 加载 starLevelCost 数据
            if (!string.IsNullOrEmpty(roleJson.roleStarLevelCost))
            {
                string starLevelCostFilePath = Path.Combine(rootPath, roleJson.roleStarLevelCost);
                if (File.Exists(starLevelCostFilePath))
                {
                    string starLevelCostJson = File.ReadAllText(starLevelCostFilePath);
                    StarLevelCostJson starLevelCost = JsonConvert.DeserializeObject<StarLevelCostJson>(starLevelCostJson);
                    starLevelCostData[roleJson.roleId] = starLevelCost;
                }
            }

            // 加载 propertyUpgrade 数据
            if (!string.IsNullOrEmpty(roleJson.rolePropertyUpgrade))
            {
                string propertyUpgradeFilePath = Path.Combine(rootPath, roleJson.rolePropertyUpgrade);
                if (File.Exists(propertyUpgradeFilePath))
                {
                    string propertyUpgradeJson = File.ReadAllText(propertyUpgradeFilePath);
                    PropertyUpgradeJson propertyUpgrade = JsonConvert.DeserializeObject<PropertyUpgradeJson>(propertyUpgradeJson);
                    propertyUpgradeData[roleJson.roleId] = propertyUpgrade;
                }
            }

            // 加载 starUpgrade 数据
            if (!string.IsNullOrEmpty(roleJson.roleStarUpgrade))
            {
                string starUpgradeFilePath = Path.Combine(rootPath, roleJson.roleStarUpgrade);
                if (File.Exists(starUpgradeFilePath))
                {
                    string starUpgradeJson = File.ReadAllText(starUpgradeFilePath);
                    StarUpgradeJson starUpgrade = JsonConvert.DeserializeObject<StarUpgradeJson>(starUpgradeJson);
                    starUpgradeData[roleJson.roleId] = starUpgrade;
                }
            }
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
        string propertyLevelCostsPath = Path.Combine(rootPath, "PropertyLevelCosts");
        string starLevelCostsPath = Path.Combine(rootPath, "StarLevelCosts");
        string propertyUpgradesPath = Path.Combine(rootPath, "PropertyUpgrades");
        string starUpgradesPath = Path.Combine(rootPath, "StarUpgrades");

        if (!Directory.Exists(infosPath)) Directory.CreateDirectory(infosPath);
        if (!Directory.Exists(assetsPath)) Directory.CreateDirectory(assetsPath);
        if (!Directory.Exists(locksPath)) Directory.CreateDirectory(locksPath);
        if (!Directory.Exists(propertiesPath)) Directory.CreateDirectory(propertiesPath);
        if (!Directory.Exists(starLevelsPath)) Directory.CreateDirectory(starLevelsPath);
        if (!Directory.Exists(propertyLevelsPath)) Directory.CreateDirectory(propertyLevelsPath);
        if (!Directory.Exists(propertyLevelCostsPath)) Directory.CreateDirectory(propertyLevelCostsPath);
        if (!Directory.Exists(starLevelCostsPath)) Directory.CreateDirectory(starLevelCostsPath);
        if (!Directory.Exists(propertyUpgradesPath)) Directory.CreateDirectory(propertyUpgradesPath);
        if (!Directory.Exists(starUpgradesPath)) Directory.CreateDirectory(starUpgradesPath);

        // 生成总表 - 直接保存 RoleJson[] 数组
        RoleJson[] roleArray = roles.ToArray();

        for (int i = 0; i < roleArray.Length; i++)
        {
            RoleJson role = roleArray[i];
            string roleId = role.roleId;

            // 确保数据存在
            InitializeRoleData(roleId);

            // 生成 info 文件
            string infoFileName = $"{roleId}_info.json";
            string infoFilePath = Path.Combine(infosPath, infoFileName);
            string infoJson = JsonConvert.SerializeObject(infoData[roleId], Formatting.Indented);
            File.WriteAllText(infoFilePath, infoJson);
            role.roleInfo = Path.Combine("Infos", infoFileName);

            // 生成 asset 文件
            string assetFileName = $"{roleId}_asset.json";
            string assetFilePath = Path.Combine(assetsPath, assetFileName);
            string assetJson = JsonConvert.SerializeObject(assetData[roleId], Formatting.Indented);
            File.WriteAllText(assetFilePath, assetJson);
            role.roleAsset = Path.Combine("Assets", assetFileName);

            // 生成 lock 文件
            string lockFileName = $"{roleId}_lock.json";
            string lockFilePath = Path.Combine(locksPath, lockFileName);
            string lockJson = JsonConvert.SerializeObject(lockData[roleId], Formatting.Indented);
            File.WriteAllText(lockFilePath, lockJson);
            role.roleLock = Path.Combine("Locks", lockFileName);

            // 生成 property 文件
            string propertyFileName = $"{roleId}_property.json";
            string propertyFilePath = Path.Combine(propertiesPath, propertyFileName);
            string propertyJson = JsonConvert.SerializeObject(propertyData[roleId], Formatting.Indented);
            File.WriteAllText(propertyFilePath, propertyJson);
            role.roleProperty = Path.Combine("Properties", propertyFileName);

            // 生成 starLevel 文件
            string starLevelFileName = $"{roleId}_starLevel.json";
            string starLevelFilePath = Path.Combine(starLevelsPath, starLevelFileName);
            string starLevelJson = JsonConvert.SerializeObject(starLevelData[roleId], Formatting.Indented);
            File.WriteAllText(starLevelFilePath, starLevelJson);
            role.roleStarLevel = Path.Combine("StarLevels", starLevelFileName);

            // 生成 propertyLevel 文件
            string propertyLevelFileName = $"{roleId}_propertyLevel.json";
            string propertyLevelFilePath = Path.Combine(propertyLevelsPath, propertyLevelFileName);
            string propertyLevelJson = JsonConvert.SerializeObject(propertyLevelData[roleId], Formatting.Indented);
            File.WriteAllText(propertyLevelFilePath, propertyLevelJson);
            role.rolePropertyLevel = Path.Combine("PropertyLevels", propertyLevelFileName);

            // 生成 propertyLevelCost 文件
            string propertyLevelCostFileName = $"{roleId}_propertyLevelCost.json";
            string propertyLevelCostFilePath = Path.Combine(propertyLevelCostsPath, propertyLevelCostFileName);
            string propertyLevelCostJson = JsonConvert.SerializeObject(propertyLevelCostData[roleId], Formatting.Indented);
            File.WriteAllText(propertyLevelCostFilePath, propertyLevelCostJson);
            role.rolePropertyLevelCost = Path.Combine("PropertyLevelCosts", propertyLevelCostFileName);

            // 生成 starLevelCost 文件
            string starLevelCostFileName = $"{roleId}_starLevelCost.json";
            string starLevelCostFilePath = Path.Combine(starLevelCostsPath, starLevelCostFileName);
            string starLevelCostJson = JsonConvert.SerializeObject(starLevelCostData[roleId], Formatting.Indented);
            File.WriteAllText(starLevelCostFilePath, starLevelCostJson);
            role.roleStarLevelCost = Path.Combine("StarLevelCosts", starLevelCostFileName);

            // 生成 propertyUpgrade 文件
            string propertyUpgradeFileName = $"{roleId}_propertyUpgrade.json";
            string propertyUpgradeFilePath = Path.Combine(propertyUpgradesPath, propertyUpgradeFileName);
            string propertyUpgradeJson = JsonConvert.SerializeObject(propertyUpgradeData[roleId], Formatting.Indented);
            File.WriteAllText(propertyUpgradeFilePath, propertyUpgradeJson);
            role.rolePropertyUpgrade = Path.Combine("PropertyUpgrades", propertyUpgradeFileName);

            // 生成 starUpgrade 文件
            string starUpgradeFileName = $"{roleId}_starUpgrade.json";
            string starUpgradeFilePath = Path.Combine(starUpgradesPath, starUpgradeFileName);
            string starUpgradeJson = JsonConvert.SerializeObject(starUpgradeData[roleId], Formatting.Indented);
            File.WriteAllText(starUpgradeFilePath, starUpgradeJson);
            role.roleStarUpgrade = Path.Combine("StarUpgrades", starUpgradeFileName);
        }

        // 保存总表 - 直接保存 RoleJson[] 数组
        string tableFilePath = Path.Combine(rootPath, "RoleTable.json");
        string tableJson = JsonConvert.SerializeObject(roleArray, Formatting.Indented);
        File.WriteAllText(tableFilePath, tableJson);

        EditorUtility.DisplayDialog("Success", "Role data files generated successfully!\n\nRoot Path: " + rootPath, "OK");
    }
}