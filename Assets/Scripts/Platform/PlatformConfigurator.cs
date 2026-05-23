using Game.Level.Data;
using Game.Utils;
using UnityEngine;

namespace Game.Level
{
    public class PlatformConfigurator : MonoBehaviour
    {
        [SerializeField] private PlatformData m_platformData;
        
        private void Awake()
        {
            Configure();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (m_platformData == null || Application.isPlaying)
            {
                return;
            }
            
            if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this))
            {
                return;
            }
            
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this == null)
                {
                    return;
                }
                
                Configure();
            };
        }
#endif

        [ContextMenu("Configure Platform")]
        public void Configure()
        {
            if (m_platformData == null || m_platformData.PlatformPrefab == null)
            {
                return;
            }

            ClearVisuals();

            GameObject visual = Instantiate(m_platformData.PlatformPrefab, transform);
            
            visual.name = Constants.PLATFORM_VISUAL_GAME_OBJECT_NAME;
            visual.transform.localPosition = Vector3.zero;
            visual.transform.localRotation = Quaternion.identity;

            SetTagByColor(gameObject, m_platformData.Color);
            SetupCollider(visual, m_platformData.UseMeshCollider);
        }


        private void ClearVisuals()
        {
            // Siempre iterar de atrás hacia adelante al destruir hijos, 
            // de lo contrario los índices cambian y te saltas elementos, provocando duplicados.
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                
                if (child.name == Constants.PLATFORM_VISUAL_GAME_OBJECT_NAME)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(child.gameObject);
                    }
                    else
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }

        private static void SetTagByColor(GameObject subject, PlatformColor color)
        {
            switch (color)
            {
                case PlatformColor.Blue:
                    subject.tag = Constants.BLUE_PLATFORM_TAG;
                    break;
                case PlatformColor.Yellow:
                    subject.tag = Constants.YELLOW_PLATFORM_TAG;
                    break;
                case PlatformColor.Black:
                default:
                    subject.tag = "Untagged";
                    break;
            }
        }
        
        private void SetupCollider(GameObject visual, bool useMeshCollider)
        {
            MeshFilter filter = visual.GetComponentInChildren<MeshFilter>();
            
            if (filter == null || filter.sharedMesh == null)
            {
                return;
            }
            
            if (useMeshCollider)
            {
                if (TryGetComponent(out BoxCollider box))
                {
                    if (Application.isPlaying) Destroy(box);
                    else DestroyImmediate(box);
                }
                
                MeshCollider meshCol = gameObject.GetComponent<MeshCollider>();
                
                if (meshCol == null)
                {
                    meshCol = gameObject.AddComponent<MeshCollider>();
                }
                
                meshCol.sharedMesh = filter.sharedMesh;
            }
            else
            {
                if (TryGetComponent(out MeshCollider mesh))
                {
                    if (Application.isPlaying) Destroy(mesh);
                    else DestroyImmediate(mesh);
                }
                
                BoxCollider boxCol = gameObject.GetComponent<BoxCollider>();
                
                if (boxCol == null)
                {
                    boxCol = gameObject.AddComponent<BoxCollider>();
                }
                
                boxCol.center = filter.sharedMesh.bounds.center;
                boxCol.size = filter.sharedMesh.bounds.size;
            }
        }
    }
}