using UnityEngine;

public class SpawnerIngrediente : MonoBehaviour
{
    [Header("Prefab de este ingrediente")]
    [Tooltip("Arrastra aqui el mismo prefab de este objeto.")]
    public GameObject prefabIngrediente;

    [Header("Respawn")]
    public float tiempoRespawn = 1.5f;

    [Tooltip("Desactiva para ingredientes bloqueados por compra.")]
    public bool activo = true;

    [Header("Bloqueo inicial")]
    public bool ocultarSiEstaBloqueado = true;

    [HideInInspector] public Vector3 posicionOriginal;
    [HideInInspector] public Quaternion rotacionOriginal;
    [HideInInspector] public Vector3 escalaOriginal;
    [HideInInspector] public bool inicializadoExternamente = false;

    private bool datosGuardados = false;

    private void Awake()
    {
        GuardarDatosOriginales();

        if (!activo && ocultarSiEstaBloqueado)
        {
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        GuardarDatosOriginales();
    }

    private void GuardarDatosOriginales()
    {
        if (datosGuardados)
            return;

        if (inicializadoExternamente)
        {
            datosGuardados = true;
            return;
        }

        posicionOriginal = transform.position;
        rotacionOriginal = transform.rotation;
        escalaOriginal = transform.localScale;

        datosGuardados = true;
    }

    public void NotificarIngredienteTomado()
    {
        if (!activo)
            return;

        CancelInvoke(nameof(Respawnear));
        Invoke(nameof(Respawnear), tiempoRespawn);
    }

    private void Respawnear()
    {
        if (!activo)
            return;

        if (prefabIngrediente == null)
        {
            Debug.LogError("[SpawnerIngrediente] No hay prefab asignado en " + gameObject.name);
            return;
        }

        GameObject nuevo = Instantiate(
            prefabIngrediente,
            posicionOriginal,
            rotacionOriginal
        );

        nuevo.transform.localScale = escalaOriginal;

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
        GuardarDatosOriginales();

        activo = true;
        CancelInvoke(nameof(Respawnear));

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);

            Ingrediente ingrediente = GetComponent<Ingrediente>();

            if (ingrediente != null)
            {
                ingrediente.Reiniciar();
            }

            return;
        }

        Debug.Log("Spawner activado: " + gameObject.name);
    }

    public void Desactivar()
    {
        GuardarDatosOriginales();

        activo = false;
        CancelInvoke(nameof(Respawnear));

        if (ocultarSiEstaBloqueado)
        {
            gameObject.SetActive(false);
        }

        Debug.Log("Spawner desactivado: " + gameObject.name);
    }
}