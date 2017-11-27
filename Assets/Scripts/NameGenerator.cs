using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

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

    List<string> names = new List<string>();

    public void Initialize(string source)
    {
        // For first letter caps
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        string[] lines = source.Split('\n');
        foreach(string l in lines)
        {
            string[] tabs = l.Split('\t');
            names.Add(textInfo.ToTitleCase(tabs[0].ToLower()));
        }
    }

    public string GetName()
    {
        return names[Random.Range(0, names.Count)];
    }
}
