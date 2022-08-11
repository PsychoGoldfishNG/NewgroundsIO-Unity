using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    public SaveSlot Slot1Object;
    public SaveSlot Slot2Object;
    public SaveSlot Slot3Object;

    /// <summary>An event to be dispatched whenever the server responds to a call.</summary>
    public event OnSetCloudSaveData SetData;

    private List<SaveSlot> slots = null;
    // Start is called before the first frame update
    void Init()
    {
        if (slots is null) {
            slots = new List<SaveSlot>() {
                Slot1Object,
                Slot2Object,
                Slot3Object
            };

            slots.ForEach(slot => {
                slot.SetData += this.OnSetData;
            });
        }
    }

    public void Open()
    {
        Init();

        this.gameObject.SetActive(true);
        slots.ForEach(slot => {
            slot.Refresh();
        });
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    void OnSetData(SaveSlot slot)
    {
        SetData?.Invoke(slot);
    }
}
