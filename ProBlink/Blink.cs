using System;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using SharpDX;

namespace ProBlink
{
    internal class Blink
    {
        #region Static Fields

        private static Hero me;

        private static Item blink;

        public static bool QuickCast { get; set; }
        public static bool Aiming;
        private static bool blinkKey;
        private static bool loaded;

        public static bool LButton;

        private const int KeyDown = 0x0100;
        private const int RButtonDown = 0x0204;
        private const int LButtonDown = 0x0201;

        #endregion

        #region OnLoad

        public static void OnLoad()
        {
            try
            {
                loaded = false;
                blink = null;

                DrawHandler.Load();

                Game.OnWndProc += OnWndProc;
                Game.OnUpdate += OnUpdate;

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
            if (!Game.IsInGame || !loaded)
            {
                return;
            }

            if (args.WParam == 1 || !Game.IsChatOpen || Utils.SleepCheck("clicker"))
            {
                LButton = true;
            }
            else
            {
                LButton = false;
            }

            if (DrawHandler.MouseOn(1850, 442, 15, 15))
            {
                QuickCast = !QuickCast;
                Utils.Sleep(250, "clicker");
            }

            if (blink != null)
            {
                if (args.Msg == KeyDown && args.WParam == 'D' && !Game.IsChatOpen)
                {
                    if (QuickCast)
                    {
                        blinkKey = true;
                    }
                    Aiming = true;
                }

                if (Aiming && args.Msg == RButtonDown)
                {
                    Aiming = false;
                }

                if (Aiming && args.Msg == LButtonDown)
                {
                    blinkKey = true;
                    return;
                }
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
                blink = me.FindItem("item_blink");
                loaded = true;
            }

            if (!Game.IsInGame || me == null)
            {
                blink = null;
                loaded = false;
                return;
            }

            if (Game.IsPaused)
            {
                return;
            }

            if (blink == null)
            {
                blink = me.FindItem("item_blink");
            }

            MaxDistanceBlink();
        }

        #endregion

        #region MaxDistanceBlink

        private static void MaxDistanceBlink()
        {
            if (blinkKey)
            {
                if (blink != null)
                {
                    var distance =
                        Math.Sqrt(
                            Math.Pow(Game.MousePosition.X - me.Position.X, 2)
                            + Math.Pow(Game.MousePosition.Y - me.Position.Y, 2));

                    if (distance > 0)
                    {
                        if (distance > 1200 && blink.Cooldown == 0)
                        {
                            var expectedX = ((Game.MousePosition.X - me.Position.X) / distance) * 1199 + me.Position.X;
                            var expectedY = ((Game.MousePosition.Y - me.Position.Y) / distance) * 1199 + me.Position.Y;
                            var blinkPos = new Vector2((float)expectedX, (float)expectedY);

                            blink.UseAbility((Vector3)blinkPos);
                            blinkKey = false;
                            Aiming = false;
                        }
                        else
                        {
                            var blinkPos = new Vector2(Game.MousePosition.X, Game.MousePosition.Y);
                            blink.UseAbility((Vector3)blinkPos);
                            blinkKey = false;
                            Aiming = false;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
