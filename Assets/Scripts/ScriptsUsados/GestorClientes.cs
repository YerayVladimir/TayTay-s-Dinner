using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorClientes : MonoBehaviour
{
    List<GameObject> poolClientes = new List<GameObject>();

    public static GestorClientes instancia;

    [Header("Clientes")]
    public GameObject[] clientes;

    [Header("Puntos")]
    public Transform spawnCliente;
    public Transform puntoSalida;

    [Header("Fila")]
    public Transform[] posicionesFila;

    [Header("Mesas")]
    public Mesas[] mesas;

    [Header("Configuración")]
    public float tiempoEntreClientes = 30f;

    [Header("Clientes en fila")]
    public List<Cliente> fila = new List<Cliente>();

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        ValidarInspector();

        Debug.Log("Cantidad de prefabs de clientes asignados: " + clientes.Length);

        StartCoroutine(GenerarClientes());
    }

    private void ValidarInspector()
    {
        if (clientes == null || clientes.Length == 0)
        {
            Debug.LogError("No hay prefabs de clientes asignados en GestorClientes.");
        }

        if (spawnCliente == null)
        {
            Debug.LogError("Falta asignar Spawn Cliente en GestorClientes.");
        }

        if (puntoSalida == null)
        {
            Debug.LogError("Falta asignar Punto Salida en GestorClientes.");
        }

        if (posicionesFila == null || posicionesFila.Length == 0)
        {
            Debug.LogError("Faltan posiciones de fila en GestorClientes.");
        }

        if (mesas == null || mesas.Length == 0)
        {
            Debug.LogError("Faltan mesas asignadas en GestorClientes.");
        }
    }

    IEnumerator GenerarClientes()
    {
        while (true)
        {
            CrearCliente();

            yield return new WaitForSeconds(tiempoEntreClientes);
        }
    }

    void CrearCliente()
    {
        if (clientes == null || clientes.Length == 0)
        {
            Debug.LogError("No hay clientes asignados.");
            return;
        }

        if (spawnCliente == null)
        {
            Debug.LogError("No se puede crear cliente porque falta Spawn Cliente.");
            return;
        }

        int indiceCliente = Random.Range(0, clientes.Length);

        GameObject prefabCliente = clientes[indiceCliente];

        if (prefabCliente == null)
        {
            Debug.LogError("Hay un elemento vacío en la lista Clientes. Revisa el Element " + indiceCliente);
            return;
        }

        GameObject nuevo = ObtenerClienteDelPool();

        nuevo.transform.position = spawnCliente.position;
        nuevo.transform.rotation = spawnCliente.rotation;

        nuevo.SetActive(true);

        Cliente cliente = nuevo.GetComponent<Cliente>();


        if (cliente == null)
        {
            Debug.LogError("El prefab instanciado no tiene el script Cliente.cs.");
            return;
        }

        fila.Add(cliente);

        ActualizarFila();

        Debug.Log("Cliente creado y agregado a la fila.");
    }

    public void ActualizarFila()
    {
        if (posicionesFila == null || posicionesFila.Length == 0)
        {
            Debug.LogError("No se puede actualizar la fila porque no hay posiciones asignadas.");
            return;
        }

        fila.RemoveAll(cliente => cliente == null);

        for (int i = 0; i < fila.Count; i++)
        {
            if (i >= posicionesFila.Length)
            {
                Debug.LogWarning("Hay más clientes que posiciones de fila. Cliente " + i + " se queda sin posición.");
                return;
            }

            if (posicionesFila[i] == null)
            {
                Debug.LogError("La posición de fila " + i + " está vacía.");
                continue;
            }

            fila[i].MoverA(posicionesFila[i].position);
        }
    }

    public Cliente ObtenerPrimero()
    {
        fila.RemoveAll(cliente => cliente == null);

        if (fila.Count == 0)
        {
            Debug.LogWarning("No hay clientes en la fila.");
            return null;
        }

        return fila[0];
    }

    public Mesas ObtenerMesaLibre()
    {
        if (mesas == null || mesas.Length == 0)
        {
            Debug.LogError("No hay mesas asignadas en GestorClientes.");
            return null;
        }

        foreach (Mesas mesa in mesas)
        {
            if (mesa == null)
                continue;

            if (!mesa.ocupada)
                return mesa;
        }

        Debug.LogWarning("No hay mesas libres.");
        return null;
    }

    GameObject ObtenerClienteDelPool()
    {
        foreach (var obj in poolClientes)
        {
            if (!obj.activeInHierarchy)
            {
                Cliente c = obj.GetComponent<Cliente>();
                c.Reiniciar();
                return obj;
            }
        }

        GameObject nuevo = Instantiate(
            clientes[Random.Range(0, clientes.Length)],
            spawnCliente.position,
            spawnCliente.rotation
        );

        poolClientes.Add(nuevo);
        return nuevo;
    }
}