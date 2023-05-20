using UnityEngine;

public class BearBrain : MonoBehaviour
{
    Bot _behavior;
    Vector3 _hivePosition;
    bool _isHiveAvailable = false;

    // Start is called before the first frame update
    void Start()
    {
        _behavior = GetComponent<Bot>();
        SubscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        DetermineBearBehavior();
    }

    void DetermineBearBehavior()
    {
        if (_isHiveAvailable)
        {
            _behavior.Seek(_hivePosition);
        }
        else
        {
            if (_behavior.CanTargetSeeMe())
            {
                _behavior.Evade();
            }
            else if (_behavior.CanSeeTarget())
            {
                _behavior.Pursue();
            }
            else
            {
                _behavior.Wander();
            }
        }
    }

    void HandleHiveDrop(Vector3 position)
    {
        _hivePosition = position;
        _isHiveAvailable = true;
    }

    void HandleHivePickUp()
    {
        _isHiveAvailable = false;
    }

    void SubscribeToEvents()
    {
        NavPlayerMovement.OnHiveDrop += HandleHiveDrop;
        HivePickUp.HivePickedUp += HandleHivePickUp;
    }
}
