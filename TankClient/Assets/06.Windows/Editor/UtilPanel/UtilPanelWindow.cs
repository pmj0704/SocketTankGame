using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;
using System.IO;

public class UtilPanelWindow : EditorWindow
{
    private int playerCounts = 2;

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

        Label label = rootVisualElement.Q<Label>("show-count");
        label.text = playerCounts.ToString();

        SliderInt cntSlider = rootVisualElement.Q<SliderInt>("player-count");
        cntSlider.RegisterCallback<ChangeEvent<int>>(e => SetPlayerCount(e.newValue, label));

    }

    public void GenerateMap()
    {
        Debug.Log("맵 생성");
        GameObject tilemap = GameObject.Find("Tilemap");

        if (tilemap == null)
        {
            Debug.LogError("There is no tilemap in hierarchy");
            return;
        }

        Tilemap tmCollision = tilemap.transform.Find("Collision").GetComponent<Tilemap>();
        Tilemap tmSafezone = tilemap.transform.Find("Safezone").GetComponent<Tilemap>();

        tmCollision.CompressBounds(); //타일맵의 찌꺼기를 제거한다.
        tmSafezone.CompressBounds();
        using (StreamWriter writer = File.CreateText($"Assets/Resources/Map/{tilemap.name}.txt"))
        {
            BoundsInt mapBound = tmCollision.cellBounds;

            writer.WriteLine(mapBound.xMin);
            writer.WriteLine(mapBound.xMax);
            writer.WriteLine(mapBound.yMin);
            writer.WriteLine(mapBound.yMin);

            //위에서부터 아래로 맵을 스캔한다.
            for (int y = mapBound.yMax - 1; y >= mapBound.yMin; y--)
            {
                for (int x = mapBound.xMin; x < mapBound.xMax; x++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    TileBase tile = tmCollision.GetTile(tilePos);
                    TileBase safe = tmSafezone.GetTile(tilePos);

                    if (tile != null)
                    {
                        writer.Write("1");
                    }
                    else if (safe != null)
                    {
                        writer.Write("2");
                    }
                    else
                    {
                        writer.Write("0");
                    }
                }
                writer.WriteLine("");//한줄 내리기
            }
        }
    }

    public void BuildPlayer()
    {
        Debug.Log("플레이어 빌드");
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

        for (int i = 1; i <= playerCounts; i++)
        {
            BuildPipeline.BuildPlayer(GetScenePaths(), $"Build/Clients{i}/{GetProjectName()}{i}.exe",
                BuildTarget.StandaloneWindows64,
                BuildOptions.AutoRunPlayer);
        }
    }

    public void SetPlayerCount(float value, Label label)
    {
        playerCounts = Mathf.RoundToInt(value);
        label.text = playerCounts.ToString();
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
