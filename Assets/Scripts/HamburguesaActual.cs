using System.Collections.Generic;
using UnityEngine;

public class HamburguesaActual : MonoBehaviour
{
    [Header("Ingredientes de la hamburguesa")]
    public List<int> ingredientes = new List<int>();

    [Header("Configuración")]
    public bool hamburguesaTerminada;

    public void AgregarIngrediente(int id)
    {
        ingredientes.Add(id);

        Debug.Log(
            "Ingrediente agregado: " +
            id);

        RevisarHamburguesaTerminada();
    }

    public bool ContieneIngrediente(int id)
    {
        return ingredientes.Contains(id);
    }

    public bool TienePanInferior()
    {
        if (ingredientes.Count == 0)
            return false;

        return ingredientes[0] == 0;
    }

    void RevisarHamburguesaTerminada()
    {
        if (ingredientes.Count < 2)
        {
            hamburguesaTerminada = false;
            return;
        }

        hamburguesaTerminada =
            ingredientes[
                ingredientes.Count - 1] == 9;
    }

    public bool HamburguesaTerminada()
    {
        return hamburguesaTerminada;
    }

    public void LimpiarHamburguesa()
    {
        ingredientes.Clear();

        hamburguesaTerminada = false;

        Debug.Log(
            "Hamburguesa limpiada");
    }

    public string MostrarIngredientes()
    {
        return string.Join(
            ", ",
            ingredientes);
    }

    public int ObtenerCantidadIngredientes()
    {
        return ingredientes.Count;
    }
}