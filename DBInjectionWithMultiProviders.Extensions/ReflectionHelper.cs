using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DBInjectionWithMultiProviders.Extensions
{
    //https://stackoverflow.com/questions/26733/getting-all-types-that-implement-an-interface
    //https://docs.microsoft.com/en-Us/dotnet/api/system.linq.enumerable.oftype?view=netcore-3.1
    //https://stackoverflow.com/questions/28342206/what-does-system-reflection-targetexception-non-static-method-requires-a-target
    public static class ReflectionHelper
    {
        public static List<Type> GetTypesAssignableFrom<T>(this Assembly assembly)
        {
            return assembly.GetTypesAssignableFrom(typeof(T));
        }
        public static List<Type> GetTypesAssignableFrom(this Assembly assembly, Type compareType)
        {
            List<Type> ret = new List<Type>();
            foreach (var type in assembly.DefinedTypes)
            {
                if (compareType.IsAssignableFrom(type) && compareType != type)
                {
                    ret.Add(type);
                }
            }
            return ret;
        }
        public static IEnumerable<Type> GetImplementingTypes(this Type itype)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                      .Where(t => t.GetInterfaces().Contains(itype));
        }

        public static IList<Type> LoadAllImplementingTypes(Type[] interfaces)
        {
            IList<Type> implementingTypes = new List<Type>();

            // find all types
            foreach (var interfaceType in interfaces)
                foreach (var currentAsm in AppDomain.CurrentDomain.GetAssemblies())
                    try
                    {
                        foreach (var currentType in currentAsm.GetTypes())
                            if (interfaceType.IsAssignableFrom(currentType) && currentType.IsClass && !currentType.IsAbstract)
                                implementingTypes.Add(currentType);
                    }
                    catch { }

            return implementingTypes;
        }
        public static List<TypeInfo> GetAssemblyFromBaseClass<T,TInterface>() where T:class
        {
            // We get the assembly through the base class
            var baseAssembly = typeof(T).GetTypeInfo().Assembly;
            // we filter the defined classes according to the interfaces they implement
            return baseAssembly.DefinedTypes.Where(type => type.ImplementedInterfaces.Any(inter => inter == typeof(TInterface))).ToList();    
        }
        public static List<Type> GetAssemblyByInterface<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(x => x).ToList();
        }
        public static T GetEnumValue<T>(this string str) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            return enumType.IsEnum ? Enum.TryParse(str, true, out T val) ? val : default : throw new Exception("T must be an Enumeration type.");
        }

        public static T GetEnumValue<T>(this int intValue) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            return enumType.IsEnum ? (T)Enum.ToObject(enumType, intValue) : throw new Exception("T must be an Enumeration type.");
        }
    }
}
