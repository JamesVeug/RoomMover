using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [System.Serializable]
    public class MapNodeLink
    {
        public Triggerable lockedBy;
        public MapNode node;

		public bool hasLock { get { return lockedBy != null; } }

        public bool isLocked
        {
            get { return node == null || ( lockedBy != null && lockedBy.IsLocked()); }
        }
    }

	public bool hasLeft { get { return Left.node != null; } }
	public bool hasRight { get { return Right.node != null; } }
	public bool hasUp { get { return Up.node != null; } }
	public bool hasDown { get { return Down.node != null; } }

    public Room room;
    public MapNodeLink Left;
    public MapNodeLink Right;
    public MapNodeLink Up;
    public MapNodeLink Down;

    public List<MapNode> GetNeighbors()
    {
        List<MapNode> neighbors = new List<MapNode>(4);
        if (Left.node != null) neighbors.Add(Left.node);
        if (Right.node != null) neighbors.Add(Right.node);
        if (Up.node != null) neighbors.Add(Up.node);
        if (Down.node != null) neighbors.Add(Down.node);

        return neighbors;
    }

	void OnDrawGizmos()
	{
		if (hasLeft)
		{
			Gizmos.color = GUIColor(Left);
			Gizmos.DrawLine(transform.position, Left.node.transform.position);
		}

		if (hasRight)
		{
			Gizmos.color = GUIColor(Right);
			Gizmos.DrawLine(transform.position, Right.node.transform.position);
		}

		if (hasUp)
		{
			Gizmos.color = GUIColor(Up);
			Gizmos.DrawLine(transform.position, Up.node.transform.position);
		}

		if (hasDown)
		{
			Gizmos.color = GUIColor(Down);
			Gizmos.DrawLine(transform.position, Down.node.transform.position);
		}
	}

	private Color GUIColor(MapNodeLink link)	
	{
		if (link.hasLock)
		{
			return Color.red;
		}

		return Color.green;
	}
}

