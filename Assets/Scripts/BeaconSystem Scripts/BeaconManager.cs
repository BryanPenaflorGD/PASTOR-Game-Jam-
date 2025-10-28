using UnityEngine;

public class BeaconManager : MonoBehaviour
{
    public BeaconActivator[] beacons; // assign all 5 in inspector
    public BakunawaController bakunawa; // reference to enemy script
    private int activatedCount = 0;

    private void Start()
    {
        if (beacons == null)
        {
            Debug.LogError("Beacons array is not assigned in BeaconManager!");
            return;
        }

        foreach (var beacon in beacons)
        {
            if (beacon != null)
            {
                if (beacon.onBeaconActivated != null)
                {
                    beacon.onBeaconActivated.AddListener(OnBeaconActivated);
                }
                else
                {
                    Debug.LogWarning($"Beacon {beacon.name} has null UnityEvent!");
                }
            }
            else
            {
                Debug.LogWarning("Found null beacon in beacons array!");
            }
        }
    }

    private void OnBeaconActivated()
    {
        activatedCount++;
        Debug.Log("Beacons activated: " + activatedCount + "/5");

        if (bakunawa != null)
        {
            bakunawa.StunBakunawa();
        }

        if (activatedCount >= beacons.Length)
        {
            bakunawa.StopCompletely();
        }
    }
}
