using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class GpsService : MonoBehaviour
{
    //****************************************************QUICK DOC**********************************************************************

    //Setup:

    //Add this script to any gameObject -> DONE

    //How to use?:

    //There are four methods which can be called StartStopGPS(), GetCurrentLocation() and AddLocations()
    //Since this is singleton those can be called from anywhere by using GpsService.Instance.MethodName()
    //Use StartStopGPS() to activate or stop Gps returns true if succesfull; false if Gps fails -> dont forget to stop
    //Use GetCurrentLocation() to check if a Location was reached; will return a location(see bottom of script)
    //To add known Locations use AddLocations() like this: GpsService.AddLocations(Location) or GpsService.AddLocations(ArrayOfLocations)
    //A new Location is created like this: Location meineLocation = new Location(string name, int id, double latitude, double longitude)
    //Use GetMapCoordinates() to get the current location inside the reference mask as x and y coordinates



    public static GpsService Instance;

    // fine tune for gps accuracy current values are just my best guess
    // update frequency only matters if gps is running in coroutine which refuses to work for now
    public double toleranceInMeters = 2f;
    public int useReadingCount = 4;
    // top left of mask
    public double northWest_x = 0;
    public double northWest_y = 0;
    public double northWest_lat = 0;
    public double northWest_lon = 0;
    // bottom right of mask
    public double southEast_x = 0;
    public double southEast_y = 0;
    public double southEast_lat = 0;
    public double southEast_lon = 0; 

    private Location unknownLocation = new Location("unknown",-1,0,0);
    private Location currentLocation;
    private List<Location> knownLocations = new List<Location>();
    private bool listening = false;
    private double earthRadius = 6378.137f;
    private double globalNorthWest_x;
    private double globalNorthWest_y;
    private double globalSouthEast_x;
    private double globalSouthEast_y;
    // change
    private double better_lat;
    private double better_lon;
    private double last_lat;
    private double last_lon;

    // ***
    public Queue<double> lonQueue = new Queue<double>();
    public Queue<double> latQueue = new Queue<double>();


    private void Awake()
    {
        Instance = this;
        double coAngle = (double)(Math.Cos((southEast_lat + northWest_lat) / 2));
        globalNorthWest_x = earthRadius * northWest_lon * coAngle;
        globalNorthWest_y = earthRadius * northWest_lat;
        globalSouthEast_x = earthRadius * southEast_lon * coAngle;
        globalSouthEast_y = earthRadius * southEast_lat;
        (better_lat, better_lon) = (52.456300897744406f, 13.526658817006869f);
    }

    private void Update()
    {
        if(listening)
        {
            Input.location.Start();
            last_lat = Input.location.lastData.latitude;
            last_lon = Input.location.lastData.longitude;
            AddToReadings(last_lat, last_lon);
            GetExcactLocation();
        }
        
    }

    public bool StartStopGPS()
    {
        listening = !listening;

        Input.location.Start();
        if (Input.location.isEnabledByUser == false)
        {
            listening = false;
            return false;
        }

        return true;
    }

    public (float, float) GetMapCoordinates()
    {
        double x, y;
        
        double global_x,global_y;
        double relativ_x ,relativ_y;
        // using simple lat lon; saving excact for location matching
        // double current_lat, current_lon;
        // (current_lat,current_lon) = GetExcactLocation();
        // (current_lat, current_lon) = (last_lat, last_lon);
        // (better_lat, better_lon) = (52.45619294882531, 13.52597904805195);
        double coAngle = (double)(Math.Cos((southEast_lat + northWest_lat) / 2));
        global_x = earthRadius * better_lon * coAngle;
        global_y = earthRadius * better_lat;
        //percentage relative to fixpoints global coords
        relativ_x = (global_x-globalNorthWest_x)/(globalSouthEast_x-globalNorthWest_x);
        relativ_y = (global_y-globalNorthWest_y)/(globalSouthEast_y-globalNorthWest_y);
        // add relation
        x = northWest_x + ((southEast_x -northWest_x) * relativ_x);
        // y axis is inverted -> all y values need to be multiplied by -1
        y = -northWest_y + ((-southEast_y + northWest_y) * relativ_y);

        return ((float)x, -(float)y);
    }

    public Location GetCurrentLocation()
    {
        
        if (CheckLocations()) 
        {
            return currentLocation;
        }
        return unknownLocation;
    }

    public void AddLocations(Location newLocation)
    { 
        knownLocations.Add(newLocation);
    }

    public void AddLocations(Location[] newLocations)
    {
        foreach (Location loc in newLocations) 
        {
            knownLocations.Add(loc);
        }
    }

    private void AddToReadings(double lat, double lon)
    {
        if(latQueue.Count > useReadingCount+1) 
        {
            latQueue.Dequeue(); 
            lonQueue.Dequeue(); 
        }

        latQueue.Enqueue(lat);
        lonQueue.Enqueue(lon);
    }

    private bool GetDistance(float lat1, float lon1, float lat2, float lon2, float radius)
    {
         
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;
        float a =  (Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
            Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
            Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2));
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        var distance = earthRadius * c;
        distance = distance * 1000f;
        double distanceFloat = (double)distance;

        if (distanceFloat < radius)
        {
            return true;
        }
        return false;
    }

    private bool CheckLocations()
    {
        foreach (Location loc in knownLocations)
        {
            if (GetDistance((float)better_lat, (float)better_lon, (float)loc.lat, (float)loc.lon, (float)toleranceInMeters))
            {
                currentLocation = loc;
                return true;
            }
        }
        return false;
    }

    private void GetExcactLocation()
    {
        int count = lonQueue.Count;
        double lat, lon;
        lat = lon = 0f;
        if (count >= useReadingCount) 
        {
            for (int i = 0;i < count; i++)
            {
                lat += latQueue.Dequeue();
                lon += lonQueue.Dequeue();
            }
            (last_lat, last_lon) = (lat / count, lon / count);
            (better_lat, better_lon) = (last_lat, last_lon);
        }
    }


}

public struct Location 
{
    public string name { get; }
    public int id { get; }
    public double lat { get; }
    public double lon { get; }

    public Location(string name,int id, double lat, double lon)
    {
        this.name = name;
        this.lat = lat;
        this.lon = lon;
        this.id = id;
    }

}

// does not execute
// moved to update
//private IEnumerator GPS()
//{
//    while (true)
//    {

//        test++;
//        Input.location.Start();
//        last_lat = Input.location.lastData.latitude;
//        last_lon = Input.location.lastData.longitude;
//        AddToReadings(last_lat, last_lon);
//        yield return new WaitForSeconds(updateFrequency);

//    }
//}
//public bool StartStopGPS()
//{
//    listening = !listening;

//    Input.location.Start();
//    if (Input.location.isEnabledByUser == false)
//    { 
//        return false;
//    }


//    if (listening)
//    {
//        StartCoroutine("GPS");
//    }
//    else
//    {
//        StopCoroutine("GPS");
//    }

//    return true;
//}
