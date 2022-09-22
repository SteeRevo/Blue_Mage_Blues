using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistance : MonoBehaviour
{

    public Unit playerUnit;
    // Start is called before the first frame update
    void Start()
    {
        playerUnit.setStats();
        Debug.Log(GameControl.playerUnitData.health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
