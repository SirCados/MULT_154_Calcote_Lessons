using UnityEngine;
using Mirror;

public class LilypadSpawner : NetworkBehaviour
{
    public GameObject[] LilypadObjects;

    // Start is called before the first frame update

    public override void OnStartServer()
    {
        InvokeRepeating("SpawnLilypad", 1f, 4f);
    }

    void SpawnLilypad()
    {
        foreach(GameObject lilyPad in LilypadObjects)
        {
            GameObject pad = Instantiate(lilyPad);
            NetworkServer.Spawn(pad);
        }        
    }
}
