using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    //Text from the UI
    public Text initial;
    public Text speech;

    //Button object
    public Button ClickHere;

    //Cursor/reticle object
    public Image customPointer;

    /* At Start() it makes the cursor disappear, and once the 'Start button is pressed, then
    it calls on UpdateTextAndButton() method */
    void Start()
    {
        customPointer.enabled = false;
        ClickHere.onClick.AddListener(UpdateTextAndButton);
    }
    /* This makes the cursor reappear on the UI, and it clears the text on the UI as well as make the button disappear. */
    void UpdateTextAndButton()
    {
        customPointer.enabled = true;
        speech.text = "";
        initial.text = "";
        ClickHere.gameObject.SetActive(false);
    }
}
