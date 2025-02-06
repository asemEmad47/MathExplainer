using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingScript : MonoBehaviour
{
    public static List<GameObject> GetAllObjectsWithName(string name)
    {
        List<GameObject> objectsWithName = new List<GameObject>();

        // Find all game objects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Iterate through each object and check its name
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name)
            {
                objectsWithName.Add(obj);
            }
        }

        return objectsWithName;
    }
}
