using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRHandGlobe : MonoBehaviour
{
    public Transform objectHolder;
    public GameObject nearbyObject;
    public GameObject grabbedObject;
    public InputActionReference IAGrip;
    public InputActionReference IADoubleRotation;
    private Vector3 controllerPos;
    private Quaternion controllerRot;
    private bool isNearAGrabbable = false;

    void Start()
    {
        // Grip
        IAGrip.action.Enable();
        IAGrip.action.started += (ctx) =>
        {
            if (nearbyObject && isNearAGrabbable)
            {
                grabbedObject = nearbyObject;
                grabbedObject.GetComponent<GrabbableObject>().EnableGrab(transform, transform.gameObject.tag);
            }
        };
        IAGrip.action.canceled += (ctx) =>
        {
            if (grabbedObject && isNearAGrabbable)
            {
                grabbedObject.GetComponent<GrabbableObject>().DisableGrab(transform, transform.gameObject.tag);
                grabbedObject = null;
            }
        };
        // Trigger
        IADoubleRotation.action.Enable();
    }

    void Update()
    {
        if (grabbedObject)
        {
            if (IAGrip.action.IsPressed())
            {
                Quaternion rotationVec = transform.rotation * Quaternion.Inverse(controllerRot);
                if (IADoubleRotation.action.IsPressed())
                    rotationVec *= rotationVec;
                grabbedObject.transform.rotation = rotationVec * grabbedObject.transform.rotation;

                Vector3 initVec = grabbedObject.transform.position - transform.position;
                Vector3 turnedVec = rotationVec * initVec;
                Vector3 attachVec = turnedVec - initVec;
                Vector3 dPos = transform.position - controllerPos;
                grabbedObject.transform.position += dPos + attachVec;
            }
        }
        controllerPos = transform.position;
        controllerRot = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (grabbedObject == null)
        {
            if (other.CompareTag("Grabbable"))
            {
                isNearAGrabbable = true;
                nearbyObject = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (grabbedObject == null)
        {
            if (other.CompareTag("Grabbable"))
            {
                isNearAGrabbable = false;
                nearbyObject = null;
            }
        }
    }
}
