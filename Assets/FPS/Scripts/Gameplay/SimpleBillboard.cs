using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class SimpleBillboard : MonoBehaviour
    {
        private Camera m_MainCamera;

        void Start()
        {
            m_MainCamera = Camera.main;
        }

        void LateUpdate()
        {
            if (m_MainCamera == null)
            {
                m_MainCamera = Camera.main;
                if (m_MainCamera == null) return;
            }

            // Face the camera plane
            transform.rotation = m_MainCamera.transform.rotation;
}
    }
}
