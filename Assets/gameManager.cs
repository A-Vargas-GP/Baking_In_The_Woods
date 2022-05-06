using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    //Text for UI
    public Text flourCarried;
    public Text waterCarried;
    public Text saltCarried;
    public Text yeastCarried;
    public Text touchBowl;
    public Text itemsCarried;
    public Text hintTitle;
    public Text hintText;
    public Text hintText2;
    public Text hintText3;

    //Boolean variables for each ingredient
    public static bool flourReceive = false;
    public static bool waterReceive = false;
    public static bool yeastReceive = false;
    public static bool saltReceive = false;
    public static bool touchTheBowl = false;
    public static bool completeIngredients = false;

    //Game objects: bread asset and bowl asset
    public GameObject bread;
    public GameObject bowl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Calls on the UpdateText() method
        UpdateText();

        //Detects if the player acquired all of the ingredients. If true, it calls on EndText()
        if (completeIngredients == true)
        {
            EndText();
        }
    }

    void UpdateText()
    {        
        /*If the time that has happened since the player has selected 'Play' is beyond 30
         seconds, then it will give hints depending on which ingredient that the Player
         has not found */
        if (Time.timeSinceLevelLoad > 30)
        {
            hintTitle.text = "HINTS:";

            if (flourReceive == false || saltReceive == false)
            {
                hintText.text = "-Check the cabinets in the kitchen!";
            }
            
            if (waterReceive == false)
            {
                hintText2.text = "-Look around the outside...";
            }

            if (yeastReceive == false)
            {
                hintText3.text = "-Maybe on top of something?...";
            }
        }
        
        /* If the player selects the needed ingredients, then it appears as text on the UI
        as if it were in their 'inventory', and the hint disappears */

        if (flourReceive == true)
        {
            flourCarried.text = "-Flour";
        }

        if (saltReceive == true)
        {
            saltCarried.text = "-Salt";
        }

        if (flourReceive == true && saltReceive == true)
        {
            hintText.text = "";
        }
        
        if (waterReceive == true)
        {
            waterCarried.text = "-Water";
            hintText2.text = "";
        }

        if (yeastReceive == true)
        {
            yeastCarried.text = "-Yeast";
            hintText3.text = "";
        }

        /*If the player has gotten the flour, yeast, water, and salt, then text appears on the
        screen, telling the player to touch the bowl
        */
        
        if (flourReceive == true && waterReceive == true && yeastReceive == true && saltReceive == true)
        {
            touchBowl.text = "Touch the bowl to make the bread!";
            hintTitle.text = ""; //Clears the 'HINTS' text on the UI
        }

        /* If the Player has touched the bowl, then they have successfully completed the game. */
        if (touchTheBowl == true)
        {
            completeIngredients = true;
        }
    }

    /* Clears each UI text and destroys the bowl, then it moves the breadAsset to the top of the countertop.
    It also states that the game has ended. */
    void EndText()
    {
        flourCarried.text = "";
        waterCarried.text = "";
        yeastCarried.text = "";
        saltCarried.text = "";
        itemsCarried.text = "";

        touchBowl.text = "CONGRATS, YOU MADE BREAD! THE END.";
        Destroy(bowl);
        bread.gameObject.transform.position = new Vector3(464.3f, 1.67f, 509.76f);

    }
}
