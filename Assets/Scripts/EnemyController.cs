using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 0.1f;
    NavMeshAgent agent;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = playerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance<=lookRadius)
        {
            agent.SetDestination(target.position);
            //agent.
            if(distance<=agent.stoppingDistance)
            {
                FaceTarget();
            }
        }

    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Pillow")
        {
            Debug.Log("Hit");
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Pillow")
        {
            Debug.Log("Enemy Got Hit");
        }
    }
}
