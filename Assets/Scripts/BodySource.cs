using System;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Cinemachine;
using Joint = Windows.Kinect.Joint;
using Vector4 = UnityEngine.Vector4;

public class BodySource : MonoBehaviour
{
    public GameObject bodySourceManager;
    public float jointSize;
    public GameObject LeftHandObject;
    public GameObject PlayerObject;
    public float rotateSensitivityX;
    public float rotateSensitivityY;

    private BodySourceManager _bodyManager;
    private Dictionary<ulong, GameObject> _bodies = new Dictionary<ulong, GameObject>();
    private Vector3 _oldLeftHandPosition;
    private Vector3 _oldRightHandPosition;

    private void FixedUpdate()
    {

        if (bodySourceManager == null)
            return;
        
        _bodyManager = bodySourceManager.GetComponent<BodySourceManager>();
        if (_bodyManager == null)
            return;
        
        Body[] data = _bodyManager.GetBodies();
        if (data == null)
            return;

        // 正在追蹤的骨架
        List<ulong> trackedIds = new List<ulong>();
        foreach (Body body in data)
            if (body != null && body.IsTracked)
                trackedIds.Add(body.TrackingId);
        
        // 從已知的骨架ID中，對比正在追蹤的骨架ID
        List<ulong> knowIds = new List<ulong>(_bodies.Keys);
        foreach (ulong trackingId in knowIds)
        {
            // 若已知的骨架ID不存在於追蹤中的骨架ID
            if (!trackedIds.Contains(trackingId))
            {
                // 則移除該已知的骨架ID
                Destroy(_bodies[trackingId]);
                _bodies.Remove(trackingId);
            }
        }

        if (data.Length > 0)
        {
            Body body = data[0];
            if (body != null && body.IsTracked)
            {
                Boolean isFirstCreate = false;
                if (!_bodies.ContainsKey(body.TrackingId))
                {
                    _bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    isFirstCreate = true;
                }
                RefreshBodyObject(body, _bodies[body.TrackingId], isFirstCreate);
            }
        }
        /*
        foreach (Body body in data)
        {
            if (body != null && body.IsTracked)
            {
                Boolean isFirstCreate = false;
                if (!_bodies.ContainsKey(body.TrackingId))
                {
                    _bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                    isFirstCreate = true;
                }
                RefreshBodyObject(body, _bodies[body.TrackingId], isFirstCreate);
            }
        }
        */
    }

    private GameObject CreateBodyObject(ulong trackingId)
    {
        GameObject bodyObject = new GameObject("Body:" + trackingId);
        for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
        {
            GameObject jointObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            jointObject.transform.localScale = Vector3.one * 0.3f;
            jointObject.name = jt.ToString();
            jointObject.transform.parent = bodyObject.transform;
        }

        return bodyObject;
    }

    private void RefreshBodyObject(Body body, GameObject bodyObject, Boolean flag)
    {
        for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
        {
            Transform jointObject = bodyObject.transform.Find(jt.ToString());
            jointObject.localPosition = GetVector3FromJoint(body.Joints[jt]);

            //if (body.Joints[jt].TrackingState == TrackingState.Tracked)
            //    Debug.Log(jt.ToString() + " is tracked.");
            //else
            //    Debug.Log(jt.ToString() + " is not tracked.");
            
            if (jt == JointType.HandLeft)
            {
                //if (body.HandLeftState == HandState.Closed)
                //    Debug.Log(jt.ToString() + " left hand is closed.");
                if (flag)
                {
                    _oldLeftHandPosition = jointObject.position;
                }
                else
                {
                    Vector3 difference = _oldLeftHandPosition - jointObject.position;
                    if (body.HandLeftState == HandState.Closed)
                    {
                        //LeftHandObject.transform.Rotate(Vector3.down * difference.x * rotateSensitivityY, Space.World);
                        //LeftHandObject.transform.Rotate(Vector3.right * difference.y * rotateSensitivityX, Space.Self);
                        /*
                        Vector3 lookAtPos = PlayerObject.transform.position + Vector3.up * 1.5f;
                        LeftHandObject.transform.RotateAround(lookAtPos, Vector3.up,
                            difference.x * rotateSensitivityY);
                        LeftHandObject.transform.RotateAround(lookAtPos, Vector3.forward,
                            difference.y * rotateSensitivityX);
                        LeftHandObject.transform.LookAt(lookAtPos,Vector3.up);
                        */
                        PlayerObject.transform.Rotate(Vector3.up * difference.x * rotateSensitivityY);
                        LeftHandObject.transform.Rotate(Vector3.right * difference.y * rotateSensitivityX);
                    }
                    _oldLeftHandPosition = jointObject.position;
                }
            } else if (jt == JointType.HandRight)
            {
                if (body.HandRightState == HandState.Closed && FindObjectOfType<PlayerControllerV2>().isStopped)
                {
                    Debug.Log(jt.ToString() + " right hand is closed.");
                    FindObjectOfType<PlayerControllerV2>().Shooting();
                }
            }
        }
    }

    private Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z) * jointSize;
    }
}
