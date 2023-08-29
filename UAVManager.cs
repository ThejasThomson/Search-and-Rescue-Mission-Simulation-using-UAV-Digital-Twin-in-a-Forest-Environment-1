using UnityEngine;

public class UAVManager : MonoBehaviour
{
    private UAVController uavController;
    private GridSearchUAV gridSearchUAV;

    void Start()
    {
        uavController = GetComponent<UAVController>();
        gridSearchUAV = GetComponent<GridSearchUAV>();

        uavController.enabled = true;
        gridSearchUAV.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Search"))
        {
            uavController.enabled = !uavController.enabled;
            gridSearchUAV.enabled = !gridSearchUAV.enabled;

            if (gridSearchUAV.enabled)
            {
                Debug.Log("Search is on"); // Message when search mode is turned on
            }
            else
            {
                Debug.Log("Search is off"); // Message when search mode is turned off
            }
        }
    }
}

