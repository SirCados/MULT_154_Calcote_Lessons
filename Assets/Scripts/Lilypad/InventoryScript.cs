using System.Collections.Generic;
using Mirror;

public class InventoryScript : NetworkBehaviour
{
    Dictionary<Collectable.VegetableType, int> _itemInventory = new Dictionary<Collectable.VegetableType, int>();

    public delegate void CollectItem(Collectable.VegetableType item);
    public static event CollectItem OnVegetablePickup;

    // Start is called before the first frame update
    void Start()
    {
        SetupInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PrintInventory()
    {
        string output = "";

        foreach (KeyValuePair<Collectable.VegetableType, int> keyValue in _itemInventory)
        {
            output += string.Format("{0}: {1} ", keyValue.Key, keyValue.Value);
        }
        print(output);
    }

    public void AddToInventory(Collectable item)
    {
        _itemInventory[item.typeOfVegetable]++;
        //Local player makes the call to the server
        Cmd_TellServerItemCollected(item.typeOfVegetable);
    }

    [Command]
    void Cmd_TellServerItemCollected(Collectable.VegetableType item)
    {
        //Server propagates info back to client
        Rpc_TellPlayerItemCollected(item);
    }

    [ClientRpc]
    void Rpc_TellPlayerItemCollected(Collectable.VegetableType item)
    {
        OnVegetablePickup?.Invoke(item);
    }

    void SetupInventory()
    {
        if (isLocalPlayer)
        {
            foreach (Collectable.VegetableType item in System.Enum.GetValues(typeof(Collectable.VegetableType)))
            {
                _itemInventory.Add(item, 0);
            }
        }
    }
}
