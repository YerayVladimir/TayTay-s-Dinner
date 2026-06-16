using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Referencias")]
    public Camera camara;
    public Transform grabPointTransform;

    [Header("Agarre")]
    public float distanciaAgarre = 4f;
    public LayerMask layerObjetos;

    private ObjectGrabbable objetoAgarrado;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IntentarAgarrar();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Soltar();
        }
    }

    private void IntentarAgarrar()
    {
        if (objetoAgarrado != null)
            return;

        if (camara == null)
        {
            Debug.LogError("Falta asignar la cámara en ObjectGrabber.");
            return;
        }

        Ray ray = camara.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        bool golpeo = Physics.Raycast(
            ray,
            out hit,
            distanciaAgarre,
            layerObjetos,
            QueryTriggerInteraction.Ignore
        );

        if (!golpeo)
            return;

        ObjectGrabbable grabbable =
            hit.collider.GetComponentInParent<ObjectGrabbable>();

        if (grabbable == null)
            return;

        if (!grabbable.PuedeAgarrarse())
        {
            Debug.Log(grabbable.name + " está bloqueado. No se puede agarrar.");
            return;
        }

        objetoAgarrado = grabbable;
        objetoAgarrado.Grab(grabPointTransform);
    }

    private void Soltar()
    {
        if (objetoAgarrado == null)
            return;

        objetoAgarrado.Drop();
        objetoAgarrado = null;
    }
}