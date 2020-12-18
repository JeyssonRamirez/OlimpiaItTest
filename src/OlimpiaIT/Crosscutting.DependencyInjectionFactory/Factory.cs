//   -----------------------------------------------------------------------
//   <copyright file=Factory.cs company="Jeysson Ramirez">
//       Copyright (c) Jeysson Ramirez Todos los derechos reservados.
//   </copyright>
//   <author>Jeysson Stevens  Ramirez </author>
//   <Date>  2020 -12-17  - 20:57</Date>
//   <Update> 2020-12-17 - 20:57</Update>
//   -----------------------------------------------------------------------

using Unity;

namespace Crosscutting.DependencyInjectionFactory
{
    public static class Factory
    {
        private static readonly DiContainer Container = new DiContainer();

        public static TService Resolve<TService>()
        {
            return Container.Current.Resolve<TService>();
        }
    }
}