using UnityEngine;

public class SpawnerIngrediente : MonoBehaviour
{
    [Header("Prefab del ingrediente")]
    public GameObject prefabIngrediente;

    [Header("Punto donde aparece")]
    public Transform puntoSpawn;

    [Header("Configuración")]
    public float tiempoRespawn = 0.5f;
    public bool spawnearAlIniciar = true;

    private GameObject ingredienteActual;
    private bool esperandoRespawn = false;

    private void Start()
    {
        if (spawnearAlIniciar)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        if (ingredienteActual != null)
            return;

        if (esperandoRespawn)
            return;

        if (prefabIngrediente == null)
        {
            Debug.LogError("Falta asignar Prefab Ingrediente en " + name);
            return;
        }

        if (puntoSpawn == null)
        {
            Debug.LogError("Falta asignar Punto Spawn en " + name);
            return;
        }

        ingredienteActual = Instantiate(
            prefabIngrediente,
            puntoSpawn.position,
            puntoSpawn.rotation
        );

        IngredienteRespawnable respawnable =
            ingredienteActual.GetComponent<IngredienteRespawnable>();

        if (respawnable == null)
        {
            respawnable = ingredienteActual.AddComponent<IngredienteRespawnable>();
        }

        respawnable.spawner = this;

        Debug.Log("Ingrediente spawneado: " + ingredienteActual.name);
    }

    public void NotificarIngredienteTomado(GameObject ingrediente)
    {
        if (ingredienteActual == null)
            return;

        if (ingrediente != ingredienteActual)
            return;

        ingredienteActual = null;

        if (!esperandoRespawn)
        {
            esperandoRespawn = true;
            Invoke(nameof(Respawnear), tiempoRespawn);
        }
    }

    private void Respawnear()
    {
        esperandoRespawn = false;
        Spawn();
    }
}