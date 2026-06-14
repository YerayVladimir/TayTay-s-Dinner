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

    public TipoIngrediente tipo;

    public EstadoCoccion estado = EstadoCoccion.NoAplica;

    public bool requiereCoccion;

    [HideInInspector]
    public bool cocinandose = false;

    [HideInInspector]
    public float tiempoCoccion = 0f;

    public float tiempoParaCocinar = 8f;

    public float tiempoParaQuemar = 14f;

    private void Update()
    {
        if (!cocinandose)
            return;

        tiempoCoccion += Time.deltaTime;

        if (tiempoCoccion >= tiempoParaQuemar)
        {
            estado = EstadoCoccion.Quemado;
        }
        else if (tiempoCoccion >= tiempoParaCocinar)
        {
            estado = EstadoCoccion.Cocinado;
        }
        else
        {
            estado = EstadoCoccion.Crudo;
        }
    }

    public void Reiniciar()
    {
        tiempoCoccion = 0;
        cocinandose = false;

        if (requiereCoccion)
            estado = EstadoCoccion.Crudo;
    }
}