using System;
using System.Collections;
using System.Reflection;
using Agile.Common.UI;
using Agile.Shared;

namespace Agile.Common.Reflections
{
    /// <summary>
    /// AgileType is a wrapper for System.Type.
    /// </summary>
    public class AgileType : IAgileControlDetails, IComparable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private AgileType(Type systemType)
        {
            ArgumentValidation.CheckForNullReference(systemType, "systemType");
            _systemType = systemType;
        }

        /// <summary>
        /// Instantiate a new AgileType with the given System type.
        /// </summary>
        /// <param name="systemType">The System.Type to wrap</param>
        /// <returns></returns>
        public static AgileType Build(Type systemType)
        {
            return new AgileType(systemType);
        }

        /// <summary>
        /// Determines the sub system name when the root namespace
        /// has not been set.
        /// Essentially returns everything to the left of the ProjectName.
        /// </summary>
        /// <returns></returns>
        private string GetSubSystemNameWhenRootNamespaceNotSet()
        {
            int startingPositionOfProjectName = AssemblyName.IndexOf("." + ProjectName);
            return AssemblyName.Substring(0, startingPositionOfProjectName);
        }

        private readonly Type _systemType;

        /// <summary>
        /// Gets the System.Type that is being wrapped.
        /// </summary>
        public Type SystemType
        {
            get { return _systemType; }
        }

        /// <summary>
        /// Gets the Sub System name from the given assembly.
        /// </summary>
        /// <remarks>WARNING: This property makes some assumptions about naming
        /// conventions for namespaces</remarks>
        /// <returns></returns>
        public string SubSystemName
        {
            get
            {
                string[] splits = AssemblyName.Split(".".ToCharArray());

                if (splits.Length == 1)
                    return string.Empty;
                else if (_rootNamespace == string.Empty)
                    return GetSubSystemNameWhenRootNamespaceNotSet();
                else if (splits.Length == 2)
                {
                    if (splits[0] == _rootNamespace)
                        return string.Empty;
                    else
                        return splits[0];
                }
                else
                {
                    if (splits[0] == _rootNamespace)
                        return splits[1];
                    else
                        return splits[0];
                }
            }
        }

        private static string _rootNamespace = "Agile";

        /// <summary>
        /// The root namespace to use for all Namespaces.
        /// i.e The first element of the namespace for all of OUR classes.
        /// </summary>
        public static string RootNameSpace
        {
            get { return _rootNamespace; }
            set
            {
                ArgumentValidation.CheckForNullReference(value, "value");
                _rootNamespace = value;
            }
        }

        /// <summary>
        /// Gets the Name of the Assembly that the type is in.
        /// </summary>
        private string AssemblyName
        {
            get { return _systemType.Assembly.GetName().Name; }
        }

        /// <summary>
        /// Gets the Name of the Type.
        /// </summary>
        public string Name
        {
            get { return _systemType.Name; }
        }

        /// <summary>
        /// override of ToString to show the contained item
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1}", GetType().Name, SystemType);
        }

        /// <summary>
        /// Gets the Project name from the given assembly.
        /// </summary>
        /// <remarks>WARNING: This property makes some assumptions about naming
        /// conventions for namespaces</remarks>
        /// <returns></returns>
        public string ProjectName
        {
            get
            {
                string[] splits = AssemblyName.Split(".".ToCharArray());
                if (splits.Length == 1)
                    return splits[0];
                else if (splits.Length == 2)
                {
//                    if (splits[0] == _rootNamespace)
//                        return splits[1];
//                    else
                    return AssemblyName;
                }
                else
                    return splits[splits.Length - 1];
            }
        }

        /// <summary>
        /// Gets the sub folders of the project (if there are any.)
        /// </summary>
        /// <example>'Agile.Common.Testing'
        /// would return 'Testing'</example>
        public string ProjectSubFolder
        {
            get
            {
                string[] splits = _systemType.Namespace.Split(".".ToCharArray());
                if (splits[splits.Length - 1].EndsWith(ProjectName))
                    return string.Empty;

                return splits[splits.Length - 1];
            }
        }

        #region IAgileControlDetails Members

        /// <summary>
        /// Gets any child objects.
        /// </summary>
        /// <example>A database table should return its collection of columns.</example>
        /// <remarks>Returns NULL if there are not any child objects.</remarks>
        IList IAgileControlDetails.ChildObjects
        {
            get { return Types.GetDirectDecendantsOf(_systemType.Assembly, _systemType, true); }
        }

        /// <summary>
        /// Gets the value to display in the control
        /// </summary>
        string IAgileControlDetails.DisplayValue
        {
            get { return _systemType.Name; }
        }

        /// <summary>
        /// Gets the color to use for the fore color
        /// </summary>
        /// <remarks>Must be an existing color, may be null</remarks>
        public string ForeColor
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the color to use for the back color
        /// </summary>
        /// <remarks>Must be an existing color, may be null</remarks>
        public string BackColor
        {
            get { return null; }
        }

        #endregion

        #region Type Overrides

