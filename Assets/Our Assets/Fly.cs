using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

public class fly : MonoBehaviour
{
    [Header("References")]
    private XRHandSubsystem handSubsystem;
    public Transform playerRig; 
    private XRHand leftHand => handSubsystem.leftHand;
    private XRHand rightHand => handSubsystem.rightHand;

    private XRHandJointID[] tips = {
        XRHandJointID.IndexTip,
        XRHandJointID.MiddleTip,
        XRHandJointID.RingTip,
        XRHandJointID.LittleTip
    };

    private XRHandJointID[] roots = {
        XRHandJointID.IndexMetacarpal,
        XRHandJointID.MiddleMetacarpal,
        XRHandJointID.RingMetacarpal,
        XRHandJointID.LittleMetacarpal
    };

    [Header("Fist Detection")]
    public float fistThreshold = 0.15f;

    [Header("Flying")]
    public float flySpeed = 3f;
    private int gear = 0;

    void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetInstances(subsystems);
         if (subsystems.Count > 0)
        {
            handSubsystem = subsystems[0];
        }
        else
        {
            Debug.LogError("XRHandSubsystem not found! Make sure XR Hands is properly set up.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (handSubsystem == null || !leftHand.isTracked || !rightHand.isTracked)
            return;

        bool leftFist = IsFistClenched(leftHand);
        bool rightFist = IsFistClenched(rightHand);

        if (leftFist && rightFist)
        {
            
            FlyForward();
        }
    }

    bool IsFistClenched(XRHand hand)
    {
        for (int i = 0; i < tips.Length; i++)
        {
            var tip = hand.GetJoint(tips[i]);
            var root = hand.GetJoint(roots[i]);

            if (!tip.TryGetPose(out Pose tipPose) || !root.TryGetPose(out Pose rootPose))
                return false;

            float dist = Vector3.Distance(tipPose.position, rootPose.position);
            if (dist > fistThreshold)
                return false;
        }

        return true;
    }

    void FlyForward()
    {
        if (playerRig == null) return;

        Vector3 forward = Camera.main.transform.forward;
        
        playerRig.position += forward.normalized * flySpeed * Time.deltaTime;
    }

    void GearShift()
    {
        if (gear == 0)
        {
            gear = 1;
            flySpeed = 5f;
        }
        else if (gear == 1)
        {
            gear = 2;
            flySpeed = 10f;
        }
        else if (gear == 2)
        {
            gear = 0;
            flySpeed = 3f;
        }
    }
}
