using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

public class GoodsDataEditor : EditorWindow
{
    private string outputPath = "";
    private List<GoodsJson> goodsList = new List<GoodsJson>();
    private Vector2 scrollPosition;
    private int selectedGoodsIndex = -1;

    [MenuItem("Tools/Goods Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<GoodsDataEditor>("Goods Data Editor");
    }

    private void OnGUI()
    {
        try
        {
            GUILayout.Label("Goods Data Editor", EditorStyles.boldLabel);
            GUILayout.Space(10);

            // 选择输出路径
            GUILayout.BeginHorizontal();
            GUILayout.Label("Output Path:", GUILayout.Width(80));
            string newPath = EditorGUILayout.TextField(outputPath);
            if (newPath != outputPath)
            {
                outputPath = newPath;
                // 当路径改变时，尝试加载数据
                LoadDataFromPath();
            }
            if (GUILayout.Button("Browse", GUILayout.Width(70)))
            {
                string path = EditorUtility.OpenFolderPanel("Select Output Folder", outputPath, "");
                if (!string.IsNullOrEmpty(path))
                {
                    outputPath = path;
                    // 当选择新路径时，尝试加载数据
                    LoadDataFromPath();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            // 物品列表
            GUILayout.Label("Goods List:", EditorStyles.boldLabel);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

            for (int i = 0; i < goodsList.Count; i++)
            {
                GUILayout.BeginHorizontal();
                bool isSelected = i == selectedGoodsIndex;
                if (GUILayout.Button(goodsList[i].goodsName, isSelected ? EditorStyles.toolbarButton : EditorStyles.label))
                {
                    selectedGoodsIndex = i;
                }
                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    goodsList.RemoveAt(i);
                    if (selectedGoodsIndex >= i)
                        selectedGoodsIndex--;
                    break;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

            GUILayout.Space(10);

            // 添加物品
            if (GUILayout.Button("Add Goods"))
            {
                GoodsJson newGoods = new GoodsJson
                {
                    goodsId = "goods_" + (goodsList.Count + 1),
                    goodsType = Goods.Resource,
                    goodsName = "New Goods",
                    goodsDesc = "Goods description"
                };
                goodsList.Add(newGoods);
                selectedGoodsIndex = goodsList.Count - 1;
            }

            GUILayout.Space(20);

            // 编辑选中物品
            if (selectedGoodsIndex >= 0 && selectedGoodsIndex < goodsList.Count)
            {
                GoodsJson goods = goodsList[selectedGoodsIndex];
                GUILayout.Label("Goods Details:", EditorStyles.boldLabel);

                goods.goodsId = EditorGUILayout.TextField("Goods ID", goods.goodsId);
                goods.goodsType = (Goods)EditorGUILayout.EnumPopup("Goods Type", goods.goodsType);
                goods.goodsName = EditorGUILayout.TextField("Goods Name", goods.goodsName);
                goods.goodsDesc = EditorGUILayout.TextField("Goods Description", goods.goodsDesc);

                goodsList[selectedGoodsIndex] = goods;
            }

            GUILayout.Space(20);

            // 生成文件
            if (GUILayout.Button("Generate GoodsTable.json"))
            {
                GenerateGoodsTable();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error in GoodsDataEditor: " + e.Message);
            GUILayout.Label("Error: " + e.Message);
        }
    }

    private void LoadDataFromPath()
    {
        if (string.IsNullOrEmpty(outputPath))
        {
            return;
        }

        string tableFilePath = Path.Combine(outputPath, "GoodsTable.json");
        if (File.Exists(tableFilePath))
        {
            // 读取 GoodsTable.json
            string tableJson = File.ReadAllText(tableFilePath);
            GoodsJson[] goodsJsons = JsonConvert.DeserializeObject<GoodsJson[]>(tableJson);

            // 清空现有数据并加载新数据
            goodsList.Clear();
            goodsList.AddRange(goodsJsons);

            // 选择第一个物品
            if (goodsList.Count > 0)
            {
                selectedGoodsIndex = 0;
            }
            else
            {
                selectedGoodsIndex = -1;
            }
        }
        else
        {
            // 如果文件不存在，清空数据
            goodsList.Clear();
            selectedGoodsIndex = -1;
        }
    }

    private void GenerateGoodsTable()
    {
        if (string.IsNullOrEmpty(outputPath))
        {
            EditorUtility.DisplayDialog("Error", "Please select an output folder first.", "OK");
            return;
        }

        // 确保输出目录存在
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        // 生成 GoodsTable.json
        string tableFilePath = Path.Combine(outputPath, "GoodsTable.json");
        string tableJson = JsonConvert.SerializeObject(goodsList.ToArray(), Formatting.Indented);
        File.WriteAllText(tableFilePath, tableJson);

        EditorUtility.DisplayDialog("Success", "GoodsTable.json generated successfully!\n\nOutput Path: " + tableFilePath, "OK");
    }
}