//        /// <summary>
//        /// Standard Type.Name behaviour
//        /// </summary>
//        public override string Name
//        {
//            get
//            {
//                return _systemType.Name;
//            }
//        }
//
//        /// <summary>
//        /// Standard Type.GetEvents behaviour
//        /// </summary>
//        public override System.Reflection.EventInfo[] GetEvents(System.Reflection.BindingFlags bindingAttr)
//        {
//            return _systemType.GetEvents(bindingAttr);
//        }
//
//        /// <summary>
//        /// Standard Type.GetFields behaviour
//        /// </summary>
//        public override System.Reflection.FieldInfo[] GetFields(System.Reflection.BindingFlags bindingAttr)
//        {
//            return _systemType.GetFields(bindingAttr);
//        }
//
//        /// <summary>
//        /// Standard Type.IsDefined behaviour
//        /// </summary>
//        public override bool IsDefined(Type attributeType, bool inherit)
//        {
//            return _systemType.IsDefined(attributeType, inherit);
//        }
//
//        /// <summary>
//        /// Standard Type.UnderlyingSystemType behaviour
//        /// </summary>
//        public override Type UnderlyingSystemType
//        {
//            get
//            {
//                return _systemType.UnderlyingSystemType;
//            }
//        }
//
//        /// <summary>
//        /// Standard Type.HasElementTypeImpl behaviour
//        /// </summary>
//        protected override bool HasElementTypeImpl()
//        {
//            return _systemType.HasElementType;
//        }
//
//        /// <summary>
//        /// Standard Type.GetElementType behaviour
//        /// </summary>
//        public override Type GetElementType()
//        {
//            return _systemType.GetElementType();
//        }
//
//        /// <summary>
//        /// Standard Type.IsCOMObjectImpl behaviour
//        /// </summary>
//        protected override bool IsCOMObjectImpl()
//        {
//            return _systemType.IsCOMObject;
//        }
//
//        /// <summary>
//        /// Standard Type.IsPrimitiveImpl behaviour
//        /// </summary>
//        protected override bool IsPrimitiveImpl()
//        {
//            return _systemType.IsPrimitive;
//        }
//
//        /// <summary>
//        /// Standard Type.GetAttributeFlagsImpl behaviour
//        /// </summary>
//        protected override System.Reflection.TypeAttributes GetAttributeFlagsImpl()
//        {
//            return new System.Reflection.TypeAttributes ();
//        }
//
//        /// <summary>
//        /// Standard Type.GetMembers behaviour
//        /// </summary>
//        public override System.Reflection.MemberInfo[] GetMembers(System.Reflection.BindingFlags bindingAttr)
//        {
//            return _systemType.GetMembers(bindingAttr);
//        }
//
//        /// <summary>
//        /// Standard Type.GetNestedTypes behaviour
//        /// </summary>
//        public override Type[] GetNestedTypes(System.Reflection.BindingFlags bindingAttr)
//        {
//            return _systemType.GetNestedTypes(bindingAttr);
//        }
//
//        /// <summary>
//        /// Standard Type.GetNestedType behaviour
//        /// </summary>
//        public override Type GetNestedType(string name, System.Reflection.BindingFlags bindingAttr)
//        {
//            return _systemType.GetNestedType(name, bindingAttr);
//        }
//
//        /// <summary>
//        /// Standard Type.GetEvents behaviour
//        /// </summary>
//        public override System.Reflection.EventInfo[] GetEvents()
//        {
//            return base.GetEvents ();
//        }
//
//        /// <summary>
//        /// Standard Type.GetField behaviour
//        /// </summary>
//        public override System.Reflection.FieldInfo GetField(string name, System.Reflection.BindingFlags bindingAttr)
//        {
//            return _systemType.GetField(name, bindingAttr);
//        }
//
//        /// <summary>
//        /// Standard Type.GetConstructors behaviour
//        /// </summary>
//        public override System.Reflection.ConstructorInfo[] GetConstructors(System.Reflection.BindingFlags bindingAttr)
//        {
//            return _systemType.GetConstructors(bindingAttr);
//        }
//
//        /// <summary>
//        /// Standard Type.GetConstructorImpl behaviour
//        /// </summary>
//        protected override System.Reflection.ConstructorInfo GetConstructorImpl(System.Reflection.BindingFlags bindingAttr
//            , System.Reflection.Binder binder, System.Reflection.CallingConventions callConvention, Type[] types, System.Reflection.ParameterModifier[] modifiers)
//        {
//            return _systemType.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
//        }
//
//        /// <summary>
//        /// Standard Type.BaseType behaviour
//        /// </summary>
//        public override Type BaseType
//        {
//            get
//            {
//                return _systemType;
//            }
//        }
//
//        /// <summary>
//        /// Standard Type.AssemblyQualifiedName behaviour
//        /// </summary>
//        public override string AssemblyQualifiedName
//        {
//            get
//            {
//                return _systemType.AssemblyQualifiedName;
//            }
//        }
//
//        /// <summary>
//        /// Standard Type.FullName behaviour
//        /// </summary>
//        public override string FullName
//        {
//            get
//            {
//                return _systemType.FullName;
//            }
//        }
//
//        /// <summary>
//        /// Standard Type.TypeHandle behaviour
//        /// </summary>
//        public override RuntimeTypeHandle TypeHandle
//        {
//            get
//            {
//                return _systemType.TypeHandle;
//            }
//        }
//
//        /// <summary>
//        /// Standard Type.InvokeMember behaviour
//        /// </summary>
//        public override object InvokeMember(string name, System.Reflection.BindingFlags invokeAttr
//            , System.Reflection.Binder binder, object target, object[] args
//            , System.Reflection.ParameterModifier[] modifiers, System.Globalization.CultureInfo culture, string[] namedParameters)
//        {
//            return _systemType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
//        }
//
//        /// <summary>
//        /// Standard Type.GetProperties behaviour
//        /// </summary>
//        public override System.Reflection.PropertyInfo[] GetProperties(System.Reflection.BindingFlags bindingAttr)
//        {
//            return _systemType.GetProperties(bindingAttr);
//        }
//
//        /// <summary>
//        /// Standard Type.GUID behaviour
//        /// </summary>
//        public override Guid GUID
//        {
//            get
//            {
//                return _systemType.GUID;
//            }
//        }
//
//        /// <summary>
//        /// Standard Type.IsByRefImpl behaviour
//        /// </summary>
//        protected override bool IsByRefImpl()
//        {
//            return _systemType.IsByRef;
//        }
//
//        /// <summary>
//        /// Standard Type.GetInterfaces behaviour
//        /// </summary>
//        public override Type[] GetInterfaces()
//        {
//            return _systemType.GetInterfaces();
//        }
//
//        /// <summary>
//        /// Standard Type.GetInterface behaviour
//        /// </summary>
//        public override Type GetInterface(string name, bool ignoreCase)
//        {
//            return _systemType.GetInterface(name, ignoreCase);
//        }
//
//        /// <summary>
//        /// Standard Type. behaviour
//        /// </summary>
//        public override string Namespace
//        {
//            get
//            {
//                return _systemType.Namespace;
//            }
//        }
//
//        /// <summary>
//        /// Standard Type.GetEvent behaviour
//        /// </summary>
//        public override System.Reflection.EventInfo GetEvent(string name, System.Reflection.BindingFlags bindingAttr)
//        {
//            return _systemType.GetEvent(name, bindingAttr);
//        }
//
//        /// <summary>
//        /// Standard Type.GetMethods behaviour
//        /// </summary>
//        public override System.Reflection.MethodInfo[] GetMethods(System.Reflection.BindingFlags bindingAttr)
//        {
//            return _systemType.GetMethods(bindingAttr);
//        }
//
//        /// <summary>
//        /// Standard Type.GetPropertyImpl behaviour
//        /// </summary>
//        protected override System.Reflection.PropertyInfo GetPropertyImpl(string name, System.Reflection.BindingFlags bindingAttr
//            , System.Reflection.Binder binder, Type returnType, Type[] types, System.Reflection.ParameterModifier[] modifiers)
//        {
//            return _systemType.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
//        }
//
//        /// <summary>
//        /// Standard Type.GetMethodImpl behaviour
//        /// </summary>
//        protected override System.Reflection.MethodInfo GetMethodImpl(string name, System.Reflection.BindingFlags bindingAttr
//            , System.Reflection.Binder binder, System.Reflection.CallingConventions callConvention, Type[] types, System.Reflection.ParameterModifier[] modifiers)
//        {
//            return _systemType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
//        }
//
//        /// <summary>
//        /// Standard Type.GetCustomAttributes behaviour
//        /// </summary>
//        public override object[] GetCustomAttributes(bool inherit)
//        {
//            return _systemType.GetCustomAttributes(inherit);
//        }
//
//        /// <summary>
//        /// Standard Type.GetCustomAttributes behaviour
//        /// </summary>
//        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
//        {
//            return _systemType.GetCustomAttributes(attributeType, inherit);
//        }
//
//        /// <summary>
//        /// Standard Type. behaviour
//        /// </summary>
//        protected override bool IsPointerImpl()
//        {
//            return _systemType.IsPointer;
//        }
//
//        /// <summary>
//        /// Standard Type.Module behaviour
//        /// </summary>
//        public override System.Reflection.Module Module
//        {
//            get
//            {
//                return _systemType.Module;
//            }
//        }
//        /// <summary>
//        /// Standard Type.Assembly behaviour
//        /// </summary>
//        public override System.Reflection.Assembly Assembly
//        {
//            get
//            {
//                return _systemType.Assembly;
//            }
//        }
//        /// <summary>
//        /// Standard Type.IsArrayImpl behaviour
//        /// </summary>
//        protected override bool IsArrayImpl()
//        {
//            return _systemType.IsArray;
//        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Implementation of IComparable
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            var item = (AgileType) obj;
            return item.Name.CompareTo(Name);
        }

        #endregion

        /// <summary>
        /// Check if the given interface is implemented by this Type.
        /// </summary>
        /// <param name="thisInterface">Check if this interface is implemented by this Type.</param>
        /// <returns>Returns true if this Type implements the given interface.</returns>
        public bool ImplementsInterface(Type thisInterface)
        {
            foreach (Type interfaceType in SystemType.GetInterfaces())
            {
                if (interfaceType.Equals(thisInterface))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the given interface is implemented by this Type.
        /// </summary>
        /// <param name="interfaceName">Check if this interface is implemented by this Type.</param>
        /// <returns>Returns true if this Type implements the given interface.</returns>
        public bool ImplementsInterface(string interfaceName)
        {
            foreach (Type interfaceType in SystemType.GetInterfaces())
            {
                if (interfaceType.Name.Equals(interfaceName))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if an instantiate method exists in this type for the 
        /// given set of method parameters.
        /// </summary>
        /// <param name="parameterDetails">Method Parameters.</param>
        /// <returns>True if an Instantiate method exists for the parameter set.</returns>
        public bool InstantiateMethodExistsFor(MethodParameterDetails parameterDetails)
        {
            return SystemType.GetMethod("Build", parameterDetails.Signature) != null;

            // This code is an attempt to find only methods with an exact match signature
            // i.e. only the specific type should product a match, not types that the object is derived
            // from as well (which is what happens with the above code.
            //            MethodInfo method = this.SystemType.GetMethod("Build", BindingFlags.Static | BindingFlags.ExactBinding, null, types, null);
            //            return method != null;
        }

        /// <summary>
        /// Determines if an instantiate method exists in this type for the 
        /// given set of method parameters.
        /// </summary>
        /// <param name="types">Types in the signature of the instantiate method.</param>
        /// <returns>True if an Instantiate method exists with that signature.</returns>
        public bool InstantiateMethodExistsFor(Type[] types)
        {
            return SystemType.GetMethod("Build", types) != null;
        }

        /// <summary>
        /// Instantiate one of these Types with the given parameters.
        /// </summary>
        /// <param name="parameterDetails">Parameters required for instantiation.
        /// NOTE: The signature MUST map directly to the signature of one of the 
        /// Instantiate methods on the class.</param>
        /// <returns></returns>
        public object InstantiateConcreteClass(MethodParameterDetails parameterDetails)
        {
            // ok, so why not use Activator.CreateInstance? Well, it needs a public constructor, alot of my
            // stuff doesn't they have the Build methods. If you want to instantiate something that DOES
            // have a public constructor then DO use the activator :-)
            MethodInfo instantiationMethod = SystemType.GetMethod("Build", parameterDetails.Signature);
            if (instantiationMethod != null)
            {
                return instantiationMethod.Invoke(null, parameterDetails.Parameters);
            }
            return null;
        }
    }
}