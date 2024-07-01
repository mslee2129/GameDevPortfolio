using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Normal,
    ExtraLife,
    ExtraTime
}
public class Brick : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemType currentItemType = ItemType.Normal;
    void Start()
    {
        gameObject.tag = "Brick";
    }

    public void TurnIntoItem() {
        ItemType randomItemType = (ItemType) Random.Range(1, 3);
        if(this.currentItemType == ItemType.Normal) {
            switch(randomItemType) {
            case (ItemType) 1:
                GetComponent<Renderer>().material.color = Color.green; // extra life
                break;
            case (ItemType) 2:
                GetComponent<Renderer>().material.color = Color.red; // extra time
                break;
            }
            this.currentItemType = randomItemType;
            Debug.Log("Brick turned into item: " + randomItemType);
        }
    }

    public ItemType GetItemType() {
        return this.currentItemType;
    }
}
