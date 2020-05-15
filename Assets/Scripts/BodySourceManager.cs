using UnityEngine;
using Windows.Kinect;

public class BodySourceManager : MonoBehaviour
{
    private KinectSensor _kinectSensor;
    private BodyFrameReader _reader;
    private Body[] _bodies = null;

    public Body[] GetBodies()
    {
        return _bodies;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _kinectSensor = KinectSensor.GetDefault();
        if (_kinectSensor != null)
        {
            _reader = _kinectSensor.BodyFrameSource.OpenReader();
            if (!_kinectSensor.IsOpen)
                _kinectSensor.Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // null propagation
        BodyFrame frame = _reader?.AcquireLatestFrame();
        if (frame != null)
        {
            if (_bodies == null)
                _bodies = new Body[_kinectSensor.BodyFrameSource.BodyCount];
            frame.GetAndRefreshBodyData(_bodies);
            frame.Dispose();
            frame = null;
        }
    }

    private void OnApplicationQuit()
    {
        if (_kinectSensor != null)
        {
            _reader.Dispose();
            _reader = null;
        }

        if (_kinectSensor != null)
        {
            if (_kinectSensor.IsOpen)
                _kinectSensor.Close();
            _kinectSensor = null;
        }
    }
}
