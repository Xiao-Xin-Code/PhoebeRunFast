using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

public class GoodsDataTool : EditorWindow
{
    private const string GOODS_TABLE_ROOT = "StreamingAssets/GoodsTable";
    private const string GOODS_TABLE_FILE = "GoodsTable.json";

    private List<GoodsJson> _goodsJsons = new List<GoodsJson>();
    private int _selectedGoodsIndex = -1;
    private string _newGoodsId = "";

    private Vector2 _scrollPosition;

    [MenuItem("Tools/Goods Data Tool")]
    public static void ShowWindow()
    {
        GetWindow<GoodsDataTool>("物品数据工具");
    }

    private void OnEnable()
    {
        LoadGoodsTable();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(200));
            {
                DrawGoodsList();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                {
                    DrawGoodsDetail();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawGoodsList()
    {
        GUILayout.Label("物品列表", EditorStyles.boldLabel);
        GUILayout.Space(5);

        for (int i = 0; i < _goodsJsons.Count; i++)
        {
            bool isSelected = _selectedGoodsIndex == i;

            if (GUILayout.Button(_goodsJsons[i].goodsId, isSelected ? EditorStyles.toolbarButton : EditorStyles.label))
            {
                _selectedGoodsIndex = i;
            }
        }

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        {
            _newGoodsId = EditorGUILayout.TextField("新物品ID", _newGoodsId);

            if (GUILayout.Button("添加", GUILayout.Width(60)))
            {
                AddGoods();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (_selectedGoodsIndex >= 0)
        {
            if (GUILayout.Button("删除选中", GUILayout.Width(200)))
            {
                if (EditorUtility.DisplayDialog("确认删除", $"确定要删除物品 {_goodsJsons[_selectedGoodsIndex].goodsId} 吗？", "确定", "取消"))
                {
                    RemoveGoods();
                }
            }
        }

        if (GUILayout.Button("保存所有", GUILayout.Width(200)))
        {
            SaveAll();
        }
    }

    private void DrawGoodsDetail()
    {
        if (_selectedGoodsIndex < 0 || _selectedGoodsIndex >= _goodsJsons.Count)
        {
            GUILayout.Label("请选择一个物品");
            return;
        }

        GoodsJson goods = _goodsJsons[_selectedGoodsIndex];

        GUILayout.Label("物品详情", EditorStyles.boldLabel);
        GUILayout.Space(10);

        goods.goodsId = EditorGUILayout.TextField("物品ID", goods.goodsId);
        goods.goodsName = EditorGUILayout.TextField("物品名称", goods.goodsName);

        goods.goodsType = (GoodsType)EditorGUILayout.EnumPopup("物品类型", goods.goodsType);
        goods.rarity = (Rarity)EditorGUILayout.EnumPopup("稀有度", goods.rarity);

        goods.goodsDesc = EditorGUILayout.TextArea(goods.goodsDesc, GUILayout.Height(100));

        GUILayout.Space(10);

        if (GUILayout.Button("保存", GUILayout.Height(30)))
        {
            SaveGoodsTable();
            EditorUtility.DisplayDialog("成功", "物品数据已保存", "确定");
        }
    }

    private void AddGoods()
    {
        if (string.IsNullOrEmpty(_newGoodsId))
        {
            EditorUtility.DisplayDialog("错误", "物品ID不能为空", "确定");
            return;
        }

        if (_goodsJsons.Exists(g => g.goodsId == _newGoodsId))
        {
            EditorUtility.DisplayDialog("错误", "物品ID已存在", "确定");
            return;
        }

        GoodsJson goods = new GoodsJson
        {
            goodsId = _newGoodsId,
            goodsName = "",
            goodsType = GoodsType.Resource,
            rarity = Rarity.OneStar,
            goodsDesc = ""
        };

        _goodsJsons.Add(goods);
        _selectedGoodsIndex = _goodsJsons.Count - 1;
        _newGoodsId = "";

        SaveGoodsTable();
        EditorUtility.DisplayDialog("成功", $"物品 {_newGoodsId} 创建成功", "确定");
    }

    private void RemoveGoods()
    {
        if (_selectedGoodsIndex < 0 || _selectedGoodsIndex >= _goodsJsons.Count)
        {
            return;
        }

        _goodsJsons.RemoveAt(_selectedGoodsIndex);
        _selectedGoodsIndex = -1;

        SaveGoodsTable();
    }

    private void SaveAll()
    {
        SaveGoodsTable();
        EditorUtility.DisplayDialog("成功", "所有数据已保存", "确定");
    }

    private void LoadGoodsTable()
    {
        string path = GetGoodsTableFilePath();

        if (!File.Exists(path))
        {
            _goodsJsons = new List<GoodsJson>();
            return;
        }

        string content = File.ReadAllText(path);
        _goodsJsons = JsonConvert.DeserializeObject<List<GoodsJson>>(content) ?? new List<GoodsJson>();
    }

    private void SaveGoodsTable()
    {
        string path = GetGoodsTableFilePath();
        string content = JsonConvert.SerializeObject(_goodsJsons, Formatting.Indented);
        File.WriteAllText(path, content);
    }

    private string GetGoodsTableFilePath()
    {
        string folderPath = Path.Combine(Application.dataPath, GOODS_TABLE_ROOT);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        return Path.Combine(folderPath, GOODS_TABLE_FILE);
    }

    public static List<GoodsJson> LoadGoodsData()
    {
        string path = Path.Combine(Application.dataPath, GOODS_TABLE_ROOT, GOODS_TABLE_FILE);

        if (!File.Exists(path))
        {
            return new List<GoodsJson>();
        }

        string content = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<List<GoodsJson>>(content) ?? new List<GoodsJson>();
    }
}