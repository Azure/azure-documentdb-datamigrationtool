using Microsoft.CSharp;
using Microsoft.DataTransfer.Basics;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Microsoft.DataTransfer.ConsoleHost.DynamicConfiguration
{
    sealed class SimpleProxyGenerator
    {
        private readonly string proxyNamespace;

        public SimpleProxyGenerator(string proxyNamespace)
        {
            Guard.NotEmpty("proxyNamespace", proxyNamespace);
            this.proxyNamespace = proxyNamespace;
        }

        public object CreateProxy(Type interfaceType, IReadOnlyDictionary<string, object> properties)
        {
            Guard.NotNull("interfaceType", interfaceType);
            Guard.NotNull("properties", properties);

            var typeDeclaration = new CodeTypeDeclaration(GetProxyClassName(interfaceType)) 
            {
                BaseTypes = { new CodeTypeReference(interfaceType) }
            };

            var proxyAssembly = new CodeCompileUnit
            {
                Namespaces = 
                {
                    new CodeNamespace(proxyNamespace)
                    {
                        Types = { typeDeclaration }
                    }
                }
            };

            proxyAssembly.ReferencedAssemblies.Add(interfaceType.Assembly.Location);
            proxyAssembly.ReferencedAssemblies.AddRange(
                interfaceType.GetInterfaces().Select(i => i.Assembly.Location).ToArray());

            foreach (var property in GetInterfaceProperties(interfaceType))
            {
                object value;
                if (!properties.TryGetValue(property.Name, out value))
                    value = GetDefaultValue(property.PropertyType);

                typeDeclaration.Members.Add(new CodeMemberProperty()
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final,
                    Name = property.Name,
                    HasGet = true,
                    GetStatements = 
                    {
                        new CodeMethodReturnStatement(GetValueExpression(value))
                    },
                    Type = new CodeTypeReference(property.PropertyType)
                });
            }

            using (var codeProvider = new CSharpCodeProvider())
            {
                return Activator.CreateInstance(
                    codeProvider
                        .CompileAssemblyFromDom(new CompilerParameters { GenerateInMemory = true }, proxyAssembly)
                        .CompiledAssembly
                        .GetType(proxyNamespace + "." + typeDeclaration.Name));
            }
        }

        private CodeExpression GetValueExpression(object value)
        {
            if (value == null)
                return new CodePrimitiveExpression(null);

            var valueType = value.GetType();

            if (valueType.IsEnum)
                return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(valueType), value.ToString());

            if (value is TimeSpan)
                return new CodeObjectCreateExpression(typeof(TimeSpan), new CodePrimitiveExpression(((TimeSpan)value).Ticks));

            if (!(value is string))
            {
                var enumerableType = valueType
                        .FindInterfaces((t, c) => t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>)), null)
                        .FirstOrDefault();
                if (enumerableType != null)
                    return new CodeArrayCreateExpression(
                        enumerableType.GetGenericArguments()[0],
                        (value as IEnumerable<object>).Select(v => GetValueExpression(v)).ToArray());
            }

            return new CodePrimitiveExpression(value);
        }

        public PropertyInfo[] GetInterfaceProperties(Type interfaceType)
        {
            return interfaceType.GetInterfaces()
                    .SelectMany(i => i.GetProperties())
                    .Union(interfaceType.GetProperties())
                    .Distinct(PropertyNameAndTypeEqualityComparer.Instance)
                    .ToArray();
        }

        private static string GetProxyClassName(Type interfaceType)
        {
            Guard.NotNull("interfaceType", interfaceType);
            return interfaceType.Name + "Proxy" + 
                Guid.NewGuid().GetHashCode().ToString("x", CultureInfo.InvariantCulture);
        }

        private static object GetDefaultValue(Type type)
        {
            Guard.NotNull("type", type);
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
