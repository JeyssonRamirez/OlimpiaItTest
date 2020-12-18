//   -----------------------------------------------------------------------
//   <copyright file=DiContainer.cs company="Jeysson Ramirez">
//       Copyright (c) Jeysson Ramirez Todos los derechos reservados.
//   </copyright>
//   <author>Jeysson Stevens  Ramirez </author>
//   <Date>  2020 -12-17  - 20:57</Date>
//   <Update> 2020-12-17 - 20:57</Update>
//   -----------------------------------------------------------------------

using Unity;

namespace Crosscutting.DependencyInjectionFactory
{
    public class DiContainer
    {
        public DiContainer()
        {
            Current = new UnityContainer();
            Current.InitializeContainer();
        }

        public IUnityContainer Current { get; set; }
    }
}