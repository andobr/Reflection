using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Framework;
using Mathematica;

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
            foreach (var dll in path)
            {
                result.AddRange(
                    Assembly.LoadFrom(dll)
                    .GetTypes()
                    .Where(typeof(IPlugin).IsAssignableFrom)
                    .Where(x => x.GetConstructor(new Type[0]) != null)         
                    .Select(Activator.CreateInstance)
                    .Select(x => ((IPlugin)x).Name)
                    .ToList());
            }
            return result;
        }
    }
}
