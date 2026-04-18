using UnityEngine;
using UnityEditor;
using System.IO;

public class RoleDataEditor : EditorWindow
{
    private int roleId = 1;
    private string roleName = "角色名称";
    
    // 基础属性
    private float health = 100f;
    private float energy = 50f;
    private float attack = 20f;
    private float defense = 10f;
    private float speed = 5f;
    private float cooldownReduction = 0f;
    private float skillCooldown = 5f;
    private float ultimateCooldown = 10f;
    
    // 等级升级属性
    private int healthPerLevel = 10;
    private int energyPerLevel = 5;
    private int defensePerLevel = 2;
    private int cooldownReductionPerLevel = 1;
    
    // 星级升级属性
    private float healthPerStar = 50f;
    private float energyPerStar = 25f;
    private float attackPerStar = 10f;
    private float defensePerStar = 5f;
    private float speedPerStar = 1f;
    private float cooldownReductionPerStar = 0.05f;
    private float skillCooldownPerStar = -0.5f;
    private float ultimateCooldownPerStar = -1f;
    
    // 文件保存设置
    private string fileName = "RoleData";
    private string savePath = "Assets/Resources";
    
    [MenuItem("Tools/角色数据编辑器")]
    public static void ShowWindow()
    {
        GetWindow<RoleDataEditor>("角色数据编辑器");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("角色基本信息", EditorStyles.boldLabel);
        roleId = EditorGUILayout.IntField("角色ID", roleId);
        roleName = EditorGUILayout.TextField("角色名称", roleName);
        
        GUILayout.Space(10);
        GUILayout.Label("基础属性", EditorStyles.boldLabel);
        health = EditorGUILayout.FloatField("生命值", health);
        energy = EditorGUILayout.FloatField("能量值", energy);
        attack = EditorGUILayout.FloatField("攻击值", attack);
        defense = EditorGUILayout.FloatField("防御值", defense);
        speed = EditorGUILayout.FloatField("速度", speed);
        cooldownReduction = EditorGUILayout.FloatField("冷却缩减", cooldownReduction);
        skillCooldown = EditorGUILayout.FloatField("技能冷却时间", skillCooldown);
        ultimateCooldown = EditorGUILayout.FloatField("最终技能冷却时间", ultimateCooldown);
        
        GUILayout.Space(10);
        GUILayout.Label("等级升级属性", EditorStyles.boldLabel);
        healthPerLevel = EditorGUILayout.IntField("每级生命值增加", healthPerLevel);
        energyPerLevel = EditorGUILayout.IntField("每级能量值增加", energyPerLevel);
        defensePerLevel = EditorGUILayout.IntField("每级防御值增加", defensePerLevel);
        cooldownReductionPerLevel = EditorGUILayout.IntField("每级冷却缩减增加", cooldownReductionPerLevel);
        
        GUILayout.Space(10);
        GUILayout.Label("星级升级属性", EditorStyles.boldLabel);
        healthPerStar = EditorGUILayout.FloatField("每星生命值增加", healthPerStar);
        energyPerStar = EditorGUILayout.FloatField("每星能量值增加", energyPerStar);
        attackPerStar = EditorGUILayout.FloatField("每星攻击值增加", attackPerStar);
        defensePerStar = EditorGUILayout.FloatField("每星防御值增加", defensePerStar);
        speedPerStar = EditorGUILayout.FloatField("每星速度增加", speedPerStar);
        cooldownReductionPerStar = EditorGUILayout.FloatField("每星冷却缩减增加", cooldownReductionPerStar);
        skillCooldownPerStar = EditorGUILayout.FloatField("每星技能冷却时间减少", skillCooldownPerStar);
        ultimateCooldownPerStar = EditorGUILayout.FloatField("每星最终技能冷却时间减少", ultimateCooldownPerStar);
        
        GUILayout.Space(10);
        GUILayout.Label("文件保存设置", EditorStyles.boldLabel);
        fileName = EditorGUILayout.TextField("文件名", fileName);
        savePath = EditorGUILayout.TextField("保存路径", savePath);
        
        if (GUILayout.Button("浏览保存路径"))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("选择保存路径", savePath, "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                // 转换为相对路径
                if (selectedPath.StartsWith(Application.dataPath))
                {
                    savePath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                }
                else
                {
                    savePath = selectedPath;
                }
            }
        }
        
        GUILayout.Space(20);
        if (GUILayout.Button("生成JSON文件"))
        {
            GenerateJsonFile();
        }
    }
    
    private void GenerateJsonFile()
    {
        // 创建角色数据
        RoleData roleData = new RoleData();
        roleData.identity = new RoleIdentity { roleId = roleId };
        roleData.level = new RoleLevel {
            propertyLevel = new RolePropertyLevel {
                healthLevel = 0,
                energyLevel = 0,
                defenseLevel = 0,
                cooldownReductionLevel = 0
            },
            starLevel = 1
        };
        roleData.baseProperty = new RoleProperty {
            health = health,
            energy = energy,
            attack = attack,
            defense = defense,
            speed = speed,
            cooldownReduction = cooldownReduction,
            skillCooldown = skillCooldown,
            ultimateCooldown = ultimateCooldown
        };
        roleData.rolePropertyUpgrades = new RolePropertyLevelUpgrade[] {
            new RolePropertyLevelUpgrade {
                healthPerLevel = healthPerLevel,
                energyPerLevel = energyPerLevel,
                defensePerLevel = defensePerLevel,
                cooldownReductionPerLevel = cooldownReductionPerLevel
            }
        };
        roleData.roleStarUpgrades = new RoleStarUpgrade[] {
            new RoleStarUpgrade {
                healthPerStar = healthPerStar,
                energyPerStar = energyPerStar,
                attackPerStar = attackPerStar,
                defensePerStar = defensePerStar,
                speedPerStar = speedPerStar,
                cooldownReductionPerStar = cooldownReductionPerStar,
                skillCooldownPerStar = skillCooldownPerStar,
                ultimateCooldownPerStar = ultimateCooldownPerStar
            }
        };
        
        // 确保保存目录存在
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        
        // 生成JSON字符串
        string json = JsonUtility.ToJson(roleData, true);
        
        // 保存文件
        string filePath = Path.Combine(savePath, fileName + ".json");
        File.WriteAllText(filePath, json);
        
        // 刷新AssetDatabase
        AssetDatabase.Refresh();
        
        EditorUtility.DisplayDialog("成功", "角色数据JSON文件生成成功！", "确定");
    }
}