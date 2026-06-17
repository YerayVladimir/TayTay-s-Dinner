using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Eventos : MonoBehaviour
{
    [Header("Elementos visuales del pedido")]
    public GameObject notaObj;
    public GameObject[] palomitas;

    [Header("UI de varios pedidos")]
    public PedidoUIManager pedidoUIManager;

    [Header("Texto simple del pedido")]
    public TextMeshProUGUI textoPedido;

    [Header("Animacion de la nota")]
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

    [Header("Mensaje de entrega")]
    public TextMeshProUGUI textoMensajeEntrega;
    public float tiempoMensajeEntrega = 2f;

    private Coroutine corrutinaMensajeEntrega;

    private void Start()
{
    OcultarPedido();
    OcultarMensajeEntregaInicial();
    ActualizarDinero();
}

    //=================================================
    // PEDIDOS
    //=================================================

    public List<int> GenerarPedido()
    {
        List<int> pedido = new List<int>();

        pedido.Add(idPanAbajo);
        pedido.Add(idCarneObligatoria);

        List<int> ingredientesValidos = ObtenerIngredientesValidosParaPedido();

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

        pedido.Add(idPanArriba);

        pedidoActual = new List<int>(pedido);

        Debug.Log("PEDIDO GENERADO IDS: " + ConvertirListaATexto(pedido));
        Debug.Log("PEDIDO GENERADO TEXTO:\n" + ConstruirTextoPedido(pedido));

        return pedido;
    }

    private List<int> ObtenerIngredientesValidosParaPedido()
    {
        List<int> ingredientesValidos = new List<int>();

        foreach (int ingrediente in ingredientesDisponibles)
        {
            if (!EsIngredienteExtraValido(ingrediente))
            {
                Debug.LogWarning(
                    "ID ignorado en Ingredientes Disponibles porque no es valido: " +
                    ingrediente
                );

                continue;
            }

            if (ingredientesValidos.Contains(ingrediente))
                continue;

            ingredientesValidos.Add(ingrediente);
        }

        if (ingredientesValidos.Count == 0)
        {
            Debug.LogWarning(
                "No hay ingredientes extra validos. Se usaran valores por defecto."
            );

            ingredientesValidos.Add(2);
            ingredientesValidos.Add(3);
            ingredientesValidos.Add(4);
            ingredientesValidos.Add(5);
            ingredientesValidos.Add(6);
            ingredientesValidos.Add(7);
            ingredientesValidos.Add(8);
            ingredientesValidos.Add(10);
        }

        return ingredientesValidos;
    }

    private bool EsIngredienteExtraValido(int id)
    {
        if (id == idPanAbajo)
            return false;

        if (id == idCarneObligatoria)
            return false;

        if (id == idPanArriba)
            return false;

        if (id == 2)
            return true;

        if (id == 3)
            return true;

        if (id == 4)
            return true;

        if (id == 5)
            return true;

        if (id == 6)
            return true;

        if (id == 7)
            return true;

        if (id == 8)
            return true;

        if (id == 10)
            return true;

        return false;
    }

    private void LimpiarIngredientesDisponiblesInvalidos()
    {
        List<int> listaLimpia = new List<int>();

        foreach (int ingrediente in ingredientesDisponibles)
        {
            if (!EsIngredienteExtraValido(ingrediente))
                continue;

            if (listaLimpia.Contains(ingrediente))
                continue;

            listaLimpia.Add(ingrediente);
        }

        ingredientesDisponibles = listaLimpia;
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

    public bool PuedeCrearOtroPedido()
    {
        if (pedidoUIManager == null)
            return true;

        return pedidoUIManager.PuedeCrearPedido();
    }

    public void MostrarPedidoDeCliente(Cliente cliente, List<int> pedido)
    {
        if (cliente == null)
        {
            Debug.LogError("No se puede mostrar pedido porque el cliente es null.");
            return;
        }

        if (pedido == null)
        {
            Debug.LogError("No se puede mostrar pedido porque la lista de pedido es null.");
            return;
        }

        if (pedidoUIManager != null)
        {
            pedidoUIManager.CrearPedidoUI(cliente, pedido, this);
        }
        else
        {
            Debug.LogWarning("No hay PedidoUIManager asignado. Se usara la UI vieja.");
            MostrarPedido(pedido);
        }
    }

    public void QuitarPedidoDeCliente(Cliente cliente)
    {
        if (cliente == null)
            return;

        if (pedidoUIManager != null)
        {
            pedidoUIManager.QuitarPedidoUI(cliente);
        }
        else
        {
            LimpiarPedidoActual();
        }
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

    public string ConvertirListaATexto(List<int> lista)
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
            textoPedido.text = "";
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

        Debug.Log("DINERO AGREGADO: $" + cantidad);
        Debug.Log("DINERO ACTUAL: $" + dineroActual);

        ActualizarDinero();
    }

    public void QuitarDinero(int cantidad)
    {
        dineroActual -= cantidad;

        if (dineroActual < 0)
        {
            dineroActual = 0;
        }

        Debug.Log("DINERO QUITADO: $" + cantidad);
        Debug.Log("DINERO ACTUAL: $" + dineroActual);

        ActualizarDinero();
    }

    public void ActualizarDinero()
    {
        if (textoDinero != null)
        {
            textoDinero.text = "$" + dineroActual;
        }
        else
        {
            Debug.LogWarning("No esta asignado Texto Dinero en GameManagerEventos.");
        }
    }

    //=================================================
    // MENSAJE DE ENTREGA
    //=================================================

    private void OcultarMensajeEntregaInicial()
    {
        if (textoMensajeEntrega != null)
        {
            textoMensajeEntrega.text = "";
            textoMensajeEntrega.gameObject.SetActive(false);
        }
    }

    public void MostrarMensajeEntrega(string mensaje, bool correcto)
    {
        if (textoMensajeEntrega == null)
        {
            Debug.LogWarning("No esta asignado Texto Mensaje Entrega en GameManagerEventos.");
            return;
        }

        textoMensajeEntrega.gameObject.SetActive(true);
        textoMensajeEntrega.text = mensaje;

        if (correcto)
        {
            textoMensajeEntrega.color = Color.green;
        }
        else
        {
            textoMensajeEntrega.color = Color.red;
        }

        if (corrutinaMensajeEntrega != null)
        {
            StopCoroutine(corrutinaMensajeEntrega);
        }

        corrutinaMensajeEntrega =
            StartCoroutine(OcultarMensajeEntregaDespues());
    }

    private IEnumerator OcultarMensajeEntregaDespues()
    {
        yield return new WaitForSeconds(tiempoMensajeEntrega);

        if (textoMensajeEntrega != null)
        {
            textoMensajeEntrega.text = "";
            textoMensajeEntrega.gameObject.SetActive(false);
        }

        corrutinaMensajeEntrega = null;
    }
}