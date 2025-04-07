using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;        // Velocidad de movimiento
    public float jumpForce = 5f;    // Fuerza de salto
    public Transform cameraTransform; // La cámara, que se usará para obtener su rotación

    private Rigidbody rb;
    private Vector2 movementInput;
    private bool isGrounded;        // Para verificar si está tocando el suelo
    private PlayerControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();  // Inicializar el Rigidbody
        controls = new PlayerControls(); // Inicializar los controles del jugador
    }

    private void OnEnable()
    {
        // Suscribirse a los controles de movimiento
        controls.Gameplay.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => movementInput = Vector2.zero;
        controls.Gameplay.Jump.performed += ctx => Jump(); // Suscribirse al botón de salto
        controls.Gameplay.Enable(); // Habilitar los controles
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable(); // Deshabilitar los controles
    }

    private void FixedUpdate()
    {
        if (cameraTransform == null) return; // Si no hay cámara asignada, no hacer nada

        // Obtener las direcciones 'forward' y 'right' de la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Asegurarse de que la dirección de la cámara esté en el plano horizontal
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Calcular la dirección de movimiento ajustada a la cámara
        Vector3 moveDirection = forward * movementInput.y + right * movementInput.x;

        // Mover al jugador (el cubo) usando Rigidbody
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    // Función para saltar
    private void Jump()
    {
        // Verificar si el jugador está en el suelo antes de permitir saltar
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Ya no está en el suelo mientras salta
        }
    }

    // Detectar si el jugador está tocando el suelo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Si tocamos el suelo
        {
            isGrounded = true;
        }
    }
}








