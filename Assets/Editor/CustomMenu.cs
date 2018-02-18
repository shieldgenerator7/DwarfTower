using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CustomMenu
{
    [MenuItem("SG7/Build/Build Windows %w")]
    public static void buildWindows()
    {
        build(BuildTarget.StandaloneWindows, "exe");
    }
    [MenuItem("SG7/Build/Build Linux %l")]
    public static void buildLinux()
    {
        build(BuildTarget.StandaloneLinux, "x86");
    }
    [MenuItem("SG7/Build/Build Mac OS X %#l")]
    public static void buildMacOSX()
    {
        build(BuildTarget.StandaloneOSXUniversal, "");
    }
    public static void build(BuildTarget buildTarget, string extension)
    {
        string defaultPath = "C:/Users/steph/Documents/Unity/DwarfTower/Builds/" + PlayerSettings.productName;
        if (!System.IO.Directory.Exists(defaultPath))
        {
            System.IO.Directory.CreateDirectory(defaultPath);
        }
        //2017-10-19 copied from https://docs.unity3d.com/Manual/BuildPlayerPipeline.html
        // Get filename.
        string buildName = EditorUtility.SaveFilePanel("Choose Location of Built Game", defaultPath, PlayerSettings.productName, extension);
        string path = buildName.Substring(0, buildName.LastIndexOf("/"));
        UnityEngine.Debug.Log("BUILDNAME: " + buildName);
        UnityEngine.Debug.Log("PATH: " + path);
        string[] levels = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            if (EditorBuildSettings.scenes[i].enabled)
            {
                levels[i] = EditorBuildSettings.scenes[i].path;
            }
            else
            {
                break;
            }
        }

        // Build player.
        BuildPipeline.BuildPlayer(levels, buildName, BuildTarget.StandaloneWindows, BuildOptions.None);

        //// Copy a file from the project folder to the build folder, alongside the built game.
        ////NOTE: Changes to the Dialogue folder won't reflected unless you delete the Dialogue folder in the build directory
        //if (!System.IO.Directory.Exists(path + "/Assets/Resources/Dialogue"))
        //{
        //    System.IO.Directory.CreateDirectory(path + "/Assets/Resources");
        //    FileUtil.CopyFileOrDirectory("Assets/Resources/Dialogue/",
        //        path + "/Assets/Resources/Dialogue/"
        //        );
        //}

        // Run the game (Process class from System.Diagnostics).
        Process proc = new Process();
        proc.StartInfo.FileName = buildName;
        proc.Start();
    }
}
