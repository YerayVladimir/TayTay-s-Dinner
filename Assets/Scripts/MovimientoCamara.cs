using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Camara")]
    public float sensibilidad = 0.1f;
    public float limiteVertical = 80f;
    public Camera camaraFPS;

    private CharacterController controller;
    private float rotacionVertical = 0f;

    private Vector2 inputMovimiento;
    private Vector2 inputMouse;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Llamado automaticamente por el Player Input cuando se mueven WASD
    public void OnMove(InputValue value)
    {
        inputMovimiento = value.Get<Vector2>();
    }

    // Llamado automaticamente por el Player Input cuando se mueve el mouse
    public void OnLook(InputValue value)
    {
        inputMouse = value.Get<Vector2>();
    }

    void Update()
    {
        MoverJugador();
        RotarCamara();
    }

    void MoverJugador()
    {
        Vector3 direccion = transform.right * inputMovimiento.x
                          + transform.forward * inputMovimiento.y;
        controller.SimpleMove(direccion * velocidad);
    }

    void RotarCamara()
    {
        float mouseX = inputMouse.x * sensibilidad;
        float mouseY = inputMouse.y * sensibilidad;

        transform.Rotate(Vector3.up * mouseX);

        rotacionVertical -= mouseY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -limiteVertical, limiteVertical);
        camaraFPS.transform.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
    }
}