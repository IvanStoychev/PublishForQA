namespace PublishForQA
{
    /// <summary>
    /// A class containing all public enums for the project.
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Specifies constants defining which icon to display on a MessageUserControl.
        /// </summary>
        public enum MessageUserControlIcons
        {
            /// <summary>
            /// Displays an "Information" icon in the control "pbIcon".
            /// </summary>
            Info = 0,
            /// <summary>
            /// Displays a "Warning" icon in the control "pbIcon".
            /// </summary>
            Warning = 1,
            /// <summary>
            /// Displays an "Error" icon in the control "pbIcon".
            /// </summary>
            Error = 2
        }

        /// <summary>
        /// Specifies constants defining which buttons to display on a MessageUserControl.
        /// </summary>
        public enum MessageUserControlButtons
        {
            /// <summary>
            /// Displays no buttons.
            /// </summary>
            None = 0,
            /// <summary>
            /// Displays a "Fix" button.
            /// </summary>
            Fix = 1,
        }
    }
}
