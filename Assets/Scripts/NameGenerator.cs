using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Linq;

public class NameGenerator
{
    private static NameGenerator _instance;

    private static object _lock = new object();
    public static NameGenerator Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new NameGenerator();
                }
                return _instance;
            }
        }
    }
    protected NameGenerator() { } // Guarantee it'll be a singleton only

    List<Persona> personas = new List<Persona>();

    public void Initialize(string source)
    {
        // For first letter caps
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        string[] lines = source.Split('\n');
        foreach(string l in lines)
        {
            string[] tabs = l.Split('\t');
            Persona p = new Persona();
            p["name"] = textInfo.ToTitleCase(tabs[0].ToLower());
            p["gender"] = tabs[1].ToLower();
            personas.Add(p);
        }
    }

    public string GetName()
    {
        return GetName(null, null);
    }

    public string GetName(string property, string val)
    {
        if (property == null && val == null)
            return personas[Random.Range(0, personas.Count)]["name"];
        else
        {
            // TODO: This is probably horribly inefficient. Some better way to get a random item that matches a query
            List<Persona> matching = new List<Persona>(from p in personas where p[property] == val select p);
            return matching[Random.Range(0,matching.Count())]["name"];
        }
    }

    public Persona GetPersona()
    {
        return personas[Random.Range(0, personas.Count)];
    }
}
