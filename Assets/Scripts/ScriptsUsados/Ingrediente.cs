using UnityEngine;

public class Ingrediente : MonoBehaviour
{
    public enum TipoIngrediente
    {
        PanAbajo,
        PanArriba,
        Carne,
        Tocino,
        Queso,
        Lechuga,
        Tomate,
        Cebolla,
        Pepinillo,
        Papas,
        ArosCebolla
    }

    public enum EstadoCoccion
    {
        NoAplica,
        Crudo,
        Cocinado,
        Quemado
    }

    [Header("Datos del ingrediente")]
    public TipoIngrediente tipo;
    public EstadoCoccion estado = EstadoCoccion.NoAplica;

    [Header("Cocción")]
    public bool requiereCoccion;
    public float tiempoParaCocinar = 8f;
    public float tiempoParaQuemar = 14f;

    [Header("Modelos 3D por estado")]
    public GameObject modeloNormal;
    public GameObject modeloCrudo;
    public GameObject modeloCocinado;
    public GameObject modeloQuemado;

    [Header("IDs para el pedido / hamburguesa")]
    public int idNormal = -1;
    public int idCrudo = -1;
    public int idCocinado = -1;
    public int idQuemado = -1;

    [HideInInspector] public bool cocinandose = false;
    [HideInInspector] public float tiempoCoccion = 0f;
    [HideInInspector] public bool fueTomadoPorJugador = false;

    private GameObject modeloActual;

    private void Start()
    {
        // Resetear siempre al iniciar, sin importar lo que traiga el prefab
        cocinandose = false;
        tiempoCoccion = 0f;
        fueTomadoPorJugador = false;

        if (requiereCoccion)
            estado = EstadoCoccion.Crudo;
        else
            estado = EstadoCoccion.NoAplica;

        ActualizarModelo();
    }

    private void Update()
    {
        if (!cocinandose) return;

        tiempoCoccion += Time.deltaTime;
        RevisarEstadoCoccion();
    }

    private void RevisarEstadoCoccion()
    {
        if (!requiereCoccion) return;

        if (tiempoCoccion >= tiempoParaQuemar)
            CambiarEstado(EstadoCoccion.Quemado);
        else if (tiempoCoccion >= tiempoParaCocinar)
            CambiarEstado(EstadoCoccion.Cocinado);
        else
            CambiarEstado(EstadoCoccion.Crudo);
    }

    private void CambiarEstado(EstadoCoccion nuevoEstado)
    {
        if (estado == nuevoEstado) return;

        estado = nuevoEstado;
        ActualizarModelo();

        if (estado == EstadoCoccion.Cocinado)
            Debug.Log(name + " ya está cocinado.");
        else if (estado == EstadoCoccion.Quemado)
            Debug.Log(name + " se quemó.");
    }

    private GameObject ObtenerModeloSegunEstado()
    {
        switch (estado)
        {
            case EstadoCoccion.NoAplica: return modeloNormal;
            case EstadoCoccion.Crudo: return modeloCrudo;
            case EstadoCoccion.Cocinado: return modeloCocinado;
            case EstadoCoccion.Quemado: return modeloQuemado;
            default: return null;
        }
    }

    private void ActualizarModelo()
    {
        GameObject modeloNuevo = ObtenerModeloSegunEstado();
        if (modeloNuevo == null) return;

        if (modeloActual != null && modeloActual != modeloNuevo)
        {
            modeloNuevo.transform.position = modeloActual.transform.position;
            modeloNuevo.transform.rotation = modeloActual.transform.rotation;
        }

        if (modeloNormal != null) modeloNormal.SetActive(false);
        if (modeloCrudo != null) modeloCrudo.SetActive(false);
        if (modeloCocinado != null) modeloCocinado.SetActive(false);
        if (modeloQuemado != null) modeloQuemado.SetActive(false);

        modeloNuevo.SetActive(true);
        modeloActual = modeloNuevo;
    }

    public void IniciarCoccion()
    {
        if (!requiereCoccion) return;

        Debug.Log(name + " IniciarCoccion. fueTomadoPorJugador: " + fueTomadoPorJugador);

        if (!fueTomadoPorJugador) return;

        cocinandose = true;
    }

    public void DetenerCoccion()
    {
        cocinandose = false;
    }

    public void Reiniciar()
    {
        tiempoCoccion = 0f;
        cocinandose = false;
        fueTomadoPorJugador = false;

        if (requiereCoccion)
            estado = EstadoCoccion.Crudo;
        else
            estado = EstadoCoccion.NoAplica;

        ActualizarModelo();
    }

    public int ObtenerIDActual()
    {
        switch (estado)
        {
            case EstadoCoccion.NoAplica: return idNormal;
            case EstadoCoccion.Crudo: return idCrudo;
            case EstadoCoccion.Cocinado: return idCocinado;
            case EstadoCoccion.Quemado: return idQuemado;
            default: return -1;
        }
    }
}