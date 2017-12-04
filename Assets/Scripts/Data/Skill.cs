using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Skill : IMenuItem, IXmlSerializable, IComparable<Skill>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Character Source { get; set; }
    public SelectMode Mode { get; set; }
    public int Cooldown { get; set; }
    public int MaxCooldown { get; protected set; }

    public List<Character> CharacterTargets = new List<Character>();
    public List<Grenade> GrenadeTargets = new List<Grenade>();

    public Skill(string name)
    {
        Name = name;
    }

    public Skill()
    {
        Name = "Throw";
    }

    public virtual void ChangeCooldown(int amount)
    {
        Cooldown += amount;
        if (Cooldown < 0)
            Cooldown = 0;
    }
    
    /// <summary>
    /// Execute Event, Override in inheriting classes, but always call base
    /// </summary>
    public virtual void Execute()
    {
        Cooldown = MaxCooldown;
    }

    /// <summary>
    /// Meets requirements. Basic requirement, skill is not on cooldown
    /// </summary>
    /// <returns></returns>
    public virtual bool MeetRequirements()
    {
        return Cooldown == 0;
    }

    #region Saving & Loading
    public XmlSchema GetSchema()
    {
        return null;
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("Name", Name);
        writer.WriteAttributeString("Type", GetType().ToString());
        writer.WriteAttributeString("Description", Description);
        writer.WriteAttributeString("Mode", Mode.ToString());
        writer.WriteAttributeString("MaxCooldown", MaxCooldown.ToString());
    }

    public void ReadXml(XmlReader reader)
    {
        Name = reader.GetAttribute("Name");
        Description = reader.GetAttribute("Description");
        Mode = (SelectMode)Enum.Parse(typeof(SelectMode), reader.GetAttribute("Mode"));
        MaxCooldown = int.Parse(reader.GetAttribute("MaxCooldown"));
    }

    public static Skill Create(XmlReader reader)
    {
        String type = reader.GetAttribute("Type");
        
        switch(type)
        {
            case "Skills.Throw":
                Skills.Throw throwSkill = new Skills.Throw();
                throwSkill.ReadXml(reader);
                return throwSkill;
            case "Skills.Knockdown":
                Skills.Knockdown knockdownSkill = new Skills.Knockdown();
                knockdownSkill.ReadXml(reader);
                return knockdownSkill;
            case "Skills.Cook":
                Skills.Cook cookSkill = new Skills.Cook();
                cookSkill.ReadXml(reader);
                return cookSkill;
            default:
                Skill emptySkill = new Skill();
                emptySkill.ReadXml(reader);
                return emptySkill;
        }
    }

    public int CompareTo(Skill other)
    {
        return String.Compare(Name + Mode.ToString() + Description, other.Name + other.Mode.ToString() + other.Description);
    }
    #endregion
}
