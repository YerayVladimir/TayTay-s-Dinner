using UnityEngine;

public class IngredienteRespawnable : MonoBehaviour
{
    [HideInInspector] public SpawnerIngrediente spawner;

    private bool yaAviso = false;

    public void AvisarQueFueTomado()
    {
        if (yaAviso)
            return;

        yaAviso = true;

        if (spawner != null)
        {
            spawner.NotificarIngredienteTomado(gameObject);
        }
    }
}