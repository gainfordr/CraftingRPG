using UnityEngine;
using System.Xml;
using System.Xml.Serialization; 
using System.Collections;

// ***********************************************************
// Creator: Ryan Gainford
// Created: March 25, 2017
// Updated: March 26, 2017
//
// Description: Contains the classes for Items, ItemPresets,
//              and ItemDatabase.
// ***********************************************************

// All of the Item classes are here
#region Item
// The base class for all Items
public class Item
{
    
    // Member Variables
    public string Name { get; private set; }
    public enItemType Type { get; private set; }
    public string Description { get; private set; }
    public int Price { get; private set; }
    public int Value { get; private set; }
    public bool CanSell { get; private set; }
    public Sprite Image { get; private set; }

    // Default Construcotr
    // Needed for serialization inside of the ItemDatabase
    private Item() { }

    // The constructor
    public Item( string aName, 
                 enItemType aType, 
                 string aDescription,
                 int aPrice, 
                 int aValue, 
                 bool aCanSell,
                 Sprite aImage )
    {

        Name = aName;
        Type = aType;
        Description = aDescription;
        Price = aPrice;
        Value = aValue;
        CanSell = aCanSell;
        Image = aImage; 

    }

}


// For items that can be used such as Potions and Bombs
public class Usable : 
             Item
{

    public Usable( string aName, enItemType aType, string aDescription, int aPrice, int aValue, bool aCanSell, Sprite aImage ) :
                       base( aName, aType, aDescription, aPrice, aValue, aCanSell, aImage )
    {

    }

}


// For items that can be equipped as Armour such as Chestplates
public class Equipment :
             Item
{

    public Equipment( string aName, enItemType aType, string aDescription, int aPrice, int aValue, bool aCanSell, Sprite aImage ) :
                      base( aName, aType, aDescription, aPrice, aValue, aCanSell, aImage )
    {

    }

}
#endregion


// All of the Item Preset Classes are here
#region Item Preset

#endregion


// The Item Database that holds all of the created Items
public class ItemDatabase : 
             SerializableDictionary<string, Item>
{

    protected override string itemName { get { return "Item"; } }
    protected override string keyName { get { return "ItemName"; } }
    protected override string valueName { get { return "ItemObject"; } }

}