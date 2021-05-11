using Pokemon;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    //GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;
    [SerializeField] GameObject buttonwrapper;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    private bool NoIsClicked = false;

    public static DialogController Instance { get; private set; }

    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;

        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;

        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

        //dialogBox = GameObject.FindGameObjectWithTag("DialogUI");

        Instance = this;
        yesButton.onClick.AddListener(TaskOnClick);
        noButton.onClick.AddListener(NoButtonClick);
    }

    Dialog dialog;
    Action onDialogFinished;
    int currentLine = 0;
    bool isTyping;
    bool dialogChoice;

    public bool IsShowing { get; private set; }

    public IEnumerator ShowDialog(Dialog dialog, bool dialogChoice = false, Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        IsShowing = true;
        this.dialog = dialog;
        onDialogFinished = onFinished;

        this.dialog = dialog;
        this.dialogChoice = dialogChoice;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public void HandleUpdate()
    {
        //Debug.Log("Typing in progress: " + isTyping);
        if (Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            GameController.inDialog = true;
            //Debug.Log("Pressed Z in dialog");
            GameController.soundFX = "Button Press";
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else if (dialogChoice)
            {
                //Debug.Log("Make a choice");
                //buttonwrapper.SetActive(true);
                StartCoroutine(HandleDialogChoice());
                Debug.Log("Coroutine ended");
            }
            else if (!dialogChoice)
            {
                /*Debug.Log("End dialog");
                currentLine = 0;
                IsShowing = false;
                dialogBox.SetActive(false);
                onDialogFinished?.Invoke();

                OnCloseDialog?.Invoke();*/
                endDialog();
            }
        }
    }

    public IEnumerator HandleDialogChoice()
    {
        buttonwrapper.SetActive(true);

        while (!NoIsClicked)
        {
            yield return null;
        }
        endDialog();
    }

    public IEnumerator TypeDialog(string dialog)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }

    void endDialog()
    {
        currentLine = 0;
        IsShowing = false;
        dialogBox.SetActive(false);
        buttonwrapper.SetActive(false);
        onDialogFinished?.Invoke();

        OnCloseDialog?.Invoke();
    }

    void TaskOnClick()
    {
        GameController.starterChosen = true;
        NoIsClicked = true;
    }

    void NoButtonClick()
    {
        NoIsClicked = true;
    }
}
