using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GenerateConfigJson
{
    class Program
    {
        private readonly static string StartOfOutPut = "wwwroot\\js\\";
        private readonly static string StartOfInput = "Scripts\\";
        private readonly static string path = "Pjs1\\Pjs1.Main.csproj";
        private readonly static string fileName = "Pjs1\\bundleconfig.json";
        private readonly static string scriptDir = "Pjs1\\Scripts\\";

        private readonly static List<ObjBundle> bundleList = new List<ObjBundle>();
        static void Main(string[] args)
        {

            var bundleFilePath = FindFileDirectory(path, fileName, scriptDir);

            GetJsNames(bundleFilePath.ScriptDirectory);

            var resultJson = JsonConvert.SerializeObject(bundleList, Formatting.Indented);

            using (StreamWriter file =
            new StreamWriter(bundleFilePath.BundleFile))
            {
                file.WriteLine(resultJson.Replace(@"\\", "/"));
            }

            var title = "// Configure bundling and minification for the project. \n";
            title += "// More info at https://go.microsoft.com/fwlink/?LinkId=808241 \n";
            title += "// https://docs.microsoft.com/en-us/aspnet/core/client-side/bundling-and-minification?view=aspnetcore-2.1&tabs=visual-studio%2Caspnetcore2x#build-time-execution-of-bundling-and-minification \n";
            title += "// Instal this for auto bundle and minify  https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BundlerMinifier \n";
            title += "// right-click the file  bundleconfig.json  and select enable bundle on build";

            //run at debugmode it will close console app when exit
            Environment.Exit(0);
        }

        public static DirResult FindFileDirectory(string mainProject, string fileName, string scriptDir)
        {
            var dirResult = new DirResult();
            var cwd = new DirectoryInfo(Directory.GetCurrentDirectory());
            do
            {
                if (File.Exists(Path.Combine(cwd.FullName, mainProject)))
                {
                    var pathcombi = Path.Combine(cwd.FullName, fileName);
                    if (File.Exists(pathcombi))
                    {
                        dirResult.BundleFile = pathcombi;
                    }

                    var dircombi = Path.Combine(cwd.FullName, scriptDir);
                    if (Directory.Exists(dircombi))
                    {
                        dirResult.ScriptDirectory = new DirectoryInfo(dircombi); ;
                    }
                    dirResult.ProjectDirectory = cwd.FullName;
                    return dirResult;
                }
                cwd = cwd.Parent;
            } while (cwd != null);


            return null;
        }

        public class DirResult
        {
            public string BundleFile { get; set; }
            public DirectoryInfo ScriptDirectory { get; set; }
            public string ProjectDirectory { get; set; }
        }

        public class ObjBundle
        {
            [JsonProperty(PropertyName = "outputFileName")]
            public string OutputFileName { get; set; }
            [JsonProperty(PropertyName = "inputFiles")]
            public string[] InputFiles { get; set; }

            [JsonProperty(PropertyName = "minify")]
            public MiniFyModified Minify { get; set; }

            [JsonProperty(PropertyName = "sourceMap")]
            public bool? SourceMap { get; set; }

        }

        public class MiniFyModified
        {
            [JsonProperty(PropertyName = "enabled")]
            public bool? Enabled { get; set; }
            [JsonProperty(PropertyName = "renameLocals")]
            public bool? RenameLocals { get; set; }
        }


        public static void GetJsNames(DirectoryInfo scriptDirectory)
        {
            var files = scriptDirectory.GetFiles();
            foreach (var file in files)
            {
                var fileNameExt = file.FullName.Substring(file.FullName.LastIndexOf("views"));
                if (file.Extension.EndsWith(".js"))
                {
                    var fileNameOut = StartOfOutPut + fileNameExt;
                    var fileNameIn = StartOfInput + fileNameExt; 
                    bundleList.Add(new ObjBundle
                    {
                        InputFiles = new[] { fileNameIn },
                        OutputFileName = fileNameOut.Replace(".js", ".min.js"),
                        Minify = new MiniFyModified { Enabled = true, RenameLocals = true },
                        SourceMap = false
                    });
                }

                //Todo start of input / output of css
                /*
                if (file.Extension.EndsWith(".css"))
                {
                    bundleList.Add(new ObjBundle
                    {
                        InputFiles = new[] { file.Name },
                        OutputFileName = file.Name.Replace(".css", ".min.css")
                    });
                }
                */
            }
            var dirs = scriptDirectory.GetDirectories();
            foreach (var dir in dirs)
            {
                GetJsNames(dir);
            }

        }
    }
}
