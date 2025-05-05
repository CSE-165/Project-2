using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

public class Turn : MonoBehaviour
{
    [Header("References")]
    public Transform playerRig; // Reference to the player rig (camera and player object)

    private XRHandSubsystem handSubsystem; // Reference to the XR hand subsystem
    private XRHand leftHand => handSubsystem.leftHand; // Reference to the left hand
    private XRHand rightHand => handSubsystem.rightHand; // Reference to the right hand

    private XRHandJointID thumb = XRHandJointID.ThumbTip; // Reference to the thumb joint
    private XRHandJointID thumbroot = XRHandJointID.ThumbMetacarpal; // Reference to the thumb root joint

    // Start is called before the first frame update
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
        bool leftThumbPointing = IsThumbPointing(leftHand); // Check if the left thumb is pointing
        bool rightThumbPointing = IsThumbPointing(rightHand); // Check if the right thumb is pointing

        if(leftThumbPointing)
        {
            // Rotate the player rig to the left
            playerRig.Rotate(Vector3.up, -1f); // Adjust the rotation speed as needed
        }
        else if(rightThumbPointing)
        {
            // Rotate the player rig to the right
            playerRig.Rotate(Vector3.up, 1f); // Adjust the rotation speed as needed
        }
    }

    private bool IsThumbPointing(XRHand hand)
    {
        // Check if the thumb is pointing by comparing the position of the thumb tip and root
        var thumbTip = hand.GetJoint(thumb);
        var thumbRoot = hand.GetJoint(thumbroot);

        if (!thumbTip.TryGetPose(out Pose tipPose) || !thumbRoot.TryGetPose(out Pose rootPose))
                return false;
        // Calculate the distance between the thumb tip and root
        float distance = Vector3.Distance(tipPose.position, rootPose.position);

        // Check if the distance is below a certain threshold (indicating a pointing gesture)
        return distance < 0.05f; // Adjust this threshold as needed
    }
}
