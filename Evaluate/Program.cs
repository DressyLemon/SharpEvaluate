using System;
using Microsoft.CSharp;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;


namespace Evaluator
{
    class Evaluate
    {
        public static T eval<T>(string eval, object o = null)
        {
            List<string> Define = new List<string>(); foreach (var prop in o.GetType().GetProperties()) { var pass = ""; if (prop.GetValue(o, null).GetType().ToString() == "System.String") { pass = $"\"{prop.GetValue(o, null)}\""; } else { pass = $"{prop.GetValue(o, null)}"; } Define.Add($"{prop.GetValue(o, null).GetType()} {prop.Name}={pass};"); }
            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(); Random rand = new Random(); string FunctionName = ""; for (int j = 1; j <= 12; j++) { int letter_num = rand.Next(0, letters.Length - 1); FunctionName += letters[letter_num]; }
            var MethodClass = new StackTrace().GetFrame(1).GetMethod().ReflectedType; var options = new CompilerParameters() { GenerateExecutable = false, GenerateInMemory = true, }; options.ReferencedAssemblies.Add("System.dll");
            var type = new CSharpCodeProvider().CompileAssemblyFromSource(options, "using System; \n namespace " + MethodClass.Namespace + " \n{\n class " + MethodClass.Name + " {\n public static " + typeof(T).Name + " " + FunctionName + "() { \n" + string.Join(" ", Define.ToArray()) + "\n" + eval + " \n} } }").CompiledAssembly.GetType($"{MethodClass.Namespace}.{MethodClass.Name}"); return (T)type.GetMethod(FunctionName).Invoke(Activator.CreateInstance(type), null);
        }
    }
}
