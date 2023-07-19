using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GrabbableObject : MonoBehaviour
{
    public List<Transform> attachPoints;
    public Transform leftGrabber;
    public Transform rightGrabber;
    public List<Transform> grabbers;
    private bool isGrabbedByLeft = false;
    private bool isGrabbedByRight = false;
    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        isGrabbedByLeft = false;
        isGrabbedByRight = false;
    }

    void Update()
    {
        // HINT! transform.SetLocalPositionAndRotation(transform.position, transform.rotation);
        /*
        Vector3 translationVec = new Vector3();
        Quaternion rotationVec = new Quaternion();


        if (isGrabbedByLeft || isGrabbedByRight)
        {
            
            if (isGrabbedByLeft && isGrabbedByRight)
            {

            }
            if (isGrabbedByLeft)
            {
                translationVec = leftGrabber.position;
                rotationVec = leftGrabber.rotation;
            }
            if (isGrabbedByRight)
            {
                translationVec = rightGrabber.position;
                rotationVec = rightGrabber.rotation;
            }
            transform.SetLocalPositionAndRotation(translationVec, rotationVec);
        }
        */
    }

    public void EnableGrab(Transform grabber, string hand)
    {
        if (hand.Equals("LHand"))
        {
            leftGrabber = grabber;
            isGrabbedByLeft = true;
            // grabbers.Add(leftGrabber);
        }
        else
        {
            rightGrabber = grabber;
            isGrabbedByRight = true;
            // grabbers.Add(rightGrabber);
        }
        rb.isKinematic = true;
        rb.useGravity= false;
    }

    public void DisableGrab(Transform grabber, string hand)
    {
        if (hand.Equals("LHand"))
        {
            leftGrabber = null;
            isGrabbedByLeft = false;
            // grabbers.Remove(leftGrabber);
        }
        else
        {
            rightGrabber = null;
            isGrabbedByRight = false;
            // grabbers.Remove(rightGrabber);
        }
        if (!IsGrabbed())
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    public bool IsGrabbed()
    {
        return isGrabbedByLeft || isGrabbedByRight;
    }
}
