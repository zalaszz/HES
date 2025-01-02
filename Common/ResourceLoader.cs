using HES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HES.Common
{
    class ResourceLoader
    {
        public static async Task LoadResourcesAsync()
        {
            await Task.Run(() => LoadResourcesImpl());
        }

        public static void LoadResources()
        {
            LoadResourcesImpl();
        }

        private static void LoadResourcesImpl()
        {
            List<Type> types = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => typeof(IResourceProvider).IsAssignableFrom(t) && !t.IsInterface)
                    .ToList();

            foreach (Type type in types)
            {
                IResourceProvider instancia = Activator.CreateInstance(type) as IResourceProvider;
                instancia?.GetResource();
            }
        }
    }
}
