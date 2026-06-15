using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controla los pedidos, la nota visual del pedido y el dinero.
/// La cocción de ingredientes se controla desde Ingrediente, Parrilla y Freidora.
/// El armado de hamburguesa se controla desde ZonaArmado y HamburguesaActual.
/// </summary>
public class Eventos : MonoBehaviour
{
    [Header("Elementos visuales del pedido")]
    public GameObject notaObj;
    public GameObject[] palomitas;

    [Header("Animación de la nota")]
    public Animator nota;
    public string animacionEntradaNota = "AnimNotaEntrada";

    [Header("Ingredientes posibles para pedidos")]
    public List<int> ingredientesDisponibles =
        new List<int>();

    [Header("IDs fijos")]
    public int idPanAbajo = 0;
    public int idPanArriba = 9;

    [Header("Cantidad de ingredientes extra")]
    public int cantidadMinimaIngredientes = 2;
    public int cantidadMaximaIngredientes = 4;

    [Header("Pedido actual")]
    public List<int> pedidoActual =
        new List<int>();

    [Header("Dinero")]
    public int dineroActual = 0;
    public TextMeshProUGUI textoDinero;

    private void Start()
    {
        OcultarPedido();
        ActualizarDinero();
    }

    //=================================================
    // PEDIDOS
    //=================================================

    public List<int> GenerarPedido()
    {
        List<int> pedido =
            new List<int>();

        pedido.Add(idPanAbajo);

        int cantidadIngredientes =
            Random.Range(
                cantidadMinimaIngredientes,
                cantidadMaximaIngredientes + 1);

        for (int i = 0; i < cantidadIngredientes; i++)
        {
            if (ingredientesDisponibles.Count == 0)
            {
                Debug.LogWarning(
                    "No hay ingredientes disponibles para generar pedidos.");
                break;
            }

            int ingrediente =
                ingredientesDisponibles[
                    Random.Range(
                        0,
                        ingredientesDisponibles.Count)];

            if (ingrediente == idPanAbajo ||
                ingrediente == idPanArriba ||
                pedido.Contains(ingrediente))
            {
                i--;
                continue;
            }

            pedido.Add(ingrediente);
        }

        pedido.Add(idPanArriba);

        pedidoActual =
            new List<int>(pedido);

        return pedido;
    }

    public void MostrarPedido(List<int> pedido)
    {
        if (pedido == null)
            return;

        pedidoActual =
            new List<int>(pedido);

        OcultarIconosPedido();

        foreach (int ingrediente in pedido)
        {
            if (ingrediente >= 0 &&
                ingrediente < palomitas.Length &&
                palomitas[ingrediente] != null)
            {
                palomitas[ingrediente].SetActive(true);
            }
        }

        if (notaObj != null)
        {
            notaObj.SetActive(true);
        }

        if (nota != null &&
            !string.IsNullOrEmpty(animacionEntradaNota))
        {
            nota.Play(animacionEntradaNota);
        }
    }

    public void OcultarPedido()
    {
        if (notaObj != null)
        {
            notaObj.SetActive(false);
        }

        OcultarIconosPedido();
    }

    private void OcultarIconosPedido()
    {
        foreach (GameObject palomita in palomitas)
        {
            if (palomita != null)
            {
                palomita.SetActive(false);
            }
        }
    }

    public void LimpiarPedidoActual()
    {
        pedidoActual.Clear();

        OcultarPedido();
    }

    //=================================================
    // DINERO
    //=================================================

    public void AgregarDinero(int cantidad)
    {
        dineroActual += cantidad;

        ActualizarDinero();
    }

    public void QuitarDinero(int cantidad)
    {
        dineroActual -= cantidad;

        if (dineroActual < 0)
        {
            dineroActual = 0;
        }

        ActualizarDinero();
    }

    public void ActualizarDinero()
    {
        if (textoDinero != null)
        {
            textoDinero.text =
                "$" + dineroActual;
        }
    }
}