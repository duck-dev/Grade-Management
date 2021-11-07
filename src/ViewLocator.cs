using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using GradeManagement.ViewModels.BaseClasses;

namespace GradeManagement
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            return BuildStatic(data);
        }

        internal static IControl BuildStatic(object data)
        {
            return BuildStatic(data.GetType());
        }

        internal static IControl BuildStatic(Type type)
        {
            var newType = ReplaceType(type, out string name);
            return BuildControl(newType, name);
        }

        private static Type? ReplaceType(Type type, out string name)
        {
            name = type.FullName!.Replace("ViewModel", "View");
            var newType = Type.GetType(name);
            
            return newType;
        }

        private static IControl BuildControl(Type? type, string name)
        {
            if (type != null)
                return (Control)Activator.CreateInstance(type)!;
            
            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object data) => data is ViewModelBase;
    }
}