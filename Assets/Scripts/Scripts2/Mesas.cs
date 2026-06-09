using System.Collections.Generic;
using UnityEngine;

public class Mesas : MonoBehaviour
{
    public Transform puntoSentarse;

    public bool ocupada;

    public Cliente clienteActual;

    public Eventos eventos;

    private void OnTriggerEnter(Collider other)
    {
        if (!ocupada)
            return;

        if (clienteActual == null)
            return;

        if (!other.CompareTag("Hamburguesa"))
            return;

        HamburguesaActual hamburguesa =
            other.GetComponent<HamburguesaActual>();

        if (hamburguesa == null)
            return;

        bool pedidoCorrecto =
            CompararPedido(
                clienteActual.pedido,
                hamburguesa.ingredientes);

        if (pedidoCorrecto)
        {
            if (eventos != null)
            {
                eventos.AgregarDinero(
                    CalcularPago());
            }

            clienteActual.PedidoEntregado();

            Debug.Log("Pedido correcto");
        }
        else
        {
            if (eventos != null)
            {
                eventos.QuitarDinero(5);
            }

            clienteActual.IrASalida();

            Debug.Log("Pedido incorrecto");
        }

        Destroy(other.gameObject);

        LiberarMesa();
    }

    bool CompararPedido(
        List<int> pedido,
        List<int> hamburguesa)
    {
        if (pedido == null ||
            hamburguesa == null)
            return false;

        if (pedido.Count !=
            hamburguesa.Count)
            return false;

        for (int i = 0;
             i < pedido.Count;
             i++)
        {
            if (pedido[i] !=
                hamburguesa[i])
                return false;
        }

        return true;
    }

    int CalcularPago()
    {
        float tiempo =
            clienteActual.pacienciaMaxima -
            clienteActual.pacienciaActual;

        if (tiempo <= 10f)
            return 35;

        if (tiempo <= 20f)
            return 30;

        if (tiempo <= 30f)
            return 25;

        if (tiempo <= 40f)
            return 20;

        return 10;
    }

    public void LiberarMesa()
    {
        ocupada = false;
        clienteActual = null;
    }
}