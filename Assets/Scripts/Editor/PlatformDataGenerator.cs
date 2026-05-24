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
                PlatformData newData = ScriptableObject.CreateInstance<PlatformData>();
                SerializedObject serializedObject = new SerializedObject(newData);
                
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = Path.GetFileNameWithoutExtension(assetPath).ToLower();
                Vector3Int dimensions = GetDimensionsFromFileName(fileName);

                GameObject modelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (modelPrefab == null) continue;

                bool shouldProcess = fileName.StartsWith("barrier") || fileName.StartsWith("platform");
                
                if (shouldProcess)
                {
                    bool isSlope = fileName.Contains("slope");
                    bool isBarrier = fileName.Contains("barrier");
                    bool isSquaredPlatform = System.Text.RegularExpressions.Regex
                        .Match(fileName, @"^platform_\d+").Success;
                    PlatformData.PlatformType platformType =
                        GetPlatformType(serializedObject, isBarrier, isSlope, isSquaredPlatform);
                    
                    serializedObject.FindProperty("Color").enumValueIndex = (int)color;
                    serializedObject.FindProperty("PlatformPrefab").objectReferenceValue = modelPrefab;
                    serializedObject.FindProperty("UseMeshCollider").boolValue = isSlope;
                    serializedObject.FindProperty("Dimensions").vector3IntValue = dimensions;
                    serializedObject.FindProperty("Type").enumValueIndex = (int)platformType;
                    
                    serializedObject.ApplyModifiedProperties();

                    string pascalName = ToPascalCase(fileName);
                    string finalPath = $"{outputFolder}/{pascalName}Data.asset";

                    AssetDatabase.CreateAsset(newData, finalPath);
                }
            }

            PlatformData.PlatformType GetPlatformType(SerializedObject serializedObject, bool isBarrier, bool isSlope, bool isSquaredPlatform)
            {
                if (isBarrier)
                {
                    return PlatformData.PlatformType.Barrier;
                }

                if (isSlope)
                {
                    return PlatformData.PlatformType.Slope;
                }

                if (isSquaredPlatform)
                {
                    return PlatformData.PlatformType.Squared;    
                }
                
                return PlatformData.PlatformType.NonRegistered;
                
            }
        }

        private static Vector3Int GetDimensionsFromFileName(string filePath)
        {
            string[] parts = filePath.Split('_');

            foreach (string part in parts)
            {
                if (part.Contains("x"))
                {
                    string numbers = System.Text.RegularExpressions.Regex
                        .Match(part, @"\d+x\d+x\d+")
                        .Value;

                    string[] values = numbers.Split('x');

                    int x = int.Parse(values[0]);
                    int y = int.Parse(values[2]);
                    int z = int.Parse(values[1]);

                    return new Vector3Int(x, y, z);
                }
            }

            return Vector3Int.zero;
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
