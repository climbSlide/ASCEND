using UnityEngine;
using UnityEngine.UI;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    [ExecuteAlways]
    public class MinimapSystem : MonoBehaviour
    {
        [Header("Minimap Settings")]
        public float MapRange = 150f;
        public float CameraHeight = 800f;
        public Vector2 UIOffset = new Vector2(-20f, -20f);
        public Vector2 UISize = new Vector2(180f, 190f);
        [Range(0f, 1f)] public float MapAlpha = 0.6f;
        [Range(0f, 1f)] public float BorderAlpha = 0.4f;

        [Header("References")]
        public Camera MinimapCamera;
        public RenderTexture MinimapRT;
        public Sprite PlayerArrowSprite;

        private PlayerCharacterController m_Player;
        private RectTransform m_PlayerIconRect;
        private GameObject m_MinimapUI;

        private void OnEnable()
        {
            FindPlayer();
            SetupMinimapResources();
            SetupMinimapUI();
        }

        private void Start()
        {
            FindPlayer();
            SetupMinimapResources();
            SetupMinimapUI();
        }

        private void FindPlayer()
        {
            if (m_Player == null)
            {
                m_Player = FindAnyObjectByType<PlayerCharacterController>();
            }
        }

        private void SetupMinimapResources()
        {
            string folderPath = "Assets/Art";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

#if UNITY_EDITOR
            // 1. Setup Render Texture
            string rtPath = folderPath + "/MinimapRenderTexture.renderTexture";
            MinimapRT = AssetDatabase.LoadAssetAtPath<RenderTexture>(rtPath);
            if (MinimapRT == null)
            {
                MinimapRT = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
                AssetDatabase.CreateAsset(MinimapRT, rtPath);
                AssetDatabase.Refresh();
            }

            // 2. Setup Player Arrow Sprite
            string arrowPath = folderPath + "/PlayerArrow.png";
            PlayerArrowSprite = AssetDatabase.LoadAssetAtPath<Sprite>(arrowPath);
            if (PlayerArrowSprite == null)
            {
                Texture2D tex = new Texture2D(32, 32, TextureFormat.RGBA32, false);
                // Draw a simple white arrow pointing up
                Color transparent = new Color(0, 0, 0, 0);
                Color arrowColor = Color.yellow; // yellow pointer looks great on dark/lava maps

                for (int y = 0; y < 32; y++)
                {
                    for (int x = 0; x < 32; x++)
                    {
                        tex.SetPixel(x, y, transparent);
                    }
                }

                // Draw triangle arrow
                for (int y = 4; y < 28; y++)
                {
                    int width = (y - 4) / 2;
                    for (int x = 16 - width; x <= 16 + width; x++)
                    {
                        tex.SetPixel(x, y, arrowColor);
                    }
                }

                tex.Apply();
                byte[] pngData = tex.EncodeToPNG();
                File.WriteAllBytes(arrowPath, pngData);
                AssetDatabase.Refresh();

                // Configure TextureImporter to Sprite
                TextureImporter importer = AssetImporter.GetAtPath(arrowPath) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.SaveAndReimport();
                }

                PlayerArrowSprite = AssetDatabase.LoadAssetAtPath<Sprite>(arrowPath);
            }
#endif

            // 3. Setup Minimap Camera GameObject
            if (MinimapCamera == null)
            {
                GameObject camGo = GameObject.Find("Minimap_Camera");
                if (camGo == null)
                {
                    camGo = new GameObject("Minimap_Camera");
                    MinimapCamera = camGo.AddComponent<Camera>();
                }
                else
                {
                    MinimapCamera = camGo.GetComponent<Camera>();
                }
            }

            // Unconditionally configure properties and targetTexture
            if (MinimapCamera != null)
            {
                MinimapCamera.orthographic = true;
                MinimapCamera.orthographicSize = MapRange;
                MinimapCamera.clearFlags = CameraClearFlags.SolidColor;
                MinimapCamera.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0f); // Fully transparent background
                MinimapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                MinimapCamera.targetTexture = MinimapRT;
                MinimapCamera.cullingMask = ~0; // Render all layers
            }
        }

        private void SetupMinimapUI()
        {
            GameObject hud = GameObject.Find("HUD");
            if (hud == null) return;

            // Find or Create Container
            Transform containerTransform = hud.transform.Find("MinimapContainer");
            if (containerTransform != null)
            {
                m_MinimapUI = containerTransform.gameObject;
            }
            else
            {
                m_MinimapUI = new GameObject("MinimapContainer");
                m_MinimapUI.transform.SetParent(hud.transform, false);
            }

            RectTransform containerRect = m_MinimapUI.GetComponent<RectTransform>();
            if (containerRect == null) containerRect = m_MinimapUI.AddComponent<RectTransform>();
            
            containerRect.anchorMin = new Vector2(1, 1); // Top-Right
            containerRect.anchorMax = new Vector2(1, 1); // Top-Right
            containerRect.pivot = new Vector2(1, 1); // Top-Right
            containerRect.anchoredPosition = UIOffset;
            containerRect.sizeDelta = UISize;

            // Find or Create Border Background Image
            Transform borderTransform = m_MinimapUI.transform.Find("Border");
            GameObject borderGo;
            if (borderTransform != null)
            {
                borderGo = borderTransform.gameObject;
            }
            else
            {
                borderGo = new GameObject("Border");
                borderGo.transform.SetParent(m_MinimapUI.transform, false);
            }

            RectTransform borderRect = borderGo.GetComponent<RectTransform>();
            if (borderRect == null) borderRect = borderGo.AddComponent<RectTransform>();
            borderRect.anchorMin = Vector2.zero;
            borderRect.anchorMax = Vector2.one;
            borderRect.sizeDelta = Vector2.zero; // Full stretch

            Image borderImg = borderGo.GetComponent<Image>();
            if (borderImg == null) borderImg = borderGo.AddComponent<Image>();
            borderImg.color = new Color(0.15f, 0.15f, 0.15f, BorderAlpha); // Highly transparent frame

            // Find or Create RawImage for Map Display
            Transform mapRawTransform = m_MinimapUI.transform.Find("MapDisplay");
            GameObject mapRawGo;
            if (mapRawTransform != null)
            {
                mapRawGo = mapRawTransform.gameObject;
            }
            else
            {
                mapRawGo = new GameObject("MapDisplay");
                mapRawGo.transform.SetParent(m_MinimapUI.transform, false);
            }

            RectTransform mapRect = mapRawGo.GetComponent<RectTransform>();
            if (mapRect == null) mapRect = mapRawGo.AddComponent<RectTransform>();
            mapRect.anchorMin = Vector2.zero;
            mapRect.anchorMax = Vector2.one;
            mapRect.offsetMin = new Vector2(6, 6); // 6px padding inside border
            mapRect.offsetMax = new Vector2(-6, -6);

            RawImage mapRaw = mapRawGo.GetComponent<RawImage>();
            if (mapRaw == null) mapRaw = mapRawGo.AddComponent<RawImage>();
            mapRaw.texture = MinimapRT;
            mapRaw.color = new Color(1f, 1f, 1f, MapAlpha); // Transparent map display

            // Find or Create Player Icon (Arrow) in center
            Transform playerIconTransform = m_MinimapUI.transform.Find("PlayerIcon");
            GameObject playerIconGo;
            if (playerIconTransform != null)
            {
                playerIconGo = playerIconTransform.gameObject;
            }
            else
            {
                playerIconGo = new GameObject("PlayerIcon");
                playerIconGo.transform.SetParent(m_MinimapUI.transform, false);
            }

            m_PlayerIconRect = playerIconGo.GetComponent<RectTransform>();
            if (m_PlayerIconRect == null) m_PlayerIconRect = playerIconGo.AddComponent<RectTransform>();
            m_PlayerIconRect.anchorMin = new Vector2(0.5f, 0.5f);
            m_PlayerIconRect.anchorMax = new Vector2(0.5f, 0.5f);
            m_PlayerIconRect.pivot = new Vector2(0.5f, 0.5f);
            m_PlayerIconRect.sizeDelta = new Vector2(24, 24);

            Image playerIconImg = playerIconGo.GetComponent<Image>();
            if (playerIconImg == null) playerIconImg = playerIconGo.AddComponent<Image>();
            playerIconImg.sprite = PlayerArrowSprite;
        }

        private void LateUpdate()
        {
            FindPlayer();

            if (m_Player != null)
            {
                // Update Minimap Camera Position
                Vector3 playerPos = m_Player.transform.position;
                if (MinimapCamera != null)
                {
                    MinimapCamera.transform.position = new Vector3(playerPos.x, CameraHeight, playerPos.z);
                    MinimapCamera.orthographicSize = MapRange;
                }

                // Update Player Icon UI Rotation
                if (m_PlayerIconRect != null)
                {
                    float targetAngle = -m_Player.transform.eulerAngles.y;
                    m_PlayerIconRect.localRotation = Quaternion.Euler(0, 0, targetAngle);
                }
            }
        }
    }
}