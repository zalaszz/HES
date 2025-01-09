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
            Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(type =>  typeof(IResourceProvider).IsAssignableFrom(type) && !type.IsInterface)
                    .OrderByDescending(type => type.Name)
                    .ToList()
                    .ForEach(type => {
                        Console.WriteLine(type.Name);
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
