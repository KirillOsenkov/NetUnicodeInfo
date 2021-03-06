﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	/// <summary>Provides information on radical and additional stroke count for a code point.</summary>
	/// <remarks>Values of this type are usually associated with the property kRSUnicode (aka. Unicode_Radical_Stroke).</remarks>
	public struct UnicodeRadicalStrokeCount
	{
		internal static readonly UnicodeRadicalStrokeCount[] EmptyArray = new UnicodeRadicalStrokeCount[0];

		/// <summary>The radical index.</summary>
		private readonly byte radical;
		/// <summary>The stroke count as a 7bit signed value, together with the <see cref="IsSimplified"/> flag as a 1bit value.</summary>
		private readonly byte strokeCount;

		/// <summary>Initializes a new instance of the class <see cref="UnicodeRadicalStrokeCount"/> from raw data.</summary>
		/// <param name="rawRadical">The raw value to use for <see cref="radical"/>.</param>
		/// <param name="rawStrokeCount">The raw value to use for <see cref="strokeCount"/>.</param>
		internal UnicodeRadicalStrokeCount(byte rawRadical, byte rawStrokeCount)
		{
			radical = rawRadical;
			strokeCount = rawStrokeCount;
		}

		/// <summary>Initializes a new instance of the class <see cref="UnicodeRadicalStrokeCount"/>.</summary>
		/// <remarks><paramref name="strokeCount"/> must be between -64 and 63 included.</remarks>
		/// <param name="radical">The index of the Kangxi radical of the character.</param>
		/// <param name="strokeCount">The number of additional strokes required to form the character from the radical.</param>
		/// <param name="isSimplified">Indicates whether the character is simplified.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="strokeCount"/> is outside of the allowed range.</exception>
		internal UnicodeRadicalStrokeCount(byte radical, sbyte strokeCount, bool isSimplified)
		{
			if (strokeCount < -64 || strokeCount > 63) throw new ArgumentOutOfRangeException(nameof(strokeCount));

			this.radical = radical;
			// Pack strokeCount together with isSimplified in a single byte.
			this.strokeCount = unchecked((byte)(strokeCount & 0x7F | (isSimplified ? 0x80 : 0x00)));
		}

		/// <summary>Gets the index of the Kangxi radical of the character.</summary>
		/// <remarks>The Kangxi radicals are numbered from 1 to 214 inclusive.</remarks>
		/// <value>The index of the Kangxi radical.</value>
		public byte Radical { get { return radical; } }
		/// <summary>Gets the value of <see cref="StrokeCount"/> packed with <see cref="IsSimplified"/>.</summary>
		/// <value>The raw value of <see cref="strokeCount"/>.</value>
		internal byte RawStrokeCount { get { return strokeCount; } }
		/// <summary>Gets the additional stroke count.</summary>
		/// <value>The additional stroke count.</value>
		public sbyte StrokeCount { get { return unchecked((sbyte)(strokeCount & 0x7F | (strokeCount & 0x40) << 1)); } } // To unpack the stroke count, we simply need to copy bit 6 to bit 7.
		/// <summary>Gets a value indicating whether the information is based on the simplified form of the radical.</summary>
		/// <value><see langword="true" /> if the information is based on the simplified form of the radical; otherwise, <see langword="false" />.</value>
		public bool IsSimplified { get { return (strokeCount & 0x80) != 0; } }
	}
}
