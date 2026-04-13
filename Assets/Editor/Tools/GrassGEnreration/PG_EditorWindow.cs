using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class PG_EditorWindow : EditorWindow
{
    public VisualTreeAsset mainContentAsset;
    public VisualTreeAsset regionContentAsset;
    public VisualTreeAsset cornerItemAsset;
    private Button createRegionButton;
    private ListView regionList;

    public PG_SaveData saveData;
    private string saveDataGUID = "";

    [MenuItem("Tools/PlantGenerator")]
    public static void ShowWindow()
    {
        var Window = GetWindow<PG_EditorWindow>();
        Window.titleContent = new GUIContent("Plant Generator");
        Window.minSize = new Vector2(300, 400);
    }



    private void OnEnable()
    {
        saveDataGUID = EditorPrefs.GetString("PG_EditorWindow_SaveDataGUID", "");
        if (!string.IsNullOrEmpty(saveDataGUID))
        {
            string path = AssetDatabase.GUIDToAssetPath(saveDataGUID);
            saveData = AssetDatabase.LoadAssetAtPath<PG_SaveData>(path);
            if (saveData == null) // asset zosta³ usuniêty
            {
                EditorPrefs.DeleteKey("PG_EditorWindow_SaveDataGUID");
            }
        }
    }

    private void OnDisable()
    {
        // Zapisz GUID obecnego saveData
        if (saveData != null)
        {
            string path = AssetDatabase.GetAssetPath(saveData);
            string guid = AssetDatabase.AssetPathToGUID(path);
            EditorPrefs.SetString("PG_EditorWindow_SaveDataGUID", guid);
        }
        else
        {
            EditorPrefs.DeleteKey("PG_EditorWindow_SaveDataGUID");
        }
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        mainContentAsset?.CloneTree(root);

        var saveDataField = new ObjectField("PG Save Data");
        saveDataField.objectType = typeof(PG_SaveData);
        saveDataField.value = saveData;
        saveDataField.RegisterValueChangedCallback(evt => {
            saveData = evt.newValue as PG_SaveData;
            if (regionList != null)
            {
                regionList.itemsSource = saveData?._regions;
                regionList.Rebuild();
            }
            // Zapisz GUID od razu
            if (saveData != null)
            {
                string path = AssetDatabase.GetAssetPath(saveData);
                string guid = AssetDatabase.AssetPathToGUID(path);
                EditorPrefs.SetString("PG_EditorWindow_SaveDataGUID", guid);
            }
            else
            {
                EditorPrefs.DeleteKey("PG_EditorWindow_SaveDataGUID");
            }
        });
        root.Insert(0, saveDataField);

        createRegionButton = root.Q<Button>("CreateRegion");
        createRegionButton.clicked += CreateButtonClicked;

        regionList = root.Q<ListView>("RegionList");
        if (regionList != null && saveData != null)
        {
            regionList.itemsSource = saveData._regions;
            regionList.itemTemplate = regionContentAsset;
            regionList.bindItem = BindItem;
            regionList.selectionType = SelectionType.None;
            regionList.Rebuild(); 
        }
        else
        {
            Debug.LogError("regionList or saveData is null");
        }
    }

    private void BindItem(VisualElement element, int rindex)
    {
        element.Q<TextField>("RegionName").RegisterValueChangedCallback((evt) => { RegionNameChanged(evt, rindex); });
        element.Q<TextField>("RegionName").value = saveData._regions[rindex].regionName;
        element.Q<ObjectField>("Mesh").RegisterValueChangedCallback((evt) => { MeshChanged(evt, rindex); });
        element.Q<ObjectField>("Mesh").value = saveData._regions[rindex].mesh;
        element.Q<ObjectField>("Material").RegisterValueChangedCallback((evt) => { MaterialChanged(evt, rindex); });
        element.Q<ObjectField>("Material").value = saveData._regions[rindex].material;
        element.Q<FloatField>("MeshScale").RegisterValueChangedCallback((evt) => { MeshScaleChanged(evt, rindex); });
        element.Q<FloatField>("MeshScale").value = saveData._regions[rindex].meshScale;
        element.Q<ColorField>("LineColor").RegisterValueChangedCallback((evt) => { ColorChanged(evt, rindex); });
        element.Q<ColorField>("LineColor").value = saveData._regions[rindex].polygonColor;
        element.Q<FloatField>("Spacing").RegisterValueChangedCallback((evt) => { SpacingChanged(evt, rindex); });
        element.Q<FloatField>("Spacing").value = saveData._regions[rindex].spacing;
        element.Q<FloatField>("JitterAmount").RegisterValueChangedCallback((evt) => { JitterChanged(evt, rindex); });
        element.Q<FloatField>("JitterAmount").value = saveData._regions[rindex].jitterAmount;
        element.Q<LayerMaskField>("Layermask").RegisterValueChangedCallback((evt) => { MaskChanged(evt, rindex); });
        element.Q<LayerMaskField>("Layermask").value = saveData._regions[rindex].groundLayer;

        var cList = element.Q<ListView>("CornerList");
        cList.itemsSource = saveData._regions[rindex].corners;
        cList.itemTemplate = cornerItemAsset;
        cList.bindItem = (element, index) => BindCorners(element, index, rindex);
        cList.selectionType = SelectionType.None;

        element.Q<Button>("AddCorner").clicked += () => AddCornerButtonClicked(rindex, cList);
        element.Q<Button>("ChangeCorners").clicked += () => ChangeCornersButtonClicked(rindex);
        element.Q<Button>("Generate").clicked += () => CallGenerateOnPlantGenerator(rindex);
    }

    private void ChangeCornersButtonClicked(int index)
    {
        if (saveData == null)
        {
            Debug.LogError("No PG_SaveData assigned.");
            return;
        }
        var generator = GetPlantGenerator();
        if (generator != null)
        {
            generator.saveData = saveData;
            generator.Generate(index);
        }
        else
        {
            Debug.LogWarning("Nie znaleziono komponentu PlantGeneration w scenie.");
        }
    }

    private void AddCornerButtonClicked(int rindex, ListView clist)
    {
        saveData._regions[rindex].corners.Add(new Vector3(0, 0, 0));
        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
        clist.Rebuild();
        RefreshGenerator(rindex);
    }

    private void BindCorners(VisualElement element, int cindex, int rindex)
    {
        var xField = element.Q<FloatField>("XValueField");
        var yField = element.Q<FloatField>("YValueField");
        var zField = element.Q<FloatField>("ZValueField");

        xField.RegisterValueChangedCallback((evt) => { XValueChanged(evt, cindex, rindex); });
        yField.RegisterValueChangedCallback((evt) => { YValueChanged(evt, cindex, rindex); });
        zField.RegisterValueChangedCallback((evt) => { ZValueChanged(evt, cindex, rindex); });

        xField.value = saveData._regions[rindex].corners[cindex].x;
        yField.value = saveData._regions[rindex].corners[cindex].y;
        zField.value = saveData._regions[rindex].corners[cindex].z;
    }

    private void ZValueChanged(ChangeEvent<float> evt, int cindex, int rindex)
    {
        var currentCorner = saveData._regions[rindex].corners[cindex];
        currentCorner = new Vector3(currentCorner.x, currentCorner.y, evt.newValue);
        saveData._regions[rindex].corners[cindex] = currentCorner;
        SaveAndRefresh(rindex);
    }

    private void YValueChanged(ChangeEvent<float> evt, int cindex, int rindex)
    {
        var currentCorner = saveData._regions[rindex].corners[cindex];
        currentCorner = new Vector3(currentCorner.x, evt.newValue, currentCorner.z);
        saveData._regions[rindex].corners[cindex] = currentCorner;
        SaveAndRefresh(rindex);
    }

    private void XValueChanged(ChangeEvent<float> evt, int cindex, int rindex)
    {
        var currentCorner = saveData._regions[rindex].corners[cindex];
        currentCorner = new Vector3(evt.newValue, currentCorner.y, currentCorner.z);
        saveData._regions[rindex].corners[cindex] = currentCorner;
        SaveAndRefresh(rindex);
    }

    private void SaveAndRefresh(int rindex)
    {
        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
        RefreshGenerator(rindex);
    }

    private void RefreshGenerator(int index)
    {
        var generator = GetPlantGenerator();
        if (generator != null && generator.saveData == saveData)
        {
            generator.Generate(index);
        }
    }

    private void MaskChanged(ChangeEvent<int> evt, int index)
    {
        saveData._regions[index].groundLayer = evt.newValue;
        SaveAndRefresh(index);
    }

    private void JitterChanged(ChangeEvent<float> evt, int index)
    {
        saveData._regions[index].jitterAmount = evt.newValue;
        SaveAndRefresh(index);
    }

    private void SpacingChanged(ChangeEvent<float> evt, int index)
    {
        saveData._regions[index].spacing = evt.newValue;
        SaveAndRefresh(index);
    }

    private void ColorChanged(ChangeEvent<Color> evt, int index)
    {
        saveData._regions[index].polygonColor = evt.newValue;
        SaveAndRefresh(index);
    }

    private void MeshScaleChanged(ChangeEvent<float> evt, int index)
    {
        saveData._regions[index].meshScale = evt.newValue;
        SaveAndRefresh(index);
    }

    private void MaterialChanged(ChangeEvent<UnityEngine.Object> evt, int index)
    {
        saveData._regions[index].material = (Material)evt.newValue;
        SaveAndRefresh(index);
    }

    private void MeshChanged(ChangeEvent<UnityEngine.Object> evt, int index)
    {
        saveData._regions[index].mesh = (Mesh)evt.newValue;
        SaveAndRefresh(index);
    }

    private void RegionNameChanged(ChangeEvent<string> evt, int index)
    {
        saveData._regions[index].regionName = evt.newValue;
        SaveAndRefresh(index);
    }

    private void CreateButtonClicked()
    {
        if (saveData == null)
        {
            Debug.LogError("No PG_SaveData assigned. Please assign one in the ObjectField at the top.");
            return;
        }
        var newRegion = new PG_RegionItem();
        Undo.RecordObject(saveData, "Created Region");
        saveData._regions.Add(newRegion);
        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
        regionList.Rebuild();
    }

    private void CallGenerateOnPlantGenerator(int index)
    {
        if (saveData == null)
        {
            Debug.LogError("No PG_SaveData assigned.");
            return;
        }
        var generator = GetPlantGenerator();
        if (generator != null)
        {
            generator.saveData = saveData;
            generator.Generate(index);
        }
        else
        {
            Debug.LogWarning("Nie znaleziono komponentu PlantGeneration w scenie.");
        }
    }

    private PlantGeneration GetPlantGenerator()
    {
        return FindFirstObjectByType<PlantGeneration>();
    }
}