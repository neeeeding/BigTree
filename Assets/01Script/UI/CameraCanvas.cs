using UnityEngine;
using UnityEngine.UI;

namespace _01Script.UI
{
    public class CameraCanvas : MonoBehaviour
    {
        [Header("Target Aspect Ratio")] [SerializeField]
        private float targetAspect = 16f / 9f; // 1920 x 1080

        [Header("References")] [SerializeField]
        private Camera targetCamera;

        [SerializeField] private Canvas targetCanvas;

        private float currentAspect;
        private float scaleHeight;
        private float scaleWidth;

        private void Start()
        {
            // 카메라가 지정되지 않았다면 메인 카메라 사용
            if (targetCamera == null)
                targetCamera = Camera.main;

            // 캔버스가 지정되지 않았다면 찾기
            if (targetCanvas == null)
                targetCanvas = FindObjectOfType<Canvas>();

            ApplyAspectRatio();
        }

        private void Update()
        {
            // 화면 크기가 변경되었는지 확인 (에디터에서 게임 뷰 크기 변경 등)
            float newAspect = (float)Screen.width / Screen.height;
            if (Mathf.Abs(currentAspect - newAspect) > 0.01f)
            {
                ApplyAspectRatio();
            }
        }

        private void ApplyAspectRatio()
        {
            currentAspect = (float)Screen.width / Screen.height;

            // 카메라 비율 조정
            AdjustCamera();

            // UI 비율 조정
            AdjustCanvas();
        }

        private void AdjustCamera()
        {
            scaleHeight = currentAspect / targetAspect;
            scaleWidth = 1f / scaleHeight;

            Rect rect = targetCamera.rect;

            if (scaleHeight < 1f)
            {
                // 세로가 더 긴 경우 (위아래 검은 영역)
                rect.width = 1f;
                rect.height = scaleHeight;
                rect.x = 0f;
                rect.y = (1f - scaleHeight) / 2f;
            }
            else
            {
                // 가로가 더 긴 경우 (좌우 검은 영역)
                rect.width = scaleWidth;
                rect.height = 1f;
                rect.x = (1f - scaleWidth) / 2f;
                rect.y = 0f;
            }

            targetCamera.rect = rect;
        }

        private void AdjustCanvas()
        {
            if (targetCanvas == null) return;

            CanvasScaler canvasScaler = targetCanvas.GetComponent<CanvasScaler>();
            if (canvasScaler == null)
            {
                canvasScaler = targetCanvas.gameObject.AddComponent<CanvasScaler>();
            }

            // Canvas Scaler 설정
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            // 현재 화면 비율에 따라 match 값 조정
            if (scaleHeight < 1f)
            {
                // 세로가 더 긴 경우 - 너비 기준으로 스케일링
                canvasScaler.matchWidthOrHeight = 0f;
            }
            else
            {
                // 가로가 더 긴 경우 - 높이 기준으로 스케일링
                canvasScaler.matchWidthOrHeight = 1f;
            }

            // Canvas의 RectTransform 조정 (필요한 경우)
            RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();
            if (canvasRect != null)
            {
                // Canvas가 카메라 영역에 맞게 조정되도록 설정
                Vector2 sizeDelta = canvasRect.sizeDelta;

                if (scaleHeight < 1f)
                {
                    // 세로 기준 조정
                    sizeDelta.y = 1080f;
                    sizeDelta.x = sizeDelta.y * currentAspect;
                }
                else
                {
                    // 가로 기준 조정
                    sizeDelta.x = 1920f;
                    sizeDelta.y = sizeDelta.x / currentAspect;
                }

                // 필요시 sizeDelta 적용 (Screen Space - Overlay 모드에서는 보통 자동 처리)
                // canvasRect.sizeDelta = sizeDelta;
            }
        }

        void OnPreCull() => GL.Clear(true, true, Color.black);

        // 에디터에서 값 변경 시 즉시 적용
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                ApplyAspectRatio();
            }
        }
    }
}