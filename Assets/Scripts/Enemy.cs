using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Enemy : Agent
{
    Rigidbody rBody;
    void Start () {
        rBody = GetComponent<Rigidbody>();
    }

    //public Transform Penjaga;
    public Transform Penyerang1;
    public Transform Penyerang2;
    public Transform Penyerang3;
    public Transform Penyerang4;
    public Transform Penyerang5;
    GameObject s1;
    GameObject s2;
    GameObject s3;
    GameObject s4;
    GameObject s5;
    private int? col1;
    private int? col2;
    private int? col3;
    private int? col4;
    private int? col5;

    public override void OnEpisodeBegin()
    {
        col1 = null;
        col2 = null;
        col3 = null;
        col4 = null;
        col5 = null;
        Globals.Episode += 1;        
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        if(this.gameObject.name == "Enemy")
        {
            this.transform.localPosition = new Vector3(18.77f,
                                           2.317779f,
                                           Random.Range(-13,13));
        }
        else if (this.gameObject.name == "Enemy2")
        {
            this.transform.localPosition = new Vector3(3.84f,
                                           2.317779f,
                                           Random.Range(-14,14));
        }
        else if (this.gameObject.name == "Enemy3")
        {
            this.transform.localPosition = new Vector3(-10.8f,
                                           2.317779f,
                                           Random.Range(-14,14));
        }
        else if (this.gameObject.name == "Enemy4")
        {
            this.transform.localPosition = new Vector3(-26.04f,
                                           2.317779f,
                                           Random.Range(-14,14));
        }
        else if (this.gameObject.name == "Enemy5")
        {
            this.transform.localPosition = new Vector3(Random.Range(17,-25),
                                           2.317779f,
                                           0.33f);
        }

        Penyerang1.localPosition = new Vector3(Random.Range(21,30), 0.001f, Random.Range(-14,14));
        Penyerang2.localPosition = new Vector3(Random.Range(21,30), 0.001f, Random.Range(-14,14));
        Penyerang3.localPosition = new Vector3(Random.Range(21,30), 0.001f, Random.Range(-14,14));
        Penyerang4.localPosition = new Vector3(Random.Range(21,30), 0.001f, Random.Range(-14,14));
        Penyerang5.localPosition = new Vector3(Random.Range(21,30), 0.001f, Random.Range(-14,14));
    }

    public float forceMultiplier = 1;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
            controlSignal.x = actionBuffers.ContinuousActions[0];
            controlSignal.z = actionBuffers.ContinuousActions[1];
            rBody.AddForce(controlSignal * forceMultiplier * Time.deltaTime);

        //kalo bisa lewatin penjaga
        if (Penyerang1.localPosition.x < 16f || Penyerang2.localPosition.x < 16f || Penyerang3.localPosition.x < 16f || Penyerang4.localPosition.x < 16f || Penyerang5.localPosition.x < 16f)
        {
            if (Penyerang1.localPosition.x < 1.9f || Penyerang2.localPosition.x < 1.9f || Penyerang3.localPosition.x < 1.9f || Penyerang4.localPosition.x < 1.9f || Penyerang5.localPosition.x < 1.9f)
            {
                if (Penyerang1.localPosition.x < -12.9f || Penyerang2.localPosition.x < -12.9f || Penyerang3.localPosition.x < -12.9f || Penyerang4.localPosition.x < -12.9f || Penyerang5.localPosition.x < -12.9f)
                {
                    AddReward(-0.1f);
                }
                AddReward(-0.1f);
            }
            AddReward(-0.1f);
        }
        //kalo penyerang ada yang sampe target
        if (Penyerang1.localPosition.x < -30.4f)
        {
            AddReward(-1f);
            col1 = 2;
        }

        if (Penyerang2.localPosition.x < -30.4f)
        {
            AddReward(-1f);
            col2 = 2;
        }

        if (Penyerang3.localPosition.x < -30.4f)
        {
            AddReward(-1f);
            col3 = 2;
        }

        if (Penyerang4.localPosition.x < -30.4f)
        {
            AddReward(-1f);
            col4 = 2;
        }

        if (Penyerang5.localPosition.x < -30.4f)
        {
            AddReward(-1f);
            col5 = 2;
        }

        //kalo semua penyerang nyampe target
        if (col1 == 2 && col2 == 2 && col3 == 2 && col4 == 2 && col5 == 2)
        {
            Debug.Log("success1");
            Globals.Fail += 1;
            AddReward(-5f);
            EndEpisode();
        }

        //kalo penyerang jatoh
        else if (Penyerang1.localPosition.y < 0 || Penyerang2.localPosition.y < 0 || Penyerang3.localPosition.y < 0 || Penyerang4.localPosition.y < 0 || Penyerang5.localPosition.y < 0)
        {
            EndEpisode();
        }

        //kalo penjaga bisa mengenai semua penyerang
        else if(col1 == 1 && col2 == 1 && col3 == 1 && col4 == 1 && col5 == 1)
        {
            Debug.Log("success2");
            AddReward(5.0f);
            EndEpisode();
        }

        //kalo ada penyerang yang sampe target dan ada penyerang yang terkena penjaga
        else if(col1.HasValue)
        {
            Debug.Log("sampe1");
            if(col2.HasValue)
            {
                Debug.Log("sampe2");
                if(col3.HasValue)
                {
                    Debug.Log("sampe3");
                    if(col4.HasValue)
                    {
                        Debug.Log("sampe4");
                        if(col5.HasValue)
                        {
                            Debug.Log("sampe5");
                            Debug.Log("SUCCESS");
                        }
                    }
                }
            }
        } 

        Debug.Log("Col1: " + col1);
        Debug.Log("Col2: " + col2);
        Debug.Log("Col3: " + col3);
        Debug.Log("Col4: " + col4);
        Debug.Log("Col5: " + col5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            if(collision.gameObject.name == "Penyerang1")
            {
                Penyerang1.localPosition = new Vector3(Random.Range(21,30), 0.001f, Random.Range(-14,14));
                s1 = GameObject.Find("Penyerang1");
                s1.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                col1 = 1;
                
            }
            if(collision.gameObject.name == "Penyerang2")
            {
                Penyerang2.localPosition = new Vector3(Random.Range(21,30), 0.001f, Random.Range(-14,14));
                s2 = GameObject.Find("Penyerang2");
                s2.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                col2 = 1;
            }
            if(collision.gameObject.name == "Penyerang3")
            {
                Penyerang3.localPosition = new Vector3(Random.Range(21,30), 0.001f, Random.Range(-14,14));
                s3 = GameObject.Find("Penyerang3");
                s3.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                col3 = 1;
            }
            if(collision.gameObject.name == "Penyerang4")
            {
                Penyerang4.localPosition = new Vector3(Random.Range(21,30), 0.001f, Random.Range(-14,14));
                s4 = GameObject.Find("Penyerang4");
                s4.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                col4 = 1;
            }
            if(collision.gameObject.name == "Penyerang5")
            {
                Penyerang5.localPosition = new Vector3(Random.Range(21,30), 0.001f, Random.Range(-14,14));
                s5 = GameObject.Find("Penyerang5");
                s5.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
                col5 = 1;
            }
            Globals.Success += 1;
            AddReward(1.0f);

            
        }
    }

    
}
