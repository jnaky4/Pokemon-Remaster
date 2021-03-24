using System.Collections;
using System.Collections.Generic;
using Pokemon;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject exclamation;
    [SerializeField] string trainerName;

    Character character;

    public bool isBeaten = false;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public IEnumerator TriggerTrainerBattle(/*PlayerMovement player*/ Vector3 playerPos)
    {
        if (isBeaten == false)
        {
            Debug.Log("Starting Trainer Battle");
            exclamation.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            exclamation.SetActive(false);

            //var diff = player.transform.position - transform.position;
            var diff = playerPos - transform.position;
            var moveVec = diff - diff.normalized;
            moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

            yield return character.Move(moveVec);

            yield return StartCoroutine(DialogController.Instance.ShowDialog(dialog, () =>
             {
                 Debug.Log("Starting Trainer Battle");
                 startCombat();
             }));
        }
        //startCombat();
    }

    public void startCombat()
    {
        Debug.Log("Engaging thrusters");
        //trainer = trainerInfo.getName();

        //Dictionary<string, Route> route1_dic = Route.get_route(location);
        Dictionary<string, Trainer> route_trainers = Trainer.get_route_trainers(GameController.location);

        for (int i = 0; i < 6; i++)
        {
            if (route_trainers[trainerName].trainer_team[i] != null)
                GameController.opponentPokemon[i] = route_trainers[trainerName].trainer_team[i];
            else
                break;
        }
        GameController.isCatchable = false;

        if (name == "Brock")
        {
            GameController.music = "Battle Gym Begin";
        }
        else
        {
            GameController.music = "Battle Trainer Begin";
        }
        GameController.triggerCombat = true;

        //fix this later this is a dirty fix
        isBeaten = true;
        reward();
    }

    //badges
    /*public static Dictionary<string, int> badges_completed = new Dictionary<string, int>()
                {{"Rock",1},{"Water",1},{"Electric",1},{"Grass",1},{"Poison",1},{"Psychic",1},{"Fire",1},{"Ground",1}
                };*/

    public void reward()
    {
        if (name == "Brock")
        {
            GameController.badges_completed.Add("Rock", 1);
        }
    }

    public string getName()
    {
        return trainerName;
    }

    public void test()
    {
        Debug.Log("Does this proc");
    }
}
