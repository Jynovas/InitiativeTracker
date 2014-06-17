using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Flags]
public enum DD4EDamageType
{
    None = 0x00,
    Unaspected = None << 1,
    Acid = Unaspected << 1,
    Cold = Acid << 1,
    Fire = Cold << 1,
    Force = Fire << 1,
    Lightning = Force << 1,
    Necrotic = Lightning << 1,
    Poison = Necrotic << 1,
    Psychic = Poison << 1,
    Radiant = Psychic << 1,
    Thunder = Radiant << 1,
    All = unchecked((int)~0)
}