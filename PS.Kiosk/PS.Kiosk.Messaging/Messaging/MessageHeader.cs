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
using log4net.ObjectRenderer;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Representa un componente de mensajería que es un cabezal del mensaje.
	/// </summary>
    [Serializable]
	public abstract class MessageHeader : MessagingComponent {

		#region Constructors
		/// <summary>
		/// Contruye un nuevo cabezal de mensaje.
		/// </summary>
		protected MessageHeader() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// Retorna una clase que puede representar en formato XML el cabezal de
		/// mensajes.
		/// </summary>
		/// <param name="renderingMap">Es un mapa con todas las clases que
		/// representan objetos.</param>
		/// <returns>Una clase que puede representar en formato XML el cabezal de
		/// mensajes.
		/// </returns>
		public override MessagingComponentXmlRendering XmlRendering(
			RendererMap renderingMap) {

			IObjectRenderer objectRendering = renderingMap.Get( typeof( MessageHeader));

			if ( objectRendering == null) {
				// Add renderer to map.
				objectRendering = new MessageHeaderXmlRendering();
				renderingMap.Put( typeof( MessageHeader), objectRendering);
			} else
				if ( !( objectRendering is MessageHeaderXmlRendering))
				objectRendering = new MessageHeaderXmlRendering();

			return ( MessagingComponentXmlRendering)objectRendering;
		}
		#endregion
	}
}
