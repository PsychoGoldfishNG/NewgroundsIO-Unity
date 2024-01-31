using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class NgioAppConnector : MonoBehaviour
{
    /** ============================= public variables (set in inspector) ============================= **/

    [Header("API Config")]
    [Tooltip("The APP ID from your API Tools page on Newgrounds.")]
    public string AppID;
    [Tooltip("The AES-128 encryption key from your API Tools page on Newgrounds.")]
    public string AesKey;
    [Tooltip("Check to enabe debug mode. Disable before publishing your game!")]
    public bool DebugMode = false;
    [Space(12)]
    [Tooltip("An optional version number to compare to the version set on your API Tools page on Newgrounds\nUse X.X.X format, or leave blank if you aren't using this feature")]
    public string Version = "";

    [Header("Preloading options (disable any you don't use)")]
    [Tooltip("Will preload all your medals, and note if the player has them unlocked yet")]
    public bool PreloadMedals = false;
    [Tooltip("Will preload all your scoreboards")]
    public bool PreloadScoreBoards = false;
    [Tooltip("Will preload the user's save slot information")]
    public bool PreloadSaveSlots = false;
    [Space(12)]
    [Tooltip("Will automatically check to see if hosting website is allowed to host this game")]
    public bool AutoCheckHostLicense = false;
    [Tooltip("Will automatically log a new view of this game")]
    public bool AutoLogView = false;
    [Space(12)]
    [Tooltip("Set a function to call when everything is loaded")]
    public EventTrigger.TriggerEvent OnPlayerDataLoaded;

    [Header("UI Objects")]
    [Tooltip("The text object shown when the user needs to log in.")]
    public GameObject PleaseLogInText;
    [Tooltip("The \"Please Wait...\" object to show when things are loading.")]
    public GameObject PleaseWaitObject;
    [Tooltip("The text object shown when the user has logged in. The string \"<USERNAME>\" will be replaced with the user's actual name.")]
    public GameObject LoggedInText;
    [Tooltip("The text object shown when a website is hosting an illegal version of this game.")]
    public GameObject HostIsIllegalText;
    [Space(12)]
    [Tooltip("A button that will be used to load the login page in a new browser tab.")]
    public GameObject LoginButton;
    [Tooltip("A button that will skip logging in altogether.")]
    public GameObject DontLoginButton;
    [Tooltip("A button that can cancel a pending login attempt.")]
    public GameObject CancelLoginButton;
    [Tooltip("A button to log a user out.")]
    public GameObject LogoutButton;
    [Tooltip("A button to load the newest version of your game.")]
    public GameObject NewVersionAvailableButton;
    [Tooltip("A button to load a legal version of your game.")]
    public GameObject PlayLegalVersionButton;
    [Space(12)]
    public GameObject Background;

    /** ============================= private vars ============================= **/

    // This will be your default LoggedInTexttext. This script will replace <USERNAME> with an actual name.
    private string LoggedInTextTemplate = "";
    
    private Dictionary<string, List<GameObject>> objectGroups = new Dictionary<string, List<GameObject>>();

    /** ============================= MonoBehaviour Calls ============================= **/

    // Start is called before the first frame update
    void Start()
    {
        // Set up the options for NGIO
        var options = new Dictionary<string,object>() 
        {
            { "version",            Version },
            { "debugMode",          DebugMode },
            { "checkHostLicense",   AutoCheckHostLicense },
            { "autoLogNewView",     AutoLogView },
            { "preloadMedals",      PreloadMedals },
            { "preloadScoreBoards", PreloadScoreBoards },
            { "preloadSaveSlots",   PreloadSaveSlots },
        };

        // initialize the API
        NGIO.Init(AppID, AesKey, options);
        
        // hide all of our UI elements. The rest of this script will determine what to show
        this.HideUIElements();
        
        // =============================== Button handlers ======================================= //

        if (DontLoginButton is not null) 
            DontLoginButton.GetComponent<Button>().onClick.AddListener(this.OnDontLoginButtonClick);

        if (LoginButton is not null) 
            LoginButton.GetComponent<Button>().onClick.AddListener(this.OnLoginButtonClick);

        if (PlayLegalVersionButton is not null) 
            PlayLegalVersionButton.GetComponent<Button>().onClick.AddListener(this.OnPlayLegalVersionButtonClick);

        if (NewVersionAvailableButton is not null) 
            NewVersionAvailableButton.GetComponent<Button>().onClick.AddListener(this.OnNewVersionAvailableButtonClick);

        if (CancelLoginButton is not null) 
            CancelLoginButton.GetComponent<Button>().onClick.AddListener(this.OnCancelLoginButtonClick);

        if (LogoutButton is not null) 
            LogoutButton.GetComponent<Button>().onClick.AddListener(this.OnLogoutButtonClick);

        // get text template for "Logged In" message
        if (LoggedInTextTemplate is not null) 
            LoggedInTextTemplate = LoggedInText.GetComponent<TextMeshProUGUI>().text;

        objectGroups.Add("LoginOverlay", new List<GameObject> {
            Background, PleaseLogInText, LoginButton, DontLoginButton
        });
        objectGroups.Add("WaitingOverlay", new List<GameObject> {
            Background, PleaseWaitObject
        });
        objectGroups.Add("LoginPageOpenOverlay", new List<GameObject> {
            Background, PleaseWaitObject, CancelLoginButton
        });
        objectGroups.Add("BadHostOverlay", new List<GameObject> {
            Background, PlayLegalVersionButton, HostIsIllegalText
        });
        objectGroups.Add("LoggedInMessage", new List<GameObject> {
            LoggedInText, LogoutButton
        });
        objectGroups.Add("NewVersionMessage", new List<GameObject> {
            NewVersionAvailableButton
        });
        objectGroups.Add("Background", new List<GameObject> {
            Background
        });

    }

    // Runs once per frame
    void Update()
    {
        /** 
         * Even though we call this on every frame, it will only trigger OnConnectionStatusChanged
         * when there is an actual status change
         **/
        StartCoroutine(NGIO.GetConnectionStatus(OnConnectionStatusChanged));
        StartCoroutine(NGIO.KeepSessionAlive());
    }

    public void OnConnectionStatusChanged(string status) {

        // hide all of our UI elements and let the code below decide what to show
        HideUIElements();

        // This is an illegal copy of the game
        if (!NGIO.legalHost) {

            ShowObjectGroup("BadHostOverlay");
            
        // The user clicked the login button
        } else if (NGIO.loginPageOpen) {

            ShowObjectGroup("LoginPageOpenOverlay");

        } else {

            // There's a newer version of the game available
            if (NGIO.isDeprecated) NewVersionAvailableButton.SetActive(true);
        
            switch(status) {

                case NGIO.STATUS_CHECKING_LOCAL_VERSION:
                    ShowObjectGroup("WaitingOverlay");
                    break;

                case NGIO.STATUS_LOGIN_REQUIRED:
                    ShowObjectGroup("LoginOverlay");
                    break;

                case NGIO.STATUS_PRELOADING_ITEMS:
                    ShowObjectGroup("WaitingOverlay");
                    break;

                case NGIO.STATUS_READY:
                    
                    // get rid of the overlay background
                    HideObjectGroup("Background");

                    // if we have a logged in user, show the message and logout button
                    if (NGIO.hasUser) {
                        LoggedInText.GetComponent<TextMeshProUGUI>().text = LoggedInTextTemplate.Replace("<USERNAME>", NGIO.user.name);
                        ShowObjectGroup("LoggedInMessage");
                    }


                    // if we set up an OnPlayerDataLoaded handler, call it now
                    if (OnPlayerDataLoaded is not null) {
                        BaseEventData eventData = new BaseEventData(EventSystem.current);
                        eventData.selectedObject = this.gameObject;
                        OnPlayerDataLoaded.Invoke(eventData);
                    }
                    break;
            }
        }
    }

    private void ShowObjectGroup(string groupName)
    {
        objectGroups[groupName].ForEach(obj => {
            obj.SetActive(true);
        });
    }

    private void HideObjectGroup(string groupName)
    {
        objectGroups[groupName].ForEach(obj => {
            obj.SetActive(false);
        });
    }

    /** ============================= UI Element Handlers ============================= **/

    /// <summary>
    /// Hides all of the NGIO UI Elements
    /// </summary>
    public void HideUIElements()
    {
        GameObject[] elements =
        {
            PleaseLogInText,
            LoggedInText,
            PleaseWaitObject,
            HostIsIllegalText,
            LoginButton,
            DontLoginButton,
            CancelLoginButton,
            LogoutButton,
            PlayLegalVersionButton,
            NewVersionAvailableButton,
        };
        
        foreach (GameObject uiElement in elements)
        {
            if (uiElement is not null) uiElement.SetActive(false);
        }
    }

    /// <summary>
    /// Hide the login button & text, and open the login page in a new browser tab.
    /// This will change the session status to "waiting-for-user"
    /// </summary>
    public void OnLoginButtonClick()
    {
        NGIO.OpenLoginPage();
    }

    /// <summary>
    /// Hide the login button & text, and open the login page in a new browser tab.
    /// This will change the session status to "waiting-for-user"
    /// </summary>
    public void OnDontLoginButtonClick()
    {
        NGIO.SkipLogin();
    }

    /// <summary>
    /// Handler for CancelLoginButton
    /// Hides the button, and cancels the current login attempt
    /// </summary>
    public void OnCancelLoginButtonClick()
    {
        if (CancelLoginButton is not null) CancelLoginButton.SetActive(false);
        NGIO.CancelLogin();
    }

    /// <summary>
    /// Handler for LogoutButton
    /// Hides the button, and tells the API to log the user out, then call OnSessionUpdated when finished.
    /// </summary>
    public void OnLogoutButtonClick()
    {
        if (LogoutButton is not null) LogoutButton.SetActive(false);
        StartCoroutine(NGIO.LogOut());
    }

    /// <summary>
    /// Handler for NewVersionAvailableButton
    /// Loads the official version, as defined on your Newgrounds APITools page.
    /// </summary>
    public void OnNewVersionAvailableButtonClick()
    {
        NGIO.LoadOfficialUrl();
    }

    /// <summary>
    /// Handler for PlayLegalVersionButton
    /// Loads the official version, as defined on your Newgrounds APITools page.
    /// </summary>
    public void OnPlayLegalVersionButtonClick()
    {
        NGIO.LoadOfficialUrl();
    }

}
