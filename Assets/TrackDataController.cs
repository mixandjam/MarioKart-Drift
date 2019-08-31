using UnityEngine;

public class TrackDataController : MonoBehaviour
{

    [SerializeField]
    public TrackData TrackNodes = null;

    private void OnDrawGizmos()
    {
        if (TrackNodes == null)
        {
            return;
        }

        var length = TrackNodes.GetNodeLength();
        for (int i = 0; i < length; i++)
        {
            var node = TrackNodes.GetTargetNode(i);
            if (!node.HasValue)
            {
                continue;
            }

            Gizmos.DrawSphere(node.Value, .5f);
        }
    }

    public int GetNodeLength()
    {
        if (TrackNodes == null)
        {
            return 0;
        }

        return TrackNodes.GetNodeLength();
    }

    public Vector3? GetTargetNode(int nodeIndex)
    {
        if (TrackNodes == null)
        {
            return null;
        }

        return TrackNodes.GetTargetNode(nodeIndex);
    }
}
