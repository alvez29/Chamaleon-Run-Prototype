using UnityEngine;

namespace Game.Core
{
   
    [System.Serializable]
    public class SceneReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        public UnityEditor.SceneAsset SceneAsset;
#endif

        [SerializeField, HideInInspector]
        private string m_sceneName = "";

        public string SceneName => m_sceneName;

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (SceneAsset != null)
            {
                m_sceneName = SceneAsset.name;
            }
            else
            {
                m_sceneName = "";
            }
#endif
        }

        public void OnAfterDeserialize() { }
        
        public static implicit operator string(SceneReference sceneReference)
        {
            return sceneReference.SceneName;
        }
    }
}
