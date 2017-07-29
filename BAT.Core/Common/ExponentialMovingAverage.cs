using System;

namespace BAT.Core.Common
{
	public class ExponentialMovingAverage
	{
		const decimal EMA_ALPHA = 0.5M;
		const int EMA_SAMPLE_SIZE = (int)Constants.SAMPLING_PERIOD_IN_MS;

		public decimal Alpha { get; set; }
        public int SampleSize { get; set; }
        EMAValue TopValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BAT.Core.Common.ExponentialMovingAverage"/> class.
        /// </summary>
        public ExponentialMovingAverage()
        {
            Alpha = EMA_ALPHA;
            SampleSize = EMA_SAMPLE_SIZE;
            InitEMALoop();
        }

        /// <summary>
        /// Gets the average.
        /// </summary>
        /// <returns>The average.</returns>
        /// <param name="newValue">New value.</param>
		public decimal GetAverage(decimal newValue)
		{
            TopValue = TopValue.Previous;
            TopValue.Value = newValue;

            // if the NEXT previous value is zero, the loop isn't full yet
            // (can't calculate an average without a full loop)
			if (TopValue.Previous?.Value == 0.0M)
                return TopValue.Previous.Value;
            
			// else, calculate exponential average of sample
			int i = 0;
			decimal total = 0.0M, offset = (1 - Alpha);
			EMAValue currentValue = TopValue;

			while (i < SampleSize)
			{
                total += (decimal)(Math.Pow((double)offset, i)) * currentValue.Value;
                currentValue = currentValue.Next;
				i++;
			}

			return (total * Alpha);
		}

		/// <summary>
		/// Inits the EMAL oop.
		/// </summary>
		/// <returns><c>true</c>, if EMAL oop was inited, <c>false</c> otherwise.</returns>
		void InitEMALoop()
		{
			EMAValue next = null, prev = null;
			for (int i = 0; i < SampleSize; i++)
			{
                if (TopValue == null)
                {
                    TopValue = new EMAValue { Index = i };
                    prev = TopValue;
                    continue;
                }

                next = new EMAValue { Index = i } ;
                next.Previous = prev;
                prev.Next = next;
				prev = next;
			}

            next.Next = TopValue;
			TopValue.Previous = next;
		}

        /// <summary>
        /// Prints the EMAL oop.
        /// </summary>
        /// <param name="forward">If set to <c>true</c> forward.</param>
        void PrintEMALoop(bool forward)
		{
            int prevIndex = -1;
            EMAValue currentVal = TopValue;

			string output = (forward ? "\nFORWARD:" : "\nBACKWARD:");
            while (forward ? currentVal.Index > prevIndex : currentVal.Index < prevIndex)
            {
                output += ($"\n\tEMA Entry: {currentVal.Index}\t" +
                           $"Value: {currentVal.Value}\t" + 
                           $"Previous: {currentVal.Previous.Value}\t" + 
                           $"Next: {currentVal.Next.Value}");

				prevIndex = currentVal.Index;
                currentVal = forward ? currentVal.Next : currentVal.Previous;
            }

            LogManager.Info(output, this);
        }

		class EMAValue
		{
            public int Index { get; set; }
			public decimal Value { get; set; }
			public EMAValue Previous { get; set; }
			public EMAValue Next { get; set; }
		}
	}
}