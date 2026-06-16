using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform grabPointTransform;

    [Header("Movimiento")]
    public float lerpSpeed = 10f;

    [Header("Estado")]
    public bool bloqueado = false;

    [Header("Layer al bloquear")]
    public string layerBloqueado = "Default";

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    public bool PuedeAgarrarse()
    {
        return !bloqueado;
    }

    public void Grab(Transform nuevoGrabPoint)
    {
        if (bloqueado)
        {
            Debug.Log(name + " está bloqueado y no se puede agarrar.");
            return;
        }

        grabPointTransform = nuevoGrabPoint;

        if (objectRigidbody != null)
        {
            objectRigidbody.useGravity = false;
            objectRigidbody.isKinematic = true;
            objectRigidbody.velocity = Vector3.zero;
            objectRigidbody.angularVelocity = Vector3.zero;
        }
    }

    public void Drop()
    {
        grabPointTransform = null;

        if (bloqueado)
            return;

        if (objectRigidbody != null)
        {
            objectRigidbody.useGravity = true;
            objectRigidbody.isKinematic = false;
        }
    }

    public void SoltarForzado()
    {
        grabPointTransform = null;
    }

    public void BloquearObjeto()
    {
        bloqueado = true;
        grabPointTransform = null;

        if (objectRigidbody != null)
        {
            objectRigidbody.velocity = Vector3.zero;
            objectRigidbody.angularVelocity = Vector3.zero;
            objectRigidbody.useGravity = false;
            objectRigidbody.isKinematic = true;
        }

        CambiarLayerRecursivo(gameObject, layerBloqueado);

        Debug.Log(
            name +
            " bloqueado. Layer actual: " +
            LayerMask.LayerToName(gameObject.layer)
        );
    }

    public void DesbloquearObjeto()
    {
        bloqueado = false;
        grabPointTransform = null;

        if (objectRigidbody != null)
        {
            objectRigidbody.useGravity = true;
            objectRigidbody.isKinematic = false;
        }
    }

    private void FixedUpdate()
    {
        if (bloqueado)
            return;

        if (grabPointTransform == null)
            return;

        if (objectRigidbody == null)
            return;

        Vector3 nuevaPosicion = Vector3.Lerp(
            transform.position,
            grabPointTransform.position,
            Time.deltaTime * lerpSpeed
        );

        objectRigidbody.MovePosition(nuevaPosicion);
    }

    private void CambiarLayerRecursivo(GameObject objeto, string nombreLayer)
    {
        int nuevaLayer = LayerMask.NameToLayer(nombreLayer);

        if (nuevaLayer == -1)
        {
            Debug.LogError("No existe la layer llamada: " + nombreLayer);
            return;
        }

        objeto.layer = nuevaLayer;

        foreach (Transform hijo in objeto.transform)
        {
            CambiarLayerRecursivo(hijo.gameObject, nombreLayer);
        }
    }
}