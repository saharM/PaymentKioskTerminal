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
using log4net.ObjectRenderer;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Representa un componente de mensajería que es un campo de mensaje.
	/// </summary>
    [Serializable]
	public abstract class Field : MessagingComponent {

		private int _fieldNumber;

		#region Constructors
		/// <summary>
		/// Contruye un nuevo campo de mensaje.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número del campo en el mensaje.
		/// </param>
		protected Field( int fieldNumber) : base() {

			_fieldNumber = fieldNumber;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna el número del campo en el mensaje.
		/// </summary>
		public int FieldNumber {

			get {

				return _fieldNumber;
			}
		}

		/// <summary>
		/// Retorna o asigna el valor del campo.
		/// </summary>
		public abstract object Value {

			get;
			set;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Retorna una clase que puede representar en formato XML el campo.
		/// </summary>
		/// <param name="renderingMap">
		/// Es un mapa con todas las clases que representan objetos.
		/// </param>
		/// <returns>
		/// Una clase que puede representar en formato XML el campo.
		/// </returns>
		public override MessagingComponentXmlRendering XmlRendering(
			RendererMap renderingMap) {

			IObjectRenderer objectRendering = renderingMap.Get( typeof( Field));

			if ( objectRendering == null) {
				// Add renderer to map.
				objectRendering = new FieldXmlRendering();
				renderingMap.Put( typeof( Field), objectRendering);
			} else {
				if ( !( objectRendering is FieldXmlRendering)) {
					objectRendering = new FieldXmlRendering();
				}
			}

			return ( MessagingComponentXmlRendering)objectRendering;
		}

		/// <summary>
		/// Reasigna el número de campo.
		/// </summary>
		/// <param name="fieldNumber">Es el nuevo número de campo.</param>
		internal void SetFieldNumber( int fieldNumber) {

			_fieldNumber = fieldNumber;
		}
		#endregion
	}
}
