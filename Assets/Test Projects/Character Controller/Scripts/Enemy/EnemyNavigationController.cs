using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyNavigationController : MonoBehaviour
{
    EnemyInputController inputController;
    Transform target;
    private void Start()
    {
        inputController = GetComponent<EnemyInputController>();
    }
    public Vector3 GetMoveDirection()
    {
        //Default if the path is unobstructed
        try
        {
            target = inputController.target;
            Vector3 direction = target.position - this.transform.position;

            //Do a Raycast to see if the path to the target is obstructed.
            Ray ray = new Ray(this.transform.position, target.transform.position - this.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.transform != target)
                {
                    //The path is obstructed
                    //Use the NavMesh to determine the path to the target
                    NavMeshPath path = new NavMeshPath();
                    NavMesh.CalculatePath(this.transform.position, target.position, NavMesh.AllAreas, path);
                    //Set direction to the direction from this position to the next nav mesh path position.
                    if (path.corners.Length > 1)
                    {
                        direction = path.corners[1] - this.transform.position;
                    }
                }
            }
            return direction;
        }
        catch (UnassignedReferenceException e) {
            return RandomDirection().normalized;
        }
    }


    private Vector3 RandomDirection() {
        float rX, rZ;
        Random.InitState(this.gameObject.name.Length * (int)(Time.deltaTime * (1.0f/7)));
        rX = Random.Range(-1.0f, 1.0f);
        rZ = Random.Range(-0.50f, 0.50f);
        return (this.transform.forward * rZ) + (this.transform.right * rX);
    }
}
