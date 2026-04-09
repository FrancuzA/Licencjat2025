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
        cList.bindItem =(element, index) => BindCorners(element, index, rindex);
        cList.selectionType = SelectionType.None;

        element.Q<Button>("AddCorner").clicked +=() => AddCornerButtonClicked(rindex, cList);
        element.Q<Button>("ChangeCorners").clicked += ChangeCornersButtonClicked;

    }

    private void ChangeCornersButtonClicked()
    {
        Debug.Log("changing Corners");
    }

    private void AddCornerButtonClicked(int rindex, ListView clist)
    {
        saveData._regions[rindex].corners.Add(new Vector3(0,0,0));
        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);

        clist.Rebuild();
    }

    private void BindCorners(VisualElement element, int cindex, int rindex)
    {
        element.Q<FloatField> ("XValueField").RegisterValueChangedCallback((evt) => { XValueChanged(evt, cindex, rindex); });
        element.Q<FloatField>("XValueField").value = saveData._regions[rindex].corners[cindex].x;
        element.Q<FloatField> ("YValueField").RegisterValueChangedCallback((evt) => { YValueChanged(evt, cindex, rindex); });
        element.Q<FloatField>("YValueField").value = saveData._regions[rindex].corners[cindex].y;
        element.Q<FloatField> ("ZValueField").RegisterValueChangedCallback((evt) => { ZValueChanged(evt, cindex, rindex); });
        element.Q<FloatField>("ZValueField").value = saveData._regions[rindex].corners[cindex].z;
    }

    private void ZValueChanged(ChangeEvent<float> evt, int cindex, int rindex)
    {
       var currentCorner = saveData._regions[rindex].corners[cindex];
       currentCorner =new Vector3(currentCorner.x, currentCorner.y, evt.newValue);

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
    }

    private void YValueChanged(ChangeEvent<float> evt, int cindex, int rindex)
    {
        var currentCorner = saveData._regions[rindex].corners[cindex];
        currentCorner = new Vector3(currentCorner.x, evt.newValue, currentCorner.z);

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
    }

    private void XValueChanged(ChangeEvent<float> evt, int cindex, int rindex)
    {
        var currentCorner = saveData._regions[rindex].corners[cindex];
        currentCorner = new Vector3(evt.newValue, currentCorner.y, currentCorner.z);

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
    }

    private void MaskChanged(ChangeEvent<int> evt, int index)
    {
        saveData._regions[index].groundLayer = evt.newValue;

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
    }

    private void JitterChanged(ChangeEvent<float> evt, int index)
    {
        saveData._regions[index].jitterAmount = evt.newValue;

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);

    }

    private void SpacingChanged(ChangeEvent<float> evt, int index)
    {
        saveData._regions[index].spacing = evt.newValue;

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
    }

    private void ColorChanged(ChangeEvent<Color> evt, int index)
    {
        saveData._regions[index].polygonColor = evt.newValue;

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
    }

    private void MeshScaleChanged(ChangeEvent<float> evt, int index)
    {
        saveData._regions[index].meshScale = evt.newValue;

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
    }

    private void MaterialChanged(ChangeEvent<UnityEngine.Object> evt, int index)
    {
        saveData._regions[index].material = (Material)evt.newValue;

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
    }

    private void MeshChanged(ChangeEvent<UnityEngine.Object> evt, int index)
    {
        saveData._regions[index].mesh = (Mesh)evt.newValue;

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
    }

    private void RegionNameChanged(ChangeEvent<string> evt, int index)
    {
        saveData._regions[index].regionName = evt.newValue;

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);
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
