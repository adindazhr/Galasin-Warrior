using UnityEngine;
using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveTarget : Agent
{
    public float Movespeed = 20;
    private Vector3 orig;
    private Bounds bndFloor;
    private GameObject Target = null, Obstacle = null;
    private bool dead, finish;

    public override void Initialize()
    {
        orig = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z); 
        bndFloor = GameObject.FindWithTag("Floor").gameObject.GetComponent<MeshRenderer>().bounds;
        Target = GameObject.FindWithTag("Target");
        Obstacle = GameObject.FindWithTag("Obstacle");

    }
    public override void OnEpisodeBegin()
    {
        Globals.Episode += 1;
        this.transform.position = new Vector3(orig.x, orig.y, orig.z);
        // RandomPlaceTarget();
       
    }
    public override void OnActionReceived(ActionBuffers vectorAction)
    {
        
        // this.transform.Translate(Vector3.right * vectorAction.ContinuousActions[0] * Movespeed * Time.deltaTime);
        // this.transform.Translate(Vector3.forward * vectorAction.ContinuousActions[1] * Movespeed * Time.deltaTime);
        // BoundCheck();
        // Globals.ScreenText();
        // Raycast to detect enemy
        RaycastHit hit;
        bool enemyDetected = Physics.Raycast(transform.position, transform.forward, out hit, 5.0f) && hit.transform.CompareTag("Obstacle");

  
        if (enemyDetected)
        {
            SetReward(-0.01f);
            Debug.Log("Enemy detected. Pausing for 1 second.");
            StartCoroutine(PauseAgent());
        }
        else
        {
            // Continue normal movement
            this.transform.Translate(Vector3.right * vectorAction.ContinuousActions[0] * Movespeed * Time.deltaTime);
            this.transform.Translate(Vector3.forward * vectorAction.ContinuousActions[1] * Movespeed * Time.deltaTime);
            BoundCheck();
            Globals.ScreenText();
        }
        
        if (dead) 
        {
            if(this.gameObject.name == "Player") this.transform.localPosition = new Vector3(9.86f, 1.83f, -24.82f); 
            else if(this.gameObject.name == "Player1") this.transform.localPosition = new Vector3(9.86f, 1.83f, -24.82f);
            else if(this.gameObject.name == "Player2") this.transform.localPosition = new Vector3(10.86f, 1.83f, -24.82f); 
            else if(this.gameObject.name == "Player3") this.transform.localPosition = new Vector3(11.86f, 1.83f, -24.82f);
            else if(this.gameObject.name == "Player4") this.transform.localPosition = new Vector3(12.86f, 1.83f, -24.82f);
            else if(this.gameObject.name == "Player5") this.transform.localPosition = new Vector3(13.86f, 1.83f, -24.82f);

        }

        if(finish)
        {
            if(this.gameObject.name == "Player") this.transform.localPosition = new Vector3(68.62f, 1.83f, -19.34f); 
            else if(this.gameObject.name == "Player1") this.transform.localPosition = new Vector3(69.62f, 1.83f, -19.34f);
            else if(this.gameObject.name == "Player2") this.transform.localPosition = new Vector3(70.62f, 1.83f, -19.34f); 
            else if(this.gameObject.name == "Player3") this.transform.localPosition = new Vector3(71.62f, 1.83f, -19.34f);
            else if(this.gameObject.name == "Player4") this.transform.localPosition = new Vector3(72.62f, 1.83f, -19.34f);
            else if(this.gameObject.name == "Player5") this.transform.localPosition = new Vector3(73.62f, 1.83f, -19.34f);
        }

        if(Globals.Player == 0) {
            dead = false;
            finish = false;
            Globals.check += 1;
            if (Globals.check % 6 == 0 )
            {
                Globals.Player = 6;

            }
            EndEpisode();
            
        } 
        
    }

    IEnumerator PauseAgent()
    {
        //Stop the agent's movement
        this.transform.Translate(Vector3.right * 0.0f * Movespeed * Time.deltaTime);
        this.transform.Translate(Vector3.forward * 0.0f * Movespeed * Time.deltaTime);
        // Pause for 1 second
        yield return new WaitForSeconds(3.0f);
        Vector3 moveDirection = Random.Range(0f, 1f) > Random.Range(0f, 1f) ? Vector3.right : Vector3.left;
        this.transform.Translate(Vector3.back * 0.01f * 20 * Time.deltaTime);
        this.transform.Translate(moveDirection * 0.05f * 25 * Time.deltaTime);
        // Resume normal movement
        Debug.Log("Resuming movement.");
        AddReward(0.001f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target") == true)
        {
            Globals.Success += 1;
            AddReward(1.0f);
            finish = true;
            Globals.Player -= 1;
            Debug.Log("finish");
            // EndEpisode();
        }

        if(collision.gameObject.CompareTag("Obstacle") == true)
        {
            Debug.Log("hit");
            Globals.Fail += 1;
            AddReward(-1.0f);
            // EndEpisode();
            // this.transform.Translate(Vector3.right * 0.0f * Movespeed * Time.deltaTime);
            // this.transform.Translate(Vector3.forward * 0.0f * Movespeed * Time.deltaTime);
            // Destroy(this.transform.gameObject);
            dead = true;
            Globals.Player -= 1;
            Debug.Log("dead");
        }
    }

    private void BoundCheck()
    {
        if (this.transform.position.x < bndFloor.min.x)
        {
            Globals.Fail += 1;
            AddReward(-0.1f);
            //EndEpisode();
            dead = true;
            // Globals.Player -= 1;
            Debug.Log("dead");
        }
        else if (this.transform.position.x > bndFloor.max.x)
        {
            Globals.Fail += 1;
            AddReward(-0.1f);
            //EndEpisode();
            dead = true;
            //Globals.Player -= 1;
            Debug.Log("dead");
        }

        if (this.transform.position.z < bndFloor.min.z)
        {
            Globals.Fail += 1;
            AddReward(-0.1f);
            //EndEpisode();
            dead = true;
            //Globals.Player -= 1;
            Debug.Log("dead");
        }
        else if (this.transform.position.z > bndFloor.max.z)
        {
            Globals.Fail += 1;
            AddReward(-0.1f);
            //EndEpisode();
            dead = true;
            //Globals.Player -= 1;
            Debug.Log("dead");
        }

        Globals.Player -= 1;

        // if (this.transform.position.x > bndArea.max.x)
        // {
        //     Globals.Success += 1;
        //     AddReward(1.0f);
        //     EndEpisode();
        // }
    }
}