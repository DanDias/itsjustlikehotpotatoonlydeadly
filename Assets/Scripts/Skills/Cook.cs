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
            return Source.myGrenades.Count > 0 && base.MeetRequirements();
        }

        public override void Execute()
        {
            // Shouldn't be null, bust just in case
            if (Source.myGrenades[0] != null)
                Source.myGrenades[0].ChangeTick(-1);
            base.Execute();
        }
    }
}