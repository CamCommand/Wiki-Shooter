using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public float wanderRadius;
    public float wanderTimerMax;
    [SerializeField] private float moveOn;
    public string link;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;
    

    void OnEnable()
    {
        moveOn = Random.Range(1, wanderTimerMax);
        agent = GetComponent<NavMeshAgent>();
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        //--Wander--
        if (timer >= moveOn)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
            moveOn = Random.Range(1, wanderTimerMax);
        }

        if(string.IsNullOrEmpty(link) && WikiScrape.instance.hyperLinks[0] != null)
        {
            addLink();
        }
    }

    //--Wander--
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void addLink()
    {
        WikiScrape.instance.addLinkToCitizen(this.GetComponent<Enemy>());
    }

    public void Die()
    {
        WikiScrape.instance.url = link;
        WikiScrape.instance.LinkScrape(WikiScrape.instance.url);
        Destroy(this.gameObject);
    }
}
