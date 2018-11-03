using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PublishForQA
{
    static class Validator
    {
        static void Validate(TextBox[] textBoxes)
        {

        }

        /// <summary>
        /// Finds the longest system entry path from
        /// </summary>
        /// <param name="directories"></param>
        /// <returns></returns>
        static string GetLongestPath(DirectoryInfo[] directories)
        {
            string longestPath = string.Empty;

            foreach (var dir in directories)
            {
                string path = dir.EnumerateFileSystemInfos().OrderByDescending(d => d.FullName).FirstOrDefault().FullName;
                if (path.Length > longestPath.Length) longestPath = path;
            }

            return longestPath;
        }
    }
}
