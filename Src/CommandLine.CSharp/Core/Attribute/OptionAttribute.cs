using System;

/// <summary>
/// 
/// name:OptionAttribute
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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class OptionAttribute : BaseAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public char shortName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string longName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool required { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object defaultValue {get;set;}

        /// <summary>
        /// 忽略空
        /// </summary>
        public bool ignoreEmpty { get; set; }

        public OptionAttribute()
        {

        }

        public OptionAttribute(char shortName)
        {
            this.shortName = shortName;
        }

        public OptionAttribute(string longName)
        {
            this.longName = longName;
        }

        public OptionAttribute(char shortName, string longName)
        {
            this.shortName = shortName;
            this.longName = longName;
        }
    }
}

