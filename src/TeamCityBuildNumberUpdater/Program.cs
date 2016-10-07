using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using TeamCityBuildNumberUpdater.Model;

namespace TeamCityBuildNumberUpdater 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
                throw new ArgumentException($"Expected 2 args, got {args.Length} instead.");

            VersionUpdater.UpdateVersion(args[0], args[1]);
        }

        public class VersionUpdater
        {
            // assume we're starting in the src dir in a project structure like:
            //
            // /<git root dir>
            // |__/src
            //    |__/ProjectA
            //       |__project.json
            //    |__/ProjectB
            //       |__project.json
            //    |__ProjectName.sln
            //    |__global.json
            //
            // assume global.json contains "Projects: [<all project subdirs>]" and "Version"
            //
            /// <summary>
            /// Given a base directory and a TeamCity build counter, walk through each project 
            /// subdirectory and update the "version" value in each project.json.
            ///
            /// </summary>
            /// <param name="baseDir">The base directory (containing global.json) to start in.</param>
            /// <param name="buildCounter">The build counter value from TeamCity.</param>
            public static void UpdateVersion(string baseDir, string buildCounter)
            {
                var globalJsonPath = Path.Combine(baseDir, "global.json");

                if (!File.Exists(globalJsonPath))
                    throw new FileNotFoundException($"Can't find global.json in {baseDir}.");

                var global = JsonConvert.DeserializeObject<Global>(File.ReadAllText(globalJsonPath));

                var buildNumber = $"{global.Version.Replace("-*", $".{buildCounter}")}";

                // assume all projects are one dir below global.json 
                var directories = Directory.GetDirectories(baseDir);

                foreach (var projectDir in directories)
                {
                    var projectJsonPath = Path.Combine(new string[] { baseDir, projectDir, "project.json" });

                    if (!File.Exists(projectJsonPath))
                    {
                        continue;
                    }

                    dynamic project = JsonConvert.DeserializeObject(File.ReadAllText(projectJsonPath));

                    project.version = buildNumber;

                    File.WriteAllText(projectJsonPath, JsonConvert.SerializeObject(project, Formatting.Indented));

                    Console.WriteLine($"Updated version in {projectJsonPath} to {buildNumber}.");
                }
            }
        }
    }
}
