using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class FindMissingScriptGUIDs : EditorWindow
{
    [MenuItem("Tools/Find Missing Script Names")]
    public static void FindMissingScriptNames()
    {
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
        Dictionary<string, string> missingScripts = new Dictionary<string, string>();

        foreach (string assetPath in allAssetPaths)
        {
            if (assetPath.EndsWith(".prefab") || assetPath.EndsWith(".unity") || assetPath.EndsWith(".asset"))
            {
                string assetData = File.ReadAllText(assetPath);
                if (assetData.Contains("m_Script: {fileID: 11500000, guid:"))
                {
                    foreach (string line in assetData.Split('\n'))
                    {
                        if (line.Contains("m_Script: {fileID: 11500000, guid:"))
                        {
                            string guid = line.Substring(line.IndexOf("guid: ") + 6, 32);
                            if (!missingScripts.ContainsKey(guid))
                            {
                                missingScripts[guid] = assetPath;
                            }
                        }
                    }
                }
            }
        }

        if (missingScripts.Count > 0)
        {
            Debug.LogWarning("Found missing scripts! Searching for names...");
            foreach (var entry in missingScripts)
            {
                Debug.Log($"Missing Script GUID: {entry.Key} found in {entry.Value}");
            }
        }
        else
        {
            Debug.Log("No missing scripts found.");
        }
    }
}
