using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    private PlayerInput playerInput;
    PlayerGroundMovement playerMove;
    [SerializeField] protected int tutIndex;
    [SerializeField]protected int promptIndex;
    protected int numberOfTimedPrompts = 4;
    [SerializeField] protected List<GameObject> cinematicPrefabList = new();
    [SerializeField] protected int cinematicIndex;
    [SerializeField] protected bool isPlayingCinematic;

    [SerializeField] protected bool hasMoved;
    [SerializeField] protected bool hasJumped;
    protected int jumpCount;
    [SerializeField] protected bool introComplete;
    [SerializeField]protected int introIndex;
    [SerializeField] protected bool hasTakeoff;
    [SerializeField] protected bool hasOverview;
    [SerializeField] protected bool speakWithQ1;
    [SerializeField] protected bool speakWithQ2;
    [SerializeField] protected bool speakWithQ3;
    [SerializeField] protected bool tutComplete;
    [SerializeField] protected AchievementUnlocker achievement;


    protected int arrowIndex;

    protected int controlBindIndex=1;

    protected int keyboardBindIndex=1;

    private void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        playerMove = playerInput.gameObject.GetComponent<PlayerGroundMovement>();
        achievement = GetComponent<AchievementUnlocker>();
        TogglePrompt(promptIndex);

    }

    // Update is called once per frame
    void Update()
    {
        if (tutComplete)
        {
            if (SteamManager.Initialized)
            {
                AchievementList.FindAnyObjectByType<AchievementList>().CompleteAchievement("SteamAch_001_Coo");
            }
            
        }
        switch (tutIndex)
        {
            case 0:
                TogglePrompt(promptIndex);
                if (!introComplete && !tutComplete)
                {
                    if (playerInput.actions.FindAction("Fire").WasPressedThisFrame())
                    {
                        if (introIndex > 5)
                        {
                            introComplete = true;
                        }
                        else
                        {
                            canvasController.DestroyPrompt();
                            promptIndex++;
                            introIndex++;
                            TogglePrompt(introIndex);
                            IncrementBindIndex();
                            if (canvasController.activeTutPrompt.GetNumberArrowPointer() - 1 <= introIndex)
                            {
                                canvasController.activeTutPrompt.SetArrowIndex(introIndex);
                            }
                        }
                    }
                            
                    
                }
                if (introComplete)
                {
                    if (playerInput.actions.FindAction("Fire").WasPressedThisFrame())
                    {
                        canvasController.DestroyPrompt();

                        IncrementBindIndex();
                        tutIndex = 1;
                    }
                }
                return;
            case 1:
                TogglePrompt(promptIndex);

                UpdateBindSprites(keyboardBindIndex, controlBindIndex);
                if (playerInput.actions.FindAction("Move").WasPressedThisFrame() && introComplete && !tutComplete)
                {
                    hasMoved = true;
                }

                if (hasMoved)
                {
                    canvasController.DestroyPrompt();
                    promptIndex++;
                    IncrementBindIndex();
                    tutIndex = 2;

                }
                return;
            case 2:
                TogglePrompt(promptIndex);
                UpdateBindSprites(keyboardBindIndex, controlBindIndex);
                if (playerInput.actions.FindAction("Jump").WasPressedThisFrame() && introComplete && hasMoved && !hasJumped && !tutComplete)
                {
                    hasJumped = true;
                }
                if (hasJumped)
                {
                    canvasController.DestroyPrompt();
                    promptIndex++;
                    jumpCount = 1;
                    IncrementBindIndex();
                    tutIndex = 3;

                }


                return;
            case 3:
                TogglePrompt(promptIndex);
                UpdateBindSprites(keyboardBindIndex, controlBindIndex);
                if (playerMove.GetIsFlying() && introComplete && hasMoved && hasJumped && !hasTakeoff && !tutComplete && jumpCount > 0)
                {
                    hasTakeoff = true;
                }
                if (hasTakeoff)
                {
                    canvasController.DestroyPrompt();
                    if (playerMove.GetIsFlying() == false)
                    {
                        canvasController.ShowPlayerCursor();
                        Cursor.visible = false;
                        jumpCount = 0;
                        promptIndex++;
                        IncrementBindIndex();
                        tutIndex = 4;
                        
                    }
                }
                return;
            case 4:
                TogglePrompt(promptIndex);
                UpdateBindSprites(keyboardBindIndex, controlBindIndex);
                if (!hasOverview && introComplete && hasMoved && hasJumped && hasTakeoff && !tutComplete)
                {
                    hasOverview = true;
                }
                if (hasOverview)
                {
                    if (playerInput.actions.FindAction("Click").WasPressedThisFrame())
                    {
                        canvasController.DestroyPrompt();
                        IncrementBindIndex();
                        tutIndex =5;
                    }
                }
                return;
            case 5:
                isPlayingCinematic = true;
                TogglePrompt(promptIndex);
                UpdateBindSprites(0, 0);
                if (!isPlayingCinematic) return;

                if (cinematicIndex > cinematicPrefabList.Count)
                {
                    canvasController.DestroyPrompt();
                    IncrementBindIndex();
                    tutIndex = 6;
                    isPlayingCinematic = false;
                    return;
                }

                if (playerInput.actions.FindAction("Click").WasPressedThisFrame()|| cinematicPrefabList[cinematicIndex].GetComponent<CinematicController>().isPlaying == false)
                {
                    canvasController.DestroyPrompt();
                    promptIndex++;

                    if (cinematicIndex <= cinematicPrefabList.Count)
                    {
                        SwitchOnCinematic();
                    }
                    else if (cinematicIndex > cinematicPrefabList.Count)
                    {
                        canvasController.DestroyPrompt();
                        IncrementBindIndex();
                        tutIndex = 6;
                        isPlayingCinematic = false;
                    }
                }

                return;
            case 6:
                TogglePrompt(promptIndex);
                UpdateBindSprites(keyboardBindIndex, controlBindIndex);
                if (!hasOverview && introComplete && hasMoved && hasJumped && hasTakeoff && !tutComplete)
                {
                    hasOverview = true;
                }
                if (hasOverview)
                {
                    if (playerInput.actions.FindAction("Click").WasPressedThisFrame())
                    {
                        canvasController.DestroyPrompt();
                        Cursor.visible = true;
                        canvasController.HidePlayerCursor();
                        IncrementBindIndex();
                        tutIndex = 7;
                    }
                }
                return;
            case 7:
                TogglePrompt(promptIndex);
                UpdateBindSprites(keyboardBindIndex, controlBindIndex);
                if (hasOverview)
                {
                    if (playerInput.actions.FindAction("Click").WasPressedThisFrame())
                    {
                        canvasController.DestroyPrompt();
                        Cursor.visible = true;
                        canvasController.HidePlayerCursor();
                        IncrementBindIndex();
                        tutIndex = 8;
                    }
                }
                return;
            case 8:
                TogglePrompt(promptIndex);
                UpdateBindSprites(keyboardBindIndex, controlBindIndex);
                if (hasOverview)
                {
                    if (playerInput.actions.FindAction("Click").WasPressedThisFrame())
                    {
                        canvasController.DestroyPrompt();
                        Cursor.visible = true;
                        canvasController.HidePlayerCursor();
                        IncrementBindIndex();
                        tutIndex = 9;
                    }
                }
                return;
            case 9:
                TogglePrompt(promptIndex);
                UpdateBindSprites(keyboardBindIndex, controlBindIndex);
                if (hasOverview)
                {
                    if (playerInput.actions.FindAction("Click").WasPressedThisFrame())
                    {
                        canvasController.DestroyPrompt();
                        Cursor.visible = true;
                        canvasController.HidePlayerCursor();
                        IncrementBindIndex();
                        tutIndex = 10;
                    }
                }return;
            case 10:
                tutComplete = true;
                return;
        }
    }

    void TogglePrompt(int index)
    {
        canvasController.cachedTutPromptIndex = index;
        canvasController.ShowTutorialPrompt();
    }

    void UpdatePrompt(int index)
    {
        canvasController.cachedTutPromptIndex = index;
        canvasController.activeTutPrompt.UpdatePrompt();
    }

    void UpdateBindSprites(int keyBindIndex, int controllerBindIndex)
    {
        canvasController.activeTutPrompt.UpdateBindSprites(keyBindIndex, controllerBindIndex);
    }

    void IncrementBindIndex()
    {
        canvasController.activeTutPrompt.IncrementBindIndex();
    }


    void SwitchOnCinematic()
    {

        if (!isPlayingCinematic) return;
        cinematicIndex++;
        PlayCine();


    }
         void PlayCine()
        {
        ToggleOverview(cinematicIndex);
            UpdatePrompt(promptIndex);
        }


        void ToggleOverview(int index)
        {
            cinematicPrefabList[index].SetActive(true);
            Debug.Log("playing cine?");
        }

        async void SwitchTimedPrompts()
        {
            for (int i = 0; i < numberOfTimedPrompts;)
            {
                TogglePrompt(promptIndex);
                await Task.Delay(3000);
                canvasController.DestroyPrompt();
                promptIndex++;
                i++;
                if (i == numberOfTimedPrompts)
                {
                    tutIndex = 4;
                }
            }

        }
    }
