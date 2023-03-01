using System.Drawing;

namespace System.Windows.Forms
{
    static class ExtendActionButtonEnableDisable
    {
        public static void EnableActionButton(this Button button)
        {
            button.Enabled = true;
            button.BackColor = Color.FromArgb(165, 206, 255);
        }

        public static void DisableActionButton(this Button button)
        {
            button.Enabled = false;
            button.BackColor = Color.FromArgb(135, 135, 135);
        }
    }
}