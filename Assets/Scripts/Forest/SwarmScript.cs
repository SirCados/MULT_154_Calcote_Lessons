using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwarmScript : MonoBehaviour
{
    [SerializeField] List<GameObject> _waypoints;
    [SerializeField] NavMeshAgent _agent;
    const float WAYPOINT_THRESHOLD = 10.0f;
    GameObject _currentWaypoint;
    int _waypointIndex = 0;

    private void Start()
    {
        _currentWaypoint = _waypoints[0];
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        MoveToWaypoint();
    }

    public void MoveToWaypoint()
    {        
        if(Vector3.Distance(transform.position, _currentWaypoint.transform.position) < WAYPOINT_THRESHOLD)
        {
            _currentWaypoint = GetNextWaypoint();
            _agent.SetDestination(_currentWaypoint.transform.position);
        }        
    }

    GameObject GetNextWaypoint()
    {
        _waypointIndex++;
        if (_waypointIndex == _waypoints.Count)
        {
            _waypointIndex = 0;
        }

        return _waypoints[_waypointIndex];
    }
}
