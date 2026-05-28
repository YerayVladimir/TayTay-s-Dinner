using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Eventos : MonoBehaviour
{
    private int seleccion;

    public GameObject notaObj;
    public GameObject[] palomitas;
    public GameObject[] personajes;
    public GameObject[] tapas;

    public GameObject botonPreparar;
    public GameObject botonEntregar;
    public GameObject botonDinero;

    public List<GameObject> personajesDisponibles;
    public List<GameObject> personajesEnEscena = new List<GameObject>();

    public List<int> ingredientesDisponibles = new List<int>();
    public List<int> pedidoCliente = new List<int>();

    public int cantidadIngredientes;

    private GameObject personajeSeleccionado;
    public GameObject confirmacionCompra;
    public GameObject rechazoCompra;
    public GameObject pantallaPrincipal;

    private Animator animatorSeleccionado;
    public Animator nota;

    public GameObject clienteActual;
    public HamburguesaActual hamburguesaActual;

    public GameObject capa11, capa2, capa12, capa4, capa6,
                      capa13, capa14, capa17, capa18, capa19,
                      capa20, capa21, capa22, pantallaGanada;

    public float tiempo = 0f;
    public float tiempoTocino = 0f;
    public float tiempoAros = 0f;
    public float tiempoPedido = 0f;
    public float tiempoPapas = 0f;

    public bool contando = false;
    public bool contandoTocino = false;
    public bool contandoPapas = false;
    public bool contandoAros = false;
    public bool contandoCliente = false;

    public int dineroInicial = 35;
    public int dineroActual = 0;

    public TextMeshProUGUI textoDinero;

    // ENUMS
    public enum EstadoCarne { cruda, cocinada, quemada }
    public EstadoCarne estadoActual = EstadoCarne.cruda;

    public enum EstadoPapas { cruda, cocinada, quemada }
    public EstadoPapas estadoPapas = EstadoPapas.cruda;

    public enum EstadoAros { cruda, cocinada, quemada }
    public EstadoAros estadoAros = EstadoAros.cruda;

    public enum EstadoTocino { crudo, cocinado, quemado }
    public EstadoTocino estadoTocino = EstadoTocino.crudo;

    void Start()
    {
        seleccionarPersonajeRandom();
        seleccionPedidoRandom();
        StartCoroutine(entradaPersonaje());
    }

    void Update()
    {
        if (contando) tiempo += Time.deltaTime;
        if (contandoTocino) tiempoTocino += Time.deltaTime;
        if (contandoPapas) tiempoPapas += Time.deltaTime;
        if (contandoAros) tiempoAros += Time.deltaTime;
        if (contandoCliente) tiempoPedido += Time.deltaTime;
    }

    void seleccionPedidoRandom()
    {
        cantidadIngredientes = Random.Range(1, ingredientesDisponibles.Count);
        pedidoCliente.Clear();

        int panAbajoId = 0;
        palomitas[0].SetActive(true);
        pedidoCliente.Add(panAbajoId);

        int ingredientesMedio = Random.Range(1, ingredientesDisponibles.Count - 1);

        for (int i = 0; i < ingredientesMedio; i++)
        {
            int idIngrediente =
                ingredientesDisponibles[Random.Range(0, ingredientesDisponibles.Count)];

            if (pedidoCliente.Contains(idIngrediente) ||
                idIngrediente == 0 ||
                idIngrediente == 9)
            {
                i--;
                continue;
            }

            pedidoCliente.Add(idIngrediente);
            palomitas[idIngrediente].SetActive(true);
        }

        int panArribaId = 9;
        pedidoCliente.Add(panArribaId);

        Debug.Log("Pedido cliente generado: " +
            string.Join(", ", pedidoCliente));
    }

    void seleccionarPersonajeRandom()
    {
        seleccion = Random.Range(0, personajesDisponibles.Count);
        personajeSeleccionado = personajesDisponibles[seleccion];
        clienteActual = personajeSeleccionado;

        while (personajesEnEscena.Contains(personajeSeleccionado))
        {
            seleccion = Random.Range(0, personajesDisponibles.Count);
            personajeSeleccionado = personajesDisponibles[seleccion];
        }

        personajesEnEscena.Add(personajeSeleccionado);
        personajeSeleccionado.SetActive(true);

        animatorSeleccionado =
            personajeSeleccionado.GetComponent<Animator>();
    }

    public void iniciar()
    {
        tiempo = 0f;
        contando = true;
        estadoActual = EstadoCarne.cruda;
    }

    public void iniciarTocino()
    {
        tiempoTocino = 0f;
        contandoTocino = true;
        estadoTocino = EstadoTocino.crudo;
    }

    public void iniciarPapas()
    {
        tiempoPapas = 0f;
        contandoPapas = true;
        estadoPapas = EstadoPapas.cruda;
    }

    public void iniciarAros()
    {
        tiempoAros = 0f;
        contandoAros = true;
        estadoAros = EstadoAros.cruda;
    }

    public void iniciarPedido()
    {
        tiempoPedido = 0f;
        contandoCliente = true;
    }

    public void detener()
    {
        contando = false;
        actualizarEstado();
    }

    public void detenerPedido()
    {
        contandoCliente = false;
        Debug.Log("Tiempo total del pedido: " + tiempoPedido);
    }

    public void detenerTocino()
    {
        contandoTocino = false;
        actualizarEstadoTocino();
    }

    public void detenerAros()
    {
        contandoAros = false;
        actualizarEstadoAros();
    }

    public void detenerPapas()
    {
        contandoPapas = false;
        actualizarEstadoPapas();
    }

    void calcularPropina()
    {
        if (tiempoPedido <= 10f) dineroInicial = 35;
        else if (tiempoPedido <= 15f) dineroInicial = 30;
        else if (tiempoPedido <= 20f) dineroInicial = 25;
        else if (tiempoPedido <= 25f) dineroInicial = 20;
        else if (tiempoPedido <= 30f) dineroInicial = 15;
        else if (tiempoPedido <= 35f) dineroInicial = 10;
        else if (tiempoPedido <= 40f) dineroInicial = 5;
        else dineroInicial = 0;

        dineroActual += dineroInicial;
    }

    void actualizarEstado()
    {
        if (tiempo < 6f)
        {
            estadoActual = EstadoCarne.cruda;
            capa11.SetActive(true);
            hamburguesaActual.agregarIngrediente(10);
        }
        else if (tiempo < 10f)
        {
            estadoActual = EstadoCarne.cocinada;
            capa2.SetActive(true);
            hamburguesaActual.agregarIngrediente(1);
        }
        else
        {
            estadoActual = EstadoCarne.quemada;
            capa12.SetActive(true);
            hamburguesaActual.agregarIngrediente(11);
        }
    }

    IEnumerator entradaPersonaje()
    {
        yield return new WaitForSeconds(1f);

        animatorSeleccionado.Play("Anim_entrada");

        notaObj.SetActive(true);
        nota.Play("AnimNotaEntrada");

        yield return new WaitForSeconds(2f);
        botonPreparar.SetActive(true);
    }

    public void botonEntregar()
    {
        bool correcta =
            compararHamburguesaIgnorandoOrden(
                pedidoCliente,
                hamburguesaActual.ingredientes);

        int dineroIngredientes =
            hamburguesaActual.ingredientes.Count;

        if (hamburguesaActual.ingredientes.Count == 0)
            dineroInicial = 0;
        else
            calcularPropina();

        dineroActual += dineroIngredientes;

        if (correcta)
            StartCoroutine(playAnimaCelebracion());
        else
        {
            dineroActual -= 5;
            StartCoroutine(playAnimaEnojo());
        }
    }

    public bool compararHamburguesaIgnorandoOrden(
        List<int> pedidoCliente,
        List<int> ingredientesJugador)
    {
        if (pedidoCliente == null ||
            pedidoCliente.Count != ingredientesJugador.Count)
            return false;

        var pedidoDict =
            pedidoCliente.GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        var jugadorDict =
            ingredientesJugador.GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        return pedidoDict.Count == jugadorDict.Count &&
               !pedidoDict.Any(kvp =>
                   !jugadorDict.ContainsKey(kvp.Key) ||
                   jugadorDict[kvp.Key] != kvp.Value);
    }

    public void btnDinero()
    {
        textoDinero.text = "$" + dineroActual.ToString();
    }
}