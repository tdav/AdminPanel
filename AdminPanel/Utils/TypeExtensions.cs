using System;
using System.Collections.Generic;
using System.Text;

namespace AdminPanel
{
    public static class TypeExtensions
    {
        public static object ConvertObject(this object input, Type type)
        {
            switch (type.ToString())
            {
                case "System.Byte":
                case "System.SByte":
                case "System.Decimal":
                case "System.Double":
                case "System.Single":
                case "System.Int32":
                case "System.UInt32":
                case "System.Int64":
                case "System.UInt64":
                case "System.Int16":
                case "System.UInt16":
                    return Convert.ToInt32(input);

                case "System.Guid":
                    return Guid.Parse(input.ToString());

                case "System.Char":
                case "System.String":
                    return input.ToString();
            }

            return Convert.ChangeType(input, type);
        }
    }
}