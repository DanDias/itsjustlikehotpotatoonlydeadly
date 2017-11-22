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
            Source.ThrowGrenade();
            base.Execute();
        }
    }
}