using System.Collections.Generic;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel01
{
	public class COALevel03DumpItemDto
	{
		public string Name { get; set; }
		public string SerialNumber { get; set; }
		public string AccountTypeName { get; set; }
	}

	public class COALevel02DumpItemDto
	{
		public string Name { get; set; }
		public string SerialNumber { get; set; }
		public string AccountTypeName { get; set; }
		public List<COALevel03DumpItemDto> Level03Items { get; set; } = new List<COALevel03DumpItemDto>();
	}

	public class COALevel01DumpItemDto
	{
		public string Name { get; set; }
		public string SerialNumber { get; set; }
		public string AccountTypeName { get; set; }
		public List<COALevel02DumpItemDto> Level02Items { get; set; } = new List<COALevel02DumpItemDto>();
	}

	public class COADumpHierarchyRequestDto
	{
		public List<COALevel01DumpItemDto> Items { get; set; } = new List<COALevel01DumpItemDto>();
	}

	public class COADumpHierarchyResultDto
	{
		public int CreatedLevel01 { get; set; }
		public int CreatedLevel02 { get; set; }
		public int CreatedLevel03 { get; set; }
		public int FailureCount { get; set; }
		public List<string> Errors { get; set; } = new List<string>();
	}
}


