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

    private bool pagadoParaEsteAgarre = false;

    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bloqueado = false;
        pagadoParaEsteAgarre = false;
        grabPointTransform = null;
        CambiarLayerRecursivo(gameObject, "Objetos");

        // Reactivar collider por si quedó desactivado al apilar
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        if (objectRigidbody != null)
        {
            objectRigidbody.useGravity = true;
            objectRigidbody.isKinematic = false;
        }
    }
    public bool PuedeAgarrarse()
    {
        if (!bloqueado)
            return true;

        IngredienteComprable comprable =
            GetComponent<IngredienteComprable>();

        if (comprable == null)
        {
            Debug.Log(name + " esta bloqueado y no tiene IngredienteComprable.");
            return false;
        }

        return comprable.PuedeTomarseComprado();
    }

    public void Grab(Transform nuevoGrabPoint)
    {
        if (bloqueado)
        {
            IngredienteComprable comprable =
                GetComponent<IngredienteComprable>();

            if (comprable == null)
            {
                Debug.Log(name + " esta bloqueado y no se puede agarrar.");
                return;
            }

            bool pudoUsarCompra =
                comprable.IntentarUsarCompra();

            if (!pudoUsarCompra)
            {
                Debug.Log(name + " esta bloqueado. Primero debes comprar.");
                return;
            }

            pagadoParaEsteAgarre = true;

            Debug.Log(name + " se puede agarrar porque ya fue pagado.");
        }
        else
        {
            pagadoParaEsteAgarre = false;
        }

        IngredienteRespawnable respawnable =
            GetComponent<IngredienteRespawnable>();

        if (respawnable != null)
        {
            respawnable.AvisarQueFueTomado();
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

        if (pagadoParaEsteAgarre)
        {
            bloqueado = true;
            pagadoParaEsteAgarre = false;
        }

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