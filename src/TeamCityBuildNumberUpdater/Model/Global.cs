using System.Collections.Generic;

namespace TeamCityBuildNumberUpdater.Model
{
    public class Global
    {
        public ICollection<string> Projects { get; set; }
        public ICollection<string> Executables { get; set; }

        public ICollection<string> Sdks { get; set; }

        public string Version { get; set; }
    }
}
