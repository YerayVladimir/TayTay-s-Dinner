using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controla pedidos, dinero y cocción de ingredientes.
/// Los clientes y mesas ahora son controlados por otros scripts.
/// </summary>
public class Eventos : MonoBehaviour
{
    [Header("Elementos visuales del pedido")]
    public GameObject notaObj;
    public GameObject[] palomitas;
    public GameObject[] tapas;

    [Header("Botones")]
    public GameObject botonPreparar;
    public GameObject botonDinero;

    [Header("Hamburguesa")]
    public HamburguesaActual hamburguesaActual;

    [Header("Ingredientes posibles")]
    public List<int> ingredientesDisponibles =
        new List<int>();

    [Header("Capas visuales")]
    public GameObject capa11;
    public GameObject capa2;
    public GameObject capa12;

    public GameObject capa4;
    public GameObject capa6;

    public GameObject capa13;
    public GameObject capa14;

    public GameObject capa17;
    public GameObject capa18;
    public GameObject capa19;

    public GameObject capa20;
    public GameObject capa21;
    public GameObject capa22;

    [Header("Tiempos")]
    public float tiempo = 0f;
    public float tiempoTocino = 0f;
    public float tiempoPapas = 0f;
    public float tiempoAros = 0f;

    [Header("Contadores")]
    public bool contando = false;
    public bool contandoTocino = false;
    public bool contandoPapas = false;
    public bool contandoAros = false;

    [Header("Dinero")]
    public int dineroActual = 0;
    public TextMeshProUGUI textoDinero;

    public enum EstadoCarne
    {
        cruda,
        cocinada,
        quemada
    }

    public enum EstadoTocino
    {
        crudo,
        cocinado,
        quemado
    }

    public enum EstadoPapas
    {
        cruda,
        cocinada,
        quemada
    }

    public enum EstadoAros
    {
        cruda,
        cocinada,
        quemada
    }

    public EstadoCarne estadoActual =
        EstadoCarne.cruda;

    public EstadoTocino estadoTocino =
        EstadoTocino.crudo;

    public EstadoPapas estadoPapas =
        EstadoPapas.cruda;

    public EstadoAros estadoAros =
        EstadoAros.cruda;

    public Animator nota;

    private void Update()
    {
        if (contando)
            tiempo += Time.deltaTime;

        if (contandoTocino)
            tiempoTocino += Time.deltaTime;

        if (contandoPapas)
            tiempoPapas += Time.deltaTime;

        if (contandoAros)
            tiempoAros += Time.deltaTime;
    }

    //=================================================
    // PEDIDOS
    //=================================================

    public List<int> GenerarPedido()
    {
        List<int> pedido =
            new List<int>();

        pedido.Add(0);

        int cantidad =
            Random.Range(2, 5);

        for (int i = 0; i < cantidad; i++)
        {
            int ingrediente =
                ingredientesDisponibles[
                    Random.Range(
                        0,
                        ingredientesDisponibles.Count)];

            if (ingrediente == 0 ||
                ingrediente == 9 ||
                pedido.Contains(ingrediente))
            {
                i--;
                continue;
            }

            pedido.Add(ingrediente);
        }

        pedido.Add(9);

        return pedido;
    }

    public void MostrarPedido(
        List<int> pedido)
    {
        foreach (GameObject p in palomitas)
        {
            p.SetActive(false);
        }

        foreach (int ingrediente in pedido)
        {
            if (ingrediente >= 0 &&
                ingrediente < palomitas.Length)
            {
                palomitas[
                    ingrediente]
                    .SetActive(true);
            }
        }

        notaObj.SetActive(true);

        if (nota != null)
        {
            nota.Play(
                "AnimNotaEntrada");
        }
    }

    //=================================================
    // DINERO
    //=================================================

    public void AgregarDinero(
        int cantidad)
    {
        dineroActual += cantidad;

        ActualizarDinero();
    }

    public void QuitarDinero(
        int cantidad)
    {
        dineroActual -= cantidad;

        if (dineroActual < 0)
            dineroActual = 0;

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

    //=================================================
    // CARNE
    //=================================================

    public void iniciar()
    {
        tiempo = 0f;
        contando = true;

        estadoActual =
            EstadoCarne.cruda;
    }

    public void detener()
    {
        contando = false;

        actualizarEstado();
    }

    void actualizarEstado()
    {
        if (tiempo < 6f)
        {
            estadoActual =
                EstadoCarne.cruda;

            if (capa11 != null)
                capa11.SetActive(true);

            hamburguesaActual
                .AgregarIngrediente(10);
        }
        else if (tiempo < 10f)
        {
            estadoActual =
                EstadoCarne.cocinada;

            if (capa2 != null)
                capa2.SetActive(true);

            hamburguesaActual
                .AgregarIngrediente(1);
        }
        else
        {
            estadoActual =
                EstadoCarne.quemada;

            if (capa12 != null)
                capa12.SetActive(true);

            hamburguesaActual
                .AgregarIngrediente(11);
        }
    }

    //=================================================
    // TOCINO
    //=================================================

    public void iniciarTocino()
    {
        tiempoTocino = 0f;
        contandoTocino = true;

        estadoTocino =
            EstadoTocino.crudo;
    }

    public void detenerTocino()
    {
        contandoTocino = false;

        actualizarEstadoTocino();
    }

    void actualizarEstadoTocino()
    {
        if (tiempoTocino < 5f)
        {
            estadoTocino =
                EstadoTocino.crudo;

            hamburguesaActual
                .AgregarIngrediente(12);
        }
        else if (tiempoTocino < 8f)
        {
            estadoTocino =
                EstadoTocino.cocinado;

            hamburguesaActual
                .AgregarIngrediente(7);
        }
        else
        {
            estadoTocino =
                EstadoTocino.quemado;

            hamburguesaActual
                .AgregarIngrediente(13);
        }
    }

    //=================================================
    // PAPAS
    //=================================================

    public void iniciarPapas()
    {
        tiempoPapas = 0f;
        contandoPapas = true;

        estadoPapas =
            EstadoPapas.cruda;
    }

    public void detenerPapas()
    {
        contandoPapas = false;

        actualizarEstadoPapas();
    }

    void actualizarEstadoPapas()
    {
        if (tiempoPapas < 6f)
        {
            estadoPapas =
                EstadoPapas.cruda;
        }
        else if (tiempoPapas < 10f)
        {
            estadoPapas =
                EstadoPapas.cocinada;

            hamburguesaActual
                .AgregarIngrediente(8);
        }
        else
        {
            estadoPapas =
                EstadoPapas.quemada;
        }
    }

    //=================================================
    // AROS
    //=================================================

    public void iniciarAros()
    {
        tiempoAros = 0f;
        contandoAros = true;

        estadoAros =
            EstadoAros.cruda;
    }

    public void detenerAros()
    {
        contandoAros = false;

        actualizarEstadoAros();
    }

    void actualizarEstadoAros()
    {
        if (tiempoAros < 6f)
        {
            estadoAros =
                EstadoAros.cruda;
        }
        else if (tiempoAros < 10f)
        {
            estadoAros =
                EstadoAros.cocinada;

            hamburguesaActual
                .AgregarIngrediente(14);
        }
        else
        {
            estadoAros =
                EstadoAros.quemada;

            hamburguesaActual
                .AgregarIngrediente(15);
        }
    }
}