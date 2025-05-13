using UnityEngine;
using UnityEngine.AI;

public class MoveNPC : MonoBehaviour
{
    public GameObject target;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }

        if (target == null)
        {
            Debug.LogError("Target GameObject not assigned in " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null && target != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }
}