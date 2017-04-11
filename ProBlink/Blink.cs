using System;
using Ensage;
using Ensage.Common;
using Ensage.Common.Extensions;
using Menu = Ensage.Common.Menu.Menu;
using SharpDX;

namespace ProBlink
{
    using Ensage.Common.Menu;

    internal class Blink
    {
        #region Static Fields

        public static Hero Me;

        private static Item blink;

        public static Menu Menu;
        private static bool loaded;

        #endregion

        #region OnLoad

        public static void OnLoad()
        {
            try
            {
                loaded = false;
                blink = null;

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
                Me = ObjectManager.LocalHero;
                if (!Game.IsInGame || Me == null)
                {
                    return;
                }
                blink = Me.FindItem("item_blink");
                loaded = true;
            }

            if (!Game.IsInGame || Me == null)
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
                blink = Me.FindItem("item_blink");
            }

            if (Menu.Item("cast.quick.enable").GetValue<bool>())
            {
                if (Utils.SleepCheck("blinkCheck"))
                {
                    MaxDistanceBlink();
                }
            }
        }

        #endregion

        #region MaxDistanceBlink

        private static void MaxDistanceBlink()
        {
            if (Menu.Item("blink.key").GetValue<KeyBind>().Active)
            {
                if (blink != null && blink.Cooldown == 0)
                {
                    var distance =
                        Math.Sqrt(
                            Math.Pow(Game.MousePosition.X - Me.Position.X, 2)
                            + Math.Pow(Game.MousePosition.Y - Me.Position.Y, 2));

                    if (distance > 0)
                    {
                        if (distance > 1200)
                        {
                            var expectedX = ((Game.MousePosition.X - Me.Position.X) / distance) * 1199 + Me.Position.X;
                            var expectedY = ((Game.MousePosition.Y - Me.Position.Y) / distance) * 1199 + Me.Position.Y;
                            var blinkPos = new Vector2((float)expectedX, (float)expectedY);

                            blink.UseAbility((Vector3)blinkPos);
                            Utils.Sleep(250, "blinkCheck");
                        }
                        else
                        {
                            var blinkPos = new Vector2(Game.MousePosition.X, Game.MousePosition.Y);
                            blink.UseAbility((Vector3)blinkPos);
                            Utils.Sleep(250, "blinkCheck");
                        }
                    }
                }
            }
        }

        #endregion
    }
}
