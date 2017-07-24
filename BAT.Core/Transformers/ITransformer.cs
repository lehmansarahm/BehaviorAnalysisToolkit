using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Transformers
{
    public interface ITransformer
	{
		string[] GetHeader();
		string GetHeaderCsv();

		List<SensorReading> Transform(IEnumerable<ICsvWritable> input);
    }
}