using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 
/// name:OptionParser
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
    public struct Argument
    {
        public string key;

        public string value;
    }

    public class OptionParser
    {
        public T Parse<T>(string[] args)
        {
            T obj = Activator.CreateInstance<T>();

            Parse(args, ref obj);

            return obj;
        }

        public T Parse<T>(string[] args, Func<T> createInstance)
        {
            T obj = createInstance.Invoke();

            Parse(args, ref obj);

            return obj;
        }

        public object Parse(string[] args, Type type)
        {
            object obj = Activator.CreateInstance(type);

            Parse(args, type, ref obj);

            return obj;
        }

        public void Parse<T>(string[] args, ref T obj)
        {
            Type type = typeof(T);

            object temp = obj;

            Parse(args, type, ref temp);

            obj = (T)temp;
        }

        public void Parse(string[] args, Type type, ref object obj)
        {
            var longDict = new Dictionary<string, OptionMember>();
            var shortDict = new Dictionary<char, OptionMember>();

            var arguments = GetArguments(args);

            GetOptionMemeberDictionary(type, ref longDict, ref shortDict);

            for (int i = 0; i < arguments.Count; i++)
            {
                var argument = arguments[i];

                OptionMember member = null;

                if(argument.key.Length == 1) //只有一个字符，猜测为短参数
                {
                    char key = argument.key[0];

                    shortDict.TryGetValue(key, out member);
                }

                if(member == null)
                {
                    longDict.TryGetValue(argument.key, out member);
                }

                if(member != null)
                {
                    if(member.option.ignoreEmpty && string.IsNullOrEmpty(argument.value))
                    {
                        //忽略空参数
                        continue;
                    }

                    member.SetValue(obj, argument.value);
                }
            }
        }

        private List<Argument> GetArguments(string[] args)
        {
            var arguments = new List<Argument>();

            string key = null;

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                if(arg.Length == 1 && arg[0] == '-')
                {
                    throw new Exception("错误的参数：-");
                }

                if (arg.Length > 1 && arg[0] == '-' && !char.IsDigit(arg[1]))//长度大于1，开始为-，并且不是紧跟数字的则为key
                {
                    if (!string.IsNullOrEmpty(key)) //上一个key没有value配对，却有下一个key，表示为无value参数
                    {
                        Argument item = new Argument();

                        item.key = key;
                        item.value = null;

                        arguments.Add(item);

                        key = null;
                    }

                    key = arg.Substring(1);
                    
                    if (string.IsNullOrEmpty(key))
                    {
                        //Exception
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        Argument item = new Argument();

                        item.key = key;
                        item.value = arg;

                        arguments.Add(item);

                        key = null;
                    }
                }

            }

            return arguments;
        }

        private void GetOptionMemeberDictionary(Type type, ref Dictionary<string, OptionMember> longDict, ref Dictionary<char, OptionMember> shortDict)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var propertyInfo in properties)
            {
                var optionProperty = new OptionProperty();

                var attribute = GetAttribute<OptionAttribute>(propertyInfo);

                if(attribute != null)
                {
                    optionProperty.name = propertyInfo.Name;

                    optionProperty.option = attribute;

                    optionProperty.property = propertyInfo;

                    PushOption(optionProperty, ref longDict, ref shortDict);
                }
            }

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            foreach (var fieldInfo in fields)
            {
                var optionField = new OptionField();

                var attribute = GetAttribute<OptionAttribute>(fieldInfo);

                if (attribute != null)
                {
                    optionField.name = fieldInfo.Name;

                    optionField.option = attribute;

                    optionField.field = fieldInfo;

                    PushOption(optionField, ref longDict, ref shortDict);
                }
            }
        }

        private void PushOption(OptionMember member, ref Dictionary<string, OptionMember> longDict, ref Dictionary<char, OptionMember> shortDict)
        {
            string longName;

            if (string.IsNullOrEmpty(member.option.longName))
            {
                longName = member.name;
            }
            else
            {
                longName = member.option.longName;
            }

            longDict.Add(longName, member);

            if (member.option.shortName != '\0')
            {
                shortDict.Add(member.option.shortName, member);
            }
        }

        private TAttribute GetAttribute<TAttribute>(ICustomAttributeProvider customAttributeProvider) where TAttribute : BaseAttribute
        {
            object[] objects = customAttributeProvider.GetCustomAttributes(typeof(TAttribute), true);

            for (int i = 0; i < objects.Length; i++)
            {
                if(objects[i] is TAttribute)
                {
                    return objects[i] as TAttribute;
                }
            }

            return null;
        }

    }
}
