using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class GPSController : MonoBehaviour
{
    public TMP_Text currentLongitude;
    public TMP_Text currentLatitude;
    public TMP_Text currentAltitude;
    public TMP_Text distance;

    private float firstLongitude;
    private float firstLatitude;

    private float secondLongitude;
    private float secondLatitude;

    IEnumerator Start()
    {
        // Check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location services are not enabled by the user.");
            yield break;
        }

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out while initializing location services.");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location.");
            yield break;
        }
    }

    void Update()
    {
        currentLongitude.text = Input.location.lastData.longitude.ToString();
        currentLatitude.text = Input.location.lastData.altitude.ToString();
        currentLatitude.text = Input.location.lastData.latitude.ToString();
    }

    public void SaveFirstCoordinate()
    {
        firstLongitude = Input.location.lastData.longitude;
        firstLatitude = Input.location.lastData.latitude;
    }

    public void SaveSecondCoordinate()
    {
        secondLongitude = Input.location.lastData.longitude;
        secondLatitude = Input.location.lastData.latitude;
    }

    public void CalculateDistance()
    {
        double LatA = ToRadians(firstLatitude);
        double LatB = ToRadians(secondLatitude);

        double LongA = ToRadians(firstLongitude);
        double LongB = ToRadians(secondLongitude);

        double Distance = 3440.1 * Math.Acos((Math.Sin(LatA) * Math.Sin(LatB)) + Math.Cos(LatA) * Math.Cos(LatB) * Math.Cos(LongA - LongB));
        distance.text = "Distance: " + Distance.ToString();
    }

    private double ToRadians(double value)
    {
        return value * (Mathf.PI / 180);
    }
}