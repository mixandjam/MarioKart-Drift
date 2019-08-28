using UnityEngine;

public class TrackData : ScriptableObject
{
    [SerializeField]
    private Vector3[] m_trackNodes = null;

    public int GetNodeLength()
    {
        return m_trackNodes.Length;
    }

    public Vector3? GetTargetNode(int index)
    {
        if (index < 0 || index >= m_trackNodes.Length)
        {
            return null;
        }

        return m_trackNodes[index];
    }

    public void SetPoints(Vector3[] poits)
    {
        m_trackNodes = poits;
    }
}
