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
using System.Threading;
using Fanap.Messaging;

namespace Fanap.Utilities {

	/// <summary>
	/// Implements a numerical sequencer.
	/// </summary>
	/// <remarks>
	/// The minimum and maximum default values are defined by
	/// <see cref="VolatileSequencerMinimumValue"/> and
	/// <see cref="Int32.MaxValue"/> respectively.
	/// </remarks>
    [Serializable]
	public class VolatileSequencer : ISequencer {

		/// <summary>
		/// The minimum default value for the sequencer.
		/// </summary>
		public const int VolatileSequencerMinimumValue = 1;

		private int _minimumValue = VolatileSequencerMinimumValue;
		private int _maximumValue = Int32.MaxValue;
		private int _traceSeq;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the class <see cref="VolatileSequencer"/>.
		/// </summary>
		public VolatileSequencer() {

			_traceSeq = _minimumValue;
		}

		/// <summary>
		/// Initializes a new instance of the class <see cref="VolatileSequencer"/>.
		/// </summary>
		/// <param name="minimumValue">
		/// The minimum value of the sequencer.
		/// </param>
		public VolatileSequencer( int minimumValue) {

			_traceSeq = _minimumValue = minimumValue;
		}

		/// <summary>
		/// Initializes a new instance of the class <see cref="VolatileSequencer"/>.
		/// </summary>
		/// <param name="minimumValue">
		/// The minimum value of the sequencer.
		/// </param>
		/// <param name="maximumValue">
		/// The maximum value of the sequencer.
		/// </param>
		public VolatileSequencer( int minimumValue, int maximumValue) :
			this( minimumValue) {

			if ( maximumValue <= minimumValue)
				throw new ArgumentOutOfRangeException( "maximumValue", maximumValue,
					SR.MustBeGreaterThanMinimumValue);

			_maximumValue = maximumValue;
		}
		#endregion
	
		#region ISequencer Members
		/// <summary>
		/// It's the value of the sequencer.
		/// </summary>
		/// <returns>
		/// The actual value of the sequencer.
		/// </returns>
		public int CurrentValue() {

			lock ( this) {
				return _traceSeq;
			}
		}

		/// <summary>
		/// It increases in one the present value of the sequencer.
		/// </summary>
		/// <returns>
		/// It returns the value of the sequencer before being increased.
		/// </returns>
		/// <remarks>
		/// If the value increased of the sequencer surpasses the maximum value
		/// permitted by <see cref="Maximum"/>, <see cref="Minimum"/>  it is assigned
		/// to present value.  
		/// </remarks>
		public int Increment() {

			int valueToReturn = 0;

			lock ( this) {
				valueToReturn = _traceSeq;

				_traceSeq++;
				if ( _traceSeq > _maximumValue) {
					_traceSeq = _minimumValue;
				}
			}

			return valueToReturn;
		}

		/// <summary>
		/// Is the minimum value that can be worth the sequencer.
		/// </summary>
		/// <returns>
		/// The minimum value that can be worth the sequencer.
		/// </returns>
		public int Maximum() {

			return _maximumValue;
		}

		/// <summary>
		/// Is the maximum value that can be worth the sequencer.
		/// </summary>
		/// <returns>
		/// The maximum value that can be worth the sequencer.
		/// </returns>
		public int Minimum() {

			return _minimumValue;
		}
		#endregion
	}
}
