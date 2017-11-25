using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public class Cook : Skill
    {
        public Cook() : base()
        {
            Name = "Cook";
            Description = "Wait a tick.";
            Mode = SelectMode.None;
            MaxCooldown = 2;
        }

        public override bool MeetRequirements()
        {
            // Must have grenades and base requirements
            return Source.Grenades.Count > 0 && base.MeetRequirements();
        }

        public override void Execute()
        {
            // Shouldn't be null, bust just in case
            // Do nothing and it'll advance next turn
            base.Execute();
            Source.ActionCompleted();
        }
    }
}