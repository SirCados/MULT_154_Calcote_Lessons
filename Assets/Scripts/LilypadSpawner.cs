using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilypadSpawner : MonoBehaviour
{
    public GameObject[] LilypadObjects;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnLilypad", 1f, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnLilypad()
    {
        foreach(GameObject lilyPad in LilypadObjects)
        {
            Instantiate(lilyPad);
        }        
    }
}
