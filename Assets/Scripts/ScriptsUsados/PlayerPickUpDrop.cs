using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform grabPointTransform;
    [SerializeField] private float pickUpDistance = 4f;
    [SerializeField] private LayerMask pickUpLayerMask;

    private ObjectGrabbable objectGrabbable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                IntentarAgarrar();
            }
            else
            {
                Soltar();
            }
        }
    }

    private void IntentarAgarrar()
    {
        if (playerCameraTransform == null)
        {
            Debug.LogError("Falta asignar Player Camera Transform.");
            return;
        }

        bool golpeo = Physics.Raycast(
            playerCameraTransform.position,
            playerCameraTransform.forward,
            out RaycastHit raycastHit,
            pickUpDistance,
            pickUpLayerMask
        );

        if (!golpeo)
        {
            Debug.Log("No golpeo ningun objeto con el raycast.");
            return;
        }

        ObjectGrabbable grabbable =
            raycastHit.collider.GetComponentInParent<ObjectGrabbable>();

        if (grabbable == null)
        {
            Debug.Log("El objeto golpeado no tiene ObjectGrabbable.");
            return;
        }

        if (!grabbable.PuedeAgarrarse())
        {
            Debug.Log(grabbable.name + " no se puede agarrar. Falta comprar.");
            return;
        }

        objectGrabbable = grabbable;
        objectGrabbable.Grab(grabPointTransform);

        Ingrediente ingrediente =
            objectGrabbable.GetComponentInParent<Ingrediente>();

        if (ingrediente != null)
        {
            ingrediente.fueTomadoPorJugador = true;
            Debug.Log("fueTomadoPorJugador activado en: " + ingrediente.name);
        }
    }

    private void Soltar()
    {
        objectGrabbable.Drop();
        objectGrabbable = null;
    }
}