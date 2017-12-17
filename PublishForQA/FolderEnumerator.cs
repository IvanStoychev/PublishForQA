using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace PublishForQA
{
    public static class FolderEnumerator
    {
        /// <summary>
        /// Enumerates through all subdirectories of the given path and returns them in a list.
        /// </summary>
        /// <param name="root">The path from which to start enumerating directories</param>
        /// <returns>List of all subdirectories in "root"</returns>
        public static IEnumerable<string> EnumerateFoldersRecursively(string root)
        {
            foreach (var folder in EnumerateFolders(root))
            {
                yield return folder;

                foreach (var subfolder in EnumerateFoldersRecursively(folder))
                    yield return subfolder;
            }
        }

        /// <summary>
        /// Finds the first directory in the given path.
        /// </summary>
        /// <param name="root">The path to search in.</param>
        /// <returns>The path to the first direcotry found.</returns>
        public static IEnumerable<string> EnumerateFolders(string root)
        {
            //We add an asterisk to make a wildcard search for all folders.
            string spec = Path.Combine(root, "*");

            //We get the first file or directory in the provided path.
            using (SafeFindHandle findHandle = FindFirstFile(spec, out WIN32_FIND_DATA findData))
            {
                if (!findHandle.IsInvalid)
                {
                    do
                    {
                        // Ignore special "." and ".." folders.
                        if ((findData.cFileName != ".") && (findData.cFileName != ".."))
                        {
                            //If it is a directory.
                            if ((findData.dwFileAttributes & FileAttributes.Directory) != 0)
                            {
                                yield return Path.Combine(root, findData.cFileName);
                            }
                        }
                    }
                    while (FindNextFile(findHandle, out findData));
                }
            }
        }

        /// <summary>
        /// Provides a class for safe handle implementations.
        /// </summary>
        internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]

            public SafeFindHandle() : base(true)
            {
            }

            protected override bool ReleaseHandle()
            {
                if (!IsInvalid && !IsClosed)
                {
                    return FindClose(this);
                }

                return (IsInvalid || IsClosed);
            }

            protected override void Dispose(bool disposing)
            {
                if (!IsInvalid && !IsClosed)
                {
                    FindClose(this);
                }

                base.Dispose(disposing);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;

            public long ToLong()
            {
                return dwLowDateTime + ((long)dwHighDateTime) << 32;
            }
        };

        /// <summary>
        /// Contains information about the file that is found by the FindFirstFile, FindFirstFileEx, or FindNextFile function.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WIN32_FIND_DATA
        {
            public FileAttributes dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public int nFileSizeHigh;
            public int nFileSizeLow;
            public int dwReserved0;
            public int dwReserved1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string cFileName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ALTERNATE)]
            public string cAlternate;
        }

        /// <summary>
        /// Searches a directory for a file or subdirectory with a name that matches a specific search pattern.
        /// </summary>
        /// <param name="lpFileName">The search pattern.</param>
        /// <param name="lpFindFileData">The data for the found file or direcotry.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern SafeFindHandle FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        /// <summary>
        /// Continues a file search from a previous call to the FindFirstFile, FindFirstFileEx, or FindFirstFileTransacted functions.
        /// </summary>
        /// <param name="hFindFile">The search handle returned by a previous call to the FindFirstFile or FindFirstFileEx function.</param>
        /// <param name="lpFindFileData">A pointer to the WIN32_FIND_DATA structure that receives information about the found file or subdirectory.</param>
        /// <returns>
        /// <para/>If the function succeeds, the return value is nonzero and the lpFindFileData parameter contains information about the next file or directory found.
        /// <para/>If the function fails, the return value is zero and the contents of lpFindFileData are indeterminate.To get extended error information, call the GetLastError function.
        /// <para/>If the function fails because no more matching files can be found, the GetLastError function returns ERROR_NO_MORE_FILES.</returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FindNextFile(SafeHandle hFindFile, out WIN32_FIND_DATA lpFindFileData);

        /// <summary>
        /// Closes a file search handle opened by the FindFirstFile function.
        /// </summary>
        /// <param name="hFindFile">The file search handle.</param>
        /// <returns>
        /// <para/>If the function succeeds, the return value is nonzero.
        /// <para/>If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FindClose(SafeHandle hFindFile);

        private const int MAX_PATH = 260;
        private const int MAX_ALTERNATE = 14;
    }
}
