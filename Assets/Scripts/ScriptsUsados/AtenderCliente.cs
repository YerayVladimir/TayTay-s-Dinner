using UnityEngine;

public class AtenderCliente : MonoBehaviour
{
    [Header("Referencias")]
    public Eventos eventos;

    [Header("Jugador")]
    public Transform jugador;

    [Header("Tecla para atender")]
    public KeyCode teclaAtender = KeyCode.R;

    [Header("Atender frente al primer cliente")]
    public bool permitirAtenderFrenteCliente = true;
    public float distanciaMaximaCliente = 2.2f;

    [Header("Atender desde caja / mostrador")]
    public bool permitirAtenderEnMostrador = true;
    public Transform puntoMostrador;
    public float distanciaMaximaMostrador = 2.5f;

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

        if (jugador == null)
        {
            Debug.LogError("Falta asignar el Jugador en AtenderCliente.");
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

        if (!PuedeAtenderPorUbicacion(cliente))
        {
            Debug.LogWarning("Debes estar frente al primer cliente o en el mostrador para tomar el pedido.");
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

        cliente.AsignarEventos(eventos);

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

    private bool PuedeAtenderPorUbicacion(Cliente cliente)
    {
        if (cliente == null)
            return false;

        bool cercaDelPrimerCliente = false;
        bool cercaDelMostrador = false;

        if (permitirAtenderFrenteCliente)
        {
            float distanciaCliente = Vector3.Distance(
                jugador.position,
                cliente.transform.position
            );

            if (distanciaCliente <= distanciaMaximaCliente)
            {
                cercaDelPrimerCliente = true;
            }
        }

        if (permitirAtenderEnMostrador && puntoMostrador != null)
        {
            float distanciaMostrador = Vector3.Distance(
                jugador.position,
                puntoMostrador.position
            );

            if (distanciaMostrador <= distanciaMaximaMostrador)
            {
                cercaDelMostrador = true;
            }
        }

        return cercaDelPrimerCliente || cercaDelMostrador;
    }
}