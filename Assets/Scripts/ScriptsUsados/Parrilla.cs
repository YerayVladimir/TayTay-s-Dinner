using System.Collections.Generic;
using UnityEngine;

public class Parrilla : MonoBehaviour
{
    private Dictionary<Ingrediente, int> ingredientesDentro =
        new Dictionary<Ingrediente, int>();

    private void OnTriggerEnter(Collider other)
    {
        Ingrediente ingrediente =
            other.GetComponentInParent<Ingrediente>();

        if (ingrediente == null)
        {
            Debug.Log(
                other.name +
                " entró a la parrilla, pero no tiene Ingrediente.cs.");

            return;
        }

        Debug.Log(
            ingrediente.name +
            " entró a la parrilla. Collider detectado: " +
            other.name);

        if (!ingrediente.requiereCoccion)
        {
            Debug.Log(
                ingrediente.name +
                " no requiere cocción.");

            return;
        }

        if (ingrediente.tipo != Ingrediente.TipoIngrediente.Carne &&
            ingrediente.tipo != Ingrediente.TipoIngrediente.Tocino)
        {
            Debug.Log(
                ingrediente.name +
                " no puede cocinarse en la parrilla. Tipo: " +
                ingrediente.tipo);

            return;
        }

        if (!ingredientesDentro.ContainsKey(ingrediente))
        {
            ingredientesDentro.Add(ingrediente, 0);
        }

        ingredientesDentro[ingrediente]++;

        ingrediente.IniciarCoccion();

        Debug.Log(
            ingrediente.name +
            " se colocó en la parrilla y comenzó a cocinarse.");
    }

    private void OnTriggerExit(Collider other)
    {
        Ingrediente ingrediente =
            other.GetComponentInParent<Ingrediente>();

        if (ingrediente == null)
            return;

        if (!ingredientesDentro.ContainsKey(ingrediente))
            return;

        ingredientesDentro[ingrediente]--;

        Debug.Log(
            ingrediente.name +
            " detectó salida de collider: " +
            other.name +
            ". Colliders restantes dentro: " +
            ingredientesDentro[ingrediente]);

        if (ingredientesDentro[ingrediente] > 0)
            return;

        ingredientesDentro.Remove(ingrediente);

        ingrediente.DetenerCoccion();

        Debug.Log(
            ingrediente.name +
            " se retiró de la parrilla. Estado actual: " +
            ingrediente.estado);
    }
}