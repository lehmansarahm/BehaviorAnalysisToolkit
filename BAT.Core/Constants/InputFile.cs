namespace BAT.Core.Constants
{
    public static class InputFile
	{
		public const string StartFlag = "Start";
		public const string EndFlag = "Quit";
		public const string NoLabelProvided = "no label provided";

		public enum ColumnOrder
		{
			Time,
			RecordNumber,
			Azimuth,
			Pitch,
			Roll,
			AccelerationX,
			AccelarationY,
			AccelerationZ,
			StartQuit,
			Label
		};
    }
}