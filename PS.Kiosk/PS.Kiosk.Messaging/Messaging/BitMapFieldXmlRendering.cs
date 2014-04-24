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
	/// Representa un objeto capaz de representar en formato XML un mapa de bits.
	/// </summary>
	/// <remarks>Solo los mapas de bits cuyos números de campo sean mayores o
	/// iguales a cero serán desplegados como campos de mensajes, en caso contrario
	/// serán desplegados como componentes directos del mensaje.</remarks>
	public class BitMapFieldXmlRendering : MessagingComponentXmlRendering {

		#region Constructors
		/// <summary>
		/// Construye un nuevo objeto capaz de representar en formato XML un
		/// mapa de bits.
		/// </summary>
		public BitMapFieldXmlRendering() {

		}
		#endregion

		#region Class method
		/// <summary>
		/// Retorna la representación XML en una cadena de caracteres de un
		/// mapa de bits.
		/// </summary>
		/// <param name="renderingMap">Es el mapa de todos los objetos que representan
		/// objetos. Vea log4net.</param>
		/// <param name="component">Es el BitMap a ser representado en XML.</param>
		/// <param name="indent">Es la indentación a emplear en la representación
		/// XML.</param>
		/// <returns>Retorna una cadena de caracteres con la representación en XML
		/// del mapa de bits.</returns>
		public override string DoRender( RendererMap renderingMap,
			MessagingComponent component, string indent) {

			if ( !( component is BitMapField))
				throw new ArgumentException( SR.ComponentIsNotABitmapField, "component");

			BitMapField bitmap = ( BitMapField)component;
			StringBuilder render = new StringBuilder();
			bool comma = false;

			string bitmapIndent = indent;

			if ( bitmap.FieldNumber >= 0) {
				render.Append( indent);
				render.Append( "<");
				render.Append( MessagingComponentXmlRendering.XmlFieldTag);
				render.Append( " ");
				render.Append( MessagingComponentXmlRendering.XmlNumberAttr);
				render.Append( "=\"");
				render.Append( bitmap.FieldNumber.ToString( CultureInfo.CurrentCulture));
				render.Append( "\" ");
				render.Append( MessagingComponentXmlRendering.XmlTypeAttr);
				render.Append( "=\"");
				render.Append( MessagingComponentXmlRendering.XmlComponentVal);
				render.Append( "\">");
				render.Append( Environment.NewLine);

				bitmapIndent += "   ";
			}

			render.Append( bitmapIndent);
			render.Append( "<");
			render.Append( MessagingComponentXmlRendering.XmlBitMapTag);
			render.Append( " ");
			render.Append( MessagingComponentXmlRendering.XmlValueAttr);
			render.Append( "=\"");

			for( int i = bitmap.LowerFieldNumber; i <= bitmap.UpperFieldNumber; i++) {
				if ( bitmap.IsSet( i)) {
					if ( comma) {
						render.Append( string.Format( ",{0}", i));
					} else {
						comma = true;
						render.Append( i);
					}
				}
			}
			render.Append( "\" />");
			render.Append( Environment.NewLine);

			if ( bitmap.FieldNumber >= 0) {
				render.Append( indent);
				render.Append( "</");
				render.Append( MessagingComponentXmlRendering.XmlFieldTag);
				render.Append( ">");
			}

			return render.ToString();
		}
		#endregion
	}
}
