using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjetoMovible : MonoBehaviour
{
    private Rigidbody rb;

    public bool siendoTomado;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Tomar()
    {
        siendoTomado = true;

        rb.useGravity = false;

        rb.velocity = Vector3.zero;
    }

    public void Soltar()
    {
        siendoTomado = false;

        rb.useGravity = true;
    }
}