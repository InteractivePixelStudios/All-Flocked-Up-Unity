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
    [SerializeField] protected int tutIndex;
    [SerializeField]protected int promptIndex;
    protected int numberOfTimedPrompts = 4;
    [SerializeField] protected List<GameObject> cinematicPrefabList = new();
    [SerializeField] protected int cinematicIndex = 0;
    [SerializeField] protected bool isPlayingCinematic;

    [SerializeField] protected bool hasMoved;
    [SerializeField] protected bool hasJumped;
    protected int jumpCount;
    [SerializeField] protected bool introComplete;
    [SerializeField]protected int introIndex = -1;
    [SerializeField] protected bool hasTakeoff;
    [SerializeField] protected bool hasOverview;
    [SerializeField] protected bool speakWithQ1;
    [SerializeField] protected bool speakWithQ2;
    [SerializeField] protected bool speakWithQ3;
    [SerializeField] protected bool tutComplete;
    [SerializeField] protected AchievementUnlocker achievement;
    bool achGiven;


    protected int arrowIndex;

    protected int controlBindIndex=1;

    protected int keyboardBindIndex=1;

    private InputAction fireAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction clickAction;

    private void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        playerMove = playerInput.gameObject.GetComponent<PlayerGroundMovement>();
        achievement = GetComponent<AchievementUnlocker>();
        fireAction = playerInput.actions.FindAction("Fire");
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        clickAction = playerInput.actions.FindAction("Click");
        TogglePrompt(promptIndex);

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
                if (!introComplete)
                {
                    if (fireAction.WasPressedThisFrame())
                    {
                        if (introIndex >= 4)
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
                }
                if (introComplete)
                {
                    if (fireAction.WasPressedThisFrame())
                    {
                        promptIndex++;
                        SetTutState(1);
                        canvasController.activeTutPrompt.SetArrowIndex(-1);
                    }
                }
                return;
            case 1:
                if (moveAction.WasPressedThisFrame() && introComplete)
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
                if (jumpAction.WasPressedThisFrame()&& !hasJumped)
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
                        jumpCount = 0;
                        promptIndex++;
                        SetTutState(4);
                    }
                }
                return;
            case 4:
                if (!hasOverview )
                {
                    hasOverview = true;
                }
                if (hasOverview)
                {
                    if (clickAction.WasPressedThisFrame())
                    {
                        promptIndex++;
                        cinematicIndex++;
                        SetTutState(5);
                    }
                }
                return;
            case 5:
                if (cinematicIndex >= cinematicPrefabList.Count)
                {
                    SetTutState(6);
                }
                var cinematic = cinematicPrefabList[cinematicIndex].GetComponent<CinematicController>();
                if (!isPlayingCinematic)
                {
                    SwitchOnCinematic();
                    return;
                }
                if (clickAction.WasPressedThisFrame())
                {
                    cinematicIndex++;
                    if (cinematicIndex < cinematicPrefabList.Count)
                    {
                        cinematicPrefabList[cinematicIndex].SetActive(false);
                    }
                    isPlayingCinematic = false;
                    promptIndex++;
                    UpdatePrompt(promptIndex);
                    return;
                }
                if (!cinematic.isPlaying)
                {
                    cinematicIndex++;
                    cinematicPrefabList[cinematicIndex].SetActive(false);
                    isPlayingCinematic = false;
                    promptIndex++;
                    UpdatePrompt(promptIndex);
                    return;
                }
                return;
            case 6:
                if (hasOverview && clickAction.WasPressedThisFrame())
                {
                    promptIndex++;
                    if (promptIndex > 17)
                    {
                        SetTutState(7);
                    }
                    UpdatePrompt(promptIndex);
                    IncrementBindIndex();
                    UpdateBindSprites(keyboardBindIndex, controlBindIndex);

                }
                return;
            case 7:
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
