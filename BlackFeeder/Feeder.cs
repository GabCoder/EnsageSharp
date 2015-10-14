using System;
using Ensage;
using Ensage.Common;
using SharpDX;
using SharpDX.Direct3D9;

namespace BlackFeeder
{
    internal class Feeder
    {
        private static Hero me;

        private static Font text;

        private static bool loaded;
        private static bool enableFeed = true;

        private static readonly Vector3 DireSpawn = new Vector3(7149, 6696, 383);
        private static readonly Vector3 RadiantSpawn = new Vector3(-7149, -6696, 383);

        public static void OnLoad()
        {
            Game.OnUpdate += OnUpdate;
            loaded = false;
            text = new Font(
                Drawing.Direct3DDevice9,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = 13,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default
                });

            Drawing.OnPreReset += Drawing_OnPreReset;
            Drawing.OnPostReset += Drawing_OnPostReset;
            Drawing.OnEndScene += Drawing_OnEndScene;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainDomainUnload;
            Game.OnWndProc += OnWndProc;
        }

        private static void CurrentDomainDomainUnload(object sender, EventArgs e)
        {
            text.Dispose();
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (Drawing.Direct3DDevice9 == null || Drawing.Direct3DDevice9.IsDisposed || !Game.IsInGame)
            {
                return;
            }

            var player = ObjectMgr.LocalPlayer;
            if (player == null || player.Team == Team.Observer)
            {
                return;
            }

            text.DrawText(
                null,
                enableFeed ? "BlackFeeder: Feeding - ENABLED! | [B] for toggle" : "BlackFeeder: Feeding - DISABLED! | [B] for toggle",
                5,
                96,
                Color.IndianRed);
        }

        private static void Drawing_OnPostReset(EventArgs args)
        {
            text.OnResetDevice();
        }

        private static void Drawing_OnPreReset(EventArgs args)
        {
            text.OnLostDevice();
        }

        private static void OnWndProc(WndEventArgs args)
        {
            if (args.Msg != (ulong)Utils.WindowsMessages.WM_KEYUP || args.WParam != 'B' || Game.IsChatOpen)
            {
                return;
            }
            
            enableFeed = !enableFeed;
        }

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

            if (enableFeed)
            {
                Feed();
            }
        }

        private static void Feed()
        {
            switch (me.Team)
            {
                case Team.Dire:
                    me.Move(RadiantSpawn);
                    break;
                case Team.Radiant:
                    me.Move(DireSpawn);
                    break;
            }
        }
    }
}
