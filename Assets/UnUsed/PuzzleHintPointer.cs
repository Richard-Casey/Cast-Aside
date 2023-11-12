using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using DG.Tweening;
using Gizmos = UnityEngine.Gizmos;

public class PuzzleHintPointer : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Transform Target;

    [SerializeField] float DistanceFromPlayerForNewPath = 15f;

    NavMeshPath path;
    int CurrentCornerIndex = 1;

    void FixedUpdate()
    {
        if (path == null)
        {
            if (!CalculatePath())return;
        }

        if (!Target || !Player) return;

        if (Vector3.Distance(Player.transform.position, path.corners[CurrentCornerIndex]) < DistanceFromPlayerForNewPath / 2f)
        {
            if (CurrentCornerIndex + 1 < path.corners.Length)
            {
                CurrentCornerIndex++;
            }
        }
        else if (Vector3.Distance(Player.transform.position, path.corners[CurrentCornerIndex]) > DistanceFromPlayerForNewPath)
        {
            CalculatePath();
        }

        Vector3 Direction = (path.corners[CurrentCornerIndex] - Player.transform.position).normalized;
        Direction.y = 0;

        transform.forward = Vector3.Lerp(transform.forward, Direction, .05f);
        //transform.DORotate(Quaternion.ToEulerAngles(Quaternion.LookRotation(Direction, Vector3.up)), .25f);
    }

    bool CalculatePath()
    {
        path=new NavMeshPath();
        CurrentCornerIndex = 1;
        return(NavMesh.CalculatePath(Player.transform.position, Target.position, NavMesh.AllAreas, path));
    }

    public void SetTarget(Transform _transform)
    {
        Target = _transform;
    }

    public void SetPlayer(Transform player)
    {
        Player = player;
    }


    public void OnDrawGizmos()
    {
        if (path != null)
        {
            if (path.corners.Length < 2) return;
            Gizmos.DrawSphere(path.corners[CurrentCornerIndex],5f);
            foreach (var VARIABLE in path.corners)
            {
                Gizmos.DrawSphere(VARIABLE,2f);
            }
        }
    }
}
