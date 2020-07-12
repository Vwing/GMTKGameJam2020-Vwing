using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRPhysicsHand : MonoBehaviour
{
    public OVRCustomSkeleton skeletonToCopy;
    public Transform[] theirJoints = new Transform[0];
    public Rigidbody[] ourJoints = new Rigidbody[0];
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Assert(theirJoints.Length == ourJoints.Length, "Our joints should be the same size as their joints!");
        Debug.Log(skeletonToCopy.Bones.Count + " " + skeletonToCopy.CustomBones.Count);
        //for (int i = 0; i < ourJoints.Length; ++i)
        //    ourJoints[i].transform.SetParent(rb.transform);
    }

    private void FixedUpdate()
    {
        rb.velocity = (skeletonToCopy.transform.position - transform.position) / Time.deltaTime;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = (skeletonToCopy.transform.rotation);
        for (int i = 0; i < theirJoints.Length; ++i)
        {
            ourJoints[i].velocity = (theirJoints[i].transform.position - ourJoints[i].transform.position) / Time.deltaTime;
            ourJoints[i].angularVelocity = Vector3.zero;
            ourJoints[i].MoveRotation(theirJoints[i].rotation);
            ourJoints[i].rotation = (theirJoints[i].rotation);
        }
    }
}
