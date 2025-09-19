using UnityEngine;
using UnityEngine.UI;

public class UnitSellUI : MonoBehaviour
{
    [Header("원본 (Canvas 1)")]
    public Image sourceImage; // 1번 캔버스에 있는 원본 이미지

    [Header("타겟 (Canvas 2)")]
    public RectTransform targetCanvasRectTransform; // 2번 캔버스의 RectTransform
    public RectTransform objectToMove;              // 2번 캔버스에서 위치를 이동시킬 UI 객체

    void Update()
    {
        // sourceImage와 targetCanvasRectTransform이 지정되었는지 확인
        if (sourceImage != null && targetCanvasRectTransform != null)
        {
            ConvertCoordinates();
        }
    }

    public void ConvertCoordinates()
    {
        // --- 1. 원본 이미지의 위치 계산 ---
        RectTransform sourceRect = sourceImage.rectTransform;

        // 이미지의 로컬 좌표계에서 (자신의 위치 + 세로 길이의 절반) 지점을 계산합니다.
        // Pivot이 중앙(0.5, 0.5)에 있다고 가정하고, Y축으로 높이의 절반만큼 올라간 위치입니다.
        // TransformPoint를 사용하면 이미지의 회전, 스케일을 모두 포함하여 정확한 월드 좌표를 얻을 수 있습니다.
        Vector3 sourceWorldPosition = sourceRect.TransformPoint(new Vector3(0, sourceRect.rect.height / 2f, 0));


        // --- 2. 월드 좌표를 타겟 캔버스의 좌표로 변환 ---

        // 변환에 사용할 카메라를 결정합니다.
        // 타겟 캔버스가 Screen Space - Overlay 모드이면 카메라는 null입니다.
        // 그 외의 경우(Screen Space - Camera, World Space)에는 캔버스에 설정된 카메라를 사용합니다.
        Canvas targetCanvas = targetCanvasRectTransform.GetComponent<Canvas>();
        Camera canvasCamera = (targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : targetCanvas.worldCamera;

        // 월드 좌표(sourceWorldPosition)를 스크린 좌표로 변환합니다.
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvasCamera, sourceWorldPosition);

        // 스크린 좌표를 타겟 캔버스의 로컬 좌표(anchoredPosition)로 변환합니다.
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetCanvasRectTransform, screenPoint, canvasCamera, out localPoint);


        // --- 3. 변환된 좌표를 타겟 객체에 적용 ---
        if (objectToMove != null)
        {
            objectToMove.anchoredPosition = localPoint;
        }

        // 변환된 좌표를 로그로 출력해볼 수 있습니다.
        // Debug.Log($"변환된 좌표: {localPoint}");
    }
}
