using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ZemnaCmd.Core;
using ZemnaCmd.Properties;

namespace ZemnaCmd
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.Class, "*")]
    [COMServerAssociation(AssociationType.Directory)]
    [COMServerAssociation(AssociationType.Class, "Directory\\Background")]
    [COMServerAssociation(AssociationType.Drive)]
    public class ZemnaCmdExtension : SharpContextMenu
    {   
        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var itemExecuteCommandPrompt = new ToolStripMenuItem
            {
                Text = "Execute Command Prompt",
                Image = Resources.ZemnaCmd
            };

            itemExecuteCommandPrompt.Click += (sender, args) => ExecuteCommandPrompt();

            menu.Items.Add(itemExecuteCommandPrompt);

            return menu;
        }

        private void ExecuteCommandPrompt()
        {
            var paths = new StringBuilder();
            paths.Append("/K \"PATH=%PATH%;");

            XmlManager xm = new XmlManager();
            var pathList = xm.LoadPaths();
            foreach (var path in pathList)
            {
                paths.Append(path + ";");
            }

            var selectedPaths = new List<String>(SelectedItemPaths);
            if (!string.IsNullOrEmpty(FolderPath))
                selectedPaths.Add(FolderPath);

            foreach (var itemPath in selectedPaths)
            {
                var args = new StringBuilder();
                args.Append(paths.ToString());

                var dir = itemPath;

                if ((File.GetAttributes(itemPath) & FileAttributes.Directory) == 0)
                {
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        args.Append("&&\"");
                        args.Append(Path.GetFileName(itemPath));
                        args.Append("\"");
                    }

                    dir = Path.GetDirectoryName(dir);
                }

                args.Append("\"");

                Process proc = new Process();
                proc.StartInfo.Verb = "open";
                proc.StartInfo.FileName = "cmd.exe ";
                proc.StartInfo.Arguments = args.ToString();
                proc.StartInfo.WorkingDirectory = dir;
                proc.Start();
            }
        }
    }
}
