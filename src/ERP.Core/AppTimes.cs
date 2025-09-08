using Abp.Dependency;
using System;

namespace ERP;

public class AppTimes : ISingletonDependency
{
    public DateTime StartupTime { get; set; }
}
