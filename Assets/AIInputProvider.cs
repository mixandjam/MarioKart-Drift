using UnityEngine;

public class AIInputProvider : MonoBehaviour
{

    private const float MINIMUM_DISTANCE_TO_POINT = 5f;

    private Vector3? target = null;

    [SerializeField]
    private KartController Kart = null;

    [SerializeField]
    private TrackDataController TrackDataController = null;

    private int nodeIndex = 0;

    private void OnDrawGizmos()
    {
        if (target.HasValue)
        {
            Gizmos.DrawLine(Kart.transform.position, target.Value);
            Gizmos.DrawWireSphere(target.Value, MINIMUM_DISTANCE_TO_POINT);
        }
    }

    private void Update()
    {
        if (Kart == null || TrackDataController == null)
        {
            return;
        }

        if (target.HasValue)
        {
            float distanceFromTarget = Vector3.Distance(target.Value, Kart.transform.position);
            if (distanceFromTarget < MINIMUM_DISTANCE_TO_POINT)
            {
                nodeIndex++;
                if (nodeIndex > TrackDataController.GetNodeLength())
                {
                    nodeIndex = 0;
                }
            }
        }

        target = TrackDataController.GetTargetNode(nodeIndex);

        if (target != null)
        {
            Vector3 directionToTarget = (target.Value - Kart.transform.position).normalized;
            Vector3 targetInLocalSpace = Vector3.Cross(Kart.transform.forward, directionToTarget);

            Kart.Steer(targetInLocalSpace.y * 5f);

            float forwardDot = Vector3.Dot(Kart.transform.forward, directionToTarget);
            if (Mathf.Abs(targetInLocalSpace.y) < .8f && forwardDot > 0f)
                Kart.Accelerate();
        }
    }

}
