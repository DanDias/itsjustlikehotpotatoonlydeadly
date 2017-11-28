using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public class Knockdown : Skill
    {
        public Knockdown() : base()
        {
            Name = "Knockdown";
            Description = "Knock 'em down.";
            Mode = SelectMode.Enemy;
            MaxCooldown = 3;
        }

        public override void Execute()
        {
            foreach (Character c in CharacterTargets)
            {
                c.SetKnockdown(true);
				c.SetGettingUp(false);
            }
            Source.ThrowGrenade();
            base.Execute();
        }
    }
}