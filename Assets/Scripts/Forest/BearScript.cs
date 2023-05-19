using UnityEngine;
using UnityEngine.AI;

public class BearScript : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] GameObject _target;
    

    // Start is called before the first frame update
    void Start()
    {
        SetupBear();
    }

    // Update is called once per frame
    void Update()
    {
        MoveAgent();
    }

    void SetupBear()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void MoveAgent()
    {
        _agent.SetDestination(_target.transform.position);        
    }
}
