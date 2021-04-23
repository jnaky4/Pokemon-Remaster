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

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

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
    }

    Dialog dialog;
    Action onDialogFinished;
    int currentLine = 0;
    bool isTyping;

    public bool IsShowing { get; private set; }

    public IEnumerator ShowDialog(Dialog dialog, Action onFinished = null)
    {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        IsShowing = true;
        this.dialog = dialog;
        onDialogFinished = onFinished;

        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    public void HandleUpdate()
    {
        //Debug.Log("Typing in progress: " + isTyping);
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            GameController.inDialog = true;
            Debug.Log("Pressed Z in dialog");
            GameController.soundFX = "Button Press";
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                IsShowing = false;
                dialogBox.SetActive(false);
                onDialogFinished?.Invoke();

                OnCloseDialog?.Invoke();
            }
        }
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
}
