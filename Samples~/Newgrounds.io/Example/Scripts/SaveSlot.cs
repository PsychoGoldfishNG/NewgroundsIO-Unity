using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public delegate void OnGetCloudSaveData(SaveSlot slot);
public delegate void OnSetCloudSaveData(SaveSlot slot);

/**
 * A custom UI used to inform your scripts if a user wants to save or load a specific slot.
 * NOTE: This does not handle the actual loading or saving, you will need to add a deletgate function to GetData or SetData.
 */
public class SaveSlot : MonoBehaviour
{
    public int slotNumber;

    [Tooltip("Check this to use this slot for saving. Leave unchecked for loading")]
    public bool saveMode = false;

    /// <summary>An event to be dispatched whenever the server responds to a call.</summary>
    public event OnGetCloudSaveData GetData;

    /// <summary>An event to be dispatched whenever the server responds to a call.</summary>
    public event OnSetCloudSaveData SetData;

    private bool hasData = false;

    // Start is called before the first frame update
    void Start()
    {
        var button = this.gameObject.transform.Find("SlotButton").gameObject.GetComponent<Button>();
        button.onClick.AddListener(this.OnButtonClicked);

        // make the buttons say Save in saveMode
        if (saveMode) {
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Save";
        }
    }

    public void Refresh()
    {
        var slot = NGIO.GetSaveSlot(slotNumber);
        hasData = (slot is not null && slot.hasData);

        this.gameObject.transform.Find("SlotName").gameObject.GetComponent<TextMeshProUGUI>().text = "Slot "+slotNumber;
        this.gameObject.transform.Find("SlotDate").gameObject.GetComponent<TextMeshProUGUI>().text = hasData ? slot.GetDateTime().ToString() : "No Data";

        if (!saveMode) {
            var button = this.gameObject.transform.Find("SlotButton").gameObject.GetComponent<Button>();
            button.gameObject.SetActive(hasData);
        }
    }

    void OnButtonClicked()
    {
        if (saveMode) {
            SetData?.Invoke(this);
        } else {
            GetData?.Invoke(this);
        }
    }

}
