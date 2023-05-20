///
/// Code modified from https://learn.unity.com/tutorial/hide-h1zl/?courseId=5dd851beedbc2a1bf7b72bed&projectId=5e0b9220edbc2a14eb8c9356&tab=materials&uv=2019.3#
/// Author: Penny de Byl
///
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    public enum BehaviorMode
    {
        EVADE,
        FLEE,
        HIDE,
        PURSUE,
        SEEK,
        WANDER
    }

    public GameObject Target;
    public GameObject[] HidingSpots;
    public BehaviorMode CurrentBehavior;

    NavMeshAgent _agent;
    Rigidbody _targetRigidbody;

    float _targetSpeed
    {
        get { return _targetRigidbody.velocity.magnitude; }
    }    

    // Start is called before the first frame update
    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _targetRigidbody = Target.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(CanTargetSeeMe())
        //Flee(Target.transform.position);
        DetermineBehavior();
    }

    void DetermineBehavior()
    {
        switch (CurrentBehavior)
        {
            case BehaviorMode.EVADE:
                Evade();
                break;
            case BehaviorMode.FLEE:
                Flee(Target.transform.position);
                break;
            case BehaviorMode.HIDE:
                Hide();
                break;
            case BehaviorMode.PURSUE:
                Pursue();
                break;
            case BehaviorMode.SEEK:
                Seek(Target.transform.position);
                break;
            case BehaviorMode.WANDER:
                Wander();
                break;
        }
    }

    void Seek(Vector3 location)
    {
        _agent.SetDestination(location);
        _targetRigidbody = Target.GetComponent<Rigidbody>();
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        _agent.SetDestination(this.transform.position - fleeVector);
    }

    void Pursue()
    {
        Vector3 targetDir = Target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(Target.transform.forward));

        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if ((toTarget > 90 && relativeHeading < 20) || _targetSpeed < 0.01f)
        {
            Seek(Target.transform.position);
            return;
        }

        float lookAhead = targetDir.magnitude / (_agent.speed + _targetSpeed);
        Seek(Target.transform.position + Target.transform.forward * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDir = Target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / (_agent.speed + _targetSpeed);
        Flee(Target.transform.position + Target.transform.forward * lookAhead);
    }


    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetLocal);
    }

    void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < HidingSpots.Length; i++)
        {
            Vector3 hideDir =HidingSpots[i].transform.position - Target.transform.position;
            Vector3 hidePos = HidingSpots[i].transform.position + hideDir.normalized * 10;

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Seek(chosenSpot);

    }

    void CleverHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = HidingSpots[0];

        for (int i = 0; i < HidingSpots.Length; i++)
        {
            Vector3 hideDir =HidingSpots[i].transform.position - Target.transform.position;
            Vector3 hidePos = HidingSpots[i].transform.position + hideDir.normalized * 100;

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = HidingSpots[i];
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);


        Seek(info.point + chosenDir.normalized);

    }

    bool CanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = Target.transform.position - this.transform.position;
        if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
                return true;
        }
        return false;
    }

    bool CanTargetSeeMe()
    {
        RaycastHit raycastInfo;
        Vector3 targetFwdWS = Target.transform.TransformDirection(Target.transform.forward);
        Debug.DrawRay(Target.transform.position, targetFwdWS * 10);
        Debug.DrawRay(Target.transform.position, Target.transform.forward * 10, Color.green);
        if (Physics.Raycast(Target.transform.position, Target.transform.forward, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject == gameObject)
                return true;
        }
        return false;
    }
}
