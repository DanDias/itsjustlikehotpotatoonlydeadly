using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuItem
{
    string Name { get; set; }
    void Execute();
}
