using UnityEngine;
using TMPro;

public class CountGUIScript : MonoBehaviour
{
    public string ItemName;
    TextMeshProUGUI _textElement;
    int _vegetableCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _textElement = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCount()
    {
        _vegetableCount++;
        _textElement.text = ItemName + ": " + _vegetableCount;
    }
}
