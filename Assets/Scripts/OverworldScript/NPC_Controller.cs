using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Controller : MonoBehaviour, Interactable
{
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;
    [SerializeField] Dialog dialog;

    Character character;
    //public GameObject dialogBox;
    //public Text dialogText;
    //public string dialog;

    public bool dialogActive;

    NPCState state;
    float idleTimer = 0f;
    int currentPattern = 0;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Interact(Transform initial)
    {
        if (state == NPCState.Idle)
        {
            /*if (dialogBox.activeInHierarchy)
                dialogBox.SetActive(false);
            else
            {
                dialogBox.SetActive(true);
                //dialogText.text = dialog;
            }*/
            state = NPCState.Dialog;
            character.LookTowards(initial.position);

            StartCoroutine(DialogController.Instance.ShowDialog(dialog, () =>
            {
                idleTimer = 0f;
                state = NPCState.Idle;

            }));
        }
        //StartCoroutine(character.Move(new Vector2(-10, 0)));
    }

    private void Update()
    {
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                if (movementPattern.Count > 0)
                    StartCoroutine(Walk());
            }
        }
        character.HandleUpdate();
    }

    IEnumerator Walk()
    {
        state = NPCState.Walking;

        var oldPosition = transform.position;

        yield return character.Move(movementPattern[currentPattern]);

        if (transform.position != oldPosition)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }

    public enum NPCState { Idle, Walking, Dialog}
}
