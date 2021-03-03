using System.Collections;
using System.Collections.Generic;
using Pokemon;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject exclamation;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public IEnumerator TriggerTrainerBattle(/*PlayerMovement player*/ Vector3 playerPos)
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

        DialogController.Instance.ShowDialog(dialog, () =>
         {
             Debug.Log("Starting Trainer Battle");
         });
      
    }

    public void test()
    {
        Debug.Log("Does this proc");
    }
}
