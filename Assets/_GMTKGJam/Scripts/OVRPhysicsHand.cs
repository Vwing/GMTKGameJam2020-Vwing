using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OVRPhysicsHand : MonoBehaviour
{
    public OVRCustomSkeleton skeletonToCopy;
    public Transform[] theirJoints = new Transform[0];
    public Rigidbody[] ourJoints = new Rigidbody[0];
    private Quaternion[] lastRotations = new Quaternion[0];
    private Quaternion[] lastLocalRotations = new Quaternion[0];
    private Quaternion rbLastRotation = Quaternion.identity;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastRotations = theirJoints.Select(s => s.rotation).ToArray();
        lastLocalRotations = theirJoints.Select(s => s.localRotation).ToArray();
        rbLastRotation = skeletonToCopy.transform.rotation;
        Debug.Assert(theirJoints.Length == ourJoints.Length, "Our joints should be the same size as their joints!");
        Debug.Log(skeletonToCopy.Bones.Count + " " + skeletonToCopy.CustomBones.Count);
        //for (int i = 0; i < ourJoints.Length; ++i)
        //    ourJoints[i].transform.SetParent(rb.transform);
    }


    private void FixedUpdate()
    {
        //FollowHandAttached();
        FollowHandDetached();
        SetLastRotations();
    }
    private void SetLastRotations()
    {
        for(int i = 0; i < theirJoints.Length; ++i)
        {
            lastRotations[i] = theirJoints[i].rotation;
            lastLocalRotations[i] = theirJoints[i].localRotation;
        }
        rbLastRotation = skeletonToCopy.transform.rotation;
    }
    private void FollowHandDetached()
    {
        for (int i = 0; i < theirJoints.Length; ++i)
        {
            ourJoints[i].velocity = (ourJoints[i].transform.parent.TransformPoint(theirJoints[i].transform.localPosition) - ourJoints[i].transform.position) / Time.deltaTime;
            ourJoints[i].angularVelocity = ourJoints[i].transform.parent.TransformVector(GetEulerDelta(lastLocalRotations[i], theirJoints[i].transform.localRotation)) * Mathf.Deg2Rad / Time.deltaTime;
            ourJoints[i].transform.localRotation = (theirJoints[i].localRotation);
        }
    }
    private void FollowHandAttached()
    {
        rb.velocity = (skeletonToCopy.transform.position - transform.position) / Time.deltaTime;
        rb.angularVelocity = GetEulerDelta(rbLastRotation, skeletonToCopy.transform.rotation) / Time.deltaTime;
        //rb.angularVelocity = Vector3.zero;
        rb.transform.rotation = (skeletonToCopy.transform.rotation);
        for (int i = 0; i < theirJoints.Length; ++i)
        {
            ourJoints[i].velocity = (theirJoints[i].transform.position - ourJoints[i].transform.position) / Time.deltaTime;
            ourJoints[i].angularVelocity = GetEulerDelta(lastRotations[i], theirJoints[i].transform.rotation) * Mathf.Deg2Rad / Time.deltaTime;
            //ourJoints[i].angularVelocity = Vector3.zero;
            //ourJoints[i].MoveRotation(theirJoints[i].rotation);
            ourJoints[i].transform.rotation = (theirJoints[i].rotation);
        }
    }
    public static Vector3 GetEulerDelta(Quaternion fromRot, Quaternion toRot)
    {
        float angle;
        Vector3 axis;
        Quaternion rot = Quaternion.Inverse(fromRot) * toRot;
        rot.ToAngleAxis(out angle, out axis);
        return axis * angle;
    }
    public static Vector3 GetEulerDelta2(Quaternion fromRot, Quaternion toRot)
    {
        Vector3 rot = (Quaternion.Inverse(fromRot) * toRot).eulerAngles;
        return new Vector3(Mathf.DeltaAngle(0, rot.x), Mathf.DeltaAngle(0, rot.y), Mathf.DeltaAngle(0, rot.z));
    }
}