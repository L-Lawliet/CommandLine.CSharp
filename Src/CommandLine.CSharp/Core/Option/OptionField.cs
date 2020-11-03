using System;
using System.Reflection;

/// <summary>
/// 
/// name:OptionField
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
    public class OptionField : OptionMember
    {
        public FieldInfo field { get; set; }

        public override void SetValue(object obj, string command)
        {
            object value = ChanageType(command, field.FieldType);

            field.SetValue(obj, value);
        }
    }
}
