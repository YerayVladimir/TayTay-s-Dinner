using UnityEngine;

public class AtenderCliente : MonoBehaviour
{
    [Header("Referencias")]
    public Eventos eventos;

    [Header("Tecla para atender")]
    public KeyCode teclaAtender = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(teclaAtender))
        {
            Atender();
        }
    }

    public void Atender()
    {
        if (eventos == null)
        {
            Debug.LogError("Falta asignar GameManagerEventos en el campo Eventos.");
            return;
        }

        if (GestorClientes.instancia == null)
        {
            Debug.LogError("No existe GestorClientes en la escena.");
            return;
        }

        Cliente cliente = GestorClientes.instancia.ObtenerPrimero();

        if (cliente == null)
        {
            Debug.LogWarning("No hay clientes en la fila para atender.");
            return;
        }

        if (cliente.yaAtendido)
        {
            Debug.LogWarning("Este cliente ya fue atendido.");
            return;
        }
        if (!eventos.PuedeCrearOtroPedido())
        {
            Debug.LogWarning("Ya hay 3 pedidos activos. Entrega uno antes de atender otro cliente.");
            return;
        }

        Mesas mesa = GestorClientes.instancia.ObtenerMesaLibre();

        if (mesa == null)
        {
            Debug.LogWarning("No hay mesas libres.");
            return;
        }

        cliente.yaAtendido = true;

        cliente.pedido = eventos.GenerarPedido();

        mesa.ocupada = true;
        mesa.clienteActual = cliente;

        if (mesa.eventos == null)
        {
            mesa.eventos = eventos;
        }

        cliente.IrAMesa(mesa);

        GestorClientes.instancia.fila.Remove(cliente);
        GestorClientes.instancia.ActualizarFila();

        eventos.MostrarPedidoDeCliente(cliente, cliente.pedido);

        Debug.Log(
            "Cliente atendido. Pedido: " +
            string.Join(", ", cliente.pedido)
        );
    }
}