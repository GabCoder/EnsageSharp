using System;
using Ensage;
using Ensage.Common;
using SharpDX;
using SharpDX.Direct3D9;

namespace ProBlink
{
    public class DrawHandler
    {
        #region Static Fields

        private static Line line;
        private static Font text;

        #endregion

        #region Load

        public static void Load()
        {
            line = new Line(Drawing.Direct3DDevice9);
            text = new Font(
                Drawing.Direct3DDevice9,
                new FontDescription
                    {
                        FaceName = "Segoe UI", Height = 17, OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.ClearTypeNatural
                    });

            Drawing.OnPreReset += OnPreReset;
            Drawing.OnPostReset += OnPostReset;
            Drawing.OnEndScene += OnEndScene;
            AppDomain.CurrentDomain.DomainUnload += DomainUnload;
        }

        #endregion

        #region OnEndScene

        public static void OnEndScene(EventArgs args)
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

            DrawFilledBox(1700, 400, 200, 70, new ColorBGRA(0, 0, 0, 100));
            DrawLine(1700, 400, 1900, 400, 1, new ColorBGRA(1, 169, 234, 100));
            DrawShadowText("ProBlink", 1710, 405, Color.White, text);
            DrawShadowText("| 'D' |", 1775, 405, Color.White, text);
            DrawShadowText(Blink.Aiming ? "Aiming" : "Not Aiming", 1825, 405, Color.White, text);
            DrawLine(1700, 430, 1900, 430, 1, new ColorBGRA(1, 169, 234, 100));
            DrawShadowText("Enable QuickCast", 1710, 440, Color.White, text);
            var enableQuickCast = Blink.QuickCast;
            DrawButton(1850, 442, 15, 15, 2, ref enableQuickCast, true);
            DrawLine(1700, 470, 1900, 470, 1, new ColorBGRA(1, 169, 234, 100));
        }

        #endregion

        #region DomainUnload

        public static void DomainUnload(object sender, EventArgs e)
        {
            text.Dispose();
            line.Dispose();
        }

        #endregion

        #region OnPostReset

        public static void OnPostReset(EventArgs args)
        {
            text.OnResetDevice();
            line.OnResetDevice();
        }

        #endregion

        #region OnPreReset

        public static void OnPreReset(EventArgs args)
        {
            text.OnLostDevice();
            line.OnLostDevice();
        }

        #endregion

        #region Drawing Methods | Credits to JumpAttacker

        private static void DrawFilledBox(float x, float y, float w, float h, Color color)
        {
            var vLine = new Vector2[2];

            line.GLLines = true;
            line.Antialias = false;
            line.Width = w;

            vLine[0].X = x + w / 2;
            vLine[0].Y = y;
            vLine[1].X = x + w / 2;
            vLine[1].Y = y + h;

            line.Begin();
            line.Draw(vLine, color);
            line.End();
        }

        private static void DrawLine(float x1, float y1, float x2, float y2, float w, Color color)
        {
            var vLine = new[] { new Vector2(x1, y1), new Vector2(x2, y2) };

            line.GLLines = true;
            line.Antialias = false;
            line.Width = w;

            line.Begin();
            line.Draw(vLine, color);
            line.End();

        }

        private static void DrawShadowText(string stext, int x, int y, Color color, Font f)
        {
            f.DrawText(null, stext, x + 1, y + 1, Color.Black);
            f.DrawText(null, stext, x, y, color);
        }

        private static void DrawBox(float x, float y, float w, float h, float px, Color color)
        {
            DrawFilledBox(x, y + h, w, px, color);
            DrawFilledBox(x - px, y, px, h, color);
            DrawFilledBox(x, y - px, w, px, color);
            DrawFilledBox(x + w, y, px, h, color);
        }

        private static void DrawButton(float x, float y, float w, float h, float px, ref bool clicked, bool isActive)
        {
            if (isActive)
            {
                var isIn = MouseOn(x, y, w, h);
                if (Blink.LButton && Utils.SleepCheck("ClickButtonCd") && isIn)
                {
                    clicked = !clicked;
                    Utils.Sleep(250, "ClickButtonCd");
                }
                Color newColor = isIn
                    ? new ColorBGRA(0, 119, 166, 100)
                    : clicked ? new ColorBGRA(1, 169, 234, 100) : new ColorBGRA(37, 37, 37, 100);
                DrawFilledBox(x, y, w, h, newColor);
                DrawBox(x, y, w, h, px, Color.Black);
            }
            else
            {
                DrawFilledBox(x, y, w, h, Color.Gray);
                DrawBox(x, y, w, h, px, Color.Black);
            }
        }

        #endregion

        #region GUI Helpers

        public static bool MouseOn(float x, float y, float sizeX, float sizeY)
        {
            var mousePos = Game.MouseScreenPosition;
            return mousePos.X >= x && mousePos.X <= x + sizeX && mousePos.Y >= y && mousePos.Y <= y + sizeY;
        }

        #endregion
    }
}
