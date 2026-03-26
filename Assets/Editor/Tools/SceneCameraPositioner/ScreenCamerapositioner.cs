using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class ScreenCamerapositioner : EditorWindow
{
    public VisualTreeAsset mainContentAsset;
    public VisualTreeAsset presetItemAsset;
    private Label posLabel;
    private Label rotLabel;
    private TextField presetnameField;
    private Button saveButton;
    private Button deleteButton;
    private ListView presetList;
    private Vector3 gizmoPos;

    public SCP_SaveData saveData;

    [MenuItem("Window/ScreenCameraPositioner")]
    public static void ShowWindow()
    {
        var Window = GetWindow<ScreenCamerapositioner>();
        Window.titleContent = new GUIContent("Scene Camera Positioner");
        Window.minSize = new Vector2(300, 400);
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        mainContentAsset?.CloneTree(root);


        posLabel = root.Q<Label>("PosLabel");
        rotLabel = root.Q<Label>("RotLabel");
        presetnameField = root.Q<TextField>("presetNameField");
        presetnameField.RegisterValueChangedCallback(PresetNameFieldChanged);
        saveButton = root.Q<Button>("SaveButton");
        saveButton.clicked += SaveButtonClicked;

        presetList = root.Q<ListView>("PresetList");
        presetList.itemsSource = saveData._presets;
        presetList.itemTemplate = presetItemAsset;
        presetList.bindItem = BindItem;
        presetList.selectionType = SelectionType.None;

    }



    private void PresetNameFieldChanged(ChangeEvent<string> evt)
    {
        saveButton.SetEnabled(evt.newValue.Length > 0);

    }


    private void SaveButtonClicked()
    {
        var pos = SceneView.lastActiveSceneView.camera.transform.position;
        var rot = SceneView.lastActiveSceneView.camera.transform.rotation;
        string presetname = presetnameField.value;

        var newPreset = new SCP_PresetItem()
        {
            presetName = presetname,
            _position = pos,
            _rotation = rot,
        };

        Undo.RecordObject(saveData, $"Created {presetname} preset");

        saveData._presets.Add(newPreset);

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);


        presetnameField.value = "";
        presetList.Rebuild();
        gizmoPos = pos;
        
    }

    
    
    private void BindItem(VisualElement element, int index)
    {
        element.Q<Label>("PresetName").text = saveData._presets[index].presetName;
        element.Q<Label>("Position").text = saveData._presets[index]._position.ToString();
        element.Q<Button>("Select").clicked +=() => SelectPresetButtonCLicked(index);
        element.Q<Button>("Delete").clicked +=() => DeletePresetButtonClicked(index);
    }

    private void SelectPresetButtonCLicked(int presetIndex)
    {
        var preset = saveData._presets[presetIndex];
        SceneView.lastActiveSceneView.pivot = preset._position;
        SceneView.lastActiveSceneView.rotation = preset._rotation;
        SceneView.lastActiveSceneView.Repaint();
    }

    private void DeletePresetButtonClicked(int index)
    {
        var currentPreset = saveData._presets[index];
        var presetname = currentPreset.presetName;
        Undo.RecordObject(saveData, $"Deleted {presetname} preset");


        saveData._presets.Remove(currentPreset);

        EditorUtility.SetDirty(saveData);
        AssetDatabase.SaveAssetIfDirty(saveData);

        presetList.Rebuild();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += SceneViewRefresh;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= SceneViewRefresh;
        saveButton.clicked -= SaveButtonClicked;
    }

    private void SceneViewRefresh(SceneView obj)
    {
        posLabel.text = $"Pos: {obj.camera.transform.position}";
        rotLabel.text = $"Rot: {obj.camera.transform.rotation.eulerAngles}";

    }
}
