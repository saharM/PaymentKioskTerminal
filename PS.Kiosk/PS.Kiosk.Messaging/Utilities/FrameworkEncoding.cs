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
using System.Text;

namespace Fanap.Utilities {

	/// <summary>
	/// Provides the way to configure the encoding to be used in the framework.
	/// </summary>
	public class FrameworkEncoding {

		private static volatile FrameworkEncoding _instance = null;

		private Encoding _encoding = Encoding.UTF7;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of <see cref="FrameworkEncoding"/>.
		/// </summary>
		private FrameworkEncoding() {

		}
		#endregion

		#region Properties
		/// <summary>
		/// It sets or returns the Encoding used by the framework.
		/// </summary>
		public Encoding Encoding {

			get {

				return _encoding;
			}

			set {

				_encoding = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// It returns an instance of <see cref="FrameworkEncoding"/> class.
		/// </summary>
		/// <returns>
		/// An <see cref="FrameworkEncoding"/> instance.
		/// </returns>
		public static FrameworkEncoding GetInstance() {

			if ( _instance == null) {
				lock ( typeof( FrameworkEncoding)) {
					if ( _instance == null) {
						_instance = new FrameworkEncoding();
					}
				}
			}

			return _instance;
		}
		#endregion
	}
}
