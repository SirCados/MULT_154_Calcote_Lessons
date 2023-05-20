using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmBrain : MonoBehaviour
{
    bool _hasHive = true;
    SwarmScript _swarm;
    Bot _movementBehavior;

    // Start is called before the first frame update
    void Start()
    {
        _swarm = GetComponent<SwarmScript>();
        _movementBehavior = GetComponent<Bot>();
        
    }

    // Update is called once per frame
    void Update()
    {
        SwarmBehavior();
    }

    void HiveTaken()
    {
        _hasHive = false;
    }

    void HandleHiveDrop(Vector3 position)
    {
        _hasHive = true;
    }

    void SwarmBehavior()
    {
        if (_hasHive)
        {
            _swarm.MoveToWaypoint();
        }
        else
        {
            _movementBehavior.Pursue();
        }
    }

    void SubscribeToEvents()
    {
        NavPlayerMovement.OnHiveDrop += HandleHiveDrop;
        HivePickUp.HivePickedUp += HiveTaken;
    }
}
