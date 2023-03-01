using System.Drawing;

namespace System.Windows.Forms
{
    static class ExtendTabButtonEnableDisable
    {
        public static void EnableTabButton(this Button button)
        {
            button.Enabled = true;
            button.BackColor = Color.FromArgb(255, 255, 255);
        }

        public static void DisableTabButton(this Button button)
        {
            button.Enabled = false;
            button.BackColor = Color.FromArgb(170, 170, 170);
        }
    }
}