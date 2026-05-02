using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -2f);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetEuler = target.eulerAngles;
        Quaternion horizontalRotation = Quaternion.Euler(0f, targetEuler.y, 0f);

        Vector3 rotatedOffset = horizontalRotation * offset;

        transform.position = target.position + rotatedOffset;

        transform.rotation = horizontalRotation;
    }
}
