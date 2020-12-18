using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Definition;
using Application.Implementation;
using Core.GlobalRepository;
using DataAccess.Provider.Reconoser;
using Unity;

namespace Crosscutting.DependencyInjectionFactory
{
    public static class ContainerInitializer
    {
        public static void InitializeContainer(this IUnityContainer container)
        {


            //External Repositories 
            container.RegisterType<IReconoserRepository, ReconoserRepository>();
            //Mongo

            //Other

            //AppServices
            container.RegisterType<IAccountService, AccountService>();
            //container.RegisterType<IRateAppService, RateAppService>();


        }
    }
}
