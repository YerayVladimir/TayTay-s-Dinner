using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform grabPointTransform;
    private float lerpSpeed = 10f;
    void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }
    public void Grab(Transform grabPointTransform)
    {
        this.grabPointTransform = grabPointTransform;  
        objectRigidbody.useGravity = false;
        objectRigidbody.isKinematic = true;
    }
    public void Drop()
    {
        this.grabPointTransform = null;  
        objectRigidbody.useGravity = true;
        objectRigidbody.isKinematic = false;
    }
    
    private void FixedUpdate()
    {
        if(grabPointTransform != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, grabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidbody.MovePosition(newPosition);
        }
    }
}
