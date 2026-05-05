using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class movement : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private float maxSpeed = 8f;        // ������������ ��������
    [SerializeField] private float force = 15f;          // ���� ������� (F = m * a)
    
    [Header("������")]
    [SerializeField] private float mass = 1f;            // ����� �������

    [Header("Поворот от мыши")]
    [SerializeField] private float mouseSensitivity = 2f;  // Чувствительность мыши
    [SerializeField] private float rotationSpeed = 10f;    // Скорость поворота

    [Header("Адаптация к поверхности")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 2f;

    [Header("Скатывание")]
    [SerializeField] private float slideForce = 12f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 currentVelocity;
    private bool isGrounded;

    private float rotationX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        rb.mass = mass;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(horizontal, 0, vertical).normalized;

        // --- Поворот от мыши ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        rotationX += mouseX;

        // Применяем поворот
        Quaternion targetRotation = Quaternion.Euler(0f, rotationX, 0f);
        Quaternion groundRotation = GetGroundRotation();
        Quaternion finalRotation = groundRotation * targetRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, (rotationSpeed) * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (moveInput != Vector3.zero)
        {
            float slopeAngle = 0f; ;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, rayDistance, groundLayer))
            {
                slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            }
            Debug.Log($"{slopeAngle}");

            Vector3 slideDirection = Vector3.zero;
            if (Physics.Raycast(transform.position, -transform.up, out hit, rayDistance, groundLayer))
            {
                    slideDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
            }

            string tag = hit.collider.gameObject.tag;
            if (slopeAngle > 5f && tag == "Ice")
            {
                float slideStrength = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * slideForce;
                // Debug.Log($"{slideStrength} : {slideDirection.x} {slideDirection.y} {slideDirection.z}");
                rb.AddForce(slideDirection * slideStrength, ForceMode.Force);
            }

            Vector3 moveDirection = transform.TransformDirection(moveInput);
            moveDirection.Normalize();

            Vector3 forceVector = moveDirection * force;
            rb.AddForce(forceVector, ForceMode.Force);

            Vector3 velocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z);
            if (velocity.magnitude > maxSpeed)
            {
                Vector3 limitedVelocity = velocity.normalized * maxSpeed;
                rb.linearVelocity = new Vector3(limitedVelocity.x, limitedVelocity.y, limitedVelocity.z);
            }
        }
    }

    private Quaternion GetGroundRotation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, rayDistance, groundLayer))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            return targetRotation;
        }
        else
        {
            return Quaternion.identity;
        }
    }
}
