using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

public class RoleDataTool : EditorWindow
{
    private const string ROLE_TABLE_ROOT = "StreamingAssets/RoleTable";
    private const string ROLE_TABLE_FILE = "RoleTable.json";

    private List<RoleJson> _roleJsons = new List<RoleJson>();
    private int _selectedRoleIndex = -1;
    private string _newRoleId = "";

    private InfoJson _currentInfo;
    private AssetJson _currentAsset;
    private LockJson _currentLock;
    private PropertyJson _currentProperty;
    private ChainLevelJson _currentChainLevel;
    private PropertyLevelJson _currentPropertyLevel;
    private ChainLevelCostJson _currentChainLevelCost;
    private PropertyLevelCostJson _currentPropertyLevelCost;
    private ChainUpgradeJson _currentChainUpgrade;
    private PropertyUpgradeJson _currentPropertyUpgrade;

    private int _selectedTabIndex = 0;
    private string[] _tabNames = { "角色列表", "基本信息", "资产", "锁定", "属性", "等级", "升级" };
    private Vector2 _scrollPosition;

    private GUIStyle _whiteLabelStyle;

    private GUIStyle WhiteLabelStyle
    {
        get
        {
            if (_whiteLabelStyle == null)
            {
                _whiteLabelStyle = new GUIStyle(EditorStyles.label);
                _whiteLabelStyle.normal.textColor = Color.white;
                _whiteLabelStyle.focused.textColor = Color.white;
                _whiteLabelStyle.active.textColor = Color.white;
                _whiteLabelStyle.hover.textColor = Color.white;
                _whiteLabelStyle.onNormal.textColor = Color.white;
                _whiteLabelStyle.onFocused.textColor = Color.white;
                _whiteLabelStyle.onActive.textColor = Color.white;
                _whiteLabelStyle.onHover.textColor = Color.white;
            }
            return _whiteLabelStyle;
        }
    }

    [MenuItem("Tools/Role Data Tool")]
    public static void ShowWindow()
    {
        GetWindow<RoleDataTool>("角色数据工具");
    }

