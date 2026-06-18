using System.Collections.Generic;
using UnityEngine;

public class ZonaArmado : MonoBehaviour
{
    [Header("Referencias")]
    public HamburguesaActual hamburguesa;
    public PuntoApilado apilado;

    [Header("Reglas de armado")]
    public bool exigirOrdenBase = true;

    private bool tienePanAbajo = false;
    private bool tieneCarne = false;
    private bool hamburguesaCerrada = false;

    private HashSet<Ingrediente> ingredientesAgregados =
        new HashSet<Ingrediente>();

    private void Awake()
    {
        if (hamburguesa == null)
            hamburguesa = GetComponentInParent<HamburguesaActual>();

        if (apilado == null)
            apilado = GetComponentInParent<PuntoApilado>();
    }

    private void Start()
    {
        tienePanAbajo = false;
        tieneCarne = false;
        hamburguesaCerrada = false;
        ingredientesAgregados.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        IntentarAgregarIngrediente(other);
    }

    private void OnTriggerStay(Collider other)
    {
        IntentarAgregarIngrediente(other);
    }

    private void IntentarAgregarIngrediente(Collider other)
    {
        if (Input.GetMouseButton(0))
            return;

        Ingrediente ingrediente = other.GetComponentInParent<Ingrediente>();

        if (ingrediente == null)
            return;

        // No agregar si el jugador nunca lo agarró
        if (!ingrediente.fueTomadoPorJugador)
            return;

        if (ingredientesAgregados.Contains(ingrediente))
            return;

        if (hamburguesa == null || apilado == null)
            return;

        int id = ingrediente.ObtenerIDActual();

        if (id == -1)
        {
            Debug.LogWarning(
                ingrediente.name +
                " no tiene ID válido. Revisa idNormal, idCrudo, idCocinado o idQuemado."
            );
            return;
        }

        if (!PuedeAgregarse(ingrediente))
            return;

        GameObject objetoParaApilar =
            ObtenerObjetoFisicoQueEntro(other, ingrediente);

        ingredientesAgregados.Add(ingrediente);

        ingrediente.DetenerCoccion();

        hamburguesa.AgregarIngrediente(id);

        ActualizarEstadoHamburguesa(ingrediente);

        apilado.AgregarIngrediente(objetoParaApilar);

        Debug.Log(
            "Ingrediente agregado: " +
            ingrediente.name +
            " | ID: " +
            id +
            " | Objeto apilado: " +
            objetoParaApilar.name
        );
    }

    private GameObject ObtenerObjetoFisicoQueEntro(
        Collider other,
        Ingrediente ingrediente)
    {
        if (ingrediente.GetComponent<Rigidbody>() != null ||
            ingrediente.GetComponent<ObjectGrabbable>() != null)
        {
            return ingrediente.gameObject;
        }

        if (other.attachedRigidbody != null)
        {
            HamburguesaActual hamburguesaPadre =
                other.attachedRigidbody.GetComponentInParent<HamburguesaActual>();

            if (hamburguesaPadre == null)
                return other.attachedRigidbody.gameObject;
        }

        ObjectGrabbable grabbable =
            other.GetComponentInParent<ObjectGrabbable>();

        if (grabbable != null &&
            grabbable.GetComponentInParent<HamburguesaActual>() == null)
        {
            return grabbable.gameObject;
        }

        return ingrediente.gameObject;
    }

    private bool PuedeAgregarse(Ingrediente ingrediente)
    {
        if (hamburguesaCerrada)
        {
            Debug.LogWarning("La hamburguesa ya está cerrada.");
            return false;
        }

        if (!exigirOrdenBase)
            return true;

        if (!tienePanAbajo)
        {
            if (ingrediente.tipo != Ingrediente.TipoIngrediente.PanAbajo)
            {
                Debug.LogWarning("Primero debes poner pan de abajo.");
                return false;
            }

            return true;
        }

        if (!tieneCarne)
        {
            if (ingrediente.tipo != Ingrediente.TipoIngrediente.Carne)
            {
                Debug.LogWarning("Después del pan de abajo debes poner carne.");
                return false;
            }

            return true;
        }

        return true;
    }

    private void ActualizarEstadoHamburguesa(Ingrediente ingrediente)
    {
        if (ingrediente.tipo == Ingrediente.TipoIngrediente.PanAbajo)
            tienePanAbajo = true;

        if (ingrediente.tipo == Ingrediente.TipoIngrediente.Carne)
            tieneCarne = true;

        if (ingrediente.tipo == Ingrediente.TipoIngrediente.PanArriba)
            hamburguesaCerrada = true;
    }
}