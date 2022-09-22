using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl control;
    public static PlayerData playerUnitData;
    // Start is called before the first frame update
    void Awake() {
        if(control == null){
            DontDestroyOnLoad(gameObject);
            control = this;
            playerUnitData = new PlayerData();
            playerUnitData.health = 10;
            playerUnitData.experience = 0;
            playerUnitData.damage = 4;
        }
        else if(control != this){
            Destroy(gameObject);
        }
    }



    public class PlayerData{
        public int health;
        public int experience;
        public int damage;
    }

}
