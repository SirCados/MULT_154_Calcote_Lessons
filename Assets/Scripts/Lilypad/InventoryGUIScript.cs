using System.Collections.Generic;
using UnityEngine;

public class InventoryGUIScript : MonoBehaviour
{
    public List<GameObject> ItemList;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleVegetablePickup(Collectable.VegetableType item)
    {
        CountGUIScript vegetableTracker = ItemList[(int)item].GetComponent<CountGUIScript>();
        vegetableTracker.UpdateCount();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        InventoryScript.OnVegetablePickup += HandleVegetablePickup;
    }

    void UnsubscribeToEvents()
    {
        InventoryScript.OnVegetablePickup -= HandleVegetablePickup;
    }
}
