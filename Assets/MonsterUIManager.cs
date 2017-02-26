using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUIManager : MonoBehaviour
{
    private List<Monster> m_monsters;

	void Update ()
    {
		
	}

    public void AddMonster(Monster monster)
    {
        m_monsters.Add(monster);
    }

    public void RemoveMonster(Monster monster)
    {
        m_monsters.Remove(monster);
    }
}
