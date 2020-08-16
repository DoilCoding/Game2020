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

        if (GUILayout.Button("Build Project"))
        {
            var scenes = Scenes.Where(x => x.Value).Select(x => $@"Assets\Resources\Scenes\{x.Key}.unity").ToArray();
            BuildPipeline.BuildPlayer(
                scenes,
                @"F:\Users\Administrator\Dayz\Builds\Dayz.exe",
                BuildTarget.StandaloneWindows,
                BuildOptions.None);
        }

        if (GUILayout.Button("Run Client"))
            System.Diagnostics.Process.Start("explorer.exe", @"/start, F:\Users\Administrator\Dayz\Builds\Dayz.exe");
    }
}