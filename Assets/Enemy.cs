using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Enemy : Agent
{
    public float MoveSpeed=20;
    private Vector3 orig; //original position
    private Bounds bndFloor; //kalo jatoh
    private GameObject Target;

    public override Initialize()
    {
        orig = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

    }

}
