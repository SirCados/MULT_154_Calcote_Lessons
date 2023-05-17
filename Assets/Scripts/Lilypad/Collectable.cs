using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Start is called before the first frame update

    public enum VegetableType
    { 
        BEET,
        CARROT,
        RADISH
    }

    public VegetableType typeOfVegetable;

}
