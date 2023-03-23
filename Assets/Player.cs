using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private Transform playerTransform;
    private NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Finish").transform;
        nav = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        nav.destination = playerTransform.position;
    }
}
