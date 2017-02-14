using UnityEngine;

public class MonsterSpawner : Triggerable
{
    public Monster monsterPrefab;
    public Transform monsterParent;
    public MapNode monsterStartNode;
    public Room.Direction startMovingDirection;

    private bool triggered;

    public override void Toggle()
    {
        if (!triggered)
        {
            Monster clone = Instantiate(monsterPrefab, monsterParent, false);
            clone.currentRoom = monsterStartNode;
            clone.SetTargetDirection(startMovingDirection);
            triggered = true;
        }
    }
}
