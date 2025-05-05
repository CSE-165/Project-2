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
    public float fingerThreshold = 0.05f;

    [Header("Flying")]
    public float flySpeed = 3f;
    private bool isFlying = false;

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

        bool leftFistClench = IsFistClenched(leftHand);
        bool rightFistClench = IsFistClenched(rightHand);

        bool leftFistRelease = IsFistReleased(leftHand);
        bool rightFistRelease = IsFistReleased(rightHand);
        
        if (leftFistRelease || rightFistRelease)
        {
            isFlying = false;
            Debug.Log("Flying stopped!");
        }
        else if (leftFistClench && rightFistClench)
        {
            isFlying = true;
            Debug.Log("Flying started!");
        }
        

        if (isFlying)
        {
            FlyForward();
            GearShift(rightHand);
        }
    }

    bool IsFistReleased(XRHand hand)
    {
        for (int i = 0; i < tips.Length; i++)
        {
            var tip = hand.GetJoint(tips[i]);
            var root = hand.GetJoint(roots[i]);

            if (!tip.TryGetPose(out Pose tipPose) || !root.TryGetPose(out Pose rootPose))
                return false;

            float dist = Vector3.Distance(tipPose.position, rootPose.position);
            if (dist < fingerThreshold)
                return false;
        }

        return true;
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
            if (dist > fingerThreshold)
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

    // This method is called when the player is flying and the gear is shifted
    void GearShift(XRHand hand)
    {
        int gear = 0;

        if (IsFingerRaised(hand, XRHandJointID.IndexTip, XRHandJointID.IndexMetacarpal)) gear++;
        if (IsFingerRaised(hand, XRHandJointID.MiddleTip, XRHandJointID.MiddleMetacarpal)) gear++;
        if (IsFingerRaised(hand, XRHandJointID.RingTip, XRHandJointID.RingMetacarpal)) gear++;

        if (gear == 1)
        {
            flySpeed = 3f; // Slow speed
            Debug.Log("Gear 1: Slow speed");
        }
        else if (gear == 2)
        {
            flySpeed = 6f; // Medium speed
            Debug.Log("Gear 2: Medium speed");
        }
        else if (gear == 3)
        {
            flySpeed = 9f; // Fast speed
            Debug.Log("Gear 3: Fast speed");
        }
       
    }

    bool IsFingerRaised(XRHand hand, XRHandJointID tipId, XRHandJointID rootId)
    {
        var tip = hand.GetJoint(tipId);
        var root = hand.GetJoint(rootId);

        if (!tip.TryGetPose(out Pose tipPose) || !root.TryGetPose(out Pose rootPose))
            return false;

        float dist = Vector3.Distance(tipPose.position, rootPose.position);
        return dist > fingerThreshold;
    }
}
