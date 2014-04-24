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
	/// Representa un objeto capaz de representar en formato XML un mensaje.
	/// </summary>
	public class MessageXmlRendering : MessagingComponentXmlRendering {

		#region Constructors
		/// <summary>
		/// Construye un nuevo objeto capaz de representar en formato XML un
		/// mensaje.
		/// </summary>
		public MessageXmlRendering() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// Retorna la representación XML en una cadena de caracteres de un
		/// mensaje.
		/// </summary>
		/// <param name="renderingMap">
		/// Es el mapa de todos los objetos que representan objetos. Vea log4net.
		/// </param>
		/// <param name="component">
		/// Es el mensaje a ser representado en XML.
		/// </param>
		/// <param name="indent">
		/// Es la indentación a emplear en la representación XML.
		/// </param>
		/// <returns>
		/// Retorna una cadena de caracteres con la representación en XML
		/// del mensaje.
		/// </returns>
		public override string DoRender( RendererMap renderingMap,
			MessagingComponent component, string indent) {

			if ( !( component is Message)) {
				throw new ArgumentException( SR.ComponentIsNotAMessage, "component");
			}

			Message message = ( Message)component;

			message.CorrectBitMapsValues();

			StringBuilder render = new StringBuilder();
			Field field;

			render.Append( indent);
			render.Append( "<");
			render.Append( MessagingComponentXmlRendering.XmlMessageTag);
			render.Append( ">");
			render.Append( Environment.NewLine);

			string newIndent = indent + "   ";

			if ( message.Header != null)
				render.Append( message.Header.XmlRendering( renderingMap).DoRender(
					renderingMap, message.Header, indent));


			int j = message.Fields.MaximumFieldNumber;
			for( int i = 0; i <= j; i++) {
				if ( ( field = message.Fields[i]) != null) {

					render.Append( field.XmlRendering( renderingMap).DoRender(
						renderingMap, field, newIndent));
				}
			}

			render.Append( indent +
				"</" + MessagingComponentXmlRendering.XmlMessageTag + ">\r\n");

			return render.ToString();
		}
		#endregion
	}
}
