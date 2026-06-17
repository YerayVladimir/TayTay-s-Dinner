using UnityEngine;

public class SpawnerIngrediente : MonoBehaviour
{
    [Header("Prefab de este ingrediente")]
    [Tooltip("Arrastra aquí el mismo prefab de este objeto.")]
    public GameObject prefabIngrediente;

    [Header("Respawn")]
    public float tiempoRespawn = 1.5f;

    [Tooltip("Desactiva para ingredientes bloqueados (compra/desbloqueo).")]
    public bool activo = true;

    // Los clones reciben estos valores del spawner que los creó,
    // antes de que Start() se ejecute, así nunca se pierden.
    [HideInInspector] public Vector3 posicionOriginal;
    [HideInInspector] public Quaternion rotacionOriginal;
    [HideInInspector] public Vector3 escalaOriginal;
    [HideInInspector] public bool inicializadoExternamente = false;

    private void Start()
    {
        // Los objetos originales de la escena se inicializan solos.
        // Los clones ya traen los valores asignados por el spawner padre.
        if (!inicializadoExternamente)
        {
            posicionOriginal = transform.position;
            rotacionOriginal = transform.rotation;
            escalaOriginal = transform.localScale;
        }
    }

    public void NotificarIngredienteTomado()
    {
        if (!activo) return;
        Invoke(nameof(Respawnear), tiempoRespawn);
    }

    private void Respawnear()
    {
        if (prefabIngrediente == null)
        {
            Debug.LogError($"[SpawnerIngrediente] No hay prefab asignado en {gameObject.name}.");
            return;
        }

        GameObject nuevo = Instantiate(prefabIngrediente, posicionOriginal, rotacionOriginal);
        nuevo.transform.localScale = escalaOriginal;

        // Propagamos los valores al clone ANTES de que su Start() corra
        if (nuevo.TryGetComponent(out SpawnerIngrediente nuevoSpawner))
        {
            nuevoSpawner.posicionOriginal = posicionOriginal;
            nuevoSpawner.rotacionOriginal = rotacionOriginal;
            nuevoSpawner.escalaOriginal = escalaOriginal;
            nuevoSpawner.activo = activo;
            nuevoSpawner.tiempoRespawn = tiempoRespawn;
            nuevoSpawner.inicializadoExternamente = true;
        }
    }

    public void Activar()
    {
        activo = true;
        Respawnear();
    }

    public void Desactivar()
    {
        activo = false;
        CancelInvoke(nameof(Respawnear));
    }
}