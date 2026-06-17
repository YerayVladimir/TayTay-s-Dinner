using UnityEngine;

/// <summary>
/// Ponle este componente a cada prefab de ingrediente.
/// Al ser agarrado, avisa al SpawnerIngrediente para que
/// instancie una copia en la posición original.
/// </summary>
public class IngredienteRespawnable : MonoBehaviour
{
    // El spawner se asigna automáticamente en Start().
    // No necesitas arrastrarlo a mano en el Inspector.
    [HideInInspector] public SpawnerIngrediente spawner;

    private bool yaAviso = false;

    private void Start()
    {
        // Busca el SpawnerIngrediente en el mismo GameObject
        spawner = GetComponent<SpawnerIngrediente>();

        if (spawner == null)
        {
            Debug.LogWarning($"[IngredienteRespawnable] {gameObject.name} no tiene SpawnerIngrediente en el mismo objeto.");
        }
    }

    public void AvisarQueFueTomado()
    {
        if (yaAviso) return;
        yaAviso = true;

        if (spawner != null)
        {
            spawner.NotificarIngredienteTomado();
        }
    }
}