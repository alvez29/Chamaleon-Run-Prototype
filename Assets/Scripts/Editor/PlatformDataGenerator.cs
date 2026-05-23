using System.IO;
using Game.Level;
using Game.Level.Data;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class PlatformDataGenerator
    {
        [MenuItem("Tools/Generate Platform Data")]
        public static void GenerateData()
        {
            string bluePath = "Assets/Art/KayKit/Platformers/Platforms/Blue/Models";
            string yellowPath = "Assets/Art/KayKit/Platformers/Platforms/Yellow/Models";
            string blackPath = "Assets/Art/KayKit/Platformers/Platforms/Black/Models";
            string outputFolder = "Assets/Data/Platforms";

            EnsureFolderExists(outputFolder);

            ProcessFolder(bluePath, PlatformColor.Blue, outputFolder);
            ProcessFolder(yellowPath, PlatformColor.Yellow, outputFolder);
            ProcessFolder(blackPath, PlatformColor.Black, outputFolder);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[PlatformDataGenerator] Platform Data generation completed successfully!");
        }

        private static void ProcessFolder(string folderPath, PlatformColor color, string outputFolder)
        {
            if (!AssetDatabase.IsValidFolder(folderPath)) return;

            string[] guids = AssetDatabase.FindAssets("t:Model", new[] { folderPath });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = Path.GetFileNameWithoutExtension(assetPath).ToLower();

                if (fileName.StartsWith("barrier") || fileName.StartsWith("platform"))
                {
                    GameObject modelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                    if (modelPrefab == null) continue;

                    bool isSlope = fileName.Contains("_slope_");

                    PlatformData newData = ScriptableObject.CreateInstance<PlatformData>();
                    SerializedObject serializedObject = new SerializedObject(newData);
                    
                    serializedObject.FindProperty("Color").enumValueIndex = (int)color;
                    serializedObject.FindProperty("PlatformPrefab").objectReferenceValue = modelPrefab;
                    serializedObject.FindProperty("UseMeshCollider").boolValue = isSlope;
                    
                    serializedObject.ApplyModifiedProperties();

                    string pascalName = ToPascalCase(fileName);
                    string finalPath = $"{outputFolder}/{pascalName}Data.asset";

                    AssetDatabase.CreateAsset(newData, finalPath);
                }
            }
        }

        private static string ToPascalCase(string str)
        {
            string[] parts = str.Split('_');
            string result = "";
            foreach (string p in parts)
            {
                if (p.Length > 0)
                {
                    result += char.ToUpper(p[0]) + p.Substring(1).ToLower();
                }
            }
            return result;
        }

        private static void EnsureFolderExists(string folderPath)
        {
            string[] folders = folderPath.Split('/');
            string currentPath = folders[0];

            for (int i = 1; i < folders.Length; i++)
            {
                if (!AssetDatabase.IsValidFolder($"{currentPath}/{folders[i]}"))
                {
                    AssetDatabase.CreateFolder(currentPath, folders[i]);
                }
                currentPath += $"/{folders[i]}";
            }
        }
    }
}
