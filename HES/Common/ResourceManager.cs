using HES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HES.Common
{
    class ResourceManager
    {
        private static List<IResourceProvider> INSTANCES = new List<IResourceProvider>();
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

            types.ForEach(type => {
                IResourceProvider instancia = Activator.CreateInstance(type) as IResourceProvider;
                INSTANCES.Add(instancia);
                instancia?.GetResource();
            });
        }

        public static T GetInstance<T>() where T : IResourceProvider
        {
            return (T)INSTANCES.FirstOrDefault(instance => instance is T);
        }
    }
}
