using System.Collections.Generic;
using System.Linq;
using Game.Level.Data;
using Game.Utils;
using UnityEditor;
using UnityEngine;

namespace Game.Level
{
    public class BoxPlatformConfigurator : MonoBehaviour
    {
        [Header("Platform Settings")]
        [SerializeField] private PlatformColor m_color = PlatformColor.Blue;
        
        [SerializeField] private bool m_useMeshCollider;

        [SerializeField] [Range(0, 1000)] private int m_length = 6;
        [SerializeField] [Range(0, 1000)] private int m_height = 2;
        [SerializeField] [Range(0, 1000)] private int m_depth = 2;
        [SerializeField] private PlatformData.PlatformType m_platformTypeUsed;
        
        [HideInInspector]
        [SerializeField] private List<PlatformData> m_cachedBlocks = new();
        
        private Vector3Int m_size => new(m_length, m_height, m_depth);

        private bool m_isConfigureScheduled;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (PrefabUtility.IsPartOfPrefabAsset(this)) return;
            
            if (m_isConfigureScheduled) return;
            m_isConfigureScheduled = true;
            
            EditorApplication.delayCall += () =>
            {
                m_isConfigureScheduled = false;
                if (this == null || gameObject == null) return;
                if (EditorApplication.isPlayingOrWillChangePlaymode) return;
                
                UpdateCachedBlocks();
                Configure();
            };
        }

        private void UpdateCachedBlocks()
        {
            m_cachedBlocks.Clear();
            string[] guids = AssetDatabase.FindAssets("t:PlatformData");
            PlatformData[] platformsData = guids
                .Select(guid => AssetDatabase.LoadAssetAtPath<PlatformData>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(data => data != null && data.Color == m_color && data.Dimensions != Vector3Int.zero &&
                               !data.UseMeshCollider && data.Type == m_platformTypeUsed)
                .ToArray();
            
            m_cachedBlocks.AddRange(platformsData);
            m_cachedBlocks.Sort((a, b) => 
            {
                int volA = a.Dimensions.x * a.Dimensions.y * a.Dimensions.z;
                int volB = b.Dimensions.x * b.Dimensions.y * b.Dimensions.z;
                return volB.CompareTo(volA);
            });
        }
#endif

        [ContextMenu("Configure Platform")]
        public void Configure()
        {
            if (m_size.x <= 0 || m_size.y <= 0 || m_size.z <= 0) return;
            if (m_cachedBlocks == null || m_cachedBlocks.Count == 0) return;

            ClearVisuals();
            GenerateBlocks();
            SetupSingleCollider();
            
            SetTagByColor(gameObject, m_color);
        }

        private void GenerateBlocks()
        {
            int sizeX = m_size.x;
            int sizeY = m_size.y;
            int sizeZ = m_size.z;
            
            bool[,,] grid = new bool[sizeX, sizeY, sizeZ];
            
            Vector3 startCorner = new Vector3(0f, -sizeY / 2f, -sizeZ / 2f);

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    for (int z = 0; z < sizeZ; z++)
                    {
                        if (grid[x, y, z]) continue;

                        PlatformData bestFit = FindBestFit(grid, x, y, z, sizeX, sizeY, sizeZ);
                        
                        if (bestFit != null)
                        {
                            PlaceBlock(bestFit, grid, x, y, z, startCorner);
                        }
                    }
                }
            }
        }

        private PlatformData FindBestFit(bool[,,] grid, int startX, int startY, int startZ, int sizeX, int sizeY, int sizeZ)
        {
            foreach (PlatformData block in m_cachedBlocks)
            {
                Vector3Int dim = block.Dimensions;
                
                if (startX + dim.x > sizeX || startY + dim.y > sizeY || startZ + dim.z > sizeZ)
                    continue;

                bool canFit = true;
                for (int i = 0; i < dim.x && canFit; i++)
                {
                    for (int j = 0; j < dim.y && canFit; j++)
                    {
                        for (int k = 0; k < dim.z && canFit; k++)
                        {
                            if (grid[startX + i, startY + j, startZ + k])
                            {
                                canFit = false;
                            }
                        }
                    }
                }

                if (canFit) return block;
            }
            return null;
        }

        private void PlaceBlock(PlatformData data, bool[,,] grid, int x, int y, int z, Vector3 startCorner)
        {
            Vector3Int dim = data.Dimensions;
            
            for (int i = 0; i < dim.x; i++)
            for (int j = 0; j < dim.y; j++)
            for (int k = 0; k < dim.z; k++)
                grid[x + i, y + j, z + k] = true;

            GameObject visual = Instantiate(data.PlatformPrefab, transform);
            visual.name = Constants.PLATFORM_VISUAL_GAME_OBJECT_NAME;
            
            MeshFilter filter = visual.GetComponentInChildren<MeshFilter>();
            
            if (filter != null && filter.sharedMesh != null)
            {
                Bounds bounds = filter.sharedMesh.bounds;
                Vector3 targetMin = startCorner + new Vector3(x, y, z);
                
                visual.transform.localPosition = targetMin - bounds.center + bounds.extents;
            }
            else
            {
                visual.transform.localPosition = startCorner + new Vector3(x + dim.x / 2f, y + dim.y / 2f, z + dim.z / 2f);
            }
            
            visual.transform.localRotation = Quaternion.identity;
        }

        private void SetupSingleCollider()
        {
            if (m_useMeshCollider)
            {
                Debug.LogWarning("[BoxPlatformConfigurator] MeshCollider is not compatible with this configuration");
            }

            if (TryGetComponent(out MeshCollider meshCol))
            {
                if (Application.isPlaying) Destroy(meshCol);
                else DestroyImmediate(meshCol);
            }

            BoxCollider boxCol = gameObject.GetComponent<BoxCollider>();
            if (boxCol == null)
            {
                boxCol = gameObject.AddComponent<BoxCollider>();
            }

            boxCol.center = new Vector3(m_size.x / 2f, 0f, 0f);
            boxCol.size = m_size;
        }

        private void ClearVisuals()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (child.name == Constants.PLATFORM_VISUAL_GAME_OBJECT_NAME)
                {
                    if (Application.isPlaying) Destroy(child.gameObject);
                    else DestroyImmediate(child.gameObject);
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
    }
}