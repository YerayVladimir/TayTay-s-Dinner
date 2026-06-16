using System.Collections.Generic;
using UnityEngine;

public class PuntoApilado : MonoBehaviour
{
    [Header("Punto donde inicia la hamburguesa")]
    public Transform puntoBase;

    [Header("Altura entre ingredientes")]
    public float alturaPorIngrediente = 0.08f;

    [Header("Rotación")]
    public bool usarRotacionFija = true;
    public Vector3 rotacionFija = Vector3.zero;

    [Header("Bloqueo")]
    public string layerDespuesDeApilar = "Default";
    public bool desactivarColliders = true;
    public bool mantenerBloqueado = true;

    private class IngredienteApilado
    {
        public GameObject objeto;
        public Transform slot;
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
    }

    private List<IngredienteApilado> ingredientesApilados =
        new List<IngredienteApilado>();

    public void AgregarIngrediente(GameObject ingrediente)
    {
        if (ingrediente == null)
            return;

        if (YaEstaApilado(ingrediente))
            return;

        if (puntoBase == null)
        {
            Debug.LogError("Falta asignar PuntoBaseHamburguesa en PuntoApilado.");
            return;
        }

        Vector3 escalaMundoOriginal =
            ingrediente.transform.lossyScale;

        BloquearFisica(ingrediente);
        BloquearGrabbableSiExiste(ingrediente);

        int indice =
            ingredientesApilados.Count;

        GameObject slotObj =
            new GameObject("Slot_" + ingrediente.name);

        Transform slot =
            slotObj.transform;

        slot.SetParent(transform, true);

        slot.position =
            puntoBase.position +
            Vector3.up * alturaPorIngrediente * indice;

        if (usarRotacionFija)
        {
            slot.rotation = Quaternion.Euler(rotacionFija);
        }
        else
        {
            slot.rotation = puntoBase.rotation;
        }

        ingrediente.transform.SetParent(slot, true);

        MantenerEscalaMundo(
            ingrediente.transform,
            escalaMundoOriginal
        );

        if (usarRotacionFija)
        {
            ingrediente.transform.rotation =
                Quaternion.Euler(rotacionFija);
        }

        CentrarVisualmenteEnSlot(
            ingrediente,
            slot
        );

        CambiarLayerRecursivo(
            ingrediente,
            layerDespuesDeApilar
        );

        if (desactivarColliders)
        {
            DesactivarCollidersRecursivo(ingrediente);
        }

        IngredienteApilado nuevo =
            new IngredienteApilado();

        nuevo.objeto = ingrediente;
        nuevo.slot = slot;
        nuevo.localPosition = ingrediente.transform.localPosition;
        nuevo.localRotation = ingrediente.transform.localRotation;
        nuevo.localScale = ingrediente.transform.localScale;

        ingredientesApilados.Add(nuevo);

        Debug.Log(
            "Ingrediente apilado en slot: " +
            ingrediente.name +
            " | Slot: " +
            slot.name
        );
    }

    private void LateUpdate()
    {
        if (!mantenerBloqueado)
            return;

        foreach (IngredienteApilado item in ingredientesApilados)
        {
            if (item == null ||
                item.objeto == null ||
                item.slot == null)
                continue;

            item.objeto.transform.localPosition =
                item.localPosition;

            item.objeto.transform.localRotation =
                item.localRotation;

            item.objeto.transform.localScale =
                item.localScale;
        }
    }

    private void BloquearFisica(GameObject objeto)
    {
        Rigidbody[] rigidbodies =
            objeto.GetComponentsInChildren<Rigidbody>(true);

        foreach (Rigidbody rb in rigidbodies)
        {
            if (rb == null)
                continue;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    private void BloquearGrabbableSiExiste(GameObject objeto)
    {
        ObjectGrabbable[] grabbables =
            objeto.GetComponentsInChildren<ObjectGrabbable>(true);

        foreach (ObjectGrabbable grabbable in grabbables)
        {
            if (grabbable == null)
                continue;

            grabbable.SoltarForzado();
            grabbable.BloquearObjeto();
        }
    }

    private void CentrarVisualmenteEnSlot(
        GameObject objeto,
        Transform slot)
    {
        Physics.SyncTransforms();

        Bounds bounds =
            ObtenerBoundsActivos(objeto);

        Vector3 puntoInferiorCentral =
            new Vector3(
                bounds.center.x,
                bounds.min.y,
                bounds.center.z
            );

        Vector3 movimiento =
            slot.position - puntoInferiorCentral;

        objeto.transform.position += movimiento;

        Physics.SyncTransforms();
    }

    private Bounds ObtenerBoundsActivos(GameObject objeto)
    {
        Renderer[] renderers =
            objeto.GetComponentsInChildren<Renderer>(false);

        bool tieneBounds = false;

        Bounds bounds =
            new Bounds(
                objeto.transform.position,
                Vector3.one * 0.1f
            );

        foreach (Renderer renderer in renderers)
        {
            if (renderer == null)
                continue;

            if (!renderer.enabled)
                continue;

            if (!renderer.gameObject.activeInHierarchy)
                continue;

            if (!tieneBounds)
            {
                bounds = renderer.bounds;
                tieneBounds = true;
            }
            else
            {
                bounds.Encapsulate(renderer.bounds);
            }
        }

        return bounds;
    }

    private void CambiarLayerRecursivo(
        GameObject objeto,
        string nombreLayer)
    {
        int layer =
            LayerMask.NameToLayer(nombreLayer);

        if (layer == -1)
        {
            Debug.LogWarning(
                "No existe la layer: " +
                nombreLayer
            );
            return;
        }

        objeto.layer = layer;

        foreach (Transform hijo in objeto.transform)
        {
            CambiarLayerRecursivo(
                hijo.gameObject,
                nombreLayer
            );
        }
    }

    private void DesactivarCollidersRecursivo(GameObject objeto)
    {
        Collider[] colliders =
            objeto.GetComponentsInChildren<Collider>(true);

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    private bool YaEstaApilado(GameObject ingrediente)
    {
        foreach (IngredienteApilado item in ingredientesApilados)
        {
            if (item != null &&
                item.objeto == ingrediente)
            {
                return true;
            }
        }

        return false;
    }

    private void MantenerEscalaMundo(
        Transform objeto,
        Vector3 escalaMundoDeseada)
    {
        if (objeto.parent == null)
        {
            objeto.localScale = escalaMundoDeseada;
            return;
        }

        Vector3 escalaPadre =
            objeto.parent.lossyScale;

        float x =
            escalaPadre.x != 0 ?
            escalaMundoDeseada.x / escalaPadre.x :
            escalaMundoDeseada.x;

        float y =
            escalaPadre.y != 0 ?
            escalaMundoDeseada.y / escalaPadre.y :
            escalaMundoDeseada.y;

        float z =
            escalaPadre.z != 0 ?
            escalaMundoDeseada.z / escalaPadre.z :
            escalaMundoDeseada.z;

        objeto.localScale =
            new Vector3(x, y, z);
    }
}