using Ensage.Common;
using Ensage.Common.Menu;

namespace ProBlink
{
    public class MenuGenerator
    {
        public static void Load()
        {
            Blink.Menu = new Menu("ProBlink", "blinkpro", true);

            Blink.Menu.AddItem(new MenuItem("cast.quick.enable", "Enable ProBlink").SetValue(true));
            Blink.Menu.AddItem(new MenuItem("blink.key", "Blink Key").SetValue(new KeyBind('D', KeyBindType.Press)));

            Blink.Menu.AddItem(new MenuItem("seperator", ""));
            Blink.Menu.AddItem(new MenuItem("by.blacky", "Made by blacky"));

            Blink.Menu.AddToMainMenu();
        }
    }
}
