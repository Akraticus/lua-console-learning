using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLua;

namespace LuaConsole
{
	public class Program
	{
		private static char separator = ' ';

		static void Main(string[] args)
		{
            var lua = new Lua();
            lua.DoFile("./UserDefinedLuaFunctions.txt");

            var luaMethods = typeof(InternalLuaFunctions).GetMethods().Where(m => m.GetCustomAttributes(typeof(LuaFunctionAttribute), true).Any()).ToList();
            //var luaMethods = typeof(Program).Assembly.GetType s().SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(LuaMethodAttribute), true).Any()).ToList();
            foreach (var luaMethod in luaMethods)
            {
                var luaFunc = luaMethod.GetCustomAttributes(typeof(LuaFunctionAttribute), true).First() as LuaFunctionAttribute;
                lua.RegisterFunction(luaFunc.FunctionName ?? luaMethod.Name, luaMethod);
            }

			while(true)
			{
				Console.Write(">> ");
				var input = Console.ReadLine();
                var inputSplit = input.Split(' ');
                var funcName = inputSplit.First();

                // Quit console if
                if (funcName.ToLowerInvariant().Equals("quit") || funcName.ToLowerInvariant().Equals("exit")) break;

                if (funcName.ToLowerInvariant().Equals("globals"))
                {
                    Console.WriteLine($"[{lua.Globals.Count()}] globals.");
                    foreach (var global in lua.Globals) Console.Write($"{global} ");
                    Console.WriteLine();
                    continue;
                }
                
                var func = lua.GetFunction(inputSplit.First());
                if (func == null)
                {
                    Console.WriteLine($"{inputSplit.First()} is not a recognised function.");
                    continue;
                }

                try
                {
                    var param = inputSplit.Except(new[] { inputSplit.First() });
                    var result = func.Call(param.ToArray());
                    Console.WriteLine(result.First());
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
			}

			//Console.WriteLine("Press any key to exit.");
			//Console.ReadKey(); // stops console from closing immediately
		}
	}
}
