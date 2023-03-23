using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerControls : Agent
{
    Rigidbody rBody;
    void Start () {
        rBody = GetComponent<Rigidbody>();
    }

    //public Transform Penjaga;
    public Transform Penjaga1;
    public Transform Penjaga2;
    public Transform Penjaga3;
    public override void OnEpisodeBegin()
    {
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3( 27.00822f, 0.08f, -2.9266f);
        
        Penjaga1.localPosition = new Vector3(Random.value,
                                           2.317779f,
                                           0.0262009f);

        Penjaga2.localPosition = new Vector3(Random.value,
                                           2.317779f,
                                           -0.39f);
        
        Penjaga3.localPosition = new Vector3(Random.value,
                                           2.317779f,
                                           -3.2f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Penjaga1.localPosition);
        sensor.AddObservation(Penjaga2.localPosition);
        sensor.AddObservation(Penjaga3.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public float forceMultiplier = 10;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        // Rewards
        float distanceToTarget1 = Vector3.Distance(this.transform.localPosition, Penjaga1.localPosition);
        float distanceToTarget2 = Vector3.Distance(this.transform.localPosition, Penjaga2.localPosition);
        float distanceToTarget3 = Vector3.Distance(this.transform.localPosition, Penjaga3.localPosition);

        // Reached target
        if (distanceToTarget1 < 2.2f || distanceToTarget2 < 2.2f || distanceToTarget3 < 2.2f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
