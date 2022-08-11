using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotData : MonoBehaviour
{
    [Header("SaveSlot Data Form")]
    public TMP_InputField SlotDataInputField;
    public Button SaveDataButton;

    [Header("SaveSlot Data Menu")]
    public SaveMenu Menu;

    // set this from your title screen if you are loading a saved game
    public static string InitialData = null;

    // Start is called before the first frame update
    void Start()
    {
        // grab any preloaded data, then clear the initial data out.
        SlotDataInputField.text = InitialData;
        InitialData = null;

        // if the user isn't logged in they can't save.
        // TODO - add PlayerPrefs fallback
        if (!NGIO.hasUser) SaveDataButton.gameObject.SetActive(false);

        SaveDataButton.onClick.AddListener(this.OpenSaveMenu);

        Menu.SetData += this.SaveData;
        Menu.Close();
    }

    void OpenSaveMenu()
    {
        Menu.Open();
    }

    void SaveData(SaveSlot slot)
    {
        StartCoroutine(NGIO.SetSaveSlotData(slot.slotNumber, SlotDataInputField.text, OnSaved));
    }

    void OnSaved(NewgroundsIO.objects.SaveSlot saveSlot)
    {
        Menu.Close();
    }
}
