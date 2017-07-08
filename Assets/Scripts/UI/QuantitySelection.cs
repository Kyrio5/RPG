using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantitySelection : Menu {

    int quantity = 0;
    public Item myItem;
    public bool sell;

    public TMPro.TextMeshProUGUI ItemDisplay;
    public TMPro.TextMeshProUGUI GilDisplay;
    public TMPro.TextMeshProUGUI quantDisplay;
    public GameObject lessArrow, moreArrow;

    int upperBound;

    void OnEnable()
    {
        quantity = 1;
    }

    public override void updatePointer(int direction)
    {
        switch (direction)
        {
            case 0:
                quantity += 10;
                break;

            case 1:
                quantity -= 10;
                break;

            case 2:
                quantity--;
                break;

            case 3:
                quantity++;
                break;
        }

    }

    public override void MenuUpdate()
    {

    }

    public override void Submit()
    {
        if (!sell)
        {
            if (GameDatabase.Instance.FindSpace(myItem, quantity))
            {
                GameDatabase.Instance.AddItem(myItem, quantity);
                GameDatabase.Instance.Gil -= myItem.value * quantity;
            }
        }
        else
        {
            GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(myItem)].quantity -= quantity;
            GameDatabase.Instance.Gil += (myItem.value / 10) * quantity;
            if(myItem.quantity == 0)
            {
                GameDatabase.Instance.Inventory[GameDatabase.Instance.Inventory.IndexOf(myItem)] = GameDatabase.Instance.ItemDatabase[0];
            }
        }
        Cancel();
    }

    void Update()
    {
        if (sell)
        {
            upperBound = myItem.quantity;
        }
        else
        {
            upperBound = GameDatabase.Instance.Gil / myItem.value;
        }

        if (quantity == -1)
        {
            quantity = upperBound;
        }
        else if (quantity == upperBound + 1)
        {
            quantity = 0;
        }

        quantity = Mathf.Clamp(quantity, 1, Mathf.Min(upperBound, 99));

        ItemDisplay.text = myItem.getNameWithIcon();
        if (quantity == 1)
        {
            lessArrow.SetActive(false);
        }
        else
        {
            lessArrow.SetActive(true);
        }

        if(quantity == upperBound)
        {
            moreArrow.SetActive(false);
        }
        else
        {
            moreArrow.SetActive(true);
        }
        int gilCharge = myItem.value * quantity;
        if (sell)
        {
            gilCharge /= 10;
        }
        GilDisplay.text = gilCharge.ToString();
        quantDisplay.text = "x " + quantity;
    }

    public override void Cancel()
    {
        gameObject.SetActive(false);
        PauseController.Instance.PopMenu();
    }
}
