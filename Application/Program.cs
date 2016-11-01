using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path = @"C:\Users\Anton\Source\Repos\Reflection\";
            var dllPath = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);

            foreach (var pluginName in GetNamesOfPlugins(dllPath))
                Console.WriteLine(pluginName);

            Console.Read();         
        }

        static List<string> GetNamesOfPlugins(string[] path)
        {
            var result = new List<string>();

            foreach (var item in path)
            {
                result.AddRange(
                    Assembly.LoadFrom(item)
                    .GetTypes()
                    .Select(Activator.CreateInstance)
                    .Select(x => ((IPlugin)x).Name)
                    .ToList());
            }
                
            return result;
        }
    }
}
