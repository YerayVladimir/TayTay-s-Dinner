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
        Debug.Log("ALGO ENTRO A LA MESA: " + other.name);

        if (pedidoRecibido)
        {
            Debug.Log("La mesa ya recibio un pedido.");
            return;
        }

        if (!ocupada)
        {
            Debug.Log("La mesa no esta ocupada.");
            return;
        }

        if (clienteActual == null)
        {
            Debug.Log("La mesa esta ocupada, pero no tiene clienteActual.");
            return;
        }

        HamburguesaActual hamburguesa =
            other.GetComponentInParent<HamburguesaActual>();

        if (hamburguesa == null)
        {
            Debug.Log("Lo que entro no tiene HamburguesaActual.");
            return;
        }

        if (hamburguesa.ingredientes == null ||
            hamburguesa.ingredientes.Count == 0)
        {
            Debug.LogWarning("La hamburguesa no tiene ingredientes registrados.");
            return;
        }

        pedidoRecibido = true;

        Cliente cliente = clienteActual;

        Debug.Log("HAMBURGUESA DETECTADA.");
        Debug.Log("PEDIDO ESPERADO: " + ConvertirListaATexto(cliente.pedido));
        Debug.Log("HAMBURGUESA ENTREGADA: " + ConvertirListaATexto(hamburguesa.ingredientes));

        bool pedidoCorrecto =
            CompararPedido(
                cliente.pedido,
                hamburguesa.ingredientes
            );

        if (pedidoCorrecto)
        {
            int pago = CalcularPago(cliente);

            if (eventos != null)
            {
                eventos.AgregarDinero(pago);
                eventos.QuitarPedidoDeCliente(cliente);
                eventos.MostrarMensajeEntrega("Pedido correcto +$" + pago, true);
            }
            else
            {
                Debug.LogError("Falta asignar Eventos en la mesa.");
            }

            cliente.PedidoEntregado();

            Debug.Log("PEDIDO CORRECTO. Cliente comiendo. Se agrego dinero: $" + pago);

            Destroy(hamburguesa.gameObject);
        }
        else
        {
            string mensajeError =
                ObtenerMensajeErrorPedido(
                    cliente.pedido,
                    hamburguesa.ingredientes
                );

            if (eventos != null)
            {
                eventos.QuitarDinero(5);
                eventos.QuitarPedidoDeCliente(cliente);
                eventos.MostrarMensajeEntrega(mensajeError + " -$5", false);
            }
            else
            {
                Debug.LogError("Falta asignar Eventos en la mesa.");
            }

            cliente.IrASalida();

            Debug.Log("PEDIDO INCORRECTO. Cliente se va. Se quito dinero.");
            Debug.Log(mensajeError);

            Destroy(hamburguesa.gameObject);
        }
    }

    private bool CompararPedido(
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

    private string ObtenerMensajeErrorPedido(
        List<int> pedido,
        List<int> hamburguesa)
    {
        if (pedido == null || hamburguesa == null)
        {
            return "Error en el pedido";
        }

        if (hamburguesa.Count < pedido.Count)
        {
            return "Error: faltan ingredientes";
        }

        if (hamburguesa.Count > pedido.Count)
        {
            return "Error: sobran ingredientes";
        }

        for (int i = 0; i < pedido.Count; i++)
        {
            if (pedido[i] != hamburguesa[i])
            {
                string esperado = ObtenerNombreSeguro(pedido[i]);
                string entregado = ObtenerNombreSeguro(hamburguesa[i]);

                return "Error: esperaba " + esperado + " y pusiste " + entregado;
            }
        }

        return "Error en el pedido";
    }

    private int CalcularPago(Cliente cliente)
    {
        if (cliente == null)
            return 0;

        float tiempo =
            cliente.pacienciaMaxima -
            cliente.pacienciaActual;

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

    private string ConvertirListaATexto(List<int> lista)
    {
        if (lista == null)
            return "null";

        if (lista.Count == 0)
            return "vacia";

        string texto = "";

        for (int i = 0; i < lista.Count; i++)
        {
            texto += lista[i];

            if (i < lista.Count - 1)
            {
                texto += ", ";
            }
        }

        return texto;
    }

    private string ObtenerNombreSeguro(int id)
    {
        if (eventos != null)
            return eventos.ObtenerNombreIngrediente(id);

        return "ID " + id;
    }

    public void LiberarMesa()
    {
        ocupada = false;
        clienteActual = null;
        pedidoRecibido = false;

        Debug.Log("Mesa liberada: " + name);
    }
}