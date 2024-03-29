﻿using System.Text.Json.Serialization;

namespace Server.Regions;

// TODO: Implement
public class UnderwaterRegion : BaseRegion
{
    [JsonConstructor] // Don't include parent, since it is special
    public UnderwaterRegion(string name, Map map, int priority, params Rectangle3D[] area) : base(name, map, priority, area)
    {
    }

    public UnderwaterRegion(string name, Map map, Region parent, int priority, params Rectangle3D[] area) : base(name, map, parent, priority, area)
    {
    }

    public UnderwaterRegion(string name, Map map, Region parent, params Rectangle3D[] area) : base(name, map, parent, area)
    {
    }
}
