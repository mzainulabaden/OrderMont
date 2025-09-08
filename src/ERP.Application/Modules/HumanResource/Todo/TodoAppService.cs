using System;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;

namespace ERP.Modules.HumanResource.Todo
{
    [AbpAuthorize(PermissionNames.LookUps_HRM_Todo)]
    public class TodoAppService : GenericSimpleAppService<HRM_TodoDto, TodoInfo, SimpleSearchDtoBase>
    {
    }

    [AutoMap(typeof(TodoInfo))]
    public class HRM_TodoDto : SimpleDtoBase
    {
        public string Description { get; set; }
    }
}
