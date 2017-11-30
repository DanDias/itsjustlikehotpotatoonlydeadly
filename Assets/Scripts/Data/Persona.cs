using System.Collections.Generic;

public class Persona
{
    public string this[string key]
    {
        get
        {
            if (properties.ContainsKey(key))
                return properties[key];
            return null;
        }
        set
        {
            properties[key] = value;
        }
    }

    protected Dictionary<string, string> properties = new Dictionary<string, string>();

    public Persona()
    {
    }
}
