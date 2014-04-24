#region Copyright (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// Copyright © 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
#endregion

using System;

namespace Fanap.Messaging {

	/// <summary>
	/// This class implements a length encoder in BCD (Binary
	/// Coded Decimal).
	/// </summary>
	/// <remarks>
	/// Length encoders are used when the messaging components data
	/// is variable.
	/// This class implements the Singleton pattern, you must use
	/// <see cref="GetInstance"/> to acquire the instance.
	/// </remarks>
	public class BcdLengthEncoder : ILengthEncoder {

		// One for each supported size, if more are required only
		// enlarge the instances array and set new lengths in _lengths.
		private static volatile BcdLengthEncoder[] _instances = {
			null, null, null
		};
		private static int[] _lengths = { 99, 9999, 999999};

		private int _lengthsIndex;

		#region Constructors
		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		/// <param name="lengthsIndex">
		/// It's the index in _lengths array storing the maximum length
		/// this class instance can encode.
		/// </param>
		private BcdLengthEncoder( int lengthsIndex) {

			_lengthsIndex = lengthsIndex;
		}
		#endregion

		#region Properties
		/// <summary>
		/// It's the maximum length to encode.
		/// </summary>
		public int MaximumLength {

			get {

				return _lengths[_lengthsIndex];
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// It returns an instance of <see cref="BcdLengthEncoder"/>
		/// class.
		/// </summary>
		/// <param name="maximumLength">
		/// It's the maximum length to encode.
		/// </param>
		/// <returns>
		/// An instance of <see cref="BcdLengthEncoder"/>.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// It's thrown when <paramref name="maximumLength"/> holds an invalid value.
		/// </exception>
		public static BcdLengthEncoder GetInstance( int maximumLength) {

			if ( maximumLength < 0) {
				throw new ArgumentOutOfRangeException( "maximumLength", maximumLength,
					SR.CantBeLowerThanZero);
			}

			if ( maximumLength > _lengths[_lengths.Length - 1]) {
				throw new ArgumentOutOfRangeException( "maximumLength", maximumLength,
					SR.OnlyZeroToNAllowed( _lengths[_lengths.Length - 1]));
			}

			int index = 0;
			for ( ; index < _lengths.Length; index++) {
				if ( maximumLength <= _lengths[index]) {
					break;
				}
			}

			if ( _instances[index] == null) {
				lock ( typeof( BcdLengthEncoder)) {
					if ( _instances[index] == null) {
						_instances[index] = new BcdLengthEncoder( index);
					}
				}
			}

			return _instances[index];
		}
		#endregion

		#region ILengthEncoder Members
		/// <summary>
		/// It returns the length in bytes of the length indicator.
		/// </summary>
		public int EncodedLength {

			get {

				return _lengthsIndex + 1;
			}
		}

		/// <summary>
		/// It formats the length of the data of the messaging components.
		/// </summary>
		/// <param name="length">
		/// It's the length to format.
		/// </param>
		/// <param name="formatterContext">
		/// It's the formatter context to store the formatted length.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// It's thrown when <paramref name="length"/> is greater than the
		/// maximum value supported by the instance.
		/// </exception>
		public void Encode( int length, ref FormatterContext formatterContext) {

			if ( length > _lengths[_lengthsIndex]) {
				throw new ArgumentOutOfRangeException( "length", length,
					SR.LessOrEqualToN( _lengths[_lengthsIndex]));
			}

			// Check if we must resize our buffer.
			if ( formatterContext.FreeBufferSpace < ( _lengthsIndex + 1)) {
				formatterContext.ResizeBuffer( _lengthsIndex + 1);
			}

			byte[] buffer = formatterContext.GetBuffer();

			// Write encoded length.
			for ( int i = formatterContext.UpperDataBound + _lengthsIndex;
				i >= formatterContext.UpperDataBound; i--) {
				buffer[i] = ( byte)( ( ( ( length % 100) / 10) << 4) + ( length % 10));
				length /= 100;
			}
			
			// Update formatter context upper data bound.
			formatterContext.UpperDataBound += _lengthsIndex + 1;
		}

		/// <summary>
		/// Gets the encoded length from the parser context.
		/// </summary>
		/// <param name="parserContext">
		/// It's the parser context holding the data to be parsed.
		/// </param>
		/// <returns>
		/// The length parsed from the parser context.
		/// </returns>
		public int Decode( ref ParserContext parserContext) {

			// Check available data.
			if ( parserContext.DataLength < ( _lengthsIndex + 1)) {
				throw new ArgumentException( SR.InsufficientData, "parserContext");
			}

			int length = 0;
			int value;
			byte[] buffer = parserContext.GetBuffer();
			int offset = parserContext.LowerDataBound;

			// Decode length.
			for ( int i = offset; i <= ( offset + _lengthsIndex); i++) {

				value = ( buffer[i] & 0xF0) >> 4;
				if ( value > 9) {
					throw new MessagingException( SR.InvalidLengthDetected( value));
				}
				length = length * 10 + value;

				value = buffer[i] & 0x0F;
				if ( value > 9) {
					throw new MessagingException( SR.InvalidLengthType( value));
				}
				length = length * 10 + value;
			}

			// Consume parser context data.
			parserContext.Consumed( _lengthsIndex + 1);

			return length;
		}
		#endregion
	}
}
