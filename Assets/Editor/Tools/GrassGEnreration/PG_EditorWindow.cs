using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PG_EditorWindow : EditorWindow
{
    public VisualTreeAsset mainContentAsset;
    public VisualTreeAsset regionContentAsset;
    public VisualTreeAsset cornerItemAsset;
    private Button createRegionButton;
    private ListView regionList;
    private TextField regionNameField;
    private ObjectField meshobject;
    private ObjectField materialObject;
    private FloatField meshScaleField;
    private ColorField colorField;
    private FloatField spacingField;
    private FloatField jitterField;
    private LayerMaskField layerField;
    private ListView cornersList;
    private Button addCornerButton;
    private Button changeCornersButton;
    private FloatField xValueField;
    private FloatField yValueField;
    private FloatField zValueField;

    public PG_SaveData saveData;

    [MenuItem("Tools/PlantGenerator")]
    public static void ShowWindow()
    {
        var Window = GetWindow<PG_EditorWindow>();
        Window.titleContent = new GUIContent("Plant Generator");
        Window.minSize = new Vector2(300, 400);
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        mainContentAsset?.CloneTree(root);

        createRegionButton = root.Q<Button>("CreateRegion");
        createRegionButton.clicked += CreateButtonClicked;

        regionList = root.Q<ListView>("RegionList");
        regionList.itemsSource = saveData._regions;
        regionList.itemTemplate = regionContentAsset;
        regionList.bindItem = BindItem;
        regionList.selectionType = SelectionType.None;
    }

    private void BindItem(VisualElement element, int index)
    {
        element.Q<TextField>("RegionName").RegisterValueChangedCallback((evt) => { RegionNameChanged(evt, index); });
        element.Q<TextField>("RegionName").value = saveData._regions[index].regionName;
        element.Q<ObjectField>("Mesh").RegisterValueChangedCallback((evt) => { MeshChanged(evt, index); });
        element.Q<ObjectField>("Mesh").value = saveData._regions[index].mesh;
        element.Q<ObjectField>("Material").RegisterValueChangedCallback((evt) => { MaterialChanged(evt, index); });
        element.Q<ObjectField>("Material").value = saveData._regions[index].material;
        //element.Q<FloatField>("MeshScale")
    }

    private void MaterialChanged(ChangeEvent<UnityEngine.Object> evt, int index)
    {
        saveData._regions[index].material = (Material)evt.newValue;
    }

    private void MeshChanged(ChangeEvent<UnityEngine.Object> evt, int index)
    {
        saveData._regions[index].mesh = (Mesh)evt.newValue;
    }

    private void RegionNameChanged(ChangeEvent<string> evt, int index)
    {
        saveData._regions[index].regionName = evt.newValue;
    }

    private void CreateButtonClicked()
    {
        var newRegion = new PG_RegionItem()
        {
           // corners = new List<>
        };

        Undo.RecordObject(saveData, $"Created Region");

        saveData._regions.Add(newRegion);

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);

        regionList.Rebuild();
    }
}
