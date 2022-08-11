using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    [Header("ScoreBoard Config")]
    [Tooltip("The Scoreboard ID from your API Tools page on Newgrounds.")]
    public int ScoreboardID;
    

    [Header("ScoreBoard UI")]
    public TextMeshProUGUI RanksText;
    public TextMeshProUGUI UserNamesText;
    public TextMeshProUGUI ScoresText;
    public TMP_Dropdown PeriodDropDown;
    public TMP_Dropdown SocialDropDown;

    [Header("Score Posting Form")]
    public TMP_InputField EnterScoreInputField;
    public Button PostScoreButton;

    // These should match the default
    public bool Social { get; set; }
    public string Period { get; set; }

    // the API expects single letter values, but this component uses a more readable drop menu
    // These letters match the indexes of the dropdown
    private string[] _periods = new string[]{"D","W","M","Y","A"};

    // Start is called before the first frame update
    void Start()
    {
        // get our default values from the dropdowns
        Social = (SocialDropDown.value == 1);
        Period = _periods[PeriodDropDown.value];

        // set up our ui stuff
        PostScoreButton.onClick.AddListener(this.PostScoreForm);
        PeriodDropDown.onValueChanged.AddListener(OnPeriodChanged);
        SocialDropDown.onValueChanged.AddListener(OnSocialChanged);

        // only logged in users can see their friend's scores, or post new scores
        if (!NGIO.isReady || !NGIO.hasUser) {
            SocialDropDown.gameObject.SetActive(false);
            EnterScoreInputField.gameObject.SetActive(false);
            PostScoreButton.gameObject.SetActive(false);

        }
        
        RanksText.text = "";
        UserNamesText.text = "";
        ScoresText.text = "";
        
        // if NGIO isn't ready, don't bother with anything else
        if (!NGIO.isInitialized) {
            RanksText.text = "Not connected to server.";
        
        // show the scores!
        } else {
            UpdateScoreBoard();
        }
    }

    // handler for the PeriodDropDown
    public void OnPeriodChanged(int index)
    {
        Period = _periods[index];
        UpdateScoreBoard();
    }

    // handler for the SocialDropDown
    public void OnSocialChanged(int index)
    {
        Social = index == 1;
        UpdateScoreBoard();
    }

    // Updates the scoreboard
    public void UpdateScoreBoard()
    {
        // clear the text out
        UserNamesText.text = "";
        ScoresText.text = "";

        // Put up a loading message and call the API
        RanksText.text = "Loading...";

        // if we don't have a user, we can't look up social scores
        var getSocial = NGIO.hasUser ? Social : false;

        // call the API, and run OnScoresLoaded when it responds
        StartCoroutine(NGIO.GetScores(ScoreboardID, Period, null, getSocial, OnScoresLoaded));
    }

    // finishes updating the scoreboard after the API responds
    void OnScoresLoaded(NewgroundsIO.objects.ScoreBoard board, List<NewgroundsIO.objects.Score> scores, string period, string tag, bool social)
    {
        RanksText.text = "";
        UserNamesText.text = "";
        ScoresText.text = "";

        if (scores is null || scores.Count < 1) {
            RanksText.text = "No score data available.";
        } else {
            short rank = 1;
            scores.ForEach(score => {
                if (rank > 1) {
                    RanksText.text += Environment.NewLine;
                    UserNamesText.text += Environment.NewLine;
                    ScoresText.text += Environment.NewLine;
                }

                RanksText.text += rank.ToString() + ":";
                UserNamesText.text += score.user.name;
                ScoresText.text += score.formatted_value;
                rank++;
            });
        }
    }

    // Posts whatever score you put in the EnterScoreInputField
    void PostScoreForm()
    {
        // convert the score to an integer, or throw an error if it's a bad string
        int score = 0;
        if (!Int32.TryParse(EnterScoreInputField.text, out score)) {
            Debug.LogError("Attempted to post \""+EnterScoreInputField.text+"\". Scores must be integers!");
            return;
        }

        // call the API, and run OnScorePosted when it responds
        StartCoroutine(NGIO.PostScore(ScoreboardID, score, null, OnScorePosted));
    }

    // After a score is posted, update the scoreboard again, just in case we made the top 10
    void OnScorePosted(NewgroundsIO.objects.ScoreBoard board, NewgroundsIO.objects.Score score)
    {
        if (score is not null)
        {
            UpdateScoreBoard();
        }
    }
}
