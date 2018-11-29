using System.Collections.Generic;
using System.Windows.Forms;

namespace PublishForQA
{
    public static class Globals
    {
        /// <summary>
        /// A reference to the main form of the program.
        /// </summary>
        public static FormPublisher MainForm;
        /// <summary>
        /// A list of all text boxes on the form.
        /// </summary>
        public static List<TextBox> AllTextBoxesList = new List<TextBox>();
        /// <summary>
        /// A list of all E-Check debug folder text boxes on the form.
        /// </summary>
        public static List<TextBox> DebugTextBoxesList = new List<TextBox>();
    }
}
