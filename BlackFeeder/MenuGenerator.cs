using Ensage.Common;
using Ensage.Common.Menu;

namespace BlackFeeder
{
    public class MenuGenerator
    {
        public static void Load()
        {
            Feeder.Menu = new Menu("BlackFeeder", "BlackFeeder", true);

            Feeder.Menu.AddItem(new MenuItem("Feeding.Activated", "Enable BlackFeeder").SetValue(true));

            Feeder.Menu.AddItem(new MenuItem("seperator", ""));
            Feeder.Menu.AddItem(new MenuItem("by.blacky", "Made by blacky"));

            Feeder.Menu.AddToMainMenu();
        }
    }
}
