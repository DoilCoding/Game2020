using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectManager : EditorWindow
{
    [MenuItem("Window/Project Manager")]
    public static void ShowWindow() => GetWindow(typeof(ProjectManager));

    private static Dictionary<string, bool> Scenes { get; } = new Dictionary<string, bool>();

    void OnGUI()
    {
        if (Scenes.Count == 0)
        {
            for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                Scenes.Add(sceneName, i < 2);
            }
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Open Project Folder"))
            System.Diagnostics.Process.Start("explorer.exe", @"/open, F:\Users\Administrator\Dayz\Assets");

        GUILayout.Label("Select scenes you wish to add to the build");
        foreach (var scenesValue in Scenes.Keys.ToList())
        {
            Scenes[scenesValue] = EditorGUILayout.Toggle(scenesValue, Scenes[scenesValue]);
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Client"))
        {
            var scenes = Scenes.Where(x => x.Value).Select(x => $@"Assets\Resources\Scenes\{x.Key}.unity").ToArray();
            BuildPipeline.BuildPlayer(
                scenes,
                @"F:\Users\Administrator\Dayz\Builds\Client\Dayz.exe",
                BuildTarget.StandaloneWindows64,
                BuildOptions.None);
        }
        if (GUILayout.Button("Build Server"))
        {
            var scenes = Scenes.Where(x => x.Value).Select(x => $@"Assets\Resources\Scenes\{x.Key}.unity").ToArray();
            BuildPipeline.BuildPlayer(
                scenes,
                @"F:\Users\Administrator\Dayz\Builds\Server\Dayz.exe",
                BuildTarget.StandaloneWindows64,
                BuildOptions.EnableHeadlessMode);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Run Client"))
            System.Diagnostics.Process.Start(@"F:\Users\Administrator\Dayz\Builds\Client\Dayz.exe");

        GUI.backgroundColor = Color.magenta;
        if (GUILayout.Button("Run Server"))
            System.Diagnostics.Process.Start(@"F:\Users\Administrator\Dayz\Builds\Dayz.exe", " -batchmode -nographics");
        GUILayout.EndHorizontal();
    }
}