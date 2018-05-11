using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaConsole
{
    public class LuaFunctionAttribute : Attribute
    {
        public string FunctionName { get; set; }

        public LuaFunctionAttribute(string functionName = null)
        {
            FunctionName = functionName;
        }
    }

    public static class InternalLuaFunctions
    {
        [LuaFunction]
        public static ILuaResult Test1()
        {
            return new LuaResult($"{nameof(Test1)} called.");
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public double Løpehastighet { get; set; } = 10;

        public double MinuttTilMål(double km)
        {
            return km / Løpehastighet * 60;
        }
    }

    public interface ILuaResult
    {
        List<object> Result { get; set; }
        Exception Exception { get; set; }
    }

    public class LuaResult : ILuaResult
    {
        private List<object> _result = new List<object>();
        private Exception _exception;

        public List<object> Result { get => _result; set => _result = value; }
        public Exception Exception { get => _exception; set => _exception = value; }

        public LuaResult(params object[] parameters)
        {
            Result = parameters.ToList();
        }
    }
}
