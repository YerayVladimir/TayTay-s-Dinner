using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Eventos : MonoBehaviour
{
    private int seleccion;
    public GameObject Nota;
    public GameObject[] Palomitas;
    public GameObject[] Personajes;
    public GameObject[] Tapas;
    public GameObject botonPreparar;
    public GameObject botonEntregar;
    public GameObject botonDinero;
    public List<GameObject> personajesDisponibles;
    public List<GameObject> personajesEnEscena = new List<GameObject>();
    public List<int> ingredientesDisponibles = new List<int>();
    public List<int> pedidoCliente = new List<int>();
    public int cantidadIngredientes;
    private GameObject personajeSeleccionado;
    public GameObject ConfirmacionCompra;
    public GameObject RechazoCompra;
    public GameObject PantallaPrincipal;
    private Animator animatorSeleccionado;
    public Animator nota;
    public GameObject clienteActual;
    public HamburguesaActual hamburguesaActual;
    public GameObject capa11, capa2, capa12, capa4,capa6, capa13, capa14, capa17, capa18, capa19, capa20,capa21,capa22,pantallaGanada;
    public float tiempo = 0f, tiempoTocino = 0f, tiempoAros = 0f, tiempoPedido = 0f, tiempoPapas = 0f;
    public bool contando = false, contandoTocino = false,contandoPapas = false, contandoAros = false, contandoCliente = false;
    public int dineroInicial = 35, dineroActual = 0;
    public TextMeshProUGUI textoDinero;
    //audios

    public enum EstadoCarne { Cruda, Cocinada, Quemada }
    public EstadoCarne estadoActual = EstadoCarne.Cruda;
    public enum EstadoPapas { Cruda, Cocinada, Quemada }
    public EstadoPapas estadoPapas = EstadoPapas.Cruda;
    public enum EstadoAros { Cruda, Cocinada, Quemada }
    public EstadoAros estadoAros = EstadoAros.Cruda;
    public enum EstadoTocino { Crudo, Cocinado, Quemado }
    public EstadoTocino estado = EstadoTocino.Crudo;

    void Start()
    {
        
        SeleccionarPersonajeRandom();
        SeleccionPedidoRandom();
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

    void SeleccionPedidoRandom()
    {
        cantidadIngredientes = Random.Range(1, ingredientesDisponibles.Count);
        pedidoCliente.Clear();
        
        int panAbajoID = 0;
        Palomitas[0].SetActive(true);
        pedidoCliente.Add(panAbajoID);

        // Determinar cantidad de ingredientes en medio (mínimo 1, máximo ingredientesDisponibles - 2)
        int ingredientesMedio = Random.Range(1, ingredientesDisponibles.Count - 1);

        for (int i = 0; i < ingredientesMedio; i++)
        {
            int idIngrediente = ingredientesDisponibles[Random.Range(0, ingredientesDisponibles.Count)];

            // Evitar ingredientes repetidos y los panes
            if (pedidoCliente.Contains(idIngrediente) || idIngrediente == 0 || idIngrediente == 9)
            {
                i--; // Repetir intento
                continue;
            }

            pedidoCliente.Add(idIngrediente);
            Palomitas[idIngrediente].SetActive(true);
        }

        // Agregar pan de arriba (INGREDIENTE OBLIGATORIO)
        int panArribaID = 9;
        pedidoCliente.Add(panArribaID);

        Debug.Log("Pedido cliente generado: " + string.Join(", ", pedidoCliente));
    }


    void SeleccionarPersonajeRandom()
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
        animatorSeleccionado = personajeSeleccionado.GetComponent<Animator>();
    }

    public void Iniciar()
    {
        tiempo = 0f; contando = true; estadoActual = EstadoCarne.Cruda;
    }

    public void IniciarTocino()
    {
        tiempoTocino = 0f; contandoTocino = true; estado = EstadoTocino.Crudo;
    }
    public void IniciarPapas()
    {
        tiempoPapas = 0f; contandoPapas = true; estadoPapas = EstadoPapas.Cruda;
    }
    public void IniciarAros()
    {
        tiempoAros = 0f; contandoAros = true; estadoAros = EstadoAros.Cruda;
    }

    public void IniciarPedido()
    {
        tiempoPedido = 0f; contandoCliente = true;
    }

    public void Detener()
    {
        contando = false;
        ActualizarEstado();
    }

    public void DetenerPedido()
    {
        contandoCliente = false;
        Debug.Log("Tiempo total del pedido: " + tiempoPedido);
    }

    public void DetenerTocino()
    {
        contandoTocino = false;
        ActualizarEstadoTocino();
    }
    public void DetenerAros()
    {
        contandoAros = false;
        ActualizarEstadoAros();
    }
    public void DetenerPapas()
    {
        contandoPapas = false;
        ActualizarEstadoPapas();
    }

    void CalcularPropina()
    {
        if (tiempoPedido <= 10f) dineroInicial = 35;
        else if (tiempoPedido <= 15f) dineroInicial = 30;
        else if (tiempoPedido <= 20f) dineroInicial = 25;
        else if (tiempoPedido <= 25f) dineroInicial = 20;
        else if (tiempoPedido <= 30f) dineroInicial = 15;
        else if (tiempoPedido <= 35f) dineroInicial = 10;
        else if (tiempoPedido <= 40f) dineroInicial = 5;
        else dineroInicial = 0;

        Debug.Log("Propina: " + dineroInicial);
        dineroActual += dineroInicial;
    }

    void ActualizarEstado()
    {
        if (tiempo < 6f) { estadoActual = EstadoCarne.Cruda; capa11.SetActive(true); hamburguesaActual.AgregarIngrediente(10); }
        else if (tiempo < 10f) { estadoActual = EstadoCarne.Cocinada; capa2.SetActive(true); hamburguesaActual.AgregarIngrediente(1); }
        else { estadoActual = EstadoCarne.Quemada; capa12.SetActive(true); hamburguesaActual.AgregarIngrediente(11); }
         
        Debug.Log("Estado final de la carne: " + estadoActual);
    }
   

    void ActualizarEstadoTocino()
    {
        if (tiempoTocino < 6f) 
        { 
            estado = EstadoTocino.Crudo; 
            capa13.SetActive(true); 
            hamburguesaActual.AgregarIngrediente(12); 
        }
        else if (tiempoTocino < 10f) 
        { 
            estado = EstadoTocino.Cocinado; 
            capa4.SetActive(true); 
            hamburguesaActual.AgregarIngrediente(3); 
        }
        else
        { 
            estado = EstadoTocino.Quemado; 
            capa14.SetActive(true); 
            hamburguesaActual.AgregarIngrediente(13); 
        }

        Debug.Log("Estado final del tocino: " + estado);
    }
    void ActualizarEstadoPapas()
    {
        if (tiempoPapas < 6f) 
        { 
            estadoPapas = EstadoPapas.Cruda; 
            capa19.SetActive(true); 
            hamburguesaActual.AgregarIngrediente(18); 
        }
        else if (tiempoPapas < 10f) 
        { 
            estadoPapas = EstadoPapas.Cocinada; 
            capa17.SetActive(true); 
            hamburguesaActual.AgregarIngrediente(16);
        }
        else
        { 
            estadoPapas = EstadoPapas.Quemada; 
            capa20.SetActive(true); 
            hamburguesaActual.AgregarIngrediente(19); 
        }

        Debug.Log("Estado final de las papas: " + estadoPapas);
    }
    void ActualizarEstadoAros()
    {
        if (tiempoAros < 6f) 
        { 
            estadoAros = EstadoAros.Cruda; 
            capa21.SetActive(true); 
            hamburguesaActual.AgregarIngrediente(20); 
        }
        else if (tiempoAros < 10f) 
        { 
            estadoAros = EstadoAros.Cocinada; 
            capa6.SetActive(true); 
            hamburguesaActual.AgregarIngrediente(5); 
        }
        else
        { 
            estadoAros = EstadoAros.Quemada; 
            capa22.SetActive(true); 
            hamburguesaActual.AgregarIngrediente(21); 
        }

        Debug.Log("Estado final de los aros: " + estadoAros);
    }

    IEnumerator entradaPersonaje()
    {
        yield return new WaitForSeconds(1f);
        animatorSeleccionado.Play("Anim_entrada");
        Nota.SetActive(true);
        nota.Play("AnimNotaEntrada");
        yield return new WaitForSeconds(2f);
        botonPreparar.SetActive(true);
    }

    IEnumerator SalidaPersonaje()
    {
        yield return new WaitForSeconds(1f);
        animatorSeleccionado.Play("Anim_salida");
        yield return new WaitForSeconds(1.5f);
        animatorSeleccionado.Play("AnimEsperaPersonaje");
        yield return new WaitForSeconds(1.5f);
        personajeSeleccionado.SetActive(false);
        SeleccionarPersonajeRandom();
        SeleccionPedidoRandom();
        StartCoroutine(entradaPersonaje());
    }

    IEnumerator SalidaPartida()
    {
        yield return new WaitForSeconds(1f);
        animatorSeleccionado.Play("Anim_salida");
        yield return new WaitForSeconds(1.5f);
        personajeSeleccionado.SetActive(false);
        pantallaGanada.SetActive(true);
        PantallaPrincipal.SetActive(false);
    }

    IEnumerator btnEntregarActivar()
    {
        yield return new WaitForSeconds(2f);
        botonEntregar.SetActive(true);
    }
    IEnumerator BtnJugos()
    {
        yield return new WaitForSeconds(2f);
        capa18.SetActive(true);
        hamburguesaActual.AgregarIngrediente(17);
    }
    //
    public AudioSource audioSource;          // Arrastra tu AudioSource
    public AudioClip audioCelebracion;       // Arrastra sonido de celebración
    public AudioClip audioEnojo;             // Arrastra sonido de enojo

    IEnumerator PlayAnimaCelebracion()
    {
        yield return new WaitForSeconds(1.5f);

        animatorSeleccionado.Play("AnimCelebracionPersonaje");

        // ▶ Audio de celebración
        audioSource.PlayOneShot(audioCelebracion);

        yield return new WaitForSeconds(0.5f);
        botonDinero.SetActive(true);
    }

    IEnumerator PlayAnimaEnojo()
    {
        yield return new WaitForSeconds(1.5f);

        animatorSeleccionado.Play("AnimEnojoPersonajes");

        // ▶ Audio de enojo
        audioSource.PlayOneShot(audioEnojo);

        yield return new WaitForSeconds(0.5f);
        botonDinero.SetActive(true);
    }

    IEnumerator BannerConfirmación()
    {
        ConfirmacionCompra.SetActive(true);
        yield return new WaitForSeconds(2f);
        ConfirmacionCompra.SetActive(false);
    }
    IEnumerator BannerRechazo()
    {
        RechazoCompra.SetActive(true);
        yield return new WaitForSeconds(2f);
        RechazoCompra.SetActive(false);
    }

    public void BtnEntregar()
    {
        StartCoroutine(btnEntregarActivar());
    }
    public void BtnJugo()
    {
        StartCoroutine(BtnJugos());
    }
    public void BtnTienda(int ID)
    {
        if(dineroActual>=50 && !ingredientesDisponibles.Contains(ID))
        {
            dineroActual-=50;
            ingredientesDisponibles.Add(ID);
            personajesDisponibles.Add(Personajes[ID]);
            Tapas[ID].SetActive(false);
            textoDinero.text = "$" + dineroActual.ToString();
            StartCoroutine(BannerConfirmación());
        }
        else
        {
            StartCoroutine(BannerRechazo());
        }
    }
    public void BtnSiguiente()
    {
        personajesEnEscena.Clear();
        textoDinero.text = "$" + dineroActual.ToString();
        SeleccionarPersonajeRandom();
        SeleccionPedidoRandom();
        StartCoroutine(entradaPersonaje());
    }
    public bool CompararHamburguesaIgnorandoOrden(List<int> pedidoCliente, List<int> ingredientesJugador)
    {
        if (pedidoCliente == null || pedidoCliente.Count != ingredientesJugador.Count) return false;

        var pedidoDict = pedidoCliente.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        var jugadorDict = ingredientesJugador.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

        return pedidoDict.Count == jugadorDict.Count && !pedidoDict.Any(kvp => !jugadorDict.ContainsKey(kvp.Key) || jugadorDict[kvp.Key] != kvp.Value);
    }

    public void BotonEntregar()
    {
        bool correcta = CompararHamburguesaIgnorandoOrden(pedidoCliente, hamburguesaActual.ingredientes);
        int dineroIngredientes = hamburguesaActual.ingredientes.Count;

        if (hamburguesaActual.ingredientes.Count == 0)
        {
            dineroInicial = 0;
            Debug.Log("Hamburguesa VACÍA. Propina cancelada.");
        }
        else
        {
            CalcularPropina();
        }

        dineroActual += dineroIngredientes;

        if (correcta) StartCoroutine(PlayAnimaCelebracion());
        else
        {
            dineroActual -= 5;
            StartCoroutine(PlayAnimaEnojo());
        }
    }

    public void btnDinero()
    {
        textoDinero.text = "$" + dineroActual.ToString();

        if (personajesDisponibles.Count == personajesEnEscena.Count)
        {
            StartCoroutine(SalidaPartida());
        }
        else
            StartCoroutine(SalidaPersonaje());

        tiempo = 0f; tiempoTocino = 0f; tiempoPedido = 0f;
        contando = false; contandoTocino = false; contandoCliente = false;
        dineroInicial = 35;
    }
}
