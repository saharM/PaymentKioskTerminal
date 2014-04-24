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

namespace Fanap.Utilities {

	/// <summary>
	/// It defines a numerical sequencer.
	/// </summary>
	public interface ISequencer {

		/// <summary>
		/// It's the value of the sequencer.
		/// </summary>
		/// <returns>
		/// The actual value of the sequencer.
		/// </returns>
		int CurrentValue();

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
		int Increment();

		/// <summary>
		/// Is the maximum value that can be worth the sequencer.
		/// </summary>
		/// <returns>
		/// The maximum value that can be worth the sequencer.
		/// </returns>
		int Minimum();

		/// <summary>
		/// Is the minimum value that can be worth the sequencer.
		/// </summary>
		/// <returns>
		/// The minimum value that can be worth the sequencer.
		/// </returns>
		int Maximum();
	}
}