    private void OnEnable()
    {
        LoadRoleTable();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(200));
            {
                DrawRoleList();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            {
                _selectedTabIndex = GUILayout.Toolbar(_selectedTabIndex, _tabNames);

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                {
                    switch (_selectedTabIndex)
                    {
                        case 0:
                            DrawRoleListTab();
                            break;
                        case 1:
                            DrawInfoTab();
                            break;
                        case 2:
                            DrawAssetTab();
                            break;
                        case 3:
                            DrawLockTab();
                            break;
                        case 4:
                            DrawPropertyTab();
                            break;
                        case 5:
                            DrawLevelTab();
                            break;
                        case 6:
                            DrawUpgradeTab();
                            break;
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawRoleList()
    {
        GUILayout.Label("角色列表", WhiteLabelStyle);
        GUILayout.Space(5);

        for (int i = 0; i < _roleJsons.Count; i++)
        {
            bool isSelected = _selectedRoleIndex == i;

            if (GUILayout.Button(_roleJsons[i].roleId, isSelected ? EditorStyles.toolbarButton : WhiteLabelStyle))
            {
                _selectedRoleIndex = i;
                LoadCurrentRoleData();
            }
        }

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        {
            _newRoleId = EditorGUILayout.TextField("新角色ID", _newRoleId);

            if (GUILayout.Button("添加", GUILayout.Width(60)))
            {
                AddRole();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (_selectedRoleIndex >= 0)
        {
            if (GUILayout.Button("删除选中", GUILayout.Width(200)))
            {
                if (EditorUtility.DisplayDialog("确认删除", $"确定要删除角色 {_roleJsons[_selectedRoleIndex].roleId} 吗？", "确定", "取消"))
                {
                    RemoveRole();
                }
            }
        }

        if (GUILayout.Button("保存所有", GUILayout.Width(200)))
        {
            SaveAll();
        }
    }

    private void DrawRoleListTab()
    {
        GUILayout.Label("角色列表管理", WhiteLabelStyle);
        GUILayout.Space(10);

        if (_roleJsons.Count == 0)
        {
            GUILayout.Label("暂无角色，请点击左侧添加", WhiteLabelStyle);
            return;
        }

        for (int i = 0; i < _roleJsons.Count; i++)
        {
            RoleJson role = _roleJsons[i];
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                GUILayout.Label($"角色ID: {role.roleId}", WhiteLabelStyle);
                GUILayout.Label($"信息路径: {role.roleInfo}", WhiteLabelStyle);
                GUILayout.Label($"资产路径: {role.roleAsset}", WhiteLabelStyle);
                GUILayout.Label($"锁定路径: {role.roleLock}", WhiteLabelStyle);
                GUILayout.Label($"属性路径: {role.roleProperty}", WhiteLabelStyle);
                GUILayout.Label($"共鸣链等级路径: {role.roleChainLevel}", WhiteLabelStyle);
                GUILayout.Label($"属性等级路径: {role.rolePropertyLevel}", WhiteLabelStyle);
                GUILayout.Label($"共鸣链升级成本路径: {role.roleChainLevelCost}", WhiteLabelStyle);
                GUILayout.Label($"属性升级成本路径: {role.rolePropertyLevelCost}", WhiteLabelStyle);
                GUILayout.Label($"共鸣链提升路径: {role.roleChainUpgrade}", WhiteLabelStyle);
                GUILayout.Label($"属性提升路径: {role.rolePropertyUpgrade}", WhiteLabelStyle);
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void DrawInfoTab()
    {
        if (_currentInfo == null)
        {
            GUILayout.Label("请先选择一个角色", WhiteLabelStyle);
            return;
        }

        GUILayout.Label("角色基本信息", WhiteLabelStyle);
        GUILayout.Space(10);

        _currentInfo.roleName = EditorGUILayout.TextField("角色名称", _currentInfo.roleName);
        _currentInfo.roleDesc = EditorGUILayout.TextArea(_currentInfo.roleDesc, GUILayout.Height(100));

        if (GUILayout.Button("保存"))
        {
            SaveCurrentInfo();
        }
    }

    private void DrawAssetTab()
    {
        if (_currentAsset == null)
        {
            GUILayout.Label("请先选择一个角色", WhiteLabelStyle);
            return;
        }

        GUILayout.Label("角色资源", WhiteLabelStyle);
        GUILayout.Space(10);

        _currentAsset.roleImg = EditorGUILayout.TextField("角色图片路径", _currentAsset.roleImg);
        _currentAsset.roleModel = EditorGUILayout.TextField("角色模型路径", _currentAsset.roleModel);

        if (GUILayout.Button("保存"))
        {
            SaveCurrentAsset();
        }
    }

    private void DrawLockTab()
    {
        if (_currentLock == null)
        {
            GUILayout.Label("请先选择一个角色", WhiteLabelStyle);
            return;
        }

        GUILayout.Label("角色解锁配置", WhiteLabelStyle);
        GUILayout.Space(10);

        if (_currentLock.unlockCosts == null)
        {
            _currentLock.unlockCosts = new CostJson[0];
        }

        _currentLock.unlockCosts = DrawCostArray("解锁消耗", _currentLock.unlockCosts);

        if (_currentLock.unlockConditionsJsons == null)
        {
            _currentLock.unlockConditionsJsons = new SpecialConditionJson[0];
        }

        _currentLock.unlockConditionsJsons = DrawConditionArray("解锁条件", _currentLock.unlockConditionsJsons);

        if (GUILayout.Button("保存"))
        {
            SaveCurrentLock();
        }
    }

    private void DrawPropertyTab()
    {
        if (_currentProperty == null)
        {
            GUILayout.Label("请先选择一个角色", WhiteLabelStyle);
            return;
        }

        GUILayout.Label("角色基础属性", WhiteLabelStyle);
        GUILayout.Space(10);

        _currentProperty.health = EditorGUILayout.FloatField("生命值", _currentProperty.health);
        _currentProperty.energy = EditorGUILayout.FloatField("能量值", _currentProperty.energy);
        _currentProperty.attack = EditorGUILayout.FloatField("攻击力", _currentProperty.attack);
        _currentProperty.defense = EditorGUILayout.FloatField("防御力", _currentProperty.defense);
        _currentProperty.speed = EditorGUILayout.FloatField("移动速度", _currentProperty.speed);
        _currentProperty.cooldownReduction = EditorGUILayout.FloatField("冷却缩减", _currentProperty.cooldownReduction);
        _currentProperty.energyRecoveryRate = EditorGUILayout.FloatField("能量恢复速率", _currentProperty.energyRecoveryRate);

        if (GUILayout.Button("保存"))
        {
            SaveCurrentProperty();
        }
    }

    private void DrawLevelTab()
    {
        if (_currentChainLevel == null || _currentPropertyLevel == null)
        {
            GUILayout.Label("请先选择一个角色", WhiteLabelStyle);
            return;
        }

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(300));
            {
                GUILayout.Label("共鸣链等级", WhiteLabelStyle);
                GUILayout.Space(5);

                if (_currentChainLevel.chainLevel == null)
                {
                    _currentChainLevel.chainLevel = new LevelJson();
                }

                EditorGUILayout.LabelField("基础等级:", "0", WhiteLabelStyle);
                _currentChainLevel.chainLevel.baseLevel = 0;
                _currentChainLevel.chainLevel.maxLevel = EditorGUILayout.IntField("最大等级", _currentChainLevel.chainLevel.maxLevel);

                if (GUILayout.Button("保存共鸣链等级"))
                {
                    SaveCurrentChainLevel();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            {
                GUILayout.Label("属性等级", WhiteLabelStyle);
                GUILayout.Space(5);

                DrawLevelField(ref _currentPropertyLevel.healthLevel, "生命值等级");
                DrawLevelField(ref _currentPropertyLevel.energyLevel, "能量值等级");
                DrawLevelField(ref _currentPropertyLevel.attackLevel, "攻击力等级");
                DrawLevelField(ref _currentPropertyLevel.speedLevel, "移动速度等级");
                DrawLevelField(ref _currentPropertyLevel.defenseLevel, "防御力等级");
                DrawLevelField(ref _currentPropertyLevel.cooldownReductionLevel, "冷却缩减等级");
                DrawLevelField(ref _currentPropertyLevel.energyRecoveryRateLevel, "能量恢复等级");

                if (GUILayout.Button("保存属性等级"))
                {
                    SaveCurrentPropertyLevel();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private bool _showChainLevelCost = true;
    private bool _showPropertyLevelCost = true;
    private bool _showChainUpgrade = true;
    private bool _showPropertyUpgrade = true;

    private void DrawUpgradeTab()
    {
        if (_currentChainLevelCost == null || _currentPropertyLevelCost == null ||
            _currentChainUpgrade == null || _currentPropertyUpgrade == null ||
            _currentChainLevel == null || _currentPropertyLevel == null)
        {
            GUILayout.Label("请先选择一个角色", WhiteLabelStyle);
            return;
        }

        GUILayout.Label("升级配置", WhiteLabelStyle);
        GUILayout.Space(5);

        int chainMaxLevel = _currentChainLevel.chainLevel != null ? _currentChainLevel.chainLevel.maxLevel : 0;
        int healthMaxLevel = _currentPropertyLevel.healthLevel != null ? _currentPropertyLevel.healthLevel.maxLevel : 0;
        int energyMaxLevel = _currentPropertyLevel.energyLevel != null ? _currentPropertyLevel.energyLevel.maxLevel : 0;
        int attackMaxLevel = _currentPropertyLevel.attackLevel != null ? _currentPropertyLevel.attackLevel.maxLevel : 0;
        int speedMaxLevel = _currentPropertyLevel.speedLevel != null ? _currentPropertyLevel.speedLevel.maxLevel : 0;
        int defenseMaxLevel = _currentPropertyLevel.defenseLevel != null ? _currentPropertyLevel.defenseLevel.maxLevel : 0;
        int cooldownMaxLevel = _currentPropertyLevel.cooldownReductionLevel != null ? _currentPropertyLevel.cooldownReductionLevel.maxLevel : 0;
        int recoveryMaxLevel = _currentPropertyLevel.energyRecoveryRateLevel != null ? _currentPropertyLevel.energyRecoveryRateLevel.maxLevel : 0;

        _showChainLevelCost = EditorGUILayout.Foldout(_showChainLevelCost, "共鸣链升级成本", true);
        if (_showChainLevelCost)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EnsureLevelCostArray(ref _currentChainLevelCost.chainLevelCosts, chainMaxLevel);
                DrawLevelCostArrayLocked(_currentChainLevelCost.chainLevelCosts);
            }
            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(8);

        _showPropertyLevelCost = EditorGUILayout.Foldout(_showPropertyLevelCost, "属性升级成本", true);
        if (_showPropertyLevelCost)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EnsureLevelCostArray(ref _currentPropertyLevelCost.healthLevelCosts, healthMaxLevel);
                DrawLevelCostFieldLocked("生命值", _currentPropertyLevelCost.healthLevelCosts);
                GUILayout.Space(4);

                EnsureLevelCostArray(ref _currentPropertyLevelCost.energyLevelCosts, energyMaxLevel);
                DrawLevelCostFieldLocked("能量值", _currentPropertyLevelCost.energyLevelCosts);
                GUILayout.Space(4);

                EnsureLevelCostArray(ref _currentPropertyLevelCost.attackLevelCosts, attackMaxLevel);
                DrawLevelCostFieldLocked("攻击力", _currentPropertyLevelCost.attackLevelCosts);
                GUILayout.Space(4);

                EnsureLevelCostArray(ref _currentPropertyLevelCost.speedLevelCosts, speedMaxLevel);
                DrawLevelCostFieldLocked("移动速度", _currentPropertyLevelCost.speedLevelCosts);
                GUILayout.Space(4);

                EnsureLevelCostArray(ref _currentPropertyLevelCost.defenseLevelCosts, defenseMaxLevel);
                DrawLevelCostFieldLocked("防御力", _currentPropertyLevelCost.defenseLevelCosts);
                GUILayout.Space(4);

                EnsureLevelCostArray(ref _currentPropertyLevelCost.cooldownReductionLevelCosts, cooldownMaxLevel);
                DrawLevelCostFieldLocked("冷却缩减", _currentPropertyLevelCost.cooldownReductionLevelCosts);
                GUILayout.Space(4);

                EnsureLevelCostArray(ref _currentPropertyLevelCost.energyRecoveryRateLevelCosts, recoveryMaxLevel);
                DrawLevelCostFieldLocked("能量恢复", _currentPropertyLevelCost.energyRecoveryRateLevelCosts);
            }
            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(8);

        _showChainUpgrade = EditorGUILayout.Foldout(_showChainUpgrade, "共鸣链提升效果", true);
        if (_showChainUpgrade)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EnsurePropertyArray(ref _currentChainUpgrade.upgradeJsons, chainMaxLevel);
                DrawPropertyArrayLocked(_currentChainUpgrade.upgradeJsons);
            }
            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(8);

        _showPropertyUpgrade = EditorGUILayout.Foldout(_showPropertyUpgrade, "属性提升效果", true);
        if (_showPropertyUpgrade)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EnsureFloatArray(ref _currentPropertyUpgrade.healthUpgrade, healthMaxLevel);
                DrawFloatArrayLocked("生命值提升", _currentPropertyUpgrade.healthUpgrade);
                GUILayout.Space(4);

                EnsureFloatArray(ref _currentPropertyUpgrade.energyUpgrade, energyMaxLevel);
                DrawFloatArrayLocked("能量值提升", _currentPropertyUpgrade.energyUpgrade);
                GUILayout.Space(4);

                EnsureFloatArray(ref _currentPropertyUpgrade.attackUpgrade, attackMaxLevel);
                DrawFloatArrayLocked("攻击力提升", _currentPropertyUpgrade.attackUpgrade);
                GUILayout.Space(4);

                EnsureFloatArray(ref _currentPropertyUpgrade.speedUpgrade, speedMaxLevel);
                DrawFloatArrayLocked("移动速度提升", _currentPropertyUpgrade.speedUpgrade);
                GUILayout.Space(4);

                EnsureFloatArray(ref _currentPropertyUpgrade.defenseUpgrade, defenseMaxLevel);
                DrawFloatArrayLocked("防御力提升", _currentPropertyUpgrade.defenseUpgrade);
                GUILayout.Space(4);

                EnsureFloatArray(ref _currentPropertyUpgrade.cooldownReductionUpgrade, cooldownMaxLevel);
                DrawFloatArrayLocked("冷却缩减提升", _currentPropertyUpgrade.cooldownReductionUpgrade);
                GUILayout.Space(4);

                EnsureFloatArray(ref _currentPropertyUpgrade.energyRecoveryRateUpgrade, recoveryMaxLevel);
                DrawFloatArrayLocked("能量恢复提升", _currentPropertyUpgrade.energyRecoveryRateUpgrade);
            }
            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("保存升级配置", GUILayout.Height(30)))
        {
            SaveCurrentUpgrade();
        }
    }

    private void DrawLevelField(ref LevelJson level, string label)
    {
        if (level == null)
        {
            level = new LevelJson();
        }

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label(label, WhiteLabelStyle, GUILayout.Width(80));
            EditorGUILayout.LabelField("0", WhiteLabelStyle, GUILayout.Width(60));
            level.baseLevel = 0;
            GUILayout.Label("→", WhiteLabelStyle, GUILayout.Width(20));
            level.maxLevel = EditorGUILayout.IntField(level.maxLevel, GUILayout.Width(60));
        }
        EditorGUILayout.EndHorizontal();
    }

    private int _costDeleteIndex = -1;
    private int _conditionDeleteIndex = -1;
    private int _levelCostDeleteIndex = -1;
    private int _propertyDeleteIndex = -1;
    private int _floatArrayAddIndex = -1;
    private int _floatArrayRemoveIndex = -1;

    private string _hoveredGoodsId = null;
    private Rect _tooltipRect = Rect.zero;

    private List<GoodsJson> _goodsList = new List<GoodsJson>();
    private Dictionary<int, string> _costSearchTexts = new Dictionary<int, string>();

    private void LoadGoodsList()
    {
        _goodsList = GoodsDataTool.LoadGoodsData();
    }

    private CostJson[] DrawCostArray(string label, CostJson[] costs)
    {
        LoadGoodsList();

        EditorGUILayout.LabelField(label, WhiteLabelStyle);
        GUILayout.Space(8);

        for (int i = 0; i < costs.Length; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField($"#{i + 1}", WhiteLabelStyle, GUILayout.Width(30));

                    GUILayout.Label("物品:", WhiteLabelStyle, GUILayout.Width(40));

                    string searchText = "";
                    _costSearchTexts.TryGetValue(i, out searchText);

                    List<GoodsJson> filteredGoods = _goodsList;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        filteredGoods = _goodsList.FindAll(g =>
                            g.goodsId.IndexOf(searchText, System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                            g.goodsName.IndexOf(searchText, System.StringComparison.OrdinalIgnoreCase) >= 0);
                    }

                    GoodsJson selectedGoods = _goodsList.Find(g => g.goodsId == costs[i].goodsId);
                    bool isSelectedInFiltered = selectedGoods != null && filteredGoods.Contains(selectedGoods);

                    List<string> goodsNames = new List<string>();
                    goodsNames.Add("请选择");
                    goodsNames.AddRange(filteredGoods.ConvertAll(g => $"{g.goodsId} - {g.goodsName}"));

                    int currentIndex = isSelectedInFiltered ? filteredGoods.IndexOf(selectedGoods) + 1 : 0;
                    int newIndex = EditorGUILayout.Popup(currentIndex, goodsNames.ToArray(), GUILayout.Width(140));

                    if (newIndex != currentIndex)
                    {
                        if (newIndex > 0)
                        {
                            costs[i].goodsId = filteredGoods[newIndex - 1].goodsId;
                        }
                        else
                        {
                            costs[i].goodsId = "";
                        }
                    }

                    GUILayout.Label("搜索:", WhiteLabelStyle, GUILayout.Width(40));
                    string currentSearchText;
                    _costSearchTexts.TryGetValue(i, out currentSearchText);
                    _costSearchTexts[i] = EditorGUILayout.TextField(currentSearchText ?? "", GUILayout.Width(100));

                    GUILayout.Label("数量:", WhiteLabelStyle, GUILayout.Width(40));
                    costs[i].amount = EditorGUILayout.IntField(costs[i].amount, GUILayout.Width(60));

                    if (!string.IsNullOrEmpty(costs[i].goodsId))
                    {
                        GoodsJson goods = _goodsList.Find(g => g.goodsId == costs[i].goodsId);
                        if (goods != null)
                        {
                            GUILayout.Label("名称:", WhiteLabelStyle, GUILayout.Width(40));
                            GUI.enabled = false;
                            EditorGUILayout.TextField(goods.goodsName, GUILayout.Width(100));
                            GUI.enabled = true;

                            GUILayout.Label("类型:", WhiteLabelStyle, GUILayout.Width(40));
                            GUI.enabled = false;
                            EditorGUILayout.TextField(goods.goodsType.ToString(), GUILayout.Width(60));
                            GUI.enabled = true;

                            GUILayout.Label("稀有度:", WhiteLabelStyle, GUILayout.Width(50));
                            GUI.enabled = false;
                            EditorGUILayout.TextField(goods.rarity.ToString(), GUILayout.Width(60));
                            GUI.enabled = true;

                            GUIContent tooltipContent = new GUIContent("?", goods.goodsDesc);
                            GUILayout.Button(tooltipContent, EditorStyles.helpBox, GUILayout.Width(22), GUILayout.Height(20));
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(8);

                if (GUILayout.Button("删除此项", GUILayout.Height(24)))
                {
                    _costDeleteIndex = i;
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(12);
        }

        if (GUILayout.Button($"+ 添加{label}", GUILayout.Height(28)))
        {
            List<CostJson> list = new List<CostJson>(costs);
            list.Add(new CostJson());
            costs = list.ToArray();
        }

        if (_costDeleteIndex >= 0 && _costDeleteIndex < costs.Length)
        {
            List<CostJson> list = new List<CostJson>(costs);
            list.RemoveAt(_costDeleteIndex);
            costs = list.ToArray();
            _costDeleteIndex = -1;
        }

        return costs;
    }

    private SpecialConditionJson[] DrawConditionArray(string label, SpecialConditionJson[] conditions)
    {
        EditorGUILayout.LabelField(label, WhiteLabelStyle);
        GUILayout.Space(2);

        for (int i = 0; i < conditions.Length; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField($"#{i + 1}", WhiteLabelStyle, GUILayout.Width(30));
                    GUILayout.Label("条件类型:", WhiteLabelStyle, GUILayout.Width(60));
                    conditions[i].condition = EditorGUILayout.TextField("", conditions[i].condition, GUILayout.Width(300));
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("", WhiteLabelStyle, GUILayout.Width(30));
                    GUILayout.Label("条件参数:", WhiteLabelStyle, GUILayout.Width(60));
                    conditions[i].conditionParams = EditorGUILayout.TextField("", conditions[i].conditionParams, GUILayout.Width(300));
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);

                if (GUILayout.Button("删除此项", GUILayout.Height(24)))
                {
                    _conditionDeleteIndex = i;
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
        }

        if (GUILayout.Button($"+ 添加{label}", GUILayout.Height(28)))
        {
            List<SpecialConditionJson> list = new List<SpecialConditionJson>(conditions);
            list.Add(new SpecialConditionJson());
            conditions = list.ToArray();
        }

        if (_conditionDeleteIndex >= 0 && _conditionDeleteIndex < conditions.Length)
        {
            List<SpecialConditionJson> list = new List<SpecialConditionJson>(conditions);
            list.RemoveAt(_conditionDeleteIndex);
            conditions = list.ToArray();
            _conditionDeleteIndex = -1;
        }

        return conditions;
    }

    private LevelCostJson[] DrawLevelCostArray(LevelCostJson[] levelCosts)
    {
        for (int i = 0; i < levelCosts.Length; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField($"等级 {i + 1} 成本", WhiteLabelStyle);
                GUILayout.Space(2);

                if (levelCosts[i].costJsons == null)
                {
                    levelCosts[i].costJsons = new CostJson[0];
                }
                levelCosts[i].costJsons = DrawCostArray("消耗", levelCosts[i].costJsons);

                GUILayout.Space(2);

                if (GUILayout.Button("删除此等级成本", GUILayout.Height(24)))
                {
                    _levelCostDeleteIndex = i;
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(8);
        }

        if (GUILayout.Button("+ 添加等级成本", GUILayout.Height(28)))
        {
            List<LevelCostJson> list = new List<LevelCostJson>(levelCosts);
            list.Add(new LevelCostJson());
            levelCosts = list.ToArray();
        }

        if (_levelCostDeleteIndex >= 0 && _levelCostDeleteIndex < levelCosts.Length)
        {
            List<LevelCostJson> list = new List<LevelCostJson>(levelCosts);
            list.RemoveAt(_levelCostDeleteIndex);
            levelCosts = list.ToArray();
            _levelCostDeleteIndex = -1;
        }

        return levelCosts;
    }

    private LevelCostJson[] DrawLevelCostField(string label, LevelCostJson[] levelCosts)
    {
        EditorGUILayout.LabelField(label, WhiteLabelStyle);
        GUILayout.Space(2);

        if (levelCosts == null)
        {
            levelCosts = new LevelCostJson[0];
        }
        return DrawLevelCostArray(levelCosts);
    }

    private PropertyJson[] DrawPropertyArray(PropertyJson[] properties)
    {
        for (int i = 0; i < properties.Length; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField($"等级 {i + 1} 属性", WhiteLabelStyle);
                GUILayout.Space(2);
                DrawPropertyFields(properties[i]);

                GUILayout.Space(2);

                if (GUILayout.Button("删除此项", GUILayout.Height(24)))
                {
                    _propertyDeleteIndex = i;
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
        }

        if (GUILayout.Button("+ 添加属性", GUILayout.Height(28)))
        {
            List<PropertyJson> list = new List<PropertyJson>(properties);
            list.Add(new PropertyJson());
            properties = list.ToArray();
        }

        if (_propertyDeleteIndex >= 0 && _propertyDeleteIndex < properties.Length)
        {
            List<PropertyJson> list = new List<PropertyJson>(properties);
            list.RemoveAt(_propertyDeleteIndex);
            properties = list.ToArray();
            _propertyDeleteIndex = -1;
        }

        return properties;
    }

    private void DrawPropertyFields(PropertyJson property)
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("生命", WhiteLabelStyle, GUILayout.Width(40));
            property.health = EditorGUILayout.FloatField(property.health, GUILayout.Width(60));
            GUILayout.Label("能量", WhiteLabelStyle, GUILayout.Width(40));
            property.energy = EditorGUILayout.FloatField(property.energy, GUILayout.Width(60));
            GUILayout.Label("攻击", WhiteLabelStyle, GUILayout.Width(40));
            property.attack = EditorGUILayout.FloatField(property.attack, GUILayout.Width(60));
            GUILayout.Label("防御", WhiteLabelStyle, GUILayout.Width(40));
            property.defense = EditorGUILayout.FloatField(property.defense, GUILayout.Width(60));
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(2);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Label("速度", WhiteLabelStyle, GUILayout.Width(40));
            property.speed = EditorGUILayout.FloatField(property.speed, GUILayout.Width(60));
            GUILayout.Label("冷却", WhiteLabelStyle, GUILayout.Width(40));
            property.cooldownReduction = EditorGUILayout.FloatField(property.cooldownReduction, GUILayout.Width(60));
            GUILayout.Label("回能", WhiteLabelStyle, GUILayout.Width(40));
            property.energyRecoveryRate = EditorGUILayout.FloatField(property.energyRecoveryRate, GUILayout.Width(60));
        }
        EditorGUILayout.EndHorizontal();
    }

    private float[] DrawFloatArray(string label, float[] values)
    {
        if (values == null)
        {
            values = new float[0];
        }

        EditorGUILayout.LabelField(label, WhiteLabelStyle);
        GUILayout.Space(2);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.BeginHorizontal();
            {
                for (int i = 0; i < values.Length; i++)
                {
                    GUILayout.Label($"Lv{i + 1}", WhiteLabelStyle, GUILayout.Width(25));
                    values[i] = EditorGUILayout.FloatField(values[i], GUILayout.Width(60));
                    GUILayout.Space(4);
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(4);

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("+ 添加等级", GUILayout.Height(24), GUILayout.Width(100)))
                {
                    _floatArrayAddIndex = 1;
                }

                if (values.Length > 0 && GUILayout.Button("- 移除最后", GUILayout.Height(24), GUILayout.Width(100)))
                {
                    _floatArrayRemoveIndex = values.Length - 1;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(4);

        if (_floatArrayAddIndex >= 0)
        {
            List<float> list = new List<float>(values);
            list.Add(0);
            values = list.ToArray();
            _floatArrayAddIndex = -1;
        }

        if (_floatArrayRemoveIndex >= 0 && _floatArrayRemoveIndex < values.Length)
        {
            List<float> list = new List<float>(values);
            list.RemoveAt(_floatArrayRemoveIndex);
            values = list.ToArray();
            _floatArrayRemoveIndex = -1;
        }

        return values;
    }

    private void EnsureLevelCostArray(ref LevelCostJson[] array, int maxLevel)
    {
        if (array == null)
        {
            array = new LevelCostJson[0];
        }

        if (array.Length != maxLevel)
        {
            LevelCostJson[] newArray = new LevelCostJson[maxLevel];
            for (int i = 0; i < maxLevel; i++)
            {
                if (i < array.Length)
                {
                    newArray[i] = array[i];
                }
                else
                {
                    newArray[i] = new LevelCostJson();
                }
            }
            array = newArray;
        }
    }

    private void EnsurePropertyArray(ref PropertyJson[] array, int maxLevel)
    {
        if (array == null)
        {
            array = new PropertyJson[0];
        }

        if (array.Length != maxLevel)
        {
            PropertyJson[] newArray = new PropertyJson[maxLevel];
            for (int i = 0; i < maxLevel; i++)
            {
                if (i < array.Length)
                {
                    newArray[i] = array[i];
                }
                else
                {
                    newArray[i] = new PropertyJson();
                }
            }
            array = newArray;
        }
    }

    private void EnsureFloatArray(ref float[] array, int maxLevel)
    {
        if (array == null)
        {
            array = new float[0];
        }

        if (array.Length != maxLevel)
        {
            float[] newArray = new float[maxLevel];
            for (int i = 0; i < maxLevel; i++)
            {
                if (i < array.Length)
                {
                    newArray[i] = array[i];
                }
                else
                {
                    newArray[i] = 0;
                }
            }
            array = newArray;
        }
    }

    private void DrawLevelCostArrayLocked(LevelCostJson[] levelCosts)
    {
        for (int i = 0; i < levelCosts.Length; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField($"等级 {i + 1} 成本", WhiteLabelStyle);
                GUILayout.Space(2);

                if (levelCosts[i].costJsons == null)
                {
                    levelCosts[i].costJsons = new CostJson[0];
                }
                levelCosts[i].costJsons = DrawCostArray("消耗", levelCosts[i].costJsons);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(8);
        }
    }

    private void DrawLevelCostFieldLocked(string label, LevelCostJson[] levelCosts)
    {
        EditorGUILayout.LabelField(label, WhiteLabelStyle);
        GUILayout.Space(2);

        if (levelCosts == null)
        {
            levelCosts = new LevelCostJson[0];
        }
        DrawLevelCostArrayLocked(levelCosts);
    }

    private void DrawPropertyArrayLocked(PropertyJson[] properties)
    {
        for (int i = 0; i < properties.Length; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField($"等级 {i + 1} 属性", WhiteLabelStyle);
                GUILayout.Space(2);
                DrawPropertyFields(properties[i]);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
        }
    }

    private void DrawFloatArrayLocked(string label, float[] values)
    {
        if (values == null)
        {
            values = new float[0];
        }

        EditorGUILayout.LabelField(label, WhiteLabelStyle);
        GUILayout.Space(2);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.BeginHorizontal();
            {
                for (int i = 0; i < values.Length; i++)
                {
                    GUILayout.Label($"Lv{i + 1}", WhiteLabelStyle, GUILayout.Width(25));
                    values[i] = EditorGUILayout.FloatField(values[i], GUILayout.Width(60));
                    GUILayout.Space(4);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(4);
    }

    private void LoadRoleTable()
    {
        string path = GetRoleTableFilePath();

        if (!File.Exists(path))
        {
            _roleJsons = new List<RoleJson>();
            return;
        }

        string content = File.ReadAllText(path);
        _roleJsons = JsonConvert.DeserializeObject<List<RoleJson>>(content) ?? new List<RoleJson>();
    }

    private void SaveRoleTable()
    {
        string path = GetRoleTableFilePath();
        string content = JsonConvert.SerializeObject(_roleJsons, Formatting.Indented);
        File.WriteAllText(path, content);
    }

    private void AddRole()
    {
        if (string.IsNullOrEmpty(_newRoleId))
        {
            EditorUtility.DisplayDialog("错误", "角色ID不能为空", "确定");
            return;
        }

        if (_roleJsons.Exists(r => r.roleId == _newRoleId))
        {
            EditorUtility.DisplayDialog("错误", "角色ID已存在", "确定");
            return;
        }

        RoleJson role = new RoleJson
        {
            roleId = _newRoleId,
            roleInfo = GeneratePath("Infos", _newRoleId, "info"),
            roleAsset = GeneratePath("Assets", _newRoleId, "asset"),
            roleLock = GeneratePath("Locks", _newRoleId, "lock"),
            roleProperty = GeneratePath("Properties", _newRoleId, "property"),
            roleChainLevel = GeneratePath("ChainLevels", _newRoleId, "chainLevel"),
            rolePropertyLevel = GeneratePath("PropertyLevels", _newRoleId, "propertyLevel"),
            roleChainLevelCost = GeneratePath("ChainLevelCosts", _newRoleId, "chainLevelCost"),
            rolePropertyLevelCost = GeneratePath("PropertyLevelCosts", _newRoleId, "propertyLevelCost"),
            roleChainUpgrade = GeneratePath("ChainUpgrades", _newRoleId, "chainUpgrade"),
            rolePropertyUpgrade = GeneratePath("PropertyUpgrades", _newRoleId, "propertyUpgrade")
        };

        _roleJsons.Add(role);
        _selectedRoleIndex = _roleJsons.Count - 1;

        CreateDefaultDataFiles(_newRoleId);
        LoadCurrentRoleData();

        _newRoleId = "";
        SaveRoleTable();

        EditorUtility.DisplayDialog("成功", $"角色 {_newRoleId} 创建成功", "确定");
    }

    private void RemoveRole()
    {
        if (_selectedRoleIndex < 0 || _selectedRoleIndex >= _roleJsons.Count)
        {
            return;
        }

        string roleId = _roleJsons[_selectedRoleIndex].roleId;

        DeleteDataFiles(roleId);

        _roleJsons.RemoveAt(_selectedRoleIndex);
        _selectedRoleIndex = -1;

        _currentInfo = null;
        _currentAsset = null;
        _currentLock = null;
        _currentProperty = null;
        _currentChainLevel = null;
        _currentPropertyLevel = null;
        _currentChainLevelCost = null;
        _currentPropertyLevelCost = null;
        _currentChainUpgrade = null;
        _currentPropertyUpgrade = null;

        SaveRoleTable();
    }

    private void LoadCurrentRoleData()
    {
        if (_selectedRoleIndex < 0 || _selectedRoleIndex >= _roleJsons.Count)
        {
            return;
        }

        RoleJson role = _roleJsons[_selectedRoleIndex];

        _currentInfo = LoadJsonData<InfoJson>(role.roleInfo);
        _currentAsset = LoadJsonData<AssetJson>(role.roleAsset);
        _currentLock = LoadJsonData<LockJson>(role.roleLock);
        _currentProperty = LoadJsonData<PropertyJson>(role.roleProperty);
        _currentChainLevel = LoadJsonData<ChainLevelJson>(role.roleChainLevel);
        _currentPropertyLevel = LoadJsonData<PropertyLevelJson>(role.rolePropertyLevel);
        _currentChainLevelCost = LoadJsonData<ChainLevelCostJson>(role.roleChainLevelCost);
        _currentPropertyLevelCost = LoadJsonData<PropertyLevelCostJson>(role.rolePropertyLevelCost);
        _currentChainUpgrade = LoadJsonData<ChainUpgradeJson>(role.roleChainUpgrade);
        _currentPropertyUpgrade = LoadJsonData<PropertyUpgradeJson>(role.rolePropertyUpgrade);
    }

    private T LoadJsonData<T>(string relativePath) where T : new()
    {
        string fullPath = GetFullPath(relativePath);

        if (!File.Exists(fullPath))
        {
            return new T();
        }

        string content = File.ReadAllText(fullPath);
        return JsonConvert.DeserializeObject<T>(content) ?? new T();
    }

    private void CreateDefaultDataFiles(string roleId)
    {
        EnsureDirectory("Infos");
        EnsureDirectory("Assets");
        EnsureDirectory("Locks");
        EnsureDirectory("Properties");
        EnsureDirectory("ChainLevels");
        EnsureDirectory("PropertyLevels");
        EnsureDirectory("ChainLevelCosts");
        EnsureDirectory("PropertyLevelCosts");
        EnsureDirectory("ChainUpgrades");
        EnsureDirectory("PropertyUpgrades");

        SaveJsonData(GeneratePath("Infos", roleId, "info"), new InfoJson());
        SaveJsonData(GeneratePath("Assets", roleId, "asset"), new AssetJson());
        SaveJsonData(GeneratePath("Locks", roleId, "lock"), new LockJson());
        SaveJsonData(GeneratePath("Properties", roleId, "property"), new PropertyJson());
        SaveJsonData(GeneratePath("ChainLevels", roleId, "chainLevel"), new ChainLevelJson());
        SaveJsonData(GeneratePath("PropertyLevels", roleId, "propertyLevel"), new PropertyLevelJson());
        SaveJsonData(GeneratePath("ChainLevelCosts", roleId, "chainLevelCost"), new ChainLevelCostJson());
        SaveJsonData(GeneratePath("PropertyLevelCosts", roleId, "propertyLevelCost"), new PropertyLevelCostJson());
        SaveJsonData(GeneratePath("ChainUpgrades", roleId, "chainUpgrade"), new ChainUpgradeJson());
        SaveJsonData(GeneratePath("PropertyUpgrades", roleId, "propertyUpgrade"), new PropertyUpgradeJson());
    }

    private void DeleteDataFiles(string roleId)
    {
        DeleteJsonFile(GeneratePath("Infos", roleId, "info"));
        DeleteJsonFile(GeneratePath("Assets", roleId, "asset"));
        DeleteJsonFile(GeneratePath("Locks", roleId, "lock"));
        DeleteJsonFile(GeneratePath("Properties", roleId, "property"));
        DeleteJsonFile(GeneratePath("ChainLevels", roleId, "chainLevel"));
        DeleteJsonFile(GeneratePath("PropertyLevels", roleId, "propertyLevel"));
        DeleteJsonFile(GeneratePath("ChainLevelCosts", roleId, "chainLevelCost"));
        DeleteJsonFile(GeneratePath("PropertyLevelCosts", roleId, "propertyLevelCost"));
        DeleteJsonFile(GeneratePath("ChainUpgrades", roleId, "chainUpgrade"));
        DeleteJsonFile(GeneratePath("PropertyUpgrades", roleId, "propertyUpgrade"));
    }

    private void DeleteJsonFile(string relativePath)
    {
        string fullPath = GetFullPath(relativePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private void SaveAll()
    {
        SaveRoleTable();

        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count)
        {
            SaveCurrentInfo();
            SaveCurrentAsset();
            SaveCurrentLock();
            SaveCurrentProperty();
            SaveCurrentChainLevel();
            SaveCurrentPropertyLevel();
            SaveCurrentChainLevelCost();
            SaveCurrentPropertyLevelCost();
            SaveCurrentChainUpgrade();
            SaveCurrentPropertyUpgrade();
        }

        EditorUtility.DisplayDialog("成功", "所有数据已保存", "确定");
    }

    private void SaveCurrentInfo()
    {
        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count && _currentInfo != null)
        {
            SaveJsonData(_roleJsons[_selectedRoleIndex].roleInfo, _currentInfo);
        }
    }

    private void SaveCurrentAsset()
    {
        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count && _currentAsset != null)
        {
            SaveJsonData(_roleJsons[_selectedRoleIndex].roleAsset, _currentAsset);
        }
    }

    private void SaveCurrentLock()
    {
        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count && _currentLock != null)
        {
            SaveJsonData(_roleJsons[_selectedRoleIndex].roleLock, _currentLock);
        }
    }

    private void SaveCurrentProperty()
    {
        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count && _currentProperty != null)
        {
            SaveJsonData(_roleJsons[_selectedRoleIndex].roleProperty, _currentProperty);
        }
    }

    private void SaveCurrentChainLevel()
    {
        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count && _currentChainLevel != null)
        {
            SaveJsonData(_roleJsons[_selectedRoleIndex].roleChainLevel, _currentChainLevel);
        }
    }

    private void SaveCurrentPropertyLevel()
    {
        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count && _currentPropertyLevel != null)
        {
            SaveJsonData(_roleJsons[_selectedRoleIndex].rolePropertyLevel, _currentPropertyLevel);
        }
    }

    private void SaveCurrentChainLevelCost()
    {
        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count && _currentChainLevelCost != null)
        {
            SaveJsonData(_roleJsons[_selectedRoleIndex].roleChainLevelCost, _currentChainLevelCost);
        }
    }

    private void SaveCurrentPropertyLevelCost()
    {
        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count && _currentPropertyLevelCost != null)
        {
            SaveJsonData(_roleJsons[_selectedRoleIndex].rolePropertyLevelCost, _currentPropertyLevelCost);
        }
    }

    private void SaveCurrentChainUpgrade()
    {
        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count && _currentChainUpgrade != null)
        {
            SaveJsonData(_roleJsons[_selectedRoleIndex].roleChainUpgrade, _currentChainUpgrade);
        }
    }

    private void SaveCurrentPropertyUpgrade()
    {
        if (_selectedRoleIndex >= 0 && _selectedRoleIndex < _roleJsons.Count && _currentPropertyUpgrade != null)
        {
            SaveJsonData(_roleJsons[_selectedRoleIndex].rolePropertyUpgrade, _currentPropertyUpgrade);
        }
    }

    private void SaveCurrentUpgrade()
    {
        SaveCurrentChainLevelCost();
        SaveCurrentPropertyLevelCost();
        SaveCurrentChainUpgrade();
        SaveCurrentPropertyUpgrade();
    }

    private void SaveJsonData<T>(string relativePath, T data)
    {
        string fullPath = GetFullPath(relativePath);
        string content = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(fullPath, content);
    }

    private string GeneratePath(string folder, string roleId, string type)
    {
        return $"{folder}/{roleId}_{type}.json";
    }

    private string GetRoleTableFilePath()
    {
        return Path.Combine(Application.dataPath, ROLE_TABLE_ROOT, ROLE_TABLE_FILE);
    }

    private string GetFullPath(string relativePath)
    {
        return Path.Combine(Application.dataPath, ROLE_TABLE_ROOT, relativePath);
    }

    private void EnsureDirectory(string folderName)
    {
        string path = Path.Combine(Application.dataPath, ROLE_TABLE_ROOT, folderName);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}