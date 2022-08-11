using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MedalUnlock : MonoBehaviour
{
    [Header("Medal Config")]
    [Tooltip("The Medal ID from your API Tools page on Newgrounds.")]
    public int medal_id = 27696;

    [Header("Medal UI")]
    public Button UnlockButton;
    public TextMeshProUGUI UnlockedText;

    // Start is called before the first frame update
    void Start()
    {
        if (!NGIO.hasUser) {
            UnlockButton.gameObject.SetActive(false);
            UnlockedText.gameObject.SetActive(false);

        } else {

            // set up the unlock button
            UnlockButton.onClick.AddListener(this.UnlockMedal);

            // update the view using any preloaded medal data
            UpdateMedal(NGIO.GetMedal(medal_id));
        }

    }

    void UpdateMedal(NewgroundsIO.objects.Medal medal)
    {
        // medal isn't loaded or doesn't exist
        if (medal is null) {
            UnlockButton.gameObject.SetActive(true);
            UnlockedText.gameObject.SetActive(false);

        // medal is locked
        } else if (!medal.unlocked) {
            UnlockButton.gameObject.SetActive(true);
            UnlockedText.gameObject.SetActive(false);

        // medal is unlocked
        } else {
            UnlockButton.gameObject.SetActive(false);
            UnlockedText.gameObject.SetActive(true);
        }
    }

    // do the unlock
    void UnlockMedal()
    {
        StartCoroutine(NGIO.UnlockMedal(medal_id, OnMedalUnlocked));
    }

    // update the UI after medal is unlocked
    void OnMedalUnlocked(NewgroundsIO.objects.Medal medal)
    {
        if (medal is not null) {
            UpdateMedal(medal);
        }
    }
}
