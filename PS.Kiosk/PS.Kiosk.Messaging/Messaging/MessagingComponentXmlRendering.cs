#region Copyright (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// Copyright � 2004-2006 Diego Zabaleta, Leonardo Zabaleta
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
using System.IO;
using log4net.ObjectRenderer;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Representa un objeto capaz de representar en formato XML un componente de
	/// mensajer�a.
	/// </summary>
	public abstract class MessagingComponentXmlRendering : IObjectRenderer {

		/// <summary>
		/// Define el tag a emplear para representar un componente de mensajer�a
		/// de tipo mensaje.
		/// </summary>
		public const string XmlMessageTag = "message";

		/// <summary>
		/// Define el tag a emplear para representar un componente de mensajer�a
		/// de tipo campo.
		/// </summary>
		public const string XmlFieldTag = "field";

		/// <summary>
		/// Define el tag a emplear para representar un componente de mensajer�a
		/// de tipo cabezal.
		/// </summary>
		public const string XmlHeaderTag = "header";

		/// <summary>
		/// Define el tag a emplear para representar un componente de mensajer�a
		/// de tipo bitmap.
		/// </summary>
		public const string XmlBitMapTag = "bitmap";

		/// <summary>
		/// Es el nombre del atributo a emplear para representar el n�mero de un
		/// campo de mensaje.
		/// </summary>
		public const string XmlNumberAttr = "number";

		/// <summary>
		/// Es el nombre del atributo a emplear para representar el valor de un
		/// campo de mensaje.
		/// </summary>
		public const string XmlValueAttr = "value";

		/// <summary>
		/// Es el nombre del atributo a emplear para representar el tipo de datos de un
		/// campo de mensaje.
		/// </summary>
		public const string XmlTypeAttr = "type";

		/// <summary>
		/// Es el valor a emplear para indicar que un campo de mensaje es una cadena
		/// de caracteres.
		/// </summary>
		public const string XmlStringVal = "string";

		/// <summary>
		/// Es el valor a emplear para indicar que un campo de mensaje es un array
		/// de bytes.
		/// </summary>
		public const string XmlBinaryVal = "binary";

		/// <summary>
		/// Es el valor a emplear para indicar que un campo de mensaje es un
		/// componente de mensajer�a diferente a los anteriores (puede ser un
		/// bitmap, un mensaje, etc.).
		/// </summary>
		public const string XmlComponentVal = "component";

		#region Constructors
		/// <summary>
		/// Construye un nuevo objeto capaz de representar en formato XML un
		/// componente de mensajer�a.
		/// </summary>
		protected MessagingComponentXmlRendering() {

		}
		#endregion

		#region Class method
		/// <summary>
		/// Retorna la representaci�n XML en una cadena de caracteres de un componente
		/// de mensajer�a.
		/// </summary>
		/// <param name="renderingMap">Es el mapa de todos los objetos que representan
		/// objetos. Vea log4net.</param>
		/// <param name="component">Es el campo a ser representado en XML.</param>
		/// <param name="indent">Es la indentaci�n a emplear en la representaci�n
		/// XML.</param>
		/// <returns>Retorna una cadena de caracteres con la representaci�n en XML
		/// del componente de mensajer�a.</returns>
		public virtual string DoRender( RendererMap renderingMap, MessagingComponent component,
			string indent) {

			return string.Empty;
		}
		#endregion

		#region IObjectRenderer Members
		/// <summary>
		/// Implementa el m�todo requerido por log4net para personalizar la
		/// representaci�n de un tipo de objetos en particular.
		/// </summary>
		/// <param name="renderingMap">
		/// Es el mapa de todos los objetos que representan objetos. Vea log4net.
		/// </param>
		/// <param name="obj">
		/// Es el objeto a representar.
		/// </param>
		/// <param name="writer">
		/// Es donde se representa el objeto.
		/// </param>
		public virtual void RenderObject( RendererMap renderingMap, object obj, TextWriter writer) {

			if ( !( obj is MessagingComponent)) {
				throw new ArgumentException( SR.ComponentIsNotAMessagingComponent, "obj");
			}

			writer.Write( DoRender( renderingMap, ( MessagingComponent)obj, string.Empty));
		}
		#endregion
	}
}
