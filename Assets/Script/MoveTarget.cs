using UnityEngine;
using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveTarget : Agent
{
    public float Movespeed = 20;
    private Vector3 orig;
    private Bounds bndFloor, bndArea;
    private GameObject Target = null, Obstacle = null;

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
        this.transform.Translate(moveDirection * 0.03f * 25 * Time.deltaTime);
        // Resume normal movement
        Debug.Log("Resuming movement.");
        SetReward(0.001f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target") == true)
        {
            Globals.Success += 1;
            Debug.Log("finish");
            AddReward(1.0f);
            EndEpisode();
        }

        if(collision.gameObject.CompareTag("Obstacle") == true)
        {
            Debug.Log("hit");
            Globals.Fail += 1;
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    private void BoundCheck()
    {
        if (this.transform.position.x < bndFloor.min.x)
        {
            Globals.Fail += 1;
            AddReward(-0.1f);
            EndEpisode();
        }
        else if (this.transform.position.x > bndFloor.max.x)
        {
            Globals.Fail += 1;
            AddReward(-0.1f);
            EndEpisode();
        }

        if (this.transform.position.z < bndFloor.min.z)
        {
            Globals.Fail += 1;
            AddReward(-0.1f);
            EndEpisode();
        }
        else if (this.transform.position.z > bndFloor.max.z)
        {
            Globals.Fail += 1;
            AddReward(-0.1f);
            EndEpisode();
        }

        // if (this.transform.position.x > bndArea.max.x)
        // {
        //     Globals.Success += 1;
        //     AddReward(1.0f);
        //     EndEpisode();
        // }
    }
}