using System.Collections.Generic;

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

// ***********************************************************
// Creator: Ryan Gainford
// Created: March 25, 2017
// Updated: March 25, 2017
//
// Description: Contains a Generic class of Diciotnary that is
//              XML Serializable. 
// ***********************************************************


// A generic xml serializable dictionary class, is inherited by 
// the various Databases. 
[XmlRoot("dictionary")]
public abstract class SerializableDictionary<TKey, TValue> : 
                      Dictionary<TKey, TValue>, IXmlSerializable
{

    // The root tag names
    protected abstract string itemName { get; }
    protected abstract string keyName { get; }
    protected abstract string valueName { get; }

    public XmlSchema GetSchema()
    {
        return null;
    }

    // Load
    public void ReadXml(XmlReader reader)
    {

        // Serializers for the Keys and Values
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        bool wasEmpty = reader.IsEmptyElement;

        reader.Read();

        // Skips over the node if there is nothing in it
        if (wasEmpty)
            return;

        // Goes through the entire dictionary as long as its not the last node
        while (reader.NodeType != XmlNodeType.EndElement)
        {

            // Starts at node
            reader.ReadStartElement(itemName);

            // Grabs the key
            reader.ReadStartElement(keyName);
            TKey key = (TKey)keySerializer.Deserialize(reader);
            reader.ReadEndElement();

            // Grabs the value
            reader.ReadStartElement(valueName);
            TValue value = (TValue)valueSerializer.Deserialize(reader);
            reader.ReadEndElement();

            // Adds it to the current dictionary
            this.Add(key, value);

            // End of node
            reader.ReadEndElement();

            // Moves to next node in order
            reader.MoveToContent();

        }

        reader.ReadEndElement();

    }

    // Save
    public void WriteXml(XmlWriter writer)
    {

        // Serializers for the keys and values
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        foreach (TKey key in this.Keys)
        {

            // Creates a node to store data
            writer.WriteStartElement(itemName);

            // Writes the key
            writer.WriteStartElement(keyName);
            keySerializer.Serialize(writer, key);
            writer.WriteEndElement();

            // Writes the value
            writer.WriteStartElement(valueName);
            TValue value = this[key];
            valueSerializer.Serialize(writer, value);
            writer.WriteEndElement();

            // Ends the node
            writer.WriteEndElement();

        }

    }

}
