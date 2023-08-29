using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class UAVPathTracker : MonoBehaviour
{
    private List<Vector3> pathPoints = new List<Vector3>();
    private string filePath;

    void Start()
    {
        // Determine where the file will be saved
        filePath = Path.Combine(Application.persistentDataPath, "uav_path2.txt");
    }

    void Update()
    {
        // Collect position data at every frame
        pathPoints.Add(transform.position);
    }

    // This function will be called when the Unity game stops
    private void OnApplicationQuit()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (Vector3 point in pathPoints)
            {
                writer.WriteLine($"{point.x},{point.y},{point.z}");
            }
        }

        Debug.Log("UAV path saved to: " + filePath);
    }
}



