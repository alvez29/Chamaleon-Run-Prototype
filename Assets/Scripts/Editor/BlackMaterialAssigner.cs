using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class BlackMaterialAssigner : AssetPostprocessor
    {
        // Se ejecuta automáticamente cada vez que Unity importa un modelo 3D
        public Material OnAssignMaterialModel(Material material, Renderer renderer)
        {
            // Solo afectamos a los modelos dentro de la carpeta Black
            if (assetPath.Contains("KayKit/Platformers/Platforms/Black"))
            {
                Material blackMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Art/CustomMaterials/M_Black.mat");
                
                if (blackMat != null)
                {
                    return blackMat; // Asignamos M_Black forzosamente
                }
            }
            
            return null; // Si no es de esta carpeta, dejamos que Unity asigne el material por defecto
        }
    }
}
