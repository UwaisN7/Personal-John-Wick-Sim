using UnityEngine;

public class WeaponSideSwitcher : MonoBehaviour
{
    [SerializeField] private Vector3 rightSidePosition = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 leftSidePosition = new Vector3(-0.45f, 1.2f, 0.6f);
    [SerializeField] private float switchSpeed = 15f;

    private Vector3 targetPosition;

    void Awake()
    {
        targetPosition = rightSidePosition;
        transform.localPosition = targetPosition;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition,
            Time.deltaTime * switchSpeed
        );
    }

    public void SetRightSide(bool isRightSide)
    {
        targetPosition = isRightSide ? rightSidePosition : leftSidePosition;
    }
}