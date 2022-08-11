using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomEventTest : MonoBehaviour
{

    [Header("Event Config")]
    [Tooltip("The event name from your API Tools page on Newgrounds.")]
    public string EventName = "Test Event";
    [Tooltip("The referral name from your API Tools page on Newgrounds.")]
    public string ReferralName = "Test Referral";

    [Header("Event UI")]
    public Button LogEventButton;
    public Button LoadReferralButton;

    // Start is called before the first frame update
    void Start()
    {
        // if we have no core, we can't do anything
        if (!NGIO.isInitialized) {
            this.gameObject.SetActive(false);

        // set up the buttons
        } else {
            LogEventButton.onClick.AddListener(this.LogEvent);
            LoadReferralButton.onClick.AddListener(this.LoadReferral);

        }
    }

    // send the custom event
    void LogEvent()
    {
        StartCoroutine(NGIO.LogEvent(EventName, onEventLogged));
    }

    void onEventLogged(string eventName)
    {
        Debug.Log("Logged "+eventName);
    }

    // load the cutom referral
    void LoadReferral()
    {
        NGIO.LoadReferral(ReferralName);
    }
}
