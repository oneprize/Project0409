using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Tilemaps;

// 시네머신 카메라의 위치를 최종적으로 결정하기 직전에 실행되는 확장 스크립트입니다.
public class CinemachineTilemapClamp : CinemachineExtension
{
    public Tilemap tilemap;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        // 1. 카메라의 최종 위치가 결정되는 단계(Body)에서 실행합니다.
        if (stage == CinemachineCore.Stage.Body && tilemap != null)
        {
            // 2. 현재 시네머신 카메라의 렌즈 정보(크기, 비율)를 가져옵니다.
            float camHalfHeight = state.Lens.OrthographicSize;
            float camHalfWidth = (float)Screen.width / Screen.height * camHalfHeight;

            // 3. 타일맵의 경계 범위를 계산합니다.
            Bounds tileBounds = tilemap.localBounds;

            // 4. 기존에 작성하셨던 Mathf.Clamp 로직을 적용합니다.
            Vector3 pos = state.RawPosition;

            float clampedX = Mathf.Clamp(pos.x, tileBounds.min.x + camHalfWidth, tileBounds.max.x - camHalfWidth);
            float clampedY = Mathf.Clamp(pos.y, tileBounds.min.y + camHalfHeight, tileBounds.max.y - camHalfHeight);

            // 5. 시네머신 카메라의 상태값을 강제로 업데이트합니다.
            state.RawPosition = new Vector3(clampedX, clampedY, pos.z);
        }
    }
}