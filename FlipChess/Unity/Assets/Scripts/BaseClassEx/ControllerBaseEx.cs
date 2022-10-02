using System.Linq;
using System.Reflection;

namespace Dreamwear
{
    public class ControllerBaseEx : ControllerBase
    {
        void Awake()
        {
            MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo method;
            for (int i = 0; i < methods.Length; i++)
            {
                method = methods[i];
                var atts = method.GetCustomAttributes<BaseAttribute>();
                if (atts == null)
                {
                    continue;
                }
                if (atts.Any() == false)
                {
                    continue;
                }
                foreach (var att in atts)
                {
                    if (att is SynchronizeMethodAttribute)
                    {
                        method.RegisterSyncMethod(this, (att as SynchronizeMethodAttribute).SyncName);
                    }
                }
            }
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo field;
            for (int i = 0; i < fields.Length; i++)
            {
                field = fields[i];
                var atts = field.GetCustomAttributes<BaseAttribute>();
                if (atts == null)
                {
                    continue;
                }
                if (atts.Any() == false)
                {
                    continue;
                }
                foreach (var att in atts)
                {
                    if (att is SynchronizeFieldAttribute)
                    {
                        field.RegisterSyncField(this, (att as SynchronizeFieldAttribute).SyncName);
                    }
                }
            }
        }
        void OnDestroy()
        {
            MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo method;
            for (int i = 0; i < methods.Length; i++)
            {
                method = methods[i];
                var atts = method.GetCustomAttributes<BaseAttribute>();
                if (atts == null)
                {
                    continue;
                }
                if (atts.Any() == false)
                {
                    continue;
                }
                foreach (var att in atts)
                {
                    if (att is SynchronizeMethodAttribute)
                    {
                        method.UnRegisterSyncMethod(this, (att as SynchronizeMethodAttribute).SyncName);
                    }
                }
            }
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo field;
            for (int i = 0; i < fields.Length; i++)
            {
                field = fields[i];
                var atts = field.GetCustomAttributes<BaseAttribute>();
                if (atts == null)
                {
                    continue;
                }
                if (atts.Any() == false)
                {
                    continue;
                }
                foreach (var att in atts)
                {
                    if (att is SynchronizeFieldAttribute)
                    {
                        field.UnRegisterSyncField(this, (att as SynchronizeFieldAttribute).SyncName);
                    }
                }
            }
        }
    }
}
