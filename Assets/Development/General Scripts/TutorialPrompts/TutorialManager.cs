using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    private PlayerInput playerInput;
    PlayerGroundMovement playerMove;
    [SerializeField] protected int tutIndex;
    protected int promptIndex;
    protected int numberOfTimedPrompts = 4;
    [SerializeField] protected List<GameObject> cinematicPrefabList = new();
    [SerializeField] protected int cinematicIndex;
    [SerializeField] protected bool isPlayingCinematic;

    [SerializeField] protected bool hasMoved;
    [SerializeField] protected bool hasJumped;
    protected int jumpCount;
    [SerializeField] protected bool hasTakeoff;
    [SerializeField] protected bool speakWithQ1;
    [SerializeField] protected bool speakWithQ2;
    [SerializeField] protected bool speakWithQ3;
    [SerializeField] protected bool tutComplete;

    private void Start()
    {
        canvasController = FindFirstObjectByType<UI_CanvasController>();
        playerInput = FindFirstObjectByType<PlayerInput>();
        playerMove = playerInput.gameObject.GetComponent<PlayerGroundMovement>();
        TogglePrompt(promptIndex);

    }

    // Update is called once per frame
    void Update()
    {
        switch (tutIndex)
        {
            case 0:
                if (playerInput.actions.FindAction("Move").WasPressedThisFrame() && !tutComplete)
                {
                    hasMoved = true;
                }

                if (hasMoved)
                {
                    canvasController.DestroyPrompt();
                    promptIndex++;
                    tutIndex = 1;

                }
                return;
            case 1:
                TogglePrompt(promptIndex);
                if (playerInput.actions.FindAction("Jump").WasPressedThisFrame() && hasMoved && !hasJumped && !tutComplete)
                {
                    hasJumped = true;
                }
                if (hasJumped)
                {
                    canvasController.DestroyPrompt();
                    promptIndex++;
                    jumpCount = 1;
                    tutIndex = 2;

                }


                return;
            case 2:
                TogglePrompt(promptIndex);
                if (playerMove.GetIsFlying() && hasMoved && hasJumped && !hasTakeoff && !tutComplete && jumpCount > 0)
                {
                    hasTakeoff = true;
                }
                if (hasTakeoff)
                {
                    canvasController.DestroyPrompt();
                    if (playerMove.GetIsFlying() == false)
                    {
                        jumpCount = 0;
                        promptIndex++;
                        tutIndex = 3;
                    }
                }
                return;
            case 3:
                isPlayingCinematic = true;
                SwitchOnCinematic();
                return;
            case 4:
                return;
        }



    }

    void TogglePrompt(int index)
    {
        canvasController.cachedTutPromptIndex = index;
        canvasController.ShowTutorialPrompt();
    }

    void SwitchOnCinematic()
    {
        if (isPlayingCinematic)
        {
            for (int i = 0; i <= cinematicPrefabList.Count;)
            {
                ToggleOverview(cinematicIndex);
                if (cinematicPrefabList[0].GetComponent<CinematicController>().isPlaying == false)
                {
                    cinematicIndex++;
                    i++;
                }
            }
           
            
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
}