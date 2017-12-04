using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{ 
    public class Throw : Skill
    {
        public Throw() : base()
        {
            Name = "Throw";
            Description = "Throw a grenade.";
            Mode = SelectMode.Enemy;
        }

        public override void Execute()
        {
            //Debug.LogFormat ("Throwing grenade");
            if (Source.Target == null)
                System.Console.WriteLine("Select a target");
            else
            {
                if (Source.Grenades.Count == 0)
                {
                    Grenade g = new Grenade(3);
                    //Grenade g = new Grenade(1); // For debugging grenade explodes
                    g.SetPosition(Source.Position);
                    Source.AddGrenade(g);
                }
                Source.ThrowGrenade();
            }
            base.Execute();
        }
    }
}