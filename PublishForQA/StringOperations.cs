using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PublishForQA
{
    /// <summary>
    /// Contains methods that automate simple operations on strings.
    /// </summary>
    public static class StringOperations
    {
        /// <summary>
        /// Converts the name of a TextBox. Removes all instances of "tb" and "Path".
        /// </summary>
        /// <param name="tb">The TextBox whose name should be converted.</param>
        /// <returns>
        /// A String instance of the TextBox name with no instances of "tb" and/or "Path".
        /// </returns>
        public static string NameReplace(TextBox tb)
        {
            return tb.Name.Replace("tb", "").Replace("Path", "");
        }

        /// <summary>
        /// Iterates through a list of TextBoxes' text properties and removes all colon
        /// characters, except the first one.
        /// </summary>
        /// <param name="list">A list of TextBoxes whose text properties should be fixed.</param>
        public static void FixColons(List<TextBox> list)
        {
            Regex regex = new Regex("^[a-zA-Z]:");
            try
            {
                foreach (var tb in list)
                {
                    if (tb.Text.StartsWith(@"\\"))
                    {
                        tb.Text = tb.Text.Replace(":", "");
                    }
                    else if (regex.IsMatch(tb.Text))
                    {
                        int firstColon = tb.Text.IndexOf(':');
                        tb.Text = tb.Text.Replace(":", "");
                        tb.Text = tb.Text.Insert(firstColon, ":");
                    }
                    else
                    {
                        //This "else" block exists to catch unusual cases
                        //and in case the functionality needs to be extended.
                        tb.Text = tb.Text.Replace(":", "");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected exception has occurred while trying to fix illegal colon characters:\n" + ex.Message, "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// Iterates through a list of TextBoxes' text properties and replaces all occurances of more
        /// than one consecutive backslash character with just a single backslash character.
        /// </summary>
        /// <param name="list">A list of TextBoxes whose text properties should be fixed.</param>
        public static void FixBackslashes(List<TextBox> list)
        {
            Regex regex = new Regex(@"[\\]{2,}");
            try
            {
                foreach (var tb in list)
                {
                    tb.Text = tb.Text.Substring(0, 1) + regex.Replace(tb.Text.Substring(1), "\\");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected exception has occurred while trying to fix repeated backslash characters:\n" + ex.Message, "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
