using UnityEngine;
using TMPro;

public class ComprarIngredienteSimple : MonoBehaviour
{
    [Header("Referencias")]
    public Eventos eventos;

    [Header("Compra")]
    public int precio = 10;
    public string nombreCompra = "Ingrediente";

    [Header("UI opcional")]
    public TextMeshProUGUI textoBoton;

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

        eventos.MostrarMensajeEntrega(
            "Compraste " + nombreCompra + " -$" + precio,
            true
        );

        Debug.Log("Compra realizada: " + nombreCompra);
    }

    private void ActualizarTexto()
    {
        if (textoBoton != null)
        {
            textoBoton.text = nombreCompra + " $" + precio;
        }
    }
}