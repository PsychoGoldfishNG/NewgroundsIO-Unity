using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

/** 
 * This is a fake title screen demonstrating how you might 
 * implement the NgioAppConnector into a real game
 */
public class TitleScreenBehaviour : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject MainMenuWrapper;
    public Button NewGameButton;
    public Button ContinueGameButton;
    public Button MoreGamesButton;
    public Button NewgroundsButton;
    public Button AuthorURLButton;

    [Header("Continue Menu")]
    public GameObject ContinueMenuWrapper;
    public SaveSlot SaveSlot1Object;
    public SaveSlot SaveSlot2Object;
    public SaveSlot SaveSlot3Object;
    public Button ExitContinueButton;

    private Dictionary<int,SaveSlot> SaveSlots;

    // Start is called before the first frame update
    void Start()
    {
        NewGameButton.onClick.AddListener(this.StartGame);
        ContinueGameButton.onClick.AddListener(this.OnContinueGameButtonClicked);
        MoreGamesButton.onClick.AddListener(this.OnMoreGamesButtonClicked);
        NewgroundsButton.onClick.AddListener(this.OnNewgroundsButtonClicked);
        AuthorURLButton.onClick.AddListener(this.OnAuthorURLButtonClicked);
        ExitContinueButton.onClick.AddListener(this.OnExitContinueButtonClicked);

        SaveSlots = new Dictionary<int,SaveSlot>() {
            {1, SaveSlot1Object},
            {2, SaveSlot2Object},
            {3, SaveSlot3Object}
        };

        foreach(var (id, slot) in SaveSlots) {
            slot.GetData += OnSaveSlotLoaded;
        }

        // hide all of our UI elements while NGIO stuff is loading
        HideMenus();
    }

    // this will be called once the API has finished loading everything
    public void OnNewgroundsIOReady(BaseEventData e)
    {
        ShowMainMenu();

        foreach(var (id, slot) in SaveSlots)
        {
            slot.Refresh();
        }
    }

    public void SetMenuVisibility(bool mainMenu=true, bool continueMenu=true)
    {
        MainMenuWrapper.SetActive(mainMenu);
        ContinueMenuWrapper.SetActive(continueMenu);
    }

    public void HideMenus()
    {
        SetMenuVisibility(false,false);
    }

    public void ShowMainMenu() 
    {
        SetMenuVisibility(true,false);

        // Disable the continue button if we have no save data
        ContinueGameButton.interactable = NGIO.GetTotalSaveSlots() > 0;
    }

    public void ShowContinueMenu() 
    {
        SetMenuVisibility(false,true);
    }
    
    public void OnSaveSlotLoaded(SaveSlot slot)
    {
        // hide the menus to avoid other actions
        HideMenus();

        StartCoroutine(NGIO.GetSaveSlotData(slot.slotNumber, OnSlotLoaded));
    }

    public void OnSlotLoaded(string data)
    {
        // Pass the loaded data to the SaveSlotData behavior used in TestScene
        SaveSlotData.InitialData = data;

        // start the game
        StartGame();
    }

    public void StartGame() {
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
    }

    public void OnMoreGamesButtonClicked() {
        NGIO.LoadMoreGames();
    }

    public void OnAuthorURLButtonClicked() {
        NGIO.LoadAuthorUrl();
    }

    public void OnNewgroundsButtonClicked() {
        NGIO.LoadNewgrounds();
    }

    public void OnContinueGameButtonClicked() {
        ShowContinueMenu();
    }

    public void OnExitContinueButtonClicked() {
        ShowMainMenu();
    }
}
