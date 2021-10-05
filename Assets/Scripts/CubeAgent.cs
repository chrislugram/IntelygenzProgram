using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CubeAgent : Agent
{

    #region Variables
    [SerializeField] private Movement movement;

    [SerializeField] private Transform exit;
    [SerializeField] private string tagWalls;
    [SerializeField] private string tagExit;

    [SerializeField] private float p_hitWalls;
    [SerializeField] private float p_perStep;
    [SerializeField] private float r_approachStep;
    [SerializeField] private float r_exitHit;

    private float previousDistance;
    #endregion

    #region Agent methods
    public override void OnEpisodeBegin()
    {
        movement.ResetMovement();

        previousDistance = GetDistanceExit();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // distancia al target
        Vector3 distanceToExit = exit.localPosition - movement.transform.localPosition;
        float realDistance = distanceToExit.magnitude;

        sensor.AddObservation(distanceToExit);        // 3
        sensor.AddObservation(realDistance);          // 1
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        movement.moveDirection = new Vector3(actions.ContinuousActions[0], 0, actions.ContinuousActions[1]);

        //int move = actions.DiscreteActions[0];
        //Vector3 directionMove = new Vector3(0, 0, 0);
        //if (move == 1) { directionMove.x = 1; }
        //if (move == 2) { directionMove.x = -1; }
        //if (move == 3) { directionMove.z = 1; }
        //if (move == 4) { directionMove.z = -1; }

        //movement.moveDirection = directionMove;

        CheckContinuosReward();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");

        //ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        //if (Input.GetKey(KeyCode.D))
        //{
        //    discreteActions[0] = 1;
        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    discreteActions[0] = 2;
        //}
        //else if (Input.GetKey(KeyCode.W))
        //{
        //    discreteActions[0] = 3;
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    discreteActions[0] = 4;
        //}
    }
    #endregion

    #region MonoBehaviour method

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == tagWalls)
        {
            AddReward(p_hitWalls);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == tagExit)
        {
            AddReward(r_exitHit);
            EndEpisode();
        }
    }
    #endregion

    #region Custom methods
    private void CheckContinuosReward()
    {
        // Penalizamos el tardar
        AddReward(p_perStep);

        // Premiamos que se acerque y que tarde poco
        float distanceToExit = GetDistanceExit();
        float deltaDistance = previousDistance - distanceToExit;
        AddReward(deltaDistance * r_approachStep);
        previousDistance = distanceToExit;
    }

    private float GetDistanceExit()
    {
        return Vector3.Distance(movement.transform.localPosition, exit.transform.localPosition);
    }
    #endregion
}
