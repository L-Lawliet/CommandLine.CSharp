using System;

/// <summary>
/// 
/// name:OptionMember
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
    public abstract class OptionMember
    {
        public string name { get; set; }

        //public Object obj { get; set; }

        public OptionAttribute option { get; set; }

        public abstract void SetValue(object obj, string command);

        public object ChanageType(string command, Type type)
        {
            object value = null;

            try
            {

                if (type.IsEnum)
                {
                    value = Enum.Parse(type, command);
                }
                else
                {
                    value = Convert.ChangeType(command, type);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("错误的类型：{0}   \"{1}\" to {2}  \n{3}", name, command, type.FullName, ex.ToString()));
            }

            return value;
        }
    }
}
