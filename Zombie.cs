using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private AudioSource AS;
    [SerializeField] private AudioClip[] zombieNoises;
    [SerializeField] private Collider[] hitboxes;
    [SerializeField] private GameManager gm;
    private NavMeshAgent agent;
    private Transform player;
    public Animator AM;
    public float health;
    private void Start()
    {
        AS = GetComponent<AudioSource>();
        hitboxes = GetComponentInChildren<Transform>().GetChild(15).GetComponentsInChildren<Collider>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        AM.SetFloat("Blend", 0);
        StartCoroutine(Noises());
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1 && !AM.GetBool("Dead"))
        {
            for(var i = 0; i < hitboxes.Length; i++)
            {
                hitboxes[i].enabled = false;
            }
            gm.money += 100;
            agent.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            Destroy(gameObject, 3);
            AM.SetBool("Dead", true);
            AS.enabled = false;
        } if (health > 0)
        {
            // Distance to the target
            float distance = Vector3.Distance(player.position, transform.position);

            // If inside the lookRadius
            // Move towards the target
            agent.SetDestination(player.position);

            // If within attacking distance
            if (distance <= agent.stoppingDistance)
            {
                gm.health--;
                AM.SetBool("Attack", true);
                FaceTarget();   // Make sure to face towards the target
            } else
            {
                AM.SetBool("Attack", false);
            }

        }
    }
    IEnumerator Noises()
    {
        while (true && !AM.GetBool("Dead"))
        {
            AS.clip = zombieNoises[Random.Range(0, zombieNoises.Length)];
            AS.Play();
            yield return new WaitForSeconds(Random.Range(1, 6));
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
