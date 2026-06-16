using System.Collections.Generic;
using UnityEngine;

public class Mesas : MonoBehaviour
{
    public Transform puntoSentarse;

    public bool ocupada;

    public Cliente clienteActual;

    public Eventos eventos;

    private bool pedidoRecibido = false;

    private void OnTriggerEnter(Collider other)
    {
        if (pedidoRecibido)
            return;

        if (!ocupada)
            return;

        if (clienteActual == null)
            return;

        HamburguesaActual hamburguesa =
            other.GetComponentInParent<HamburguesaActual>();

        if (hamburguesa == null)
            return;

        pedidoRecibido = true;

        bool pedidoCorrecto =
            CompararPedido(
                clienteActual.pedido,
                hamburguesa.ingredientes
            );

        if (pedidoCorrecto)
        {
            if (eventos != null)
            {
                eventos.AgregarDinero(CalcularPago());
                eventos.QuitarPedidoDeCliente(clienteActual);
            }

            clienteActual.PedidoEntregado();

            Debug.Log("Pedido correcto. Se agregó dinero.");
        }
        else
        {
            if (eventos != null)
            {
                eventos.QuitarDinero(5);
                eventos.QuitarPedidoDeCliente(clienteActual);
            }

            clienteActual.IrASalida();

            Debug.Log("Pedido incorrecto. Se quitó dinero.");
        }

        Destroy(hamburguesa.gameObject);

        LiberarMesa();
    }

    bool CompararPedido(
        List<int> pedido,
        List<int> hamburguesa)
    {
        if (pedido == null || hamburguesa == null)
            return false;

        if (pedido.Count != hamburguesa.Count)
            return false;

        for (int i = 0; i < pedido.Count; i++)
        {
            if (pedido[i] != hamburguesa[i])
            {
                return false;
            }
        }

        return true;
    }

    int CalcularPago()
    {
        if (clienteActual == null)
            return 0;

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
        pedidoRecibido = false;
    }
}