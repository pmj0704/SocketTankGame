using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class UtilPanelWindow : EditorWindow
{
    [MenuItem("Tools/UtilPanel")]
    public static void ShowWindow()
    {
        UtilPanelWindow win = GetWindow<UtilPanelWindow>();
        win.titleContent = new GUIContent("Util Panel");
        win.minSize = new Vector2(400, 200);
    }

    private void OnEnable()
    {
        VisualTreeAsset xml = 
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/06.Windows/Editor/UtilPanel/UtilPanelWindow.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        //StyleSheet uss =
        //    AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/06.Windows/Editor/UtilPanel/UtilPanelWindow.uss");
        //rootVisualElement.styleSheets.Add(uss);

        Button mapBtn = rootVisualElement.Q<Button>("generate-map");
        mapBtn.RegisterCallback<MouseUpEvent>(e => GenerateMap());

        Button buildBtn = rootVisualElement.Q<Button>("build-player");
        buildBtn.RegisterCallback<MouseUpEvent>(e => BuildPlayer());
    }

    public void GenerateMap()
    {
        Debug.Log("맵 생성");
    }

    public void BuildPlayer()
    {
        Debug.Log("플레이어 빌드");
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

        for (int i = 1; i <= 2; i++)
        {
            BuildPipeline.BuildPlayer(GetScenePaths(), $"Build/Clients{i}/{GetProjectName()}{i}.exe",
                BuildTarget.StandaloneWindows64,
                BuildOptions.AutoRunPlayer);
        }
    }

    private string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }

    private string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path; //이렇게 하면 씬의 경로가 불러와짐
        }
        return scenes;
    }
}
