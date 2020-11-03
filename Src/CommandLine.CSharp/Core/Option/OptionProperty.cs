using System;
using System.Reflection;

/// <summary>
/// 
/// name:OptionProperty
/// author:罐子
/// vindicator:
/// versions:
/// introduce:
/// note:
/// 
/// 
/// list:
/// 
/// 
/// 
/// </summary>
namespace CommandLine.CSharp
{
    public class OptionProperty : OptionMember
    {
        public PropertyInfo property { get; set; }

        public override void SetValue(object obj, string command)
        {
            object value = ChanageType(command, property.PropertyType);

            property.SetValue(obj, value, null);
        }
    }
}
