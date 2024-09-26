using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Oculus.Interaction;
using Meta;
using Oculus.Interaction.HandGrab;

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public LayerMask sliceableLayer;
    public VelocityEstimator velocityEstimator;

    public Material crossSectionMaterial;
    public float cutForce = 4000;

    private bool isSlicingEnabled = false;

    public GameObject grabbablePrefab;


    void OnEnable()
    {
        if (velocityEstimator != null)
        {
            velocityEstimator.BeginEstimatingVelocity();
        }
        isSlicingEnabled = true;
    }

    void OnDisable()
    {
        if (velocityEstimator != null)
        {
            velocityEstimator.FinishEstimatingVelocity();
        }
        isSlicingEnabled = false;
    }

    void FixedUpdate()
    {
        if (!isSlicingEnabled)
            return;

        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);

        if (hasHit)
        {

            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {

        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            SetupSlicedComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedComponent(lowerHull);

            Destroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider meshCollider = slicedObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;

        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
        slicedObject.layer = LayerMask.NameToLayer("Sliceable");

        BoxCollider boxCollider = slicedObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true; 
     
        List<Collider> colliderList = new List<Collider> { meshCollider };

        GameObject childObject = Instantiate(grabbablePrefab, slicedObject.transform);
        childObject.transform.localPosition = Vector3.zero;
        childObject.transform.localRotation = Quaternion.identity;

        Grabbable grabbable = childObject.GetComponent<Grabbable>();
        grabbable.InjectOptionalTargetTransform(slicedObject.transform);
        TouchHandGrabInteractable touchHandGrabInteractable = childObject.GetComponent<TouchHandGrabInteractable>();

        
        if (grabbable != null)
        {
            grabbable.InjectOptionalRigidbody(rb);
        }

        if (touchHandGrabInteractable != null)
        {
            touchHandGrabInteractable.InjectAllTouchHandGrabInteractable(boxCollider, colliderList);
        }

        // META YOURE GREAT MY BESTIE ACTUALLY HJIAGFJKLHGSDHJKFGJKLNBFGSDHJKLÖFGB
    }
}
