/***************************************************************************
 *                               VirtueInfo.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id$
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

namespace Server
{
  [PropertyObject]
  public class VirtueInfo
  {
    public VirtueInfo()
    {
    }

    public VirtueInfo(IGenericReader reader)
    {
      int version = reader.ReadByte();

      switch (version)
      {
        case 1: //Changed the values throughout the virtue system
        case 0:
        {
          int mask = reader.ReadByte();

          if (mask != 0)
          {
            Values = new int[8];

            for (int i = 0; i < 8; ++i)
              if ((mask & 1 << i) != 0)
                Values[i] = reader.ReadInt();
          }

          break;
        }
      }

      if (version == 0)
      {
        Compassion *= 200;
        Sacrifice *= 250; //Even though 40 (the max) only gives 10k, It's because it was formerly too easy

        //No direct conversion factor for Justice, this is just an approximation
        Justice *= 500;

        //All the other virtues haven't been defined at 'version 0' point in time in the scripts.
      }
    }

    public int[] Values{ get; private set; }

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public int Humility
    {
      get => GetValue(0);
      set => SetValue(0, value);
    }

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public int Sacrifice
    {
      get => GetValue(1);
      set => SetValue(1, value);
    }

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public int Compassion
    {
      get => GetValue(2);
      set => SetValue(2, value);
    }

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public int Spirituality
    {
      get => GetValue(3);
      set => SetValue(3, value);
    }

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public int Valor
    {
      get => GetValue(4);
      set => SetValue(4, value);
    }

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public int Honor
    {
      get => GetValue(5);
      set => SetValue(5, value);
    }

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public int Justice
    {
      get => GetValue(6);
      set => SetValue(6, value);
    }

    [CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
    public int Honesty
    {
      get => GetValue(7);
      set => SetValue(7, value);
    }

    public int GetValue(int index) => Values?[index] ?? 0;

    public void SetValue(int index, int value)
    {
      Values ??= new int[8];
      Values[index] = value;
    }

    public override string ToString() => "...";

    public static void Serialize(IGenericWriter writer, VirtueInfo info)
    {
      writer.Write((byte)1); // version

      if (info.Values == null)
      {
        writer.Write((byte)0);
      }
      else
      {
        int mask = 0;

        for (int i = 0; i < 8; ++i)
          if (info.Values[i] != 0)
            mask |= 1 << i;

        writer.Write((byte)mask);

        for (int i = 0; i < 8; ++i)
          if (info.Values[i] != 0)
            writer.Write(info.Values[i]);
      }
    }
  }
}
