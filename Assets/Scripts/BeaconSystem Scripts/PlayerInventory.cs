using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int fragments = 0;

    public void AddFragments(int amount)
    {
        fragments += amount;
        Debug.Log("Fragments collected: " + fragments);
    }

    public bool HasEnoughFragments(int required)
    {
        return fragments >= required;
    }

    public void SpendFragments(int amount)
    {
        fragments -= amount;
    }
}
