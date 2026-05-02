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
    [SerializeField] private LayerMask groundLayer;  // Слой земли
    [SerializeField] private float rayDistance = 0.8f;      // Длина луча вниз

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

        // ������ ���� �������������
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;
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
            Vector3 moveDirection = transform.TransformDirection(moveInput);
            moveDirection.y = 0;  // Убираем вертикальную составляющую
            moveDirection.Normalize();  // Нормализуем для сохранения скорости

            // a = F / m
            float acceleration = force / mass;

            // V = V + a * dt (����������� ��������)
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                moveDirection * maxSpeed,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            float deceleration = force / mass;

            // V = V - a * dt (��������� �������� �� ����)
            currentVelocity = Vector3.MoveTowards(
                currentVelocity,
                Vector3.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        // ��������� ��������
        Vector3 newVelocity = currentVelocity;
        newVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = newVelocity;
    }

    private Quaternion GetGroundRotation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, groundLayer))
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
