using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform player;         // El cubo
    public float distance = 5f;      // Distancia desde el jugador
    public float height = 2f;        // Altura sobre el jugador
    public Vector2 sensitivity = new Vector2(100f, 70f);

    private PlayerControls controls;
    private Vector2 lookInput;
    private float yaw = 0f;
    private float pitch = 20f;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Actualizar ángulos con el joystick derecho
        yaw += lookInput.x * sensitivity.x * Time.deltaTime;
        pitch -= lookInput.y * sensitivity.y * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, 10f, 80f);

        // Calcular posición de cámara en base a rotación
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);
        Vector3 desiredPosition = player.position + Vector3.up * height + offset;

        // Posicionar y mirar al jugador
        transform.position = desiredPosition;
        transform.LookAt(player.position + Vector3.up * height);
    }
}
