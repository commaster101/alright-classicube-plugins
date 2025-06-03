//set the mininum visit rank for all levels. useful for adding ranks under guest but above banned
using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;

using MCGalaxy;


namespace MCGalaxy
{
    public class setvisit : Plugin
    {
        public override string creator { get { return "LJplays"; } }
        public override string MCGalaxy_Version { get { return "1.9.3.8"; } }
        public override string name { get { return "setglobalvisit"; } }

        public override void Load(bool startup)
        {
            Command.Register(new Cmdsetglobalvisit());
        }

        public sealed class Cmdsetglobalvisit : Command2
        {
            public override string name { get { return "setglobalvisit"; } }
            public override string type { get { return CommandTypes.Other; } }
            public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

            public override void Use(Player p, string message, CommandData data)
            {
                string[] args = message.SplitSpaces();
                Group grp = Matcher.FindRanks(p, args[0]);
                string[] files = LevelInfo.AllMapFiles();
                AccessController access;
                foreach (string f in files)
                {
                    string map = Path.GetFileNameWithoutExtension(f);
                    Level lvl = Matcher.FindLevels(p, map);
                    LevelActions.Load(p, map, true);
                    access = lvl.VisitAccess;
                    access.SetMin(p, data.Rank, lvl, grp);
                    p.Message(map);
                }
            }

            public override void Help(Player p)
            {
                p.Message("&Tmass sets pervisit for every level to use type /setglobalvisit [rank]");
            }
        }

        public override void Unload(bool shutdown)
        {
            Command.Unregister(Command.Find("setglobalvisit"));
        }
    }
}
