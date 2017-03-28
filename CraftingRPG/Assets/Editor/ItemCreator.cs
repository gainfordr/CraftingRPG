using UnityEditor;
using UnityEngine;

using System.IO;

using System.Xml;
using System.Xml.Serialization;

// ***********************************************************
// Creator: Ryan Gainford
// Created: March 25, 2017
// Updated: March 26, 2017
//
// Description: Class for the custom Item Creator window.
//              
// ***********************************************************

public class ItemCreator : 
             EditorWindow
{

    // Member Variables
    // ****************
    #region GUI 
    static Vector2 windowSize = new Vector2(500.0f, 750.0f);

    int iCurrenTab = 0;
    string[] sToolbarNames = new string[] { "Basic", "Usable", "Equipment", "Modify" };

    GUILayoutOption[] buttonHW = new GUILayoutOption[] { GUILayout.MinWidth(243), GUILayout.MaxWidth(243), GUILayout.MinHeight(25), GUILayout.MaxHeight(25) };
    #endregion

    #region Basic Information
    string sName = "";
    enItemType eType = enItemType.NONE;
    string sDescription = "";
    TextAsset tDescription = null; 
    int iPrice = 0;
    int iValue = 0;
    bool bCanSell = true;
    Sprite sprImage = null;
    #endregion

    // Sets the path for its location in the upper tool bar
    [MenuItem("Tools/Create Editors/Create Item %i")]
    public static void ShowWindow()
    {
        
        // Opens the window
        ItemCreator window = (ItemCreator)EditorWindow.GetWindow( typeof(ItemCreator), 
                                                                  true, 
                                                                  "Item Creator");

        // Sets the size of the window
        window.minSize = windowSize;
        window.maxSize = windowSize; 

        window.Show();

    }

    void OnGUI()
    {

        iCurrenTab = GUILayout.Toolbar(iCurrenTab, sToolbarNames );
        SetCurrentTab( iCurrenTab );

    }

    // The Tab Functions
    // *****************
    #region Editor Layout
    public void SetCurrentTab( int aTab )
    {

        // Changes the current window based on the selected tab
        switch(aTab)
        {

            case 0:
                BasicTab();
                CommonTab();
                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            default:
                Debug.Log("Passed in tab number has either not been handled or doesn't exist.");
                break;

        }

    }

    #region Basic Tab
    public void BasicTab()
    {

            // The setup for everything needed in the Basic Tab
            GUILayout.Label("Basic Information", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();

            // Item Name
            GUILayout.Label("Item Name");
            sName = EditorGUILayout.TextField("", sName);

        GUILayout.EndVertical();
        GUILayout.BeginVertical();

            // Item Type
            GUILayout.Label("Item Type");
            eType = (enItemType)EditorGUILayout.EnumPopup("", eType);

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

            // Description Text
            GUILayout.Label("Description Text");
            sDescription = GUILayout.TextArea(sDescription, GUI.skin.textArea, GUILayout.MinHeight(100));

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

            // Clear Text button
            if (GUILayout.Button("Clear Description Text", buttonHW))
            {
                sDescription = ""; 
                GUIUtility.keyboardControl = 0;
            }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal(); 

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

            // Item Price
            GUILayout.Label("Item Price");
            iPrice = EditorGUILayout.IntField(iPrice);

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();

            // Item Value
            GUILayout.Label("Item Value");
            iValue = EditorGUILayout.IntField(iValue);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        
            // Is the Item Sellable?
            bCanSell = EditorGUILayout.Toggle("Can Sell? :", bCanSell);

        EditorGUILayout.EndHorizontal(); 

        EditorGUILayout.Space(); 

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();

            // Item Image
            GUILayout.Label("Item Image");
            sprImage = (Sprite)EditorGUILayout.ObjectField(sprImage, typeof(Sprite), false);

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();

            // Desciriotn Text File Variant
            GUILayout.Label("Description Text (Text File)");
            tDescription = (TextAsset)EditorGUILayout.ObjectField(tDescription, typeof(TextAsset), false);

        EditorGUILayout.EndVertical(); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

            // Clear Fields buttons
            if (GUILayout.Button("Clear Basic Fields", buttonHW))
                ClearBasicFields();

            if (GUILayout.Button("Clear All Fields", buttonHW))
                ClearAllFields();

        EditorGUILayout.EndHorizontal(); 

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

    }

    public void ClearBasicFields()
    {

        // Sets all the values in the Basic Tab to initial states
        sName = "";
        eType = enItemType.NONE;
        sDescription = "";
        tDescription = null;
        iPrice = 0;
        iValue = 0;
        bCanSell = true;
        sprImage = null;

        GUIUtility.keyboardControl = 0;

    }

    #endregion

    #region Common Tab
    public void CommonTab()
    { 

        GUILayout.BeginHorizontal(); 
        GUILayout.BeginVertical();

            // Preset Buttons
            if (GUILayout.Button("Save Preset", buttonHW))
                SavePreset();

            if (GUILayout.Button("Load Preset", buttonHW))
                LoadPreset(); 

        GUILayout.EndVertical();
        GUILayout.BeginVertical();

            // Create Item Button
            if (GUILayout.Button("Create Item", buttonHW))
            {

                if (CheckForItemEntry())
                {

                    // Defines the path
                    string ItemDatabasePath = "Assets/Databases/ItemDatabase.dat";

                    // checks that the Databases folder exists
                    if (!AssetDatabase.IsValidFolder("Assets/Databases"))
                        AssetDatabase.CreateFolder("Assets", "Databases");

                    string NormDescription = sDescription.Replace(" ", "");
                    Item aItem;     

                    if( NormDescription == "" )
                        aItem = new Item(sName, eType, tDescription.ToString(), iPrice, iValue, bCanSell, sprImage);

                    else
                        aItem = new Item(sName, eType, sDescription, iPrice, iValue, bCanSell, sprImage);

                    SaveItemDatabase(ItemDatabasePath, aItem);

                }

                else
                {

                    // string used for the dialog box that opens if not all entries have been filled in
                    string DialogBoxText = "Item Cannot be created because:\n\n";

                    string NormName = sName.Replace(" ", "");
                    string NormDescription = sDescription.Replace(" ", "");

                    if (NormName == "")
                        DialogBoxText += "Name is empty or only spaces\n";

                    if (NormDescription == "" && tDescription == null)
                        DialogBoxText += "Description is empty or no text file has been given\n";

                    if (NormDescription != "" && tDescription != null)
                        DialogBoxText += "Description has text and a text file has been given\n";

                    if (eType == enItemType.NONE)
                        DialogBoxText += "Item Type is set to NONE\n";

                    if (sprImage == null)
                        DialogBoxText += "Image has not been chosen";

                    EditorUtility.DisplayDialog( "Cannot Create Item",
                                                 DialogBoxText,
                                                 "Close" );

                }

            }

            // Close Button
            if (GUILayout.Button("Close", buttonHW))
                this.Close();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal(); 

    }

    public bool CheckForItemEntry()
    {

        // Remove blank spaces from Name and Description
        string NormName = sName.Replace(" ", "");
        string NormDescription = sDescription.Replace(" ", "");

        // If the requisite fields are not filled in the function returns false
        if ( NormName == "" || 
             eType == enItemType.NONE || 
             (NormDescription == "" && tDescription == null) ||
             (NormDescription != null && tDescription != null) ||
             sprImage == null )
            return false;

        else
            return true;  

    }
    #endregion
   
    public void ClearAllFields()
    {

        // Calls all the Clear functions
        ClearBasicFields();

        GUIUtility.keyboardControl = 0;

    }

    #endregion


    // Item Creation Functions
    // ***********************
    #region Item Database Saving and Loading
    public void SaveItemDatabase( string aPath, Item aItem )
    {

        ItemDatabase itemDatabase = new ItemDatabase();

        // Loads the current ItemDatabase in, or creates a new one
        if (File.Exists(aPath))
        {

            itemDatabase = LoadItemDatabase(aPath);
            itemDatabase.Add(aItem.Name, aItem);

        }

        else
            itemDatabase.Add(aItem.Name, aItem);

        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
        TextWriter writer = new StreamWriter(aPath);

        // Saves the Item Database
        serializer.Serialize(writer, itemDatabase);
        writer.Close();

    }

    public ItemDatabase LoadItemDatabase( string aPath )
    {

        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));

        // Events to handle nodes and attributes that don't belong in the file
        serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
        serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

        ItemDatabase aDatabase = new ItemDatabase();

        // Loads in the Item Database 
        // PROBLEM AREA
        using (FileStream fs = new FileStream(aPath, FileMode.Open))
            aDatabase = (ItemDatabase)serializer.Deserialize(fs);

        return aDatabase; 

    }

    private void serializer_UnknownNode( object sender, XmlNodeEventArgs e )
    {
        // Outputs any Nodes that are not suppose to be there
        Debug.Log("Unkown Node: " + e.Name + "\t" + e.Text);
    }

    private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
    {

        // Outputs any attributes that are not suppose to be there
        XmlAttribute attr = e.Attr;
        Debug.Log("Unknown Attribute: " + attr.Name + "with value of " + attr.Value);

    }
    #endregion


    // Item Preset Functions
    // *********************
    #region Item Preset Saving and Loading
    public void SavePreset()
    {

    }

    public void LoadPreset()
    {

    }
    #endregion

}
