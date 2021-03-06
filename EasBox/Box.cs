using System.Windows.Forms;

namespace EasBox
{
    public static class Box
    {
        public static bool Confirm(string Message) //Shows a confirmation box and returns the result taken from user
        {
            if (DialogResult.Yes == MessageBox.Show(Message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                return true;
            return false;

        }
        public static void Show(string Message) //Shows simple message box 
        {
            MessageBox.Show(Message);
        }
        public static void Warn(string Message) //Shows a warning message box with only OK button
        {
            MessageBox.Show(Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void Info(string Message) //Shows an information message box with only OK buttong
        {
            MessageBox.Show(Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void Error(string Message) //Shows an Error message box with only OK buttong
        {
            MessageBox.Show(Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void Stop(string Message) //Shows an Stop message box with only OK buttong
        {
            MessageBox.Show(Message, "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

    }

}
