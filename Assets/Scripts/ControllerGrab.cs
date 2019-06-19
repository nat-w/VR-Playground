using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrab : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;

    private GameObject collidingObj;
    private GameObject objInHand;

    private void SetCollidingObj(Collider col)
    {
        if (col || !col.GetComponent<Rigidbody>()) {
            return;
        }

        collidingObj = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObj(other);
    }

    public void OnTriggerStay(Collider other)
    {
       SetCollidingObj(other);
    }

    public void OnTriggerExit(Collider other)
    {
        // check if null already
        if (!collidingObj)
        {
            return;
        }
        collidingObj = null;
    }

    private void GrabObject()
    {
        objInHand = collidingObj;
        collidingObj = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fix = gameObject.AddComponent<FixedJoint>();
        fix.breakForce = 20000;
        fix.breakTorque = 20000;
        return fix;
    }

    private void ReleaseObject()
    {
        var fix = GetComponent<FixedJoint>();

        if (fix)
        {
            fix.connectedBody = null;
            Destroy(fix);

            objInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }
        objInHand = null;
    }


    // Update is called once per frame
    void Update()
    {
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObj)
            {
                GrabObject();
            }
        }

        if (grabAction.GetLastStateUp(handType))
        {
            if (objInHand)
            {
                ReleaseObject();
            }
        }
    }
}
