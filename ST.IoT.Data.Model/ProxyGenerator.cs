using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Data.Model
{
    public class Builder
    {
        public void build()
        {
            /*
            var aName = new AssemblyName("DynamicAssemblyExample");
            var ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.RunAndSave);

            // For a single-module assembly, the module name is usually 
            // the assembly name plus an extension.
            var mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            var tb = mb.DefineType("MyDynamicType", TypeAttributes.Public, typeof (Dynamic));
            tb.DefineProperty("MyProp", PropertyAttributes.HasDefault, typeof (object), null);
            var getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the "get" accessor method
            MethodBuilder mbNewPropGetAccessor = tb.DefineMethod(
                "get_NewProp",
                getSetAttr,
                typeof(int),
                Type.EmptyTypes);

            ILGenerator NewPropGetIL = mbNewPropGetAccessor.GetILGenerator();
            NewPropGetIL.Emit(OpCodes.Ldarg_0);
            NewPropGetIL.Emit(OpCodes.Ldc_I4_1);
            NewPropGetIL.Emit(OpCodes.Call, typeof(Test).GetMethod("GetVal"));
            NewPropGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method 
            MethodBuilder mbNewPropSetAccessor = tb.DefineMethod(
                "set_NewProp",
                getSetAttr,
                null,
                new Type[] { typeof(int) });

            ILGenerator NewPropSetIL = mbNewPropSetAccessor.GetILGenerator();
            NewPropSetIL.Emit(OpCodes.Ldarg_0);
            NewPropSetIL.Emit(OpCodes.Ldc_I4_1);
            NewPropSetIL.Emit(OpCodes.Ldarg_1);
            NewPropSetIL.Emit(OpCodes.Call, typeof(Test).GetMethod("SetVal"));
            NewPropSetIL.Emit(OpCodes.Ret);

            // Map the accessor methods
            pbNewProp.SetGetMethod(mbNewPropGetAccessor);
            pbNewProp.SetSetMethod(mbNewPropSetAccessor);
            
            var type = tb.CreateType();

            var prop = 

            tb = null;
/*
            var pbNumber = tb.DefineProperty(
            "Number",
            PropertyAttributes.HasDefault,
            typeof(int),
            null);

            // The property "set" and property "get" methods require a special
            // set of attributes.
            MethodAttributes getSetAttr = MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the "get" accessor method for Number. The method returns
            // an integer and has no arguments. (Note that null could be  
            // used instead of Types.EmptyTypes)
            MethodBuilder mbNumberGetAccessor = tb.DefineMethod(
                "get_Number",
                getSetAttr,
                typeof(int),
                Type.EmptyTypes);

            ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();
            // For an instance property, argument zero is the instance. Load the  
            // instance, then load the private field and return, leaving the 
            // field value on the stack.
            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
            numberGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method for Number, which has no return 
            // type and takes one argument of type int (Int32).
            MethodBuilder mbNumberSetAccessor = tb.DefineMethod(
                "set_Number",
                getSetAttr,
                null,
                new Type[] { typeof(int) });

            ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
            // Load the instance and then the numeric argument, then store the 
            // argument in the field.
            numberSetIL.Emit(OpCodes.Ldarg_0);
            numberSetIL.Emit(OpCodes.Ldarg_1);
            numberSetIL.Emit(OpCodes.Stfld, fbNumber);
            numberSetIL.Emit(OpCodes.Ret);

            // Last, map the "get" and "set" accessor methods to the 
            // PropertyBuilder. The property is now complete. 
            pbNumber.SetGetMethod(mbNumberGetAccessor);
            pbNumber.SetSetMethod(mbNumberSetAccessor);
        }
 * */
        }
    }
}
