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
    public float cutForce = 500;

    private bool isSlicingEnabled = false;


    List<Collider> collist;

    
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
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        MeshRenderer MeshRen = slicedObject.AddComponent<MeshRenderer>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
        slicedObject.layer = LayerMask.NameToLayer("Sliceable");

        BoxCollider colliderbox = slicedObject.AddComponent<BoxCollider>();
        colliderbox.isTrigger = true;

       slicedObject.AddComponent<Grabbable>().InjectOptionalRigidbody(rb);

      // slicedObject.AddComponent<TouchHandGrabInteractable>();

        //collist.Add(collider);
        //slicedObject.GetComponent<TouchHandGrabInteractable>().PointableElement();

        //slicedObject.AddComponent<Grabbable>()

        //slicedObject.GetComponent<PhysicsGrabbable>().InjectAllPhysicsGrabbable(IPointable pointable, rb);

        //collist.Add(collider);


        //slicedObject.GetComponent<PhysicsGrabbable>().InjectPointable(IPointable pointable);

        //slicedObject.GetComponent<PhysicsGrabbable>().

    }
}
