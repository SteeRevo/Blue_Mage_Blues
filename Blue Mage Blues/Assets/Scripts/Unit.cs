using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int dmg){
        currentHP -= dmg;

        if(currentHP <= 0){
            currentHP = 0;
            return true;
        }
        return false;
    }

    public void HealDamage(int health){
        currentHP += health;
        currentHP = Mathf.Min(maxHP, currentHP);
    }
  
}
