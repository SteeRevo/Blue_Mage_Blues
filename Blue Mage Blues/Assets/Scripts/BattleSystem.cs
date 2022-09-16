using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public enum BattleState{START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    Unit playerUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogueText;

    public BattleControls playerControls;


    public BattleHud playerHUD;
    public BattleHud enemyHUD;

    private InputAction attack;
    private InputAction heal;

    private void Awake(){
        playerControls = new BattleControls();
    }

    private void OnEnable(){
       attack = playerControls.Battle.Attack;
       attack.Enable();       

       heal = playerControls.Battle.Heal;
       heal.Enable();
    }

    private void OnDisable(){
        attack.Disable();
    }

    
    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

    }

    IEnumerator SetupBattle(){
        GameObject playerGO = Instantiate(playerPrefab);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = enemyUnit.unitName + " approaches.";

        playerHUD.SetHud(playerUnit);
        enemyHUD.SetHud(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        StartCoroutine(PlayerTurn());

    }

    IEnumerator PlayerAttack(){
        dialogueText.text = "You deal " + playerUnit.damage + " damage.";
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(2f);

        if(isDead){
            state = BattleState.WON;
            EndBattle();
        }
        else{
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal(){
        dialogueText.text = "You heal " + 4 + " damage.";
        playerUnit.HealDamage(4);

        playerHUD.SetHP(playerUnit.currentHP);
        
        state = BattleState.ENEMYTURN;

        yield return WaitForInput();
       
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn(){
        dialogueText.text = enemyUnit.unitName + " attacks and deals " + enemyUnit.damage + " damage!";

        yield return new WaitForSeconds(2f);
        
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return WaitForInput();

        if(isDead){
            state = BattleState.LOST;
            EndBattle();
           
        }
        else{
                state = BattleState.PLAYERTURN;
                StartCoroutine(PlayerTurn());
        }
    }

    void EndBattle(){
        if(state == BattleState.WON){
            dialogueText.text = "battle won";
        }
        else if(state == BattleState.LOST){
            dialogueText.text = "YA LOST DIPSHIT";
        }
    }

    IEnumerator PlayerTurn(){
        dialogueText.text = "Choose an action:";
        yield return WaitForInput();
        
    }


    public void OnAttackButton(){
        if(state != BattleState.PLAYERTURN){
            return;
        }
        StartCoroutine(PlayerAttack());
    }

    private IEnumerator WaitForInput(){
        bool done = false;
        while(!done){
            if(attack.triggered && state == BattleState.ENEMYTURN){
                done = true;
            }
            if(attack.triggered && state == BattleState.PLAYERTURN){
                done = true;
                StartCoroutine(PlayerAttack());
            }
            if(heal.triggered && state == BattleState.PLAYERTURN){
                done = true;
                StartCoroutine(PlayerHeal());
            }
           
            yield return null;
        }
    }


   

}
