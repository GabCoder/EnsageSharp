using System;
using Ensage;
using Ensage.Common;
using Menu = Ensage.Common.Menu.Menu;
using SharpDX;

namespace BlackFeeder
{
    internal class Feeder
    {
        #region Static Fields

        private static Hero me;

        public static Menu Menu;
        private static bool loaded;

        private static readonly Vector3 DireSpawn = new Vector3(7149, 6696, 383);
        private static readonly Vector3 RadiantSpawn = new Vector3(-7149, -6696, 383);

        #endregion

        #region OnLoad

        public static void OnLoad()
        {
            try
            {
                loaded = false;

                MenuGenerator.Load();
                Game.OnUpdate += OnUpdate;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }

        #endregion

        #region OnUpdate

        private static void OnUpdate(EventArgs args)
        {
            if (!loaded)
            {
                me = ObjectMgr.LocalHero;
                if (!Game.IsInGame || me == null)
                {
                    return;
                }
                loaded = true;
            }

            if (!Game.IsInGame || me == null)
            {
                loaded = false;
                return;
            }

            if (Game.IsPaused)
            {
                return;
            }

            if (Menu.Item("Feeding.Activated").GetValue<bool>())
            {
                if (Utils.SleepCheck("feedCheck"))
                {
                    OnFeed();
                }
            }
        }

        #endregion

        #region OnFeed

        private static void OnFeed()
        {
            switch (me.Team)
            {
                case Team.Dire:
                    me.Move(RadiantSpawn);
                    Utils.Sleep(250, "feedCheck");
                    break;
                case Team.Radiant:
                    me.Move(DireSpawn);
                    Utils.Sleep(250, "feedCheck");
                    break;
            }
        }

        #endregion
    }
}
