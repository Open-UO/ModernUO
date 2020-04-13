using System.Collections.Generic;
using Server.Network;

namespace Server.Misc
{
  public static class Paperdoll
  {
    public static void Initialize()
    {
      EventSink.PaperdollRequest += EventSink_PaperdollRequest;
    }

    public static void EventSink_PaperdollRequest(Mobile beholder, Mobile beheld)
    {
      beholder.Send(new DisplayPaperdoll(beheld, Titles.ComputeTitle(beholder, beheld),
        beheld.AllowEquipFrom(beholder)));

      if (ObjectPropertyList.Enabled)
      {
        List<Item> items = beheld.Items;

        for (int i = 0; i < items.Count; ++i)
          beholder.Send(items[i].OPLPacket);

        // NOTE: OSI sends MobileUpdate when opening your own paperdoll.
        // It has a very bad rubber-banding affect. What positive affects does it have?
      }
    }
  }
}