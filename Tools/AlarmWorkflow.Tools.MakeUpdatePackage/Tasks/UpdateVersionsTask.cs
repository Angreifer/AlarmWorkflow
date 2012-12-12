﻿using System.IO;
using System.Text.RegularExpressions;

namespace AlarmWorkflow.Tools.MakeUpdatePackage.Tasks
{
    class UpdateVersionsTask : ITask
    {
        #region Constants

        private static readonly Regex VersionRegex = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");

        #endregion

        #region ITask Members

        void ITask.Execute(Context context)
        {
            FindAndUpateAssemblyInfos(context);
            FindAndUpadateVersionConfigs(context);
        }

        private void FindAndUpateAssemblyInfos(Context context)
        {
            foreach (FileInfo file in context.ProjectRootDirectory.GetFiles("AssemblyInfo.cs", SearchOption.AllDirectories))
            {
                ProcessAssemblyInfo(file, context);
            }
        }

        private void ProcessAssemblyInfo(FileInfo file, Context context)
        {
            if (file.IsReadOnly)
            {
                // TODO: Error!
                return;
            }

            bool shouldSaveFile = false;
            string newVersionString = context.NewVersion.ToString();

            string[] lines = File.ReadAllLines(file.FullName);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                Match match = VersionRegex.Match(line);
                // Only modify the file contents if the version does differ
                if (match != null && match.Success && match.Value != newVersionString)
                {
                    line = VersionRegex.Replace(line, newVersionString);
                    shouldSaveFile = true;
                }

                lines[i] = line;
            }

            if (shouldSaveFile)
            {
                File.WriteAllLines(file.FullName, lines);
            }
        }

        private void FindAndUpadateVersionConfigs(Context context)
        {
            // TODO
        }

        #endregion

    }
}
