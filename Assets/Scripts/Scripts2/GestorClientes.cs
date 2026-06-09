using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorClientes : MonoBehaviour
{
    public static GestorClientes
        instancia;

    public GameObject[] clientes;

    public Transform spawnCliente;

    public Transform puntoSalida;

    public Transform[] posicionesFila;

    public Mesas[] mesas;

    public List<Cliente> fila =
        new List<Cliente>();

    private void Awake()
    {
        instancia = this;
    }

    private void Start()
    {
        StartCoroutine(
            GenerarClientes());
    }

    IEnumerator GenerarClientes()
    {
        while (true)
        {
            CrearCliente();

            yield return
                new WaitForSeconds(10f);
        }
    }

    void CrearCliente()
    {
        if (clientes.Length == 0)
        {
            Debug.LogError(
                "No hay clientes asignados");
            return;
        }

        GameObject nuevo =
            Instantiate(
                clientes[
                    Random.Range(
                        0,
                        clientes.Length)],
                spawnCliente.position,
                Quaternion.identity);

        Cliente cliente =
            nuevo.GetComponent<Cliente>();

        if (cliente == null)
        {
            Debug.LogError(
                "El prefab no tiene Cliente.cs");
            return;
        }

        fila.Add(cliente);

        ActualizarFila();
    }

    public void ActualizarFila()
    {
        for (int i = 0;
             i < fila.Count &&
             i < posicionesFila.Length;
             i++)
        {
            fila[i].MoverA(
                posicionesFila[i]
                .position);
        }
    }

    public Cliente ObtenerPrimero()
    {
        if (fila.Count == 0)
            return null;

        return fila[0];
    }

    public Mesas ObtenerMesaLibre()
    {
        foreach (Mesas mesa in mesas)
        {
            if (!mesa.ocupada)
                return mesa;
        }

        return null;
    }
}