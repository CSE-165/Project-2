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
    public float fingerThreshold = 0.06f;

    [Header("Flying")]
    public float flySpeed = 50f;
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
        playerRig.rotation *= Quaternion.Euler(0f, 180f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (handSubsystem == null || !leftHand.isTracked || !rightHand.isTracked)
            return;

        bool leftFistRelease = IsFistReleased(leftHand);
        bool rightFistRelease = IsFistReleased(rightHand);

        bool leftFistClench = IsFistClenched(leftHand);
        bool rightFistClench = IsFistClenched(rightHand);

        if (leftFistRelease && rightFistRelease)
        {
            isFlying = false;
            
        }
        else if (leftFistClench && rightFistClench)
        {
            isFlying = true;
            
        }
        

        if (isFlying)
        {
            FlyForward();
            Velocity();
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
        if (!rightHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose rightPose))
            return; // Right hand not tracked properly

        Vector3 forward = rightPose.forward;
        //forward.y *= -1; // Invert the vertical (Y) direction
        

        //Vector3 flyDirection = -rightPose.up; // Palm is pointing forward (like Superman)
        playerRig.position += forward * flySpeed * Time.deltaTime;
    }

    // This method is called when the player is flying and the gear is shifted
    void Velocity()
    {
        if (!leftHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose leftPose) || !rightHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose rightPose))
        {
            return; // Hands not tracked properly
        }

        float handDistance = Vector3.Distance(leftPose.position, rightPose.position);

        // Clamp and scale the distance to a reasonable speed range
        float minDistance = 0.1f;  // Hands very close
        float maxDistance = 0.5f;  // Arms outstretched
        float minSpeed = 30f;
        float maxSpeed = 600f;

        float t = Mathf.InverseLerp(minDistance, maxDistance, handDistance);
        flySpeed = Mathf.Lerp(minSpeed, maxSpeed, t);
       
    }


    void DebugFingerDistances(XRHand hand)
    {
        for (int i = 0; i < tips.Length; i++)
        {
            var tip = hand.GetJoint(tips[i]);
            var root = hand.GetJoint(roots[i]);

            if (tip.TryGetPose(out Pose tipPose) && root.TryGetPose(out Pose rootPose))
            {
                float dist = Vector3.Distance(tipPose.position, rootPose.position);
                Debug.Log($"{hand.handedness} Finger {tips[i]} distance: {dist}");
            }
        }
    }
}

