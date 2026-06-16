using System.Collections.Generic;
using UnityEngine;

public class Cliente : MonoBehaviour
{
    public enum EstadoCliente
    {
        EnFila,
        EsperandoPedido,
        CaminandoMesa,
        EsperandoComida,
        Comiendo,
        Saliendo
    }

    [Header("Estado")]
    public EstadoCliente estado;

    [Header("Movimiento")]
    public float velocidad = 3f;

    [Header("Paciencia")]
    public float pacienciaMaxima = 60f;
    public float pacienciaActual;

    [Header("Pedido")]
    public List<int> pedido =
        new List<int>();

    [Header("Control de atención")]
    public bool yaAtendido = false;

    [Header("Mesa")]
    public Mesas mesaAsignada;

    private Vector3 destino;
    private bool moviendose;

    private void Start()
    {
        pacienciaActual =
            pacienciaMaxima;
    }

    private void Update()
    {
        Movimiento();

        ControlPaciencia();
    }

    void Movimiento()
    {
        if (!moviendose)
            return;

        Vector3 direccion =
            destino - transform.position;

        if (direccion.magnitude > 0.1f)
        {
            transform.position +=
                direccion.normalized *
                velocidad *
                Time.deltaTime;

            transform.forward =
                direccion.normalized;
        }
        else
        {
            moviendose = false;

            if (estado ==
                EstadoCliente.CaminandoMesa)
            {
                estado =
                    EstadoCliente.EsperandoComida;
            }

            if (estado ==
                EstadoCliente.Saliendo)
            {
                Destroy(gameObject);
            }
        }
    }

    void ControlPaciencia()
    {
        if (estado !=
            EstadoCliente.EsperandoComida)
            return;

        pacienciaActual -=
            Time.deltaTime;

        if (pacienciaActual <= 0)
        {
            IrASalida();
        }
    }

    public void AsignarPedido(
        List<int> nuevoPedido)
    {
        pedido =
            new List<int>(nuevoPedido);
    }

    public void MoverA(
        Vector3 nuevaPosicion)
    {
        destino =
            nuevaPosicion;

        moviendose = true;
    }

    public void IrAMesa(
        Mesas mesa)
    {
        mesaAsignada = mesa;

        estado =
            EstadoCliente.CaminandoMesa;

        MoverA(
            mesa.puntoSentarse.position);
    }

    public void PedidoEntregado()
    {
        estado =
            EstadoCliente.Comiendo;

        Invoke(
            nameof(IrASalida),
            5f);
    }

    public void IrASalida()
    {
        estado =
            EstadoCliente.Saliendo;

        if (mesaAsignada != null)
        {
            mesaAsignada.LiberarMesa();
        }

        MoverA(
            GestorClientes.instancia
            .puntoSalida.position);
    }
}