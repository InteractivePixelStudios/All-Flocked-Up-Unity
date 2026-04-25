using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UI;
using System;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    private PlayerInput playerInput;
    PlayerGroundMovement playerMove;
    PlayerFlightMovement playerFlight;
    [SerializeField] protected int tutIndex;
    [SerializeField] protected int promptIndex;
    protected int numberOfTimedPrompts = 4;
    [SerializeField] protected List<GameObject> cinematicPrefabList = new();
    [SerializeField] protected int cinematicIndex = 0;
    [SerializeField] protected bool isPlayingCinematic;

    [SerializeField] protected bool hasMoved;
    [SerializeField] protected bool hasJumped;
    protected int jumpCount;
    [SerializeField] protected bool introComplete;
    [SerializeField] protected int introIndex = 0;
    [SerializeField] protected bool hasTakeoff;
    [SerializeField] protected bool hasOverview;
    [SerializeField] protected bool speakWithQ1;
    [SerializeField] protected bool speakWithQ2;
    [SerializeField] protected bool speakWithQ3;
    [SerializeField] protected bool tutComplete;
    [SerializeField] protected AchievementUnlocker achievement;
    bool achGiven;


    protected int arrowIndex;

    protected int controlBindIndex = 1;

    protected int keyboardBindIndex = 1;

    private InputAction fireAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction clickAction;

    private void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        playerMove = playerInput.gameObject.GetComponent<PlayerGroundMovement>();
        playerFlight = playerInput.gameObject.GetComponent<PlayerFlightMovement>();
        achievement = GetComponent<AchievementUnlocker>();
        fireAction = playerInput.actions.FindAction("Fire");
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        clickAction = playerInput.actions.FindAction("Click");
        TogglePrompt(promptIndex);
        playerMove.enabled = false;
        playerFlight.enabled = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (tutComplete && !achGiven)
        {
            if (SteamManager.Initialized)
            {
                AchievementList.FindAnyObjectByType<AchievementList>().CompleteAchievement("SteamAch_001_Coo");
                achGiven = true;
            }

        }
        switch (tutIndex)
        {
            case 0:
                if (fireAction.WasPressedThisFrame())
                {
                    playerMove.enabled = true;
                    playerFlight.enabled = true;
                    promptIndex++;
                    UpdatePrompt(promptIndex);
                    SetTutState(1);
                }
                return;
            case 1:
                if (moveAction.WasPressedThisFrame())
                {
                    hasMoved = true;
                }
                if (hasMoved)
                {
                    promptIndex++;
                    SetTutState(2);
                }
                return;
            case 2:
                if (jumpAction.WasPressedThisFrame() && !hasJumped)
                {
                    hasJumped = true;
                }
                if (hasJumped)
                {
                    promptIndex++;
                    jumpCount = 1;
                    SetTutState(3);
                }
                return;
            case 3:
                if (playerMove.GetIsFlying() && !hasTakeoff && jumpCount > 0)
                {
                    hasTakeoff = true;
                }
                if (hasTakeoff)
                {
                    if (playerMove.GetIsFlying() == false)
                    {
                        playerMove.enabled = false;
                        playerFlight.enabled = false;
                        jumpCount = 0;
                        promptIndex++;
                        introIndex++;
                        canvasController.activeTutPrompt.SetArrowIndex(introIndex);
                        SetTutState(4);
                    }
                }
                return;
            case 4:
                if (clickAction.WasPressedThisFrame())
                {
                    if (!introComplete)
                    {


                        if (introIndex > 4)
                        {
                            introComplete = true;
                        }
                        else
                        {
                            promptIndex++;
                            introIndex++;
                            UpdatePrompt(promptIndex);
                            canvasController.activeTutPrompt.SetArrowIndex(introIndex);
                        }
                    }
                    else if (introComplete)
                    {

                        canvasController.activeTutPrompt.SetArrowIndex(-1);
                        promptIndex++;
                        SetTutState(5);

                    }
                }
                return;
            case 5:
                if (cinematicIndex >= cinematicPrefabList.Count && !isPlayingCinematic)
                {
                    SetTutState(6);
                    return;
                }
                var cinematic = cinematicPrefabList[cinematicIndex].GetComponent<CinematicController>();
                if (!isPlayingCinematic)
                {
                    SwitchOnCinematic();
                    return;
                }
                else if (clickAction.WasPressedThisFrame())
                {

                    promptIndex++;
                    UpdatePrompt(promptIndex);
                    if (cinematicIndex < cinematicPrefabList.Count)
                    {
                        cinematicIndex++;
                    }
                    isPlayingCinematic = false;
                    return;
                }
                else if (isPlayingCinematic && !cinematic.isPlaying)
                {


                    promptIndex++;
                    UpdatePrompt(promptIndex);
                    if (cinematicIndex < cinematicPrefabList.Count)
                    {
                        cinematicIndex++;
                    }
                    isPlayingCinematic = false;
                    return;
                }
                return;
            case 6:
                if (clickAction.WasPressedThisFrame())
                {
                    promptIndex++;
                    if (promptIndex >= 15)
                    {
                        SetTutState(7);
                    }
                    UpdatePrompt(promptIndex);
                    IncrementBindIndex();
                    UpdateBindSprites(keyboardBindIndex, controlBindIndex);

                }
                return;
            case 7:
                playerMove.enabled = true;
                playerFlight.enabled = true;
                canvasController.DestroyPrompt();
                tutComplete = true;
                return;
        }
    }

    void SetTutState(int index)
    {
        tutIndex = index;
        UpdatePrompt(promptIndex);
        IncrementBindIndex();
        UpdateBindSprites(keyboardBindIndex, controlBindIndex);
    }

    void TogglePrompt(int index)
    {
        canvasController.cachedTutPromptIndex = index;
        canvasController.cachedIntroIndex = introIndex;
        canvasController.ShowTutorialPrompt();
    }

    void UpdatePrompt(int index)
    {
        canvasController.activeTutPrompt.UpdatePrompt(index);
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
        if (cinematicIndex >= cinematicPrefabList.Count) return;

        if (!cinematicPrefabList[cinematicIndex].activeSelf)
        {
            cinematicPrefabList[cinematicIndex].SetActive(true);
        }

        isPlayingCinematic = true;
    }

}
