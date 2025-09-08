using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ERP.Authorization;
using AutoMapper;
using AutoMapper.Data;

namespace ERP;

[DependsOn(
    typeof(ERPCoreModule),
    typeof(AbpAutoMapperModule))]
public class ERPApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<ERPAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(ERPApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            cfg =>
            {
                cfg.AddMaps(thisAssembly);
            }
        );
    }
}
