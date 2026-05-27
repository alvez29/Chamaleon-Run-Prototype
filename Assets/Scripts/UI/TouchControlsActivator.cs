using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// Activa el Canvas de controles táctiles en móviles y lo desactiva en PC 
    /// para evitar que los botones invisibles bloqueen los clics del ratón.
    /// </summary>
    public class TouchControlsActivator : MonoBehaviour
    {
        [Tooltip("Activa esto si quieres probar los controles táctiles en el Editor de PC haciendo clic con el ratón")]
        [SerializeField] private bool m_forceEnableInEditor = false;

        private void Awake()
        {
#if UNITY_EDITOR
            if (m_forceEnableInEditor)
            {
                gameObject.SetActive(true);
                return;
            }
#endif

            // SystemInfo.deviceType detecta si el hardware real es un dispositivo de mano (Móvil/Tablet)
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
