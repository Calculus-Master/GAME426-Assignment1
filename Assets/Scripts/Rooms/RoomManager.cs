using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //TODO: list of enemies
    private List<Kinematic> enemies;
    
    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<Kinematic>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Enemy"))
            {
                enemies.Add(child.GetComponent<Steering>().character);
            }
        }
    }

    public List<Kinematic> GetEnemies()
    {
        return enemies;
    }
}
