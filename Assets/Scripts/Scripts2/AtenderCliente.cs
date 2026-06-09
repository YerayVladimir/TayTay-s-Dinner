using UnityEngine;

public class AtenderCliente : MonoBehaviour
{
    public Eventos eventos;

    public void Atender()
    {
        Cliente cliente =
            GestorClientes
            .instancia
            .ObtenerPrimero();

        if (cliente == null)
            return;

        Mesas mesa =
            GestorClientes
            .instancia
            .ObtenerMesaLibre();

        if (mesa == null)
            return;

        cliente.pedido =
            eventos.GenerarPedido();

        mesa.ocupada = true;

        mesa.clienteActual =
            cliente;

        cliente.IrAMesa(mesa);

        GestorClientes
            .instancia
            .fila
            .Remove(cliente);

        GestorClientes
            .instancia
            .ActualizarFila();

        eventos.MostrarPedido(
            cliente.pedido);
    }
}