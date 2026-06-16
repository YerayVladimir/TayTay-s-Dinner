using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controla los pedidos, la visualización simple del pedido y el dinero.
/// Puede funcionar aunque no tengas nota, iconos ni texto de dinero.
/// </summary>
public class Eventos : MonoBehaviour
{
    [Header("Elementos visuales del pedido")]
    public GameObject notaObj;
    public GameObject[] palomitas;

    [Header("Texto simple del pedido")]
    public TextMeshProUGUI textoPedido;

    [Header("Animación de la nota")]
    public Animator nota;
    public string animacionEntradaNota = "AnimNotaEntrada";

    [Header("Ingredientes posibles para pedidos")]
    public List<int> ingredientesDisponibles = new List<int>();

    [Header("IDs fijos")]
    public int idPanAbajo = 0;
    public int idPanArriba = 9;

    [Header("Ingrediente obligatorio")]
    public int idCarneObligatoria = 1;

    [Header("Cantidad de ingredientes extra")]
    public int cantidadMinimaIngredientes = 2;
    public int cantidadMaximaIngredientes = 4;

    [Header("Pedido actual")]
    public List<int> pedidoActual = new List<int>();

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
        List<int> pedido = new List<int>();

        // Siempre inicia con pan abajo
        pedido.Add(idPanAbajo);

        // Siempre lleva carne
        pedido.Add(idCarneObligatoria);

        List<int> ingredientesValidos = new List<int>();

        foreach (int ingrediente in ingredientesDisponibles)
        {
            if (ingrediente == idPanAbajo)
                continue;

            if (ingrediente == idPanArriba)
                continue;

            if (ingrediente == idCarneObligatoria)
                continue;

            if (ingredientesValidos.Contains(ingrediente))
                continue;

            ingredientesValidos.Add(ingrediente);
        }

        if (ingredientesValidos.Count == 0)
        {
            Debug.LogWarning("No hay ingredientes extra disponibles para generar pedidos.");
        }

        int minimo = Mathf.Max(0, cantidadMinimaIngredientes);
        int maximo = Mathf.Max(minimo, cantidadMaximaIngredientes);

        int cantidadIngredientes = Random.Range(
            minimo,
            maximo + 1
        );

        cantidadIngredientes = Mathf.Clamp(
            cantidadIngredientes,
            0,
            ingredientesValidos.Count
        );

        for (int i = 0; i < cantidadIngredientes; i++)
        {
            int indice = Random.Range(0, ingredientesValidos.Count);

            int ingredienteElegido = ingredientesValidos[indice];

            pedido.Add(ingredienteElegido);

            ingredientesValidos.RemoveAt(indice);
        }

        // Siempre termina con pan arriba
        pedido.Add(idPanArriba);

        pedidoActual = new List<int>(pedido);

        Debug.Log("Pedido generado: " + ConstruirTextoPedidoConIDs(pedido));

        return pedido;
    }

    public void MostrarPedido(List<int> pedido)
    {
        if (pedido == null)
            return;

        pedidoActual = new List<int>(pedido);

        OcultarIconosPedido();

        MostrarIconosPedido(pedido);

        MostrarNotaVisual();

        MostrarPedidoTexto(pedido);
    }

    private void MostrarIconosPedido(List<int> pedido)
    {
        if (palomitas == null || palomitas.Length == 0)
            return;

        foreach (int ingrediente in pedido)
        {
            if (ingrediente >= 0 &&
                ingrediente < palomitas.Length &&
                palomitas[ingrediente] != null)
            {
                palomitas[ingrediente].SetActive(true);
            }
        }
    }

    private void MostrarNotaVisual()
    {
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

    public void MostrarPedidoTexto(List<int> pedido)
    {
        if (textoPedido == null)
        {
            Debug.Log("Pedido actual:\n" + ConstruirTextoPedido(pedido));
            return;
        }

        textoPedido.text = ConstruirTextoPedido(pedido);
    }

    private string ConstruirTextoPedido(List<int> pedido)
    {
        string texto = "Pedido:\n";

        for (int i = 0; i < pedido.Count; i++)
        {
            int id = pedido[i];

            texto += "- " + ObtenerNombreIngrediente(id) + "\n";
        }

        return texto;
    }

    private string ConstruirTextoPedidoConIDs(List<int> pedido)
    {
        string texto = "";

        for (int i = 0; i < pedido.Count; i++)
        {
            texto += pedido[i];

            if (i < pedido.Count - 1)
            {
                texto += ", ";
            }
        }

        return texto;
    }

    public string ObtenerNombreIngrediente(int id)
    {
        switch (id)
        {
            case 0:
                return "Pan abajo";

            case 1:
                return "Carne";

            case 2:
                return "Tocino";

            case 3:
                return "Queso";

            case 4:
                return "Lechuga";

            case 5:
                return "Tomate";

            case 6:
                return "Cebolla";

            case 7:
                return "Pepinillo";

            case 8:
                return "Aros de Cebolla";

            case 9:
                return "Pan arriba";

            case 10:
                return "Papas";

            default:
                return "Ingrediente ID " + id;
        }
    }

    public void OcultarPedido()
    {
        if (notaObj != null)
        {
            notaObj.SetActive(false);
        }

        OcultarIconosPedido();

        if (textoPedido != null)
        {
            textoPedido.text = "Pedido:\nSin pedido";
        }
    }

    private void OcultarIconosPedido()
    {
        if (palomitas == null)
            return;

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
            textoDinero.text = "$" + dineroActual;
        }
    }
}