using System.Collections;
using UnityEngine;

public class UAVBattery : MonoBehaviour
{
    public float MaxBatteryCapacity = 5935f;    // mAh
    public float MaxEnergy = 274f;              // Wh
    public float MaxFlightTime = 55f * 60f;     // Converted to seconds (55 minutes)
    public float TypicalLidarPowerConsumption = 30f;   // Watts (Typical)
    public float MaxLidarPowerConsumption = 60f;      // Watts (Max)

    private float _currentBatteryPercentage = 100f;
    private float _flightElapsedTime = 0f;
    private int _lastLoggedPercentage = 100;

    private float _uavAltitude = 0f;    // Altitude in meters
    private float _uavSpeed = 0f;      // Speed in m/s

    // Throttle-to-power consumed chart
    private float[] throttleValues = { 0f, 25f, 50f, 75f, 100f };
    private float[] powerValues = { 0f, 162.5f, 325f, 487.5f, 650f };

    void Start()
    {
        StartCoroutine(BatteryConsumptionRoutine());
    }

    IEnumerator BatteryConsumptionRoutine()
    {
        while (_currentBatteryPercentage > 10f) // Critical battery level set to 10%
        {
            yield return new WaitForSeconds(1f);
            _flightElapsedTime += 1f;

            // Factor power consumption based on altitude and speed
            float altitudeFactor = 1f + (_uavAltitude / 1000f); // Increase by 0.1% per meter
            float speedFactor = 1f + (_uavSpeed / 100f);       // Increase by 1% per m/s

            // Get motor power consumption based on throttle (considering speed for throttle estimation)
            float motorPowerConsumed = Mathf.Lerp(
                powerValues[0],
                powerValues[powerValues.Length - 1],
                _uavSpeed / 23f
            ); // 23 m/s is max speed in S-Mode

            // Calculate total power consumed considering all motors, Lidar, altitude and speed
            float lidarPower = (_uavAltitude > 50) ? MaxLidarPowerConsumption : TypicalLidarPowerConsumption;  // Use max power if altitude > 50 meters, otherwise use typical
            float totalPowerConsumed = 4 * motorPowerConsumed + lidarPower;
            totalPowerConsumed *= altitudeFactor * speedFactor;

            // Adjust flight time based on power consumption
            float adjustedFlightTime = MaxEnergy * 3600 / totalPowerConsumed;  // MaxEnergy multiplied by 3600 to get Wh to Ws

            // Calculate battery consumption
            _currentBatteryPercentage = 100f - (100f * (_flightElapsedTime / adjustedFlightTime));

            if (Mathf.FloorToInt(_currentBatteryPercentage) != _lastLoggedPercentage)
            {
                _lastLoggedPercentage = Mathf.FloorToInt(_currentBatteryPercentage);
                Debug.Log("Battery Percentage: " + _lastLoggedPercentage + "%");
            }
        }

        if (_currentBatteryPercentage <= 10f) // Critical battery level set to 10%
        {
            Debug.Log("Critical battery warning! UAV needs to land immediately or it will crash.");
        }
    }

    public float GetCurrentBatteryPercentage()
    {
        return _currentBatteryPercentage;
    }

    public void SetUAVAltitude(float altitude)
    {
        _uavAltitude = altitude;
    }

    public void SetUAVSpeed(float speed)
    {
        _uavSpeed = speed;
    }
}
