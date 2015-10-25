using System;
using Ensage;
using Ensage.Common;
using SharpDX;

namespace BlackFeeder
{
    internal class Feeder
    {
        #region Static Fields

        private static Hero me;

        public static bool LButton;

        private static bool loaded;
        public static bool EnableFeed { get; set; }

        private static readonly Vector3 DireSpawn = new Vector3(7149, 6696, 383);
        private static readonly Vector3 RadiantSpawn = new Vector3(-7149, -6696, 383);

        #endregion

        #region OnLoad

        public static void OnLoad()
        {
            try
            {
                loaded = false;

                DrawHandler.Load();

                Game.OnUpdate += OnUpdate;
                Game.OnWndProc += OnWndProc;

                Drawing.OnPreReset += DrawHandler.OnPreReset;
                Drawing.OnPostReset += DrawHandler.OnPostReset;
                Drawing.OnEndScene += DrawHandler.OnEndScene;
                AppDomain.CurrentDomain.DomainUnload += DrawHandler.DomainUnload;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }

        #endregion

        #region OnWndProc

        private static void OnWndProc(WndEventArgs args)
        {
            if (args.WParam != 1 || Game.IsChatOpen || !Utils.SleepCheck("clicker"))
            {
                LButton = false;
                return;
            }
            else
            {
                LButton = true;
            }

            if (DrawHandler.MouseOn(DrawHandler.X + 150, DrawHandler.Y + 40, 15, 15))
            {
                EnableFeed = !EnableFeed;
                Utils.Sleep(250, "clicker");
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

            if (EnableFeed)
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
