using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform grabPointTransform;
    [SerializeField] private float pickUpDistance;
    [SerializeField] private LayerMask pickUpLayerMask;

    private ObjectGrabbable objectGrabbable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objectGrabbable == null)
            {
                if (Physics.Raycast(
                        playerCameraTransform.position,
                        playerCameraTransform.forward,
                        out RaycastHit raycastHit,
                        pickUpDistance,
                        pickUpLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(grabPointTransform);

                        // Avisa al spawner para que haga respawn
                        if (raycastHit.transform.TryGetComponent(out IngredienteRespawnable respawnable))
                        {
                            respawnable.AvisarQueFueTomado();
                        }

                        // Habilita la cocción solo si el jugador lo agarra
                        if (raycastHit.transform.GetComponentInParent<Ingrediente>() is Ingrediente ingrediente)
                        {
                            ingrediente.fueTomadoPorJugador = true;
                            Debug.Log("fueTomadoPorJugador activado en: " + ingrediente.name);
                        }
                    }
                }
            }
            else
            {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
    }
}