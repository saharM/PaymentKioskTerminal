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
using System.Globalization;
using System.Text;
using log4net.ObjectRenderer;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Representa un objeto capaz de representar en formato XML un campo.
	/// </summary>
	public class FieldXmlRendering : MessagingComponentXmlRendering {

		#region Constructors
		/// <summary>
		/// Construye un nuevo objeto capaz de representar en formato XML un
		/// campo.
		/// </summary>
		public FieldXmlRendering() {

		}
		#endregion

		#region Class method
		/// <summary>
		/// Retorna la representación XML en una cadena de caracteres de un campo
		/// de mensaje.
		/// </summary>
		/// <param name="renderingMap">
		/// Es el mapa de todos los objetos que representan objetos. Vea log4net.
		/// </param>
		/// <param name="component">
		/// Es el campo a ser representado en XML.
		/// </param>
		/// <param name="indent">
		/// Es la indentación a emplear en la representación XML.
		/// </param>
		/// <returns>
		/// Retorna una cadena de caracteres con la representación en XML
		/// del campo de mensaje.
		/// </returns>
		public override string DoRender( RendererMap renderingMap,
			MessagingComponent component, string indent) {

			if ( !( component is Field)) {
				throw new ArgumentException( SR.ComponentIsNotAField, "component");
			}

			Field field = ( Field)component;
			StringBuilder render = new StringBuilder();

			render.Append( indent);
			render.Append( "<");
			render.Append( MessagingComponentXmlRendering.XmlFieldTag);
			render.Append( " ");
			render.Append( MessagingComponentXmlRendering.XmlNumberAttr);
			render.Append( "=\"");
			render.Append( field.FieldNumber.ToString( CultureInfo.CurrentCulture));
			render.Append( "\" ");
			render.Append( MessagingComponentXmlRendering.XmlTypeAttr);
			render.Append( "=\"");

			if ( field.Value is MessagingComponent) {
				render.Append( MessagingComponentXmlRendering.XmlComponentVal);
				render.Append( "\">");
				render.Append( Environment.NewLine);
				render.Append( ( ( MessagingComponent)( field.Value)).XmlRendering(
					renderingMap).DoRender( renderingMap,
					( ( MessagingComponent)( field.Value)), indent + "   "));
				render.Append( indent);
				render.Append( "</");
				render.Append( MessagingComponentXmlRendering.XmlFieldTag);
				render.Append( ">");
			} else {
				if ( field is StringField) {
					render.Append( MessagingComponentXmlRendering.XmlStringVal);
				} else {
					render.Append( MessagingComponentXmlRendering.XmlBinaryVal);
				}
				render.Append( "\" ");
				render.Append( MessagingComponentXmlRendering.XmlValueAttr);
				render.Append( "=\"");
				render.Append( field.ToString());
				render.Append( "\" />");
			}
			render.Append( Environment.NewLine);

			return render.ToString();
		}
		#endregion
	}
}
