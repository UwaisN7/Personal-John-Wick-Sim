using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float height = 1.5f;
    [SerializeField] private float offsetLerpSpeed = 12f;

    private float currentSideOffset;
    private float targetSideOffset;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 sideDirection = GetCameraRightDirection();

        currentSideOffset = Mathf.Lerp(
            currentSideOffset,
            targetSideOffset,
            Time.deltaTime * offsetLerpSpeed
        );

        transform.position =
            player.position +
            Vector3.up * height +
            sideDirection * currentSideOffset;

        // Important:
        // Do NOT copy player rotation.
        transform.rotation = Quaternion.identity;
    }

    Vector3 GetCameraRightDirection()
    {
        if (Camera.main == null)
        {
            return player.right;
        }

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;

        if (cameraRight.sqrMagnitude < 0.001f)
        {
            return player.right;
        }

        return cameraRight.normalized;
    }

    public void SetSideOffset(float sideOffset)
    {
        targetSideOffset = sideOffset;
    }
}