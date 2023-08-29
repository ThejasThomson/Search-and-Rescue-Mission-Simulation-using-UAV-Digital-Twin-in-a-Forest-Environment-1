using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LidarSensor : MonoBehaviour
{
    public Transform lidarSensor;
    public float lidarRange = 50f;
    public float lidarFieldOfView = 30f;

    private List<Vector3> capturedPoints = new List<Vector3>();
    private string fileName;

    private void Start()
    {
        fileName = Path.Combine(Application.persistentDataPath, "points27.txt");
        // Write the CSV header to the file
    }

    private void Update()
    {
        Vector3 uavPosition = transform.position;
        Quaternion uavRotation = transform.rotation;

        float halfFOV = lidarFieldOfView * 0.5f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up) * uavRotation;
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up) * uavRotation;

        int raysCount = 100;
        for (int i = 0; i < raysCount; i++)
        {
            float t = (float)i / (float)(raysCount - 1);
            Quaternion rayRotation = Quaternion.Lerp(leftRayRotation, rightRayRotation, t);
            Ray ray = new Ray(lidarSensor.position, rayRotation * Vector3.forward);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, lidarRange))
            {
                capturedPoints.Add(hit.point);
            }
        }
    }

    private void OnDestroy()
    {
        SaveCapturedPoints();
    }

    private void SaveCapturedPoints()
    {
        using (StreamWriter writer = File.AppendText(fileName))
        {
            foreach (Vector3 point in capturedPoints)
            {
                string line = $"{point.x},{point.y},{point.z}";
                writer.WriteLine(line);
            }
        }
    }

    // Draw Gizmos for ray visualization and LiDAR sensor's position
    private void OnDrawGizmos()
    {
        if (lidarSensor != null)
        {
            Vector3 uavPosition = transform.position;
            Quaternion uavRotation = transform.rotation;

            float halfFOV = lidarFieldOfView * 0.5f;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up) * uavRotation;
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up) * uavRotation;

            int raysCount = 100;
            for (int i = 0; i < raysCount; i++)
            {
                float t = (float)i / (float)(raysCount - 1);
                Quaternion rayRotation = Quaternion.Lerp(leftRayRotation, rightRayRotation, t);
                Ray ray = new Ray(lidarSensor.position, rayRotation * Vector3.forward);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(ray.origin, ray.direction * lidarRange);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(lidarSensor.position, 0.5f);
        }
    }
}

