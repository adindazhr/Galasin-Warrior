using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerControls : Agent
{
    public float Movespeed = 20;
    private Vector3 orig;
    private Bounds bndFloor, bndArea;
    private GameObject Target = null, Obstacle = null;

    public override void Initialize()
    {
        //orig = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        bndFloor = GameObject.FindWithTag("Floor").gameObject.GetComponent<MeshRenderer>().bounds;
        Target = GameObject.FindWithTag("Finish");
        Obstacle = GameObject.FindWithTag("Penyerang");

    }

    public float forceMultiplier = 1;
    public override void OnActionReceived(ActionBuffers vectorAction)
    {
        /*Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier * Time.deltaTime);*/
        this.transform.Translate(Vector3.right * vectorAction.ContinuousActions[0] * Movespeed * Time.deltaTime);
        this.transform.Translate(Vector3.forward * vectorAction.ContinuousActions[1] * Movespeed * Time.deltaTime);
        BoundCheck();
        //Globals.ScreenText();
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