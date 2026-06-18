using UnityEngine;
using TMPro;

public class CompraGrupoIngredientes : MonoBehaviour
{
    [Header("Referencias")]
    public Eventos eventos;

    [Header("Compra")]
    public int precio = 10;
    public string nombreIngrediente = "Lechuga";

    [Header("Compras disponibles")]
    public int cantidadComprada = 0;

    [Header("UI opcional")]
    public TextMeshProUGUI textoCantidad;

    private void Start()
    {
        ActualizarTexto();
    }

    private void OnMouseDown()
    {
        Comprar();
    }

    public void Comprar()
    {
        if (eventos == null)
        {
            Debug.LogError("Falta asignar Eventos.");
            return;
        }

        if (eventos.dineroActual < precio)
        {
            eventos.MostrarMensajeEntrega("No tienes suficiente dinero.", false);
            return;
        }

        eventos.QuitarDinero(precio);

        cantidadComprada++;

        eventos.MostrarMensajeEntrega(
            "Compraste " + nombreIngrediente + " x1 -$" + precio,
            true
        );

        ActualizarTexto();

        Debug.Log(
            "Compras disponibles de " +
            nombreIngrediente +
            ": " +
            cantidadComprada
        );
    }

    public bool TieneCompraDisponible()
    {
        return cantidadComprada > 0;
    }

    public bool UsarCompra()
    {
        if (cantidadComprada <= 0)
        {
            return false;
        }

        cantidadComprada--;

        ActualizarTexto();

        Debug.Log(
            "Se uso 1 " +
            nombreIngrediente +
            ". Restantes: " +
            cantidadComprada
        );

        return true;
    }

    private void ActualizarTexto()
    {
        if (textoCantidad != null)
        {
            textoCantidad.text =
                nombreIngrediente + ": " + cantidadComprada;
        }
    }
